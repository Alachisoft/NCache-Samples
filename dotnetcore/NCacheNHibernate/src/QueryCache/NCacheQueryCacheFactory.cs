using NHibernate.Cache;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace NHibernate.Caches.NCache
{
    public class NCacheQueryCacheFactory : IQueryCacheFactory
    {

        public IQueryCache GetQueryCache(
                                        UpdateTimestampsCache updateTimestampsCache,
                                        IDictionary<string, string> props,
                                        CacheBase regionCache)
        {
            return new NCacheQueryCache(
                                updateTimestampsCache,
                                regionCache);
        }

        [Obsolete]
        public IQueryCache GetQueryCache(
                                        string regionName,
                                        UpdateTimestampsCache updateTimestampsCache,
                                        Settings settings,
                                        IDictionary<string, string> props)
        {
            throw new NotImplementedException();
        }
    }
}
