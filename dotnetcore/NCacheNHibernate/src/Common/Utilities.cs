using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Dependencies;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Cache.Entry;
using NHibernate.Cache;
using NHibernate.Engine;

namespace NHibernate.Caches.NCache
{
    internal static class Utilities
    {
        internal static Alachisoft.NCache.Client.ICache CreateCache(
            string cacheId,
            CacheConnectionOptions options)
        {
            if (options == null)
            {
                return CacheManager.GetCache(cacheId);
            }

            return CacheManager.GetCache(cacheId, options);
        }

        internal static string GetCacheKey(
            object key,
            string regionKey)
        {
            if (key == null)
            {
                return null;
            }


            return $"{{{regionKey}}}:{key}";
        }

        internal static string[] GetCacheKeys(
            object[] keys,
            string regionKey)
        {
            return keys.Select(x => GetCacheKey(x, regionKey)).ToArray();
        }

        internal static CacheItem GetCacheItem(
            object key,
            object value,
            RegionConfig regionConfig,
            string regionPrefix,
            string regionKey,
            TimeSpan? itemExpiration,
            bool useSlidingExpiration,
            string connectionString)
        {
            var cacheItem = new CacheItem(value)
            {
                Tags = new Tag[] { new Tag(regionKey) }
            };

            Expiration expiration;
            if (itemExpiration.HasValue &&
                itemExpiration.Value.TotalMilliseconds > 0.0d)
            {
                if (useSlidingExpiration)
                {
                    expiration = new Expiration(ExpirationType.Sliding,
                                                    itemExpiration.Value);
                }
                else
                {
                    expiration = new Expiration(ExpirationType.Absolute,
                                                    itemExpiration.Value);
                }

                cacheItem.Expiration = expiration;
            }

            var aggregateDependency = GetDependency(
                                                    key,
                                                    value,
                                                    regionPrefix,
                                                    regionConfig,
                                                    connectionString);

            if (aggregateDependency != null)
            {
                cacheItem.Dependency = aggregateDependency;
            }

            return cacheItem;
        }

        internal static CacheItem GetQueryCacheItem(
            string key,
            object value,
            string regionKey,
            TimeSpan? itemExpiration,
            bool useSlidingExpiration,
            string connectionString,
            ISessionImplementor session,
            List<QueryDependencyConfiguration> queryDependencies)
        {
            var cacheItem = new CacheItem(value)
            {
                Tags = new Tag[] { new Tag(regionKey) }
            };

            Expiration expiration;
            if (itemExpiration.HasValue &&
                itemExpiration.Value.TotalMilliseconds > 0.0d)
            {
                if (useSlidingExpiration)
                {
                    expiration = new Expiration(ExpirationType.Sliding,
                                                    itemExpiration.Value);
                }
                else
                {
                    expiration = new Expiration(ExpirationType.Absolute,
                                                    itemExpiration.Value);
                }

                cacheItem.Expiration = expiration;
            }

            var aggregateDependency = GetQueryDependency(
                                                    key,
                                                    session,
                                                    connectionString,
                                                    queryDependencies);

            if (aggregateDependency != null)
            {
                cacheItem.Dependency = aggregateDependency;
            }

            return cacheItem;
        }

        internal static CacheItem[] GetCacheItems(
            object[] keys,
            object[] values,
            RegionConfig regionConfig,
            string regionPrefix,
            string regionKey,
            TimeSpan? itemExpiration,
            bool useSlidingExpiration,
            string connectionString)
        {
            CacheItem[] items = new CacheItem[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                items[i] = GetCacheItem(
                                keys[i],
                                values[i],
                                regionConfig,
                                regionPrefix,
                                regionKey,
                                itemExpiration,
                                useSlidingExpiration,
                                connectionString);
            }

            return items;
        }

        internal static CacheItem[] GetQueryCacheItems(
            string[] keys,
            object[] values,
            string regionKey,
            TimeSpan? itemExpiration,
            bool useSlidingExpiration,
            string connectionString,
            ISessionImplementor session,
            List<QueryDependencyConfiguration> queryDependencies)
        {
            CacheItem[] items = new CacheItem[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                items[i] = GetQueryCacheItem(
                                keys[i],
                                values[i],
                                regionKey,
                                itemExpiration,
                                useSlidingExpiration,
                                connectionString,
                                session,
                                queryDependencies);
            }

            return items;
        }



        internal static PolicyWrap<bool> SetFallBackPolicy(
            INHibernateLogger logger,
            CircuitBreakerPolicy circuitBreaker)
        {
            return Policy<bool>
                .Handle<BrokenCircuitException>()
                .Or<Alachisoft.NCache.Runtime.Exceptions.CacheException>(ex =>
                    ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE)
                .Fallback<bool>(
                    fallbackValue:
                        false,
                    onFallback:
                        (res, ctx) =>
                        {
                            if (logger.IsDebugEnabled())
                            {
                                logger.Debug(
                                    $"Fallback policy:\n" +
                                    $"\tRegion: {ctx["region"]}" +
                                    $"\tOperation: {ctx["operation"]}\n" +
                                    $"\tFallback result: {res.Result}");
                            }
                        })
                .Wrap(circuitBreaker);
        }

        internal static PolicyWrap<object> GetFallBackPolicy(
            INHibernateLogger logger,
            CircuitBreakerPolicy circuitBreaker)
        {
            return Policy<object>
                .Handle<BrokenCircuitException>()
                .Or<Alachisoft.NCache.Runtime.Exceptions.CacheException>(ex =>
                    ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE)
                .Fallback<object>(
                        fallbackValue:
                            null,
                        onFallback:
                            (res, ctx) =>
                            {
                                if (logger.IsDebugEnabled())
                                {
                                    logger.Debug(
                                        $"Fallback policy:\n" +
                                        $"\tRegion: {ctx["region"]}" +
                                        $"\tOperation: {ctx["operation"]}\n" +
                                        $"\tFallback result: {res.Result ?? "null"}");
                                }
                            })
                .Wrap(circuitBreaker);
        }

        internal static PolicyWrap<object> GetLockFallBackPolicy(
            RegionConfig regionConfig,
            CircuitBreakerPolicy circuitBreaker,
            INHibernateLogger logger)
        {
            var retryCircuitBreakerPolicyWrap =
                     Policy<object>
                       .HandleResult(x => x == null)
                       .WaitAndRetry(
                         retryCount:
                            regionConfig.LockRetryTimes,
                         sleepDurationProvider:
                            (attempt, ctx) =>
                            {
                                return GetRetryWaitDuration(
                                            attempt,
                                            ctx,
                                            regionConfig.LockMinRetryDelay,
                                            regionConfig.LockMaxRetryDelay);
                            },
                         onRetry:
                            (res, ts, attempt, ctx) =>
                            {
                                if (logger.IsDebugEnabled())
                                {
                                    logger.Debug(
                                        $"Retry policy:\n" +
                                        $"\tRegion: {ctx["region"]}" +
                                        $"\tOperation: {ctx["operation"]}\n" +
                                        $"\tRetry attempt: {attempt},\n" +
                                        $"\tSleepDuration:" +
                                            $" {ts.TotalMilliseconds} ms");
                                }
                            }
                        )
                   .Wrap(circuitBreaker);

            var timeoutRetryCircuitBreakerPolicyWrap =
                                       Policy
                                        .Timeout(
                                           timeout:
                                            regionConfig.LockAcquireTimeout,
                                           timeoutStrategy:
                                            TimeoutStrategy.Optimistic,
                                           onTimeout:
                                            (ctx, ts, task) =>
                                            {
                                                if (logger.IsDebugEnabled())
                                                {
                                                    logger.Debug(
                                                        $"Timeout policy:\n" +
                                                        $"\tRegion: {ctx["region"]}" +
                                                        $"\tOperation: {ctx["operation"]}\n" +
                                                        $"\tTimeSpan: {ts.TotalMilliseconds} ms");
                                                }
                                            })
                                        .Wrap(retryCircuitBreakerPolicyWrap);

            return Policy<object>
                     .Handle<TimeoutRejectedException>()
                     .Or<BrokenCircuitException>()
                     .Or<Alachisoft.NCache.Runtime.Exceptions.CacheException>(e =>
                                        e.ErrorCode ==
                                            NCacheErrorCodes.NO_SERVER_AVAILABLE)
                     .OrResult(x => x == null)
                     .Fallback<object>(
                        fallbackValue:
                            (object)null,
                        onFallback:
                            (res, ctx) =>
                            {
                                if (logger.IsDebugEnabled())
                                {
                                    logger.Debug(
                                        $"Fallback policy:\n" +
                                        $"\tRegion: {ctx["region"]}" +
                                        $"\tOperation: {ctx["operation"]}\n" +
                                        $"\tFallback result: {res?.Result ?? "null"}");
                                }
                            }
                        )
                     .Wrap(timeoutRetryCircuitBreakerPolicyWrap);
        }

        internal static CircuitBreakerPolicy GetCircuitBreakerPolicy(
            RegionConfig regionConfig,
            CacheConfig cacheConfig,
            INHibernateLogger logger)
        {
            return Policy
                .Handle<Alachisoft.NCache.Runtime.Exceptions.CacheException>(ex =>
                    ex.ErrorCode ==
                        NCacheErrorCodes.NO_SERVER_AVAILABLE)
                .CircuitBreaker(exceptionsAllowedBeforeBreaking:
                        regionConfig.CircuitBreakerExceptionsAllowed,
                    durationOfBreak:
                        TimeSpan.FromSeconds(
                            regionConfig.CircuitBreakerDurationOfBreak),
                    onBreak:
                        (ex, ts, ctx) =>
                        {
                            if (logger.IsDebugEnabled())
                            {
                                logger.Debug(
                                    $"Circuitbreaker policy:\n" +
                                    $"\tRegion: {ctx["region"]}" +
                                    $"\tOperation: {ctx["operation"]}" +
                                    $"\tCircuit broken for " +
                                    $"{cacheConfig.CacheId}\n" +
                                    $"\tReason:{ex.Message}");
                            }
                        },
                    onReset: (ctx) =>
                    {
                        if (logger.IsDebugEnabled())
                        {
                            logger.Debug(
                                $"Circuitbreaker policy:\n" +
                                $"\tRegion: {ctx["region"]}" +
                                $"\tCircuit reset for {cacheConfig.CacheId}");
                        }
                    },
                    onHalfOpen: () =>
                    {
                        if (logger.IsDebugEnabled())
                        {
                            logger.Debug(
                                $"Circuitbreaker policy:\n" +
                                $"\tCircuit half-open for" +
                                $" {cacheConfig.CacheId}");
                        }
                    }
                );
        }



        internal static AsyncPolicyWrap<bool> SetAsyncFallBackPolicy(
            INHibernateLogger logger,
            AsyncCircuitBreakerPolicy circuitBreaker)
        {
            return Policy<bool>
                .Handle<BrokenCircuitException>()
                .Or<Alachisoft.NCache.Runtime.Exceptions.CacheException>(ex =>
                    ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE)
                .FallbackAsync<bool>(
                    fallbackAction:
                        (ctx, ct) =>
                        {
                            return Task.FromResult(false);
                        },
                    onFallbackAsync:
                        (res, ctx) =>
                        {
                            if (logger.IsDebugEnabled())
                            {
                                logger.Debug(
                                        $"AsyncFallback policy:\n" +
                                        $"\tRegion: {ctx["region"]}" +
                                        $"\tOperation: {ctx["operation"]}\n" +
                                        $"\tFallback result: {res.Result}");
                            }

                            return Task.CompletedTask;
                        }
                )
                .WrapAsync(circuitBreaker);
        }

        internal static AsyncPolicyWrap<object> GetAsyncFallBackPolicy(
            INHibernateLogger logger,
            AsyncCircuitBreakerPolicy circuitBreaker)
        {
            return Policy<object>
                .Handle<BrokenCircuitException>()
                .Or<Alachisoft.NCache.Runtime.Exceptions.CacheException>(ex =>
                    ex.ErrorCode == NCacheErrorCodes.NO_SERVER_AVAILABLE)
                .FallbackAsync(
                    fallbackAction:
                        (ctx, ct) =>
                        {
                            return Task.FromResult((object)null);
                        },
                    onFallbackAsync:
                        (res, ctx) =>
                        {
                            if (logger.IsDebugEnabled())
                            {
                                logger.Debug(
                                        $"AsyncFallback policy:\n" +
                                        $"\tRegion: {ctx["region"]}" +
                                        $"\tOperation: {ctx["operation"]}\n" +
                                        $"\tFallback result: {res.Result ?? "null"}");
                            }

                            return Task.CompletedTask;
                        }
                )
                .WrapAsync(circuitBreaker);
        }

        internal static AsyncPolicyWrap<object> GetAsyncLockFallBackPolicy(
            RegionConfig regionConfig,
            AsyncCircuitBreakerPolicy circuitBreaker,
            INHibernateLogger logger)
        {
            var retryCircuitBreakerAsyncPolicyWrap =
                    Policy<object>
                    .HandleResult(x => x == null)
                    .WaitAndRetryAsync(
                        retryCount:
                            regionConfig.LockRetryTimes,
                        sleepDurationProvider:
                            (retryAttempt, context) =>
                            {
                                return GetRetryWaitDuration(
                                            retryAttempt,
                                            context,
                                            regionConfig.LockMinRetryDelay,
                                            regionConfig.LockMaxRetryDelay);
                            },
                        onRetry:
                            (res, ts, attempt, ctx) =>
                            {
                                if (logger.IsDebugEnabled())
                                {
                                    logger.Debug(
                                        $"AsyncRetry policy:\n" +
                                        $"\tRegion: {ctx["region"]}" +
                                        $"\tOperation: {ctx["operation"]}\n" +
                                        $"\tRetry attempt: {attempt},\n" +
                                        $"\tSleepDuration:" +
                                            $" {ts.TotalMilliseconds} ms");
                                }
                            }
                    )
                    .WrapAsync(circuitBreaker);

            var timeoutRetryCircuitBreakerAsyncPolicyWrap =
                                  Policy
                                    .TimeoutAsync(
                                           timeout:
                                            regionConfig.LockAcquireTimeout,
                                           timeoutStrategy:
                                            TimeoutStrategy.Optimistic,
                                           onTimeoutAsync:
                                            (ctx, ts, task) =>
                                            {
                                                if (logger.IsDebugEnabled())
                                                {
                                                    logger.Debug(
                                                        $"AsyncTimeout policy:\n" +
                                                        $"\tRegion: {ctx["region"]}" +
                                                        $"\tOperation: {ctx["operation"]}\n" +
                                                        $"\tTimeSpan: {ts.TotalMilliseconds} ms");
                                                }

                                                return Task.CompletedTask;
                                            })
                                    .WrapAsync(retryCircuitBreakerAsyncPolicyWrap);

            return Policy<object>
                     .Handle<TimeoutRejectedException>()
                     .Or<BrokenCircuitException>()
                     .Or<Alachisoft.NCache.Runtime.Exceptions.CacheException>(e =>
                                        e.ErrorCode ==
                                            NCacheErrorCodes.NO_SERVER_AVAILABLE)
                     .OrResult(x => x == null)
                     .FallbackAsync<object>(
                        fallbackAction:
                            (ctx, ct) =>
                            {
                                return Task.FromResult((object)null);
                            },
                        onFallbackAsync:
                            (res, ctx) =>
                            {
                                if (logger.IsDebugEnabled())
                                {
                                    logger.Debug(
                                        $"AsyncFallback policy:\n" +
                                        $"\tRegion: {ctx["region"]}" +
                                        $"\tOperation: {ctx["operation"]}\n" +
                                        $"\tFallback result: {res.Result ?? "null"}");
                                }

                                return Task.CompletedTask;
                            }
                        )
                     .WrapAsync(timeoutRetryCircuitBreakerAsyncPolicyWrap);
        }

        internal static AsyncCircuitBreakerPolicy GetAsyncCircuitBreakerPolicy(
            RegionConfig regionConfig,
            CacheConfig cacheConfig,
            INHibernateLogger logger)
        {
            return Policy
                .Handle<Alachisoft.NCache.Runtime.Exceptions.CacheException>(ex =>
                    ex.ErrorCode ==
                        NCacheErrorCodes.NO_SERVER_AVAILABLE)
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking:
                        regionConfig.CircuitBreakerExceptionsAllowed,
                    durationOfBreak:
                        TimeSpan.FromSeconds(
                            regionConfig.CircuitBreakerDurationOfBreak),
                    onBreak:
                        (ex, ts, ctx) =>
                        {
                            if (logger.IsDebugEnabled())
                            {
                                logger.Debug(
                                    $"AsyncCircuitbreaker policy:\n" +
                                    $"\tRegion: {ctx["region"]}" +
                                    $"\tOperation: {ctx["operation"]}" +
                                    $"\tCircuit broken for " +
                                    $"{cacheConfig.CacheId}\n" +
                                    $"\tReason:{ex.Message}");
                            }
                        },
                    onReset: (ctx) =>
                    {
                        if (logger.IsDebugEnabled())
                        {
                            logger.Debug(
                                $"AsyncCircuitbreaker policy:\n" +
                                $"\tRegion: {ctx["region"]}" +
                                $"\tCircuit reset for {cacheConfig.CacheId}");
                        }
                    },
                    onHalfOpen: () =>
                    {
                        if (logger.IsDebugEnabled())
                        {
                            logger.Debug(
                                $"AsyncCircuitbreaker policy:\n" +
                                $"\tCircuit half-open for" +
                                $" {cacheConfig.CacheId}");
                        }
                    }
                );
        }


        private static AggregateCacheDependency GetDependency(
            object key,
            object value,
            string regionPrefix,
            RegionConfig regionConfig,
            string connectionString)
        {
            CacheEntry cachedEntry = null;

            if (value is CachedItem item)
            {
                if (item.Value is CacheEntry entry)
                {
                    cachedEntry = entry;
                }
                else
                {
                    return null;
                }
            }
            else if (value is CacheLock cacheLock)
            {
                return null;
            }
            else
            {
                cachedEntry = value as CacheEntry;

                if (cachedEntry == null)
                {
                    return null;
                }
            }


            var id = (key as CacheKey).Key;


            var entityClassFullName = cachedEntry.Subclass;

            var dependencies = NCacheProvider.GetEntityDependencyConfigs(
                                                    regionPrefix,
                                                    entityClassFullName)
                                    .ToArray();

            if (dependencies.Length == 0)
            {
                return null;
            }

            List<CacheDependency> cacheDependencies =
                                new List<CacheDependency>(dependencies.Length);

            for (int i = 0; i < dependencies.Length; i++)
            {
                var cacheDependency =
                    dependencies[i].GetCacheDependency(
                        connectionString,
                        id);

                cacheDependencies.Add(cacheDependency);
            }

            var aggregateDependency = new AggregateCacheDependency();

            aggregateDependency.Add(cacheDependencies.ToArray());

            return aggregateDependency;

        }

        private static AggregateCacheDependency GetQueryDependency(
                            string key,
                            ISessionImplementor session,
                            string connectionString,
                            List<QueryDependencyConfiguration> queryDependencies)
        {
            var dialectString = session.Factory.Dialect.ToString();

            HashSet<QueryDependencyConfiguration> filteredConfigs =
                new HashSet<QueryDependencyConfiguration>(
                                new QueryDependencyConfigComparer());

            if (dialectString.Contains("MsSql"))
            {
                foreach (var queryDependency in queryDependencies)
                {
                    if (key.Contains(queryDependency.QualifiedTableName) &&
                        queryDependency.DatabaseType == DatabaseType.Sql)
                    {
                        if (queryDependency.IsPollingDependencyUsed)
                        {
                            filteredConfigs.Add(queryDependency);
                        }
                        else
                        {
                            bool passed = true;

                            var commandDependency =
                                queryDependency as QueryCommandDependencyConfiguration;

                            foreach (var columnName in commandDependency.KeyColumnNames)
                            {
                                if (!key.Contains(columnName))
                                {
                                    passed = false;
                                    break;
                                }
                            }

                            if (passed)
                            {
                                filteredConfigs.Add(commandDependency);
                            }
                        }
                    }
                }
            }
            else if (dialectString.Contains("Oracle"))
            {
                foreach (var queryDependency in queryDependencies)
                {
                    if (key.Contains(queryDependency.QualifiedTableName) &&
                        queryDependency.DatabaseType == DatabaseType.Oracle)
                    {
                        if (queryDependency.IsPollingDependencyUsed)
                        {
                            filteredConfigs.Add(queryDependency);
                        }
                        else
                        {
                            bool passed = true;

                            var commandDependency =
                                queryDependency as QueryCommandDependencyConfiguration;

                            foreach (var columnName in commandDependency.KeyColumnNames)
                            {
                                if (!key.Contains(columnName))
                                {
                                    passed = false;
                                    break;
                                }
                            }

                            if (passed)
                            {
                                filteredConfigs.Add(commandDependency);
                            }
                        }
                    }
                }
            }
            else
            {
                return null;
            }

            List<CacheDependency> cacheDependencies = new List<CacheDependency>();

            foreach (var filteredConfig in filteredConfigs)
            {
                cacheDependencies.Add(
                    filteredConfig.CreateDependency(
                                            connectionString));
            }



            if (cacheDependencies.Count > 0)
            {
                AggregateCacheDependency aggregateCacheDependency =
                                                new AggregateCacheDependency();
                aggregateCacheDependency.Add(cacheDependencies.ToArray());

                return aggregateCacheDependency;
            }

            return null;
        }


        private static TimeSpan GetRetryWaitDuration(
            int retryAttempt,
            Polly.Context context,
            TimeSpan minRetryDelay,
            TimeSpan maxRetryDelay)
        {
            var random = context["random"] as Random;
            double prev = (double)context["prev"];

            const double PFactor = 4.0;

            const double RPScalingFactor = 1 / 1.4d;

            double t = (retryAttempt - 1) + random.NextDouble();
            double next = Math.Pow(2, t) * Math.Tanh(Math.Sqrt(PFactor * t));

            var formulaValue = next - prev;

            context["prev"] = next;

            long ticks =
                (long)Math.Min(
                    formulaValue * RPScalingFactor * minRetryDelay.Ticks,
                    maxRetryDelay.Ticks);

            return TimeSpan.FromTicks(ticks);
        }
    }
}
