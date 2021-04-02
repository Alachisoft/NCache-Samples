using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHibernate.Caches.NCache
{
    public class NCacheQueryCache : StandardQueryCache, IQueryCache, IBatchableQueryCache
    {
        private static readonly INHibernateLogger Log = 
                                    NHibernateLogger.For(typeof(NCacheQueryCache));

        private readonly NCacheClient _ncacheClient;

        public NCacheQueryCache(
                        UpdateTimestampsCache updateTimestampsCache,
                        CacheBase regionCache) 
                            : base(updateTimestampsCache,
                                   regionCache)
        {
            _ncacheClient = regionCache as NCacheClient;

            if (_ncacheClient == null)
            {
                throw new ArgumentException(
                    $"{nameof(regionCache)} should be an instance of NCacheClient which " +
                    $"requires use of the NCacheProvider ICacheProvider implementation",
                    nameof(regionCache));
            }
        }

        public new void Clear()
        {
            _ncacheClient.Clear();
        }

        public new Task ClearAsync(CancellationToken cancellationToken)
        {
            return _ncacheClient.ClearAsync(
                                    cancellationToken);
        }

        public new void Destroy()
        {
            _ncacheClient.Destroy();
        }

        public new IList Get(
                            QueryKey key,
                            QueryParameters queryParameters,
                            ICacheAssembler[] returnTypes,
                            ISet<string> spaces,
                            ISessionImplementor session)
        {
            return base.Get(
                            key,
                            queryParameters,
                            returnTypes,
                            spaces,
                            session);
        }

        [Obsolete]
        public new IList Get(
                           QueryKey key,
                           ICacheAssembler[] returnTypes,
                           bool isNaturalKeyLookup,
                           ISet<string> spaces,
                           ISessionImplementor session)
        {
            return base.Get(
                            key,
                            returnTypes,
                            isNaturalKeyLookup,
                            spaces,
                            session);
        }

        public new Task<IList> GetAsync(
                                QueryKey key, 
                                QueryParameters queryParameters, 
                                ICacheAssembler[] returnTypes, 
                                ISet<string> spaces, 
                                ISessionImplementor session, 
                                CancellationToken cancellationToken)
        {
            return base.GetAsync(
                                key,
                                queryParameters,
                                returnTypes,
                                spaces,
                                session,
                                cancellationToken);
        }

        [Obsolete]
        public new Task<IList> GetAsync(
                                    QueryKey key,
                                    ICacheAssembler[] returnTypes,
                                    bool isNaturalKeyLookup,
                                    ISet<string> spaces,
                                    ISessionImplementor session,
                                    CancellationToken cancellationToken)
        {
            return base.GetAsync(
                                key,
                                returnTypes,
                                isNaturalKeyLookup,
                                spaces,
                                session,
                                cancellationToken);
        }

        public new IList[] GetMany(
                            QueryKey[] keys,
                            QueryParameters[] queryParameters,
                            ICacheAssembler[][] returnTypes,
                            ISet<string>[] spaces,
                            ISessionImplementor session)
        {
            return base.GetMany(
                             keys,
                             queryParameters,
                             returnTypes,
                             spaces,
                             session);
        }

        public new Task<IList[]> GetManyAsync(
                                        QueryKey[] keys,
                                        QueryParameters[] queryParameters,
                                        ICacheAssembler[][] returnTypes,
                                        ISet<string>[] spaces,
                                        ISessionImplementor session,
                                        CancellationToken cancellationToken)
        {
            return base.GetManyAsync(
                                    keys,
                                    queryParameters,
                                    returnTypes,
                                    spaces,
                                    session,
                                    cancellationToken);
        }

        public new bool Put(
                         QueryKey key,
                         QueryParameters queryParameters,
                         ICacheAssembler[] returnTypes,
                         IList result,
                         ISessionImplementor session)
        {
            if (queryParameters.NaturalKeyLookup &&
                result.Count == 0)
            {
                return false;
            }

            var ts = session.Factory.Settings.CacheProvider.NextTimestamp();

            if (Log.IsDebugEnabled())
            {
                Log.Debug(
                    "caching query results in region: '{0}'; {1}",
                    RegionName,
                    key);
            }

            var queryDependencies = 
                NCacheProvider.GetQueryDependencyConfigs(_ncacheClient.RegionPrefix);

            _ncacheClient.Put(
                   key,
                   GetCacheableResult(
                                returnTypes,
                                session,
                                result,
                                ts),
                   session,
                   queryDependencies);

            return true;
        }

        public new async Task<bool> PutAsync(
                                    QueryKey key,
                                    QueryParameters queryParameters,
                                    ICacheAssembler[] returnTypes,
                                    IList result,
                                    ISessionImplementor session,
                                    CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (queryParameters.NaturalKeyLookup &&
                result.Count == 0)
            {
                return false;
            }

            var ts = session.Factory.Settings.CacheProvider.NextTimestamp();

            if (Log.IsDebugEnabled())
            {
                Log.Debug(
                    "caching query results in region: '{0}'; {1}",
                    RegionName,
                    key);
            }

            var value = await GetCacheableResultAsync(
                                                returnTypes,
                                                session,
                                                result,
                                                ts,
                                                cancellationToken)
                                        .ConfigureAwait(false);

            var queryDependencies = 
                NCacheProvider.GetQueryDependencyConfigs(_ncacheClient.RegionPrefix);

            await _ncacheClient.PutAsync(
                                        key,
                                        value,
                                        session,
                                        queryDependencies,
                                        cancellationToken)
                                    .ConfigureAwait(false);

            return true;
        }

        public new bool[] PutMany(
                                QueryKey[] keys,
                                QueryParameters[] queryParameters,
                                ICacheAssembler[][] returnTypes,
                                IList[] results,
                                ISessionImplementor session)
        {
            if (Log.IsDebugEnabled())
            {
                Log.Debug(
                    "caching query results in region: '{0}'; {1}",
                    RegionName,
                    StringHelper.CollectionToString(keys));
            }

            var cached = new bool[keys.Length];

            var ts = session.Factory.Settings.CacheProvider.NextTimestamp();

            var cachedKeys = new List<object>();

            var cachedResults = new List<object>();

            for (var i = 0; i < keys.Length; i++)
            {
                var result = results[i];
                if (queryParameters[i].NaturalKeyLookup &&
                    result.Count == 0)
                {
                    continue;
                }

                cached[i] = true;
                cachedKeys.Add(keys[i]);

                cachedResults.Add(
                       GetCacheableResult(
                                       returnTypes[i],
                                       session,
                                       result,
                                       ts));
            }

            var queryDependencies = 
                NCacheProvider.GetQueryDependencyConfigs(_ncacheClient.RegionPrefix);

            _ncacheClient.PutMany(
                            cachedKeys.ToArray(),
                            cachedResults.ToArray(),
                            session,
                            queryDependencies);

            return cached;
        }

        public new async Task<bool[]> PutManyAsync(
                                                QueryKey[] keys,
                                                QueryParameters[] queryParameters,
                                                ICacheAssembler[][] returnTypes,
                                                IList[] results,
                                                ISessionImplementor session,
                                                CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (Log.IsDebugEnabled())
            {
                Log.Debug(
                    "caching query results in region: '{0}'; {1}",
                    RegionName,
                    StringHelper.CollectionToString(keys));
            }
                

            var cached = new bool[keys.Length];

            var ts = session.Factory.Settings.CacheProvider.NextTimestamp();

            var cachedKeys = new List<object>();
            var cachedResults = new List<object>();

            for (var i = 0; i < keys.Length; i++)
            {
                var result = results[i];

                if (queryParameters[i].NaturalKeyLookup &&
                    result.Count == 0)
                {
                    continue;
                }

                cached[i] = true;

                cachedKeys.Add(keys[i]);

                cachedResults.Add(
                       await GetCacheableResultAsync(
                                               returnTypes[i],
                                               session,
                                               result,
                                               ts,
                                               cancellationToken)
                                        .ConfigureAwait(false));
            }

            var queryDependencies = 
                NCacheProvider.GetQueryDependencyConfigs(_ncacheClient.RegionPrefix);

            await _ncacheClient.PutManyAsync(
                                        cachedKeys.ToArray(),
                                        cachedResults.ToArray(),
                                        session,
                                        queryDependencies,
                                        cancellationToken)
                                .ConfigureAwait(false);

            return cached;
        }



        private static List<object> GetCacheableResult(
                                                ICacheAssembler[] returnTypes,
                                                ISessionImplementor session,
                                                IList result,
                                                long ts)
        {
            var cacheable = new List<object>(result.Count + 1) { ts };

            foreach (var row in result)
            {
                if (returnTypes.Length == 1)
                {
                    cacheable.Add(
                                returnTypes[0].Disassemble(
                                                        row,
                                                        session,
                                                        null));
                }
                else
                {
                    cacheable.Add(TypeHelper.Disassemble(
                                                        (object[])row,
                                                        returnTypes,
                                                        null,
                                                        session,
                                                        null));
                }
            }

            return cacheable;
        }

        private static async Task<List<object>> GetCacheableResultAsync(
                                                        ICacheAssembler[] returnTypes,
                                                        ISessionImplementor session,
                                                        IList result,
                                                        long ts, 
                                                        CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cacheable = new List<object>(result.Count + 1) { ts };

            foreach (var row in result)
            {
                if (returnTypes.Length == 1)
                {
                    cacheable.Add(
                                await returnTypes[0].DisassembleAsync(
                                                                    row, 
                                                                    session,
                                                                    null,
                                                                    cancellationToken)
                                                           .ConfigureAwait(false));
                }
                else
                {
                    cacheable.Add(
                               await TypeHelper.DisassembleAsync(
                                                            (object[])row,
                                                            returnTypes,
                                                            null,
                                                            session,
                                                            null,
                                                            cancellationToken)
                                                    .ConfigureAwait(false));
                }
            }

            return cacheable;
        }
    }
}
