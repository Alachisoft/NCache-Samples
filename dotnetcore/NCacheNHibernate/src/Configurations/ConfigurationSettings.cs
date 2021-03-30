using System.Collections.Generic;
using System.Linq;

namespace NHibernate.Caches.NCache
{
    public class ConfigurationSettings
    {
        public List<RegionConfig> CacheRegions { get; set; } =
            new List<RegionConfig>();

        public List<CacheConfig> CacheConfigs { get; set; } =
            new List<CacheConfig>();

        internal HashSet<QueryDependencyConfiguration> QueryDependencies { get; set; } =
            new HashSet<QueryDependencyConfiguration>(
                                new QueryDependencyConfigComparer());

        public List<QueryCommandDependencyConfiguration> CommandDependencies
        {
            get
            {
                return QueryDependencies
                        .Where(x => x.IsPollingDependencyUsed == false)
                        .Select(x => x as QueryCommandDependencyConfiguration)
                        .ToList();
            }
            set
            {
                if (value != null)
                {
                    foreach (var el in value)
                    {
                        QueryDependencies.Add(el);
                    }
                }
            }
        }

        public List<QueryTableDependencyConfiguration> TableDependencies
        {
            get
            {
                return QueryDependencies
                        .Where(x => x.IsPollingDependencyUsed == true)
                        .Select(x => x as QueryTableDependencyConfiguration)
                        .ToList();
            }
            set
            {
                if (value != null)
                {
                    foreach (var el in value)
                    {
                        QueryDependencies.Add(el);
                    }
                }
            }
        }

        internal HashSet<DependencyConfig> EntityDependencies { get; } =
            new HashSet<DependencyConfig>(new DependecyConfigEqualityComparer());

        public List<SQLDependencyConfig> SqlDependencies
        {
            get
            {
                return EntityDependencies
                            .Where(
                                x => x.DatabaseDependencyType == DatabaseDependencyType.Sql)
                            .Select(x => x as SQLDependencyConfig)
                            .ToList();
            }
            set
            {
                if (value != null)
                {
                    foreach (var el in value)
                    {
                        EntityDependencies.Add(el);
                    }
                }
            }
        }

        public List<OracleDependencyConfig> OracleDependencies
        {
            get
            {
                return EntityDependencies
                            .Where(
                                x => x.DatabaseDependencyType == DatabaseDependencyType.Oracle)
                            .Select(x => x as OracleDependencyConfig)
                            .ToList();
            }
            set
            {
                if (value != null)
                {
                    foreach (var el in value)
                    {
                        EntityDependencies.Add(el);
                    }
                }
            }
        }

        public List<OleDbDependencyConfig> OledbDependencies
        {
            get
            {
                return EntityDependencies
                            .Where(
                                x => x.DatabaseDependencyType == DatabaseDependencyType.Oledb)
                            .Select(x => x as OleDbDependencyConfig)
                            .ToList();
            }
            set
            {
                if (value != null)
                {
                    foreach (var el in value)
                    {
                        EntityDependencies.Add(el);
                    }
                }
            }
        }
    }
}
