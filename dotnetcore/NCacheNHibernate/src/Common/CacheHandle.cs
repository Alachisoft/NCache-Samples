using Alachisoft.NCache.Client;

namespace NHibernate.Caches.NCache
{
    public class CacheHandle
    {
        internal readonly ICache Cache;
        internal readonly CacheConfig CacheConfig;

        public CacheHandle(
            CacheConfig cacheConfig)
        {
            CacheConfig = cacheConfig;
            Cache = Utilities.CreateCache(
                                cacheConfig.CacheId,
                                cacheConfig.connectionOptions);
        }
    }
}
