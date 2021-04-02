using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;

namespace SampleApp.DatabaseObjects.Oracle
{
    public abstract class OracleDatabaseObject : IAuxiliaryDatabaseObject
    {
        public virtual void AddDialectScope(string dialectName)
        {
        }
        public virtual bool AppliesToDialect(Dialect dialect)
        {
            if (dialect == null)
            {
                return false;
            }

            return dialect is Oracle10gDialect;
        }
        public virtual void SetParameterValues(IDictionary<string, string> parameters)
        {
        }
        public virtual string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return ExecuteSqlCreateString(defaultSchema);
        }
        public virtual string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return ExecuteSqlDropString(defaultSchema);
        }

        public abstract string ExecuteSqlCreateString(
            string defaultSchema);

        public abstract string ExecuteSqlDropString(
            string defaultSchema);
    }
}
