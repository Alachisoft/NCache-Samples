using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using Polly.Wrap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NHibernate.Caches.NCache
{
    public abstract class NCacheClientBase : CacheBase
    {
        private readonly CacheHandle _cacheHandle;
        private readonly RegionConfig _regionConfig;

        private readonly PolicyWrap<bool> _setFallback;
        private readonly PolicyWrap<object> _getFallback;
        private readonly PolicyWrap<object> _getLockFallback;

        private readonly AsyncPolicyWrap<bool> _setAsyncFallback;
        private readonly AsyncPolicyWrap<object> _getAsyncFallback;
        private readonly AsyncPolicyWrap<object> _getAsyncLockFallback;



        private const string LOCKING_USED = "LOCKING_IN_USE";

        private bool _useSlidingExpiration = false;
        private TimeSpan? _itemExpiration = null;
        
        private string _connectionString = null;
        private string regionKey
        {
            get
            {
                return $"{_regionConfig.CacheKeyPrefix}{RegionName}";
            }
        }

        private readonly INHibernateLogger _logger =
            NHibernateLogger.For(typeof(NCacheClient));


        public NCacheClientBase(
                    CacheHandle cacheHandle,
                    string regionPrefix,
                    RegionConfig regionConfig,
                    IDictionary<string, string> properties)
        {
            if (regionConfig == null)
            {
                throw new ArgumentNullException(
                                    nameof(regionConfig),
                                    $"Constructor needs a valid RegionConfig argument");
            }

            if (cacheHandle == null)
            {
                throw new ArgumentNullException(
                                    nameof(cacheHandle),
                                    $"Constructor needs a valid CacheConfig argument");
            }

            _cacheHandle = cacheHandle;

            _regionConfig = regionConfig;

            RegionPrefix = regionPrefix;

            var circuitBreaker = Utilities.GetCircuitBreakerPolicy(
                                                   regionConfig,
                                                    _cacheHandle.CacheConfig,
                                                   _logger);

            _setFallback = Utilities.SetFallBackPolicy(
                                                _logger,
                                                circuitBreaker);

            _getFallback = Utilities.GetFallBackPolicy(
                                                _logger,
                                                circuitBreaker);

            _getLockFallback = Utilities.GetLockFallBackPolicy(
                                                        regionConfig,
                                                        circuitBreaker,
                                                        _logger);

            var asyncCircuitBreaker = Utilities.GetAsyncCircuitBreakerPolicy(
                                                   regionConfig,
                                                   _cacheHandle.CacheConfig,
                                                   _logger);

            _setAsyncFallback = Utilities.SetAsyncFallBackPolicy(
                                                        _logger,
                                                        asyncCircuitBreaker);
            _getAsyncFallback = Utilities.GetAsyncFallBackPolicy(
                                                        _logger,
                                                        asyncCircuitBreaker);
            _getAsyncLockFallback = Utilities.GetAsyncLockFallBackPolicy(
                                                            regionConfig,
                                                            asyncCircuitBreaker,
                                                            _logger);

            AddAdditionalConfig(
                regionConfig,
                properties);
        }

        internal readonly string RegionPrefix;
        public override string RegionName =>
            _regionConfig.RegionName;

        public override bool PreferMultipleGet =>
            true;

        public override int Timeout =>
            Timestamper.OneMs * (int)_regionConfig.LockKeyTimeout.TotalMilliseconds;

        public override void Clear()
        {
            if (_logger.IsDebugEnabled())
            {
                _logger.Debug($"Clearing cache region {regionKey}");
            }

            var result = _setFallback.Execute(
                            (ctx) =>
                            {
                                return ExecuteClear();
                            },
                            new Dictionary<string, object>
                            {
                                { "operation", "Clear"},
                                {"region",$"{regionKey}" }
                            }
                        );

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Unable to clear cache region {regionKey}");
                }
            }
        }

        public async override Task ClearAsync(
                            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug($"Clearing cache region {regionKey}");
            }
            var result = await _setAsyncFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecuteClearAsync(ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","ClearAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken).ConfigureAwait(false);

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Unable to clear cache region {regionKey}");
                }
            }
        }

        public override void Destroy()
        { }

        public override object Get(
                        object key)
        {
            if (key == null)
            {
                return null;
            }

            var cacheKey = Utilities.GetCacheKey(key, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Fetching object from cache region {regionKey} with key " +
                    $"{cacheKey}");
            }

            return _getFallback.Execute(
            (ctx) =>
            {
                return ExecuteGet(cacheKey);
            },
            new Dictionary<string, object>
            {
                {"operation","Get" },
                {"region",$"{regionKey}" }
            });

        }

        public async override Task<object> GetAsync(
                                object key,
                                CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (key == null)
            {
                return Task.FromResult<object>(null);
            }

            var cacheKey = Utilities.GetCacheKey(key, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Fetching object from cache region {regionKey} with key {cacheKey}");
            }

            return await _getAsyncFallback.ExecuteAsync(
                    (ctx, ct) =>
                    {
                        return ExecuteGetAsync(
                                            cacheKey,
                                            ct);
                    },
                    new Dictionary<string, object>
                    {
                        {"operation","GetAsync" },
                        {"region",$"{regionKey}" }
                    },
                    cancellationToken).ConfigureAwait(false);

        }

        public override object[] GetMany(
                            object[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var cacheKeys = Utilities.GetCacheKeys(keys, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Fetching object from cache region {regionKey}" +
                    $" with keys {string.Join(",", cacheKeys)}");
            }

            return _getFallback.Execute(
                            (ctx) =>
                            {
                                return ExecuteGetMany(cacheKeys);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","GetMany" },
                                {"region",$"{regionKey}" }
                            }
                        ) as object[];
        }

        public async override Task<object[]> GetManyAsync(
                                    object[] keys,
                                    CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));

            }

            var cacheKeys = Utilities.GetCacheKeys(keys, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Fetching object from cache region {regionKey} " +
                    $"with keys {string.Join(",", cacheKeys)}");
            }

            var result = await _getAsyncFallback.ExecuteAsync(
                            async (ctx, ct) =>
                            {
                                return await ExecuteGetManyAsync(
                                                                cacheKeys,
                                                                ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","GetManyAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken)
                                .ConfigureAwait(false);

            if (result == null)
            {
                return new object[cacheKeys.Length];
            }

            return result as object[];
        }

        public override long NextTimestamp()
        {
            return Timestamper.Next();
        }

        public override void Put(
                        object key,
                        object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var cacheKey = Utilities.GetCacheKey(
                                            key,
                                            regionKey);


            var cacheItem = Utilities.GetCacheItem(
                                            key,
                                            value,
                                            _regionConfig,
                                            RegionPrefix,
                                            regionKey,
                                            _itemExpiration,
                                            _useSlidingExpiration,
                                            _connectionString);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Inserting object {value} in cache region {regionKey}" +
                    $" with key {key}");
            }

            var result =
                _setFallback.Execute(
                        (ctx) =>
                        {
                            return ExecutePut(
                                        cacheKey,
                                        cacheItem);
                        },
                        new Dictionary<string, object>
                        {
                            {"operation","Put" },
                            {"region",$"{regionKey}" }
                        }
                    );

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error adding key {key} with value {value}" +
                        $"into the cache region {regionKey}");
                }
            }
        }

        public async override Task PutAsync(
                            object key,
                            object value,
                            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var cacheKey = Utilities.GetCacheKey(
                                        key, regionKey);
            var cacheItem = Utilities.GetCacheItem(
                                        key,
                                        value,
                                        _regionConfig,
                                        RegionPrefix,
                                        regionKey,
                                        _itemExpiration,
                                        _useSlidingExpiration,
                                        _connectionString);

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Inserting object {value} in cache region {regionKey} " +
                    $"with key {key}");
            }

            var result = await _setAsyncFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecutePutAsync(
                                    cacheKey,
                                    cacheItem,
                                    ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","PutAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken
                        ).ConfigureAwait(false);

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error adding key {key} with value {value}" +
                        $"into the cache region {regionKey}");
                }
            }
        }

        public override void PutMany(
                        object[] keys,
                        object[] values)
        {
            if (keys == null ||
                keys.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (values == null ||
                values.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (keys.Length != values.Length)
            {
                throw new ArgumentException(
                            $"{nameof(keys)} and {nameof(values)} must have same " +
                            $"length. Found keys to " +
                            $"have length {keys.Length} and values to " +
                            $"have length {values.Length}.");
            }

            var cacheKeys = Utilities.GetCacheKeys(
                                            keys,
                                            regionKey);
            var cacheValues = Utilities.GetCacheItems(
                                            keys,
                                            values,
                                            _regionConfig,
                                            RegionPrefix,
                                            regionKey,
                                            _itemExpiration,
                                            _useSlidingExpiration,
                                            _connectionString);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Inserting objects {string.Join(",", values)} in " +
                    $"cache region {regionKey} " +
                    $"with keys {string.Join(",", keys)}");
            }

            var result = _setFallback.Execute(
                            (ctx) =>
                            {
                                return ExecutePutMany(
                                            cacheKeys,
                                            cacheValues);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation", "PutMany" },
                                {"region",$"{regionKey}" }
                            }
                        );

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error adding keys {string.Join(",", keys)} in cache" +
                        $" region {regionKey}");
                }
            }
        }

        public async override Task PutManyAsync(
                            object[] keys,
                            object[] values,
                            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (keys == null ||
                keys.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (values == null ||
                values.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (keys.Length != values.Length)
            {
                throw new ArgumentException(
                            $"{nameof(keys)} and {nameof(values)} must have same " +
                            $"length. Found keys to " +
                            $"have length {keys.Length} and values to " +
                            $"have length {values.Length}.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var cacheKeys = Utilities.GetCacheKeys(
                                            keys,
                                            regionKey);
            var cacheValues = Utilities.GetCacheItems(
                                            keys,
                                            values,
                                            _regionConfig,
                                            RegionPrefix,
                                            regionKey,
                                            _itemExpiration,
                                            _useSlidingExpiration,
                                            _connectionString);

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Inserting objects {string.Join(",", values)} in cache " +
                    $"region {regionKey} " +
                    $"with keys {string.Join(",", keys)}");
            }

            var result = await _setAsyncFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecutePutManyAsync(
                                            cacheKeys,
                                            cacheValues,
                                            ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","PutManyAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken
                        ).ConfigureAwait(false);

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error adding keys {string.Join(",", keys)} in cache " +
                        $"region {regionKey}");
                }
            }
        }

        public override void Remove(
                        object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheKey = Utilities.GetCacheKey(key, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Removing key {key} from cache region {regionKey}");
            }

            var result = _setFallback.Execute(
                            (ctx) =>
                            {
                                return ExecuteRemove(cacheKey);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","Remove" },
                                {"region",$"{regionKey}" }
                            }
                        );

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error($"Error in removing key {key} in cache" +
                        $" region {regionKey}");
                }
            }
        }

        public async override Task RemoveAsync(
                                object key,
                                CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheKey = Utilities.GetCacheKey(key, regionKey);

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Removing key {key} from cache region {regionKey}");
            }

            var result = await _setAsyncFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecuteRemoveAsync(
                                                cacheKey,
                                                ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","RemoveAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken
                        ).ConfigureAwait(false);

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error removing key {key} in cache region {regionKey}");
                }
            }
        }

        public override object Lock(
                        object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }


            var cacheKey = Utilities.GetCacheKey(key, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Locking key {key} in cache region {regionKey}");
            }

            var result = _getLockFallback.Execute(
                            (ctx) =>
                            {
                                return ExecuteLock(cacheKey);
                            },
                            new Dictionary<string, object>
                            {
                                {"random", RandomGenerator.Instance() },
                                {"prev", 0.0d },
                                { "operation", "Lock"},
                                {"region",$"{regionKey}" }
                            }
                        );

            if (result == null)
            {
                throw new CacheException(
                                $"Could not lock key {key} in cache region {regionKey}");
            }

            return result;
        }

        public override object LockMany(
                        object[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var cacheKeys = Utilities.GetCacheKeys(keys, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Locking keys {string.Join(",", keys)} in cache region {regionKey}");
            }

            var result = _getLockFallback.Execute(
                            (ctx) =>
                            {
                                return ExecuteLockMany(cacheKeys);
                            },
                            new Dictionary<string, object>
                            {
                                {"random", RandomGenerator.Instance() },
                                {"prev", 0.0d },
                                { "operation", "LockMany"},
                                {"region",$"{regionKey}" }
                            }
                        );

            if (result == null)
            {
                throw new CacheException(
                                $"Could not lock keys {string.Join(",", keys)} in " +
                                $"cache region {regionKey}");
            }

            return result;
        }

        public async override Task<object> LockAsync(
                                        object key,
                                        CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheKey = Utilities.GetCacheKey(key, regionKey);

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Locking key {key} in cache region {regionKey}");
            }

            var result = await _getAsyncLockFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecuteLockAsync(cacheKey, ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"random", RandomGenerator.Instance() },
                                {"prev", 0.0d },
                                { "operation", "LockAsync"},
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken
                        );

            if (result == null)
            {
                throw new CacheException(
                                $"Could not lock key in cache region {regionKey}");
            }

            return result;
        }

        public async override Task<object> LockManyAsync(
                                object[] keys,
                                CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var cacheKeys = Utilities.GetCacheKeys(keys, regionKey);
            LockHandle[] lockHandles = new LockHandle[keys.Length];

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Locking keys {string.Join(",", keys)} in cache region {regionKey}");
            }

            var result = await _getAsyncLockFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecuteLockManyAsync(cacheKeys, ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"random", RandomGenerator.Instance() },
                                {"prev", 0.0d },
                                { "operation", "LockManyAsync"},
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken
                        );

            if (result == null)
            {
                throw new CacheException(
                                $"Could not lock keys in cache region {regionKey}");
            }

            return result;
        }

        public override void Unlock(
                        object key,
                        object lockValue)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheKey = Utilities.GetCacheKey(key, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Unocking key {key} in cache region {regionKey}");
            }

            var result = _setFallback.Execute(
                            (ctx) =>
                            {
                                return ExecuteUnlock(cacheKey);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","Unlock" },
                                {"region",$"{regionKey}" }
                            }
                        );

            if (result == false)
            {
                throw new CacheException(
                    $"Unable to unlock key {key} in cache region {regionKey}");
            }
        }

        public async override Task UnlockAsync(
                            object key,
                            object lockValue,
                            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheKey = Utilities.GetCacheKey(key, regionKey);

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Unocking key {key} in cache region {regionKey}");
            }

            var result = await _setAsyncFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecuteUnlockAsync(cacheKey, ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","UnlockAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken).ConfigureAwait(false);

            if (result == false)
            {
                throw new CacheException(
                    $"Unable to unlock key {key} in cache region {regionKey}");
            }

        }

        public override void UnlockMany(
                        object[] keys,
                        object lockValue)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var cacheKeys = Utilities.GetCacheKeys(keys, regionKey);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Unocking key {string.Join(",", keys)} in cache " +
                    $"region {_regionConfig.RegionName}");
            }

            var result = _setFallback.Execute(
                            (ctx) =>
                            {
                                return ExecuteUnlockMany(cacheKeys);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","UnlockMany" },
                                {"region",$"{regionKey}" }
                            }
                        );

            if (result == false)
            {
                throw new CacheException(
                    $"Unable to unlock keys {string.Join(",", keys)} in " +
                    $"cache region {regionKey}");
            }
        }

        public async override Task UnlockManyAsync(
                            object[] keys,
                            object lockValue,
                            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var cacheKeys = Utilities.GetCacheKeys(keys, regionKey);

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Unocking key {string.Join(",", keys)} in cache " +
                    $"region {regionKey}");
            }

            var result = await _setAsyncFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecuteUnlockManyAsync(cacheKeys, ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","UnlockManyAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken).ConfigureAwait(false);

            if (result == false)
            {
                throw new CacheException(
                    $"Unable to unlock keys {string.Join(",", keys)} " +
                    $"cache region {regionKey}");
            }
        }



        internal virtual void Put(
                        object key,
                        object value,
                        ISessionImplementor session,
                        List<QueryDependencyConfiguration> queryDependencies)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var cacheKey = Utilities.GetCacheKey(
                                            key,
                                            regionKey);


            var cacheItem = Utilities.GetQueryCacheItem(
                                                    cacheKey,
                                                    value,
                                                    regionKey,
                                                    _itemExpiration,
                                                    _useSlidingExpiration,
                                                    _connectionString,
                                                    session,
                                                    queryDependencies);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Inserting object {value} in cache region {regionKey}" +
                    $" with key {cacheKey}");
            }

            var result =
                _setFallback.Execute(
                        (ctx) =>
                        {
                            return ExecutePut(
                                        cacheKey,
                                        cacheItem);
                        },
                        new Dictionary<string, object>
                        {
                            {"operation","Put" },
                            {"region",$"{regionKey}" }
                        }
                    );

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error adding key {cacheKey} with value {value}" +
                        $"into the cache region {regionKey}");
                }
            }
        }

        internal virtual async Task PutAsync(
                        object key,
                        object value,
                        ISessionImplementor session,
                        List<QueryDependencyConfiguration> queryDependencies,
                        CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var cacheKey = Utilities.GetCacheKey(
                                        key, regionKey);
            var cacheItem = Utilities.GetQueryCacheItem(
                                        cacheKey,
                                        value,
                                        regionKey,
                                        _itemExpiration,
                                        _useSlidingExpiration,
                                        _connectionString,
                                        session,
                                        queryDependencies);

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Inserting object {value} in cache region {regionKey} " +
                    $"with key {cacheKey}");
            }

            var result = await _setAsyncFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecutePutAsync(
                                    cacheKey,
                                    cacheItem,
                                    ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","PutAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken
                        ).ConfigureAwait(false);

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error adding key {cacheKey} with value {value}" +
                        $"into the cache region {regionKey}");
                }
            }
        }

        internal virtual void PutMany(
                        object[] keys,
                        object[] values,
                        ISessionImplementor session,
                        List<QueryDependencyConfiguration> queryDependencies)
        {
            if (keys == null ||
                keys.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (values == null ||
                values.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (keys.Length != values.Length)
            {
                throw new ArgumentException(
                            $"{nameof(keys)} and {nameof(values)} must have same " +
                            $"length. Found keys to " +
                            $"have length {keys.Length} and values to " +
                            $"have length {values.Length}.");
            }

            var cacheKeys = Utilities.GetCacheKeys(
                                            keys,
                                            regionKey);
            var cacheValues = Utilities.GetQueryCacheItems(
                                            cacheKeys,
                                            values,
                                            regionKey,
                                            _itemExpiration,
                                            _useSlidingExpiration,
                                            _connectionString,
                                            session,
                                            queryDependencies);

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Inserting objects {string.Join(",", values)} in " +
                    $"cache region {regionKey} " +
                    $"with keys {string.Join(",", cacheKeys)}");
            }

            var result = _setFallback.Execute(
                            (ctx) =>
                            {
                                return ExecutePutMany(
                                            cacheKeys,
                                            cacheValues);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation", "PutMany" },
                                {"region",$"{regionKey}" }
                            }
                        );

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error adding keys {string.Join(",", cacheKeys)} in cache" +
                        $" region {regionKey}");
                }
            }
        }

        internal virtual async Task PutManyAsync(
                        object[] keys,
                        object[] values,
                        ISessionImplementor session,
                        List<QueryDependencyConfiguration> queryDependencies,
                        CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (keys == null ||
                keys.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (values == null ||
                values.Any(x => x == null))
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (keys.Length != values.Length)
            {
                throw new ArgumentException(
                            $"{nameof(keys)} and {nameof(values)} must have same " +
                            $"length. Found keys to " +
                            $"have length {keys.Length} and values to " +
                            $"have length {values.Length}.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var cacheKeys = Utilities.GetCacheKeys(
                                            keys,
                                            regionKey);
            var cacheValues = Utilities.GetQueryCacheItems(
                                            cacheKeys,
                                            values,
                                            regionKey,
                                            _itemExpiration,
                                            _useSlidingExpiration,
                                            _connectionString,
                                            session,
                                            queryDependencies);

            cancellationToken.ThrowIfCancellationRequested();

            if (_logger.IsDebugEnabled())
            {
                _logger.Debug(
                    $"Inserting objects {string.Join(",", values)} in cache " +
                    $"region {regionKey} " +
                    $"with keys {string.Join(",", cacheKeys)}");
            }

            var result = await _setAsyncFallback.ExecuteAsync(
                            (ctx, ct) =>
                            {
                                return ExecutePutManyAsync(
                                            cacheKeys,
                                            cacheValues,
                                            ct);
                            },
                            new Dictionary<string, object>
                            {
                                {"operation","PutManyAsync" },
                                {"region",$"{regionKey}" }
                            },
                            cancellationToken
                        ).ConfigureAwait(false);

            if (result == false)
            {
                if (_logger.IsErrorEnabled())
                {
                    _logger.Error(
                        $"Error adding keys {string.Join(",", cacheKeys)} in cache " +
                        $"region {regionKey}");
                }
            }
        }




        private bool ExecuteClear()
        {
            _cacheHandle.Cache.SearchService
                                    .RemoveByTag(new Tag(regionKey));
            return true;
        }

        private Task<bool> ExecuteClearAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _cacheHandle.Cache.SearchService
                                    .RemoveByTag(new Tag(regionKey));
            return Task.FromResult(true);
        }

        private object ExecuteGet(
                    string cacheKey)
        {
            return _cacheHandle.Cache.Get<object>(cacheKey);

        }

        private Task<object> ExecuteGetAsync(
                    string cacheKey,
                    CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result =
                _cacheHandle.Cache.Get<object>(cacheKey);

            return Task.FromResult(result);

        }

        private object[] ExecuteGetMany(
                    string[] cacheKeys)
        {
            for (int i = 0; i < cacheKeys.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(cacheKeys[i]))
                {
                    cacheKeys[i] = Guid.NewGuid().ToString();
                }
            }

            var result = _cacheHandle.Cache.GetBulk<object>(cacheKeys);

            if (result == null || result.Count == 0)
            {
                return new object[cacheKeys.Length];
            }

            return result.Values.ToArray();
        }

        private Task<object[]> ExecuteGetManyAsync(
                    string[] cacheKeys,
                    CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            for (int i = 0; i < cacheKeys.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(cacheKeys[i]))
                {
                    cacheKeys[i] = Guid.NewGuid().ToString();
                }
            }

            var result = _cacheHandle.Cache.GetBulk<object>(cacheKeys);

            if (result == null || result.Count == 0)
            {
                return Task.FromResult(new object[cacheKeys.Length]);
            }

            return Task.FromResult(result.Values.ToArray());
        }

        private bool ExecutePut(
            string cacheKey,
            CacheItem item)
        {
            _cacheHandle.Cache.Insert(
                cacheKey,
                item);

            return true;
        }

        private async Task<bool> ExecutePutAsync(
            string cacheKey,
            CacheItem item,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _cacheHandle.Cache.InsertAsync(
                                            cacheKey,
                                            item);

            return true;
        }

        private bool ExecutePutMany(
            string[] cacheKeys,
            CacheItem[] items)
        {
            Dictionary<string, CacheItem> cachedItems =
                new Dictionary<string, CacheItem>(cacheKeys.Length);

            for (int i = 0; i < cacheKeys.Length; i++)
            {
                cachedItems.Add(cacheKeys[i], items[i]);
            }

            _cacheHandle.Cache.InsertBulk(cachedItems);

            return true;
        }

        private Task<bool> ExecutePutManyAsync(
            string[] cacheKeys,
            CacheItem[] items,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Dictionary<string, CacheItem> cachedItems =
                new Dictionary<string, CacheItem>(cacheKeys.Length);

            for (int i = 0; i < cacheKeys.Length; i++)
            {
                cachedItems.Add(cacheKeys[i], items[i]);
            }

            cancellationToken.ThrowIfCancellationRequested();

            _cacheHandle.Cache.InsertBulk(cachedItems);

            return Task.FromResult(true);
        }

        private bool ExecuteRemove(
            string cacheKey)
        {
            _cacheHandle.Cache.Remove(
                cacheKey);

            return true;
        }

        private Task<bool> ExecuteRemoveAsync(
            string cacheKey,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _cacheHandle.Cache.Remove(
                cacheKey);

            return Task.FromResult(true);
        }

        private object ExecuteLock(
            string cacheKey)
        {
            return ExecuteLockMany(
                    new string[]
                    {
                        cacheKey
                    });
        }

        private object ExecuteLockMany(
            string[] cacheKeys)
        {
            var lockKeys = cacheKeys.Select(x => LOCKING_USED + x).ToArray();
            var cacheItems = cacheKeys.Select(x => new CacheItem('a')
            {
                Expiration = new Expiration(
                                ExpirationType.Absolute,
                                _regionConfig.LockKeyTimeout),
                Tags = new Tag[] { new Tag(regionKey) }
            }).ToArray();

            var cacheItemsDict = new Dictionary<string, CacheItem>(lockKeys.Length);
            for (int i = 0; i < lockKeys.Length; i++)
            {
                cacheItemsDict.Add(lockKeys[i], cacheItems[i]);
            }

            var failures = _cacheHandle.Cache.AddBulk(cacheItemsDict);

            if (failures != null && failures.Count > 0)
            {
                var successes = lockKeys.Except(failures.Keys, StringComparer.Ordinal);
                _cacheHandle.Cache.RemoveBulk(successes);
                return null;
            }

            return new object();

        }

        private Task<object> ExecuteLockAsync(
            string cacheKey,
            CancellationToken cancellationToken)
        {
            return ExecuteLockManyAsync(
                new string[]
                {
                    cacheKey
                },
                cancellationToken);
        }

        private Task<object> ExecuteLockManyAsync(
            string[] cacheKeys,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lockKeys = cacheKeys.Select(x => LOCKING_USED + x).ToArray();
            var cacheItems = cacheKeys.Select(x => new CacheItem('a')
            {
                Expiration = new Expiration(
                                ExpirationType.Absolute,
                                _regionConfig.LockKeyTimeout),
                Tags = new Tag[] { new Tag(regionKey) }
            }).ToArray();

            var cacheItemsDict = new Dictionary<string, CacheItem>(lockKeys.Length);
            for (int i = 0; i < lockKeys.Length; i++)
            {
                cacheItemsDict.Add(lockKeys[i], cacheItems[i]);
            }

            cancellationToken.ThrowIfCancellationRequested();

            var failures = _cacheHandle.Cache.AddBulk(cacheItemsDict);

            if (failures != null && failures.Count > 0)
            {
                var successes = lockKeys.Except(failures.Keys, StringComparer.Ordinal);
                _cacheHandle.Cache.RemoveBulk(successes);
                return Task.FromResult<object>(null);
            }

            return Task.FromResult(new object());
        }

        private bool ExecuteUnlock(
            string cacheKey)
        {
            _cacheHandle.Cache.Remove(LOCKING_USED + cacheKey);
            return true;
        }

        private async Task<bool> ExecuteUnlockAsync(
            string cacheKey,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _cacheHandle.Cache.RemoveAsync<object>(LOCKING_USED + cacheKey);
            return true;
        }

        private bool ExecuteUnlockMany(
            string[] cacheKeys)
        {
            var lockKeys = cacheKeys.Select(x => LOCKING_USED + x);
            _cacheHandle.Cache.RemoveBulk(lockKeys);
            return true;
        }

        private Task<bool> ExecuteUnlockManyAsync(
            string[] cacheKeys,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var lockKeys = cacheKeys.Select(x => LOCKING_USED + x);
            _cacheHandle.Cache.RemoveBulk(lockKeys);
            return Task.FromResult(true);
        }

        private void AddAdditionalConfig(
            RegionConfig regionConfig,
            IDictionary<string, string> props)
        {
            if (regionConfig.Expiration.HasValue)
            {
                if (_logger.IsDebugEnabled())
                {
                    _logger.Debug(
                        $"New expiration value: " +
                        $"{regionConfig.Expiration.Value.TotalSeconds}s");
                }
                _itemExpiration = regionConfig.Expiration.Value;
            }
            else
            {
                if (props != null)
                {
                    props.TryGetValue(
                                Cfg.Environment.CacheDefaultExpiration,
                                out var expirationString);

                    if (expirationString != null)
                    {
                        try
                        {
                            var seconds = Convert.ToDouble(expirationString);
                            _itemExpiration = TimeSpan.FromSeconds(seconds);

                            if (_logger.IsDebugEnabled())
                            {
                                _logger.Debug(
                                    $"New expiration value: {seconds}s");
                            }
                        }
                        catch (Exception)
                        {
                            if (_logger.IsErrorEnabled())
                            {
                                _logger.Error(
                                    $"Error getting the expiration value from NHibernate " +
                                    $"configuration {expirationString}");
                                throw new ArgumentException(
                                    $"Could not parse {expirationString} as number of seconds");
                            }
                        }
                    }
                }
                else
                {
                    if (_logger.IsDebugEnabled())
                    {
                        _logger.Debug(
                            $"No expiration value given anywhere. Setting no expiration " +
                            $"on items in region {regionKey}");
                    }
                }
            }

            if (regionConfig.UseSlidingExpiration.HasValue)
            {
                _useSlidingExpiration =
                    regionConfig.UseSlidingExpiration.Value;
                if (_logger.IsDebugEnabled())
                {
                    _logger.Debug(
                        $"UseSlidingExpiration:{_useSlidingExpiration}");
                }
            }
            else
            {
                if (props != null)
                {
                    _useSlidingExpiration =
                                    PropertiesHelper.GetBoolean(
                                                "cache.use_sliding_expiration",
                                                props,
                                                false);
                    if (_logger.IsDebugEnabled())
                    {
                        _logger.Debug(
                            $"UseSlidingExpiration:{_useSlidingExpiration}");
                    }
                }
                else
                {
                    if (_logger.IsDebugEnabled())
                    {
                        _logger.Debug(
                            $"UseSlidingExpiration has not been specified anywhere." +
                            $"Defaulting to false");
                    }
                }
            }



            if (props != null)
            {
                props.TryGetValue(
                                Cfg.Environment.ConnectionString,
                                out var connectionString);

                if (connectionString != null)
                {
                    _connectionString = connectionString;
                }
                else
                {
                    props.TryGetValue(
                        Cfg.Environment.ConnectionStringName,
                        out var connectionStringName);

                    ConnectionStringSettings connectionStringSettings = null;
                    if (connectionStringName != null)
                    {
                        connectionStringSettings =
                        ConfigurationManager.ConnectionStrings[connectionStringName];
                    }
                    else
                    {
                        connectionStringSettings =
                            ConfigurationManager.ConnectionStrings[0];
                    }


                    if (connectionStringSettings != null)
                    {
                        _connectionString = connectionStringSettings.ConnectionString;
                    }
                }
            }
            else
            {
                var connectionStringSettings =
                        ConfigurationManager.ConnectionStrings[0];

                if (connectionStringSettings != null)
                {
                    _connectionString = connectionStringSettings.ConnectionString;
                }
            }
        }
    }
}
