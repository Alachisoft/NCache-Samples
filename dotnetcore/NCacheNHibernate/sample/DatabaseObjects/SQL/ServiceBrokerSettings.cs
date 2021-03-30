using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using System.Collections.Generic;

namespace SampleApp.DatabaseObjects.SQL
{
    public class ServiceBrokerSettings:IAuxiliaryDatabaseObject
    {
        public ServiceBrokerSettings()
        {
        }

        public void AddDialectScope(string dialectName)
        {
        }

        public bool AppliesToDialect(Dialect dialect)
        {
            if (dialect == null)
            {
                return false;
            }

            return dialect is MsSql2012Dialect ||
                   dialect is MsSql2008Dialect ||
                   dialect is MsSql2005Dialect;
        }

        public void SetParameterValues(IDictionary<string, string> parameters)
        {
        }

        public string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return
                $"ALTER DATABASE {defaultCatalog} SET ENABLE_BROKER " +
                        $"WITH ROLLBACK IMMEDIATE;  \n" +
                $"GO\n" +
                $"ALTER DATABASE {defaultCatalog} SET ANSI_NULLS ON\n" +
                $"GO\n" +
                $"ALTER DATABASE {defaultCatalog} SET ANSI_PADDING ON\n" +
                $"GO\n" +
                $"ALTER DATABASE {defaultCatalog} SET ANSI_WARNINGS ON\n" +
                $"GO\n" +
                $"ALTER DATABASE {defaultCatalog} SET CONCAT_NULL_YIELDS_NULL ON\n" +
                $"GO\n" +
                $"ALTER DATABASE {defaultCatalog} SET QUOTED_IDENTIFIER ON\n" +
                $"GO\n" +
                $"ALTER DATABASE {defaultCatalog} SET NUMERIC_ROUNDABORT OFF\n" +
                $"GO\n" +
                $"ALTER DATABASE {defaultCatalog} SET ARITHABORT ON\n" +
                $"GO\n";
        }

        public string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return
                $"ALTER DATABASE {defaultCatalog} SET DISABLE_BROKER;";
        }
    }
}
