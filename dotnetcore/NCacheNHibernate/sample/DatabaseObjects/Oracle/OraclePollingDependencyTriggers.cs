using System;

namespace SampleApp.DatabaseObjects.Oracle
{
    public class OracleProductPollingDependencyTrigger : OracleDatabaseObject
    {
        public override string ExecuteSqlCreateString(string defaultSchema)
        {
            return
                $"CREATE OR REPLACE TRIGGER {defaultSchema}.\"ProductsPolling\"\n " +
                $"AFTER DELETE OR UPDATE\n" +
                $"ON\n" +
                $"\t{defaultSchema}.\"Products\"\n" +
                $"FOR EACH ROW\n" +
                $"DECLARE\n" +
                $"\tpid {defaultSchema}.\"Products\".\"ProductID\"%TYPE;\n" +
                $"BEGIN\n" +
                $"\tpid := :old.\"ProductID\";\n" +
                $"\tUPDATE {defaultSchema}.NCACHE_DB_SYNC\n" +
                $"\tSET MODIFIED = 1\n" +
                $"\tWHERE CACHE_KEY =" +
                $" '{defaultSchema}.\"Products\":ProductID#' || " +
                    $"TO_CHAR(pid);\n" +
                $"END \"ProductsPolling\";";
        }

        public override string ExecuteSqlDropString(string defaultSchema)
        {
            return
                $"DROP TRIGGER {defaultSchema}.\"ProductsPolling\"";
        }
    }


    public class OracleEmployeePollingDependencyTrigger : OracleDatabaseObject
    {
        public override string ExecuteSqlCreateString(string defaultSchema)
        {
            return
                $"CREATE OR REPLACE TRIGGER {defaultSchema}.\"EmployeesPolling\"\n " +
                $"AFTER DELETE OR UPDATE\n" +
                $"ON {defaultSchema}.\"Employees\"\n" +
                $"DECLARE\n" +
                $"BEGIN\n" +
                $"\tUPDATE {defaultSchema}.NCACHE_DB_SYNC\n" +
                $"\tSET MODIFIED =1\n" +
                $"\tWHERE CACHE_KEY = '{defaultSchema}.\"Employees\":ALL';\n" +
                $"END \"EmployeesPolling\";";
        }

        public override string ExecuteSqlDropString(string defaultSchema)
        {
            return
                $"DROP TRIGGER {defaultSchema}.\"EmployeesPolling\";";
        }
    }

}
