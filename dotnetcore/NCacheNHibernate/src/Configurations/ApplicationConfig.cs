using System.Collections.Generic;
using System.Linq;

namespace NHibernate.Caches.NCache
{
    internal class ApplicationConfig
    {
        public Dictionary<string, RegionConfig> CacheRegions { get; } =
            new Dictionary<string, RegionConfig>();

        public Dictionary<string, CacheConfig> CacheConfigurations { get; } =
            new Dictionary<string, CacheConfig>();


        internal HashSet<QueryDependencyConfiguration> QueryDependencies { get; } =
            new HashSet<QueryDependencyConfiguration>(
                                new QueryDependencyConfigComparer());


        internal HashSet<DependencyConfig> EntityDependencies { get; } =
            new HashSet<DependencyConfig>(new DependecyConfigEqualityComparer());
    }
}
