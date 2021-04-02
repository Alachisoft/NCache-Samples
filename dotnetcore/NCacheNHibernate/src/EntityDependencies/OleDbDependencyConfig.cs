using Alachisoft.NCache.Runtime.Dependencies;
using System;

namespace NHibernate.Caches.NCache
{
    public class OleDbDependencyConfig : DependencyConfig
    {
        public override DatabaseDependencyType DatabaseDependencyType =>
            DatabaseDependencyType.Oledb;

        public string QualifiedTableName { get; set; }

        public string DbCacheKey { get; set; }

        public DatabaseType DatabaseType { get; set; } = DatabaseType.Sql;

        public override CacheDependency GetCacheDependency(string connectionString, object key)
        {
            if (string.IsNullOrWhiteSpace(DbCacheKey))
            {
                throw new ArgumentException("DbCacheKey can not be empty");
            }

            if (string.IsNullOrWhiteSpace(QualifiedTableName))
            {
                throw new ArgumentException("Qualified table name cannot be null or empty");
            }

            if (!DbCacheKey.Contains("[en]"))
            {
                throw new ArgumentException("DbCacheKey should have [en] placeholder for the " +
                    "qualified table name");
            }

            int occurrencesOfEN = CountSubStrings(DbCacheKey, "[en]");

            if (occurrencesOfEN > 1)
            {
                throw new ArgumentException(
                    "DbCacheKey can only have one occurrence of [en] substring");
            }

            var dbCacheKey = DbCacheKey.Replace("[en]", QualifiedTableName);

            if (DbCacheKey.Contains("[pk]"))
            {
                int occurrencesOfPK = CountSubStrings(DbCacheKey, "[pk]");

                if (occurrencesOfPK > 1)
                {
                    throw new ArgumentException(
                        "DbCacheKey can only have one occurrence of [pk] substring");
                }

                if (!ValidateKey(key))
                {
                    if (key == null)
                    {
                        throw new ArgumentException($"Key cannot be null");
                    }

                    throw new ArgumentOutOfRangeException(
                        $"Key type {key.GetType().FullName} not supported");
                }

                dbCacheKey = dbCacheKey.Replace("[pk]", key.ToString());
            }

            if (DatabaseType == DatabaseType.Sql)
            {
                return DBDependencyFactory.CreateSqlCacheDependency(
                        connectionString,
                        dbCacheKey);
            }
            else
            {
                var connString = $"Provider=OraOLEDB.Oracle;{connectionString}";
                return DBDependencyFactory.CreateOleDbCacheDependency(
                        connString,
                        dbCacheKey);
            }



        }

        private static int CountSubStrings(string text, string pattern)
        {
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
    }
}
