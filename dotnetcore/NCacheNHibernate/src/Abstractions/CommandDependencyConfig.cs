using System;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.Caches.NCache
{
    public abstract class CommandDependencyConfig : DependencyConfig
    {
        public virtual string SQLStatement { get; set; }
        public virtual bool IsStoredProcedure { get; set; } = false;

        public virtual string PrimaryKeyInputParameter { get; set; }
        public virtual string PrimaryKeyDbType { get; set; }

        public virtual Dictionary<string, string> OutputParametersAndDbTypes { get; set; }
            = new Dictionary<string, string>();

        protected virtual string GetCommand(char inserter)
        {
            if (string.IsNullOrWhiteSpace(SQLStatement))
            {
                throw new ArgumentNullException($"SQL statement can't be null or empty");
            }

            if (!IsStoredProcedure)
            {
                if (SQLStatement.Contains('?'))
                {
                    if (SQLStatement.Count(f => f == '?') > 1)
                    {
                        throw new ArgumentException($"There can only be one occurrence of the " +
                            $"primary key placeholder '?'");
                    }

                    if (string.IsNullOrWhiteSpace(PrimaryKeyInputParameter))
                    {
                        throw new ArgumentNullException(
                            $"For dependencies with parameters, the " +
                            $"{nameof(PrimaryKeyInputParameter)} value can't be null or empty");
                    }

                    return SQLStatement.Replace("?",
                        $"{inserter}{PrimaryKeyInputParameter.Trim()}");
                }
                else
                {
                    return SQLStatement;
                }
            }
            else
            {
                return SQLStatement;
            }
        }

    }
}
