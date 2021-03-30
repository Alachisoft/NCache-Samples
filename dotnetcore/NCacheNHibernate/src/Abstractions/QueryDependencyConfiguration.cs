using Alachisoft.NCache.Runtime.Dependencies;

namespace NHibernate.Caches.NCache
{
    public abstract class QueryDependencyConfiguration
    {
        public virtual string QualifiedTableName { get; set; }

        public virtual string RegionPrefix { get; set; } = "nhibernate";

        public virtual DatabaseType DatabaseType { get; set; } =
                                                        DatabaseType.Sql;

        public virtual bool IsPollingDependencyUsed { get; }

        public abstract CacheDependency CreateDependency(
            string connectionString);
    }
}
