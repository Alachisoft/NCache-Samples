using Alachisoft.NCache.Runtime.Dependencies;
using System;

namespace NHibernate.Caches.NCache
{
    public class QueryTableDependencyConfiguration : QueryDependencyConfiguration
    {
        public override bool IsPollingDependencyUsed => true;

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as QueryTableDependencyConfiguration;

            if (other == null)
            {
                return false;
            }

            if (!QualifiedTableName.Equals(
                                        other.QualifiedTableName,
                                            StringComparison.Ordinal))
            {
                return false;
            }

            if (!RegionPrefix.Equals(
                                other.RegionPrefix,
                                            StringComparison.Ordinal))
            {
                return false;
            }

            if (IsPollingDependencyUsed != other.IsPollingDependencyUsed)
            {
                return false;
            }

            if (DatabaseType != other.DatabaseType)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(QualifiedTableName))
            {
                return base.GetHashCode();
            }

            string result =
                       $"{QualifiedTableName} | " +
                       $"{RegionPrefix} | " +
                       $"{DatabaseType} | " +
                       $"True";

            return result.GetHashCode();
        }

        public override CacheDependency CreateDependency(
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(QualifiedTableName))
            {
                throw new Exception(nameof(QualifiedTableName));
            }

            if (DatabaseType == DatabaseType.Sql)
            {
                return DBDependencyFactory.CreateSqlCacheDependency(
                    connectionString,
                    $"{QualifiedTableName}:ALL");
            }
            else
            {
                var connString = $"Provider=OraOLEDB.Oracle;{connectionString}";
                return DBDependencyFactory.CreateOleDbCacheDependency(
                        connString,
                        $"{QualifiedTableName}:ALL");
            }
        }
    }
}
