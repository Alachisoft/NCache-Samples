using Alachisoft.NCache.Runtime.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.Caches.NCache
{
    public class QueryCommandDependencyConfiguration : QueryDependencyConfiguration
    {
        public string[] KeyColumnNames { get; set; }

        public override bool IsPollingDependencyUsed => false;

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

            var other = obj as QueryCommandDependencyConfiguration;

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

            if (KeyColumnNames == null && other.KeyColumnNames != null)
            {
                return false;
            }

            if (KeyColumnNames != null && other.KeyColumnNames == null)
            {
                return false;
            }

            var keyColumn1 =
                new HashSet<string>(KeyColumnNames).Select(x => x.Trim()).ToArray();
            var keyColumn2 =
                new HashSet<string>(other.KeyColumnNames).Select(x => x.Trim()).ToArray();

            if (keyColumn1.Length != keyColumn2.Length)
            {
                return false;
            }

            foreach (var keyColumnName in keyColumn1)
            {
                if (!keyColumn2.Contains(
                                    keyColumnName))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(QualifiedTableName) ||
                KeyColumnNames == null ||
                KeyColumnNames.Length == 0 ||
                KeyColumnNames.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                return base.GetHashCode();
            }

            var column1 = new HashSet<string>(KeyColumnNames).ToArray();

            var columnList = column1.Select(x => x.Trim()).ToList();

            columnList.Sort();



            string result =
                       $"{QualifiedTableName} | " +
                       $"{RegionPrefix} | " +
                       $"{string.Join(",", columnList.ToArray())} | " +
                       $"{DatabaseType} | " +
                       $"False";

            return result.GetHashCode();
        }

        public override CacheDependency CreateDependency(
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(QualifiedTableName))
            {
                throw new Exception(nameof(QualifiedTableName));
            }

            if (KeyColumnNames == null ||
                KeyColumnNames.Length == 0)
            {
                throw new Exception(
                    $"ColumnNames array can not be null or empty");
            }

            foreach (var keyColumnName in KeyColumnNames)
            {
                if (string.IsNullOrWhiteSpace(keyColumnName))
                {
                    throw new Exception(
                    $"ColumnNames array can not have empty key column names");
                }
            }

            var keyColumnNames = new HashSet<string>(KeyColumnNames).ToArray();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            keyColumnNames = keyColumnNames.Select(x => x.Trim()).ToArray();

            var sqlStatement =
                $"SELECT {string.Join(", ", keyColumnNames)} " +
                $"FROM {QualifiedTableName}";

            if (DatabaseType == DatabaseType.Sql)
            {
                return new SqlCacheDependency(
                                        connectionString,
                                        sqlStatement);
            }
            else
            {
                return new OracleCacheDependency(
                                        connectionString,
                                        sqlStatement);
            }
        }
    }
}
