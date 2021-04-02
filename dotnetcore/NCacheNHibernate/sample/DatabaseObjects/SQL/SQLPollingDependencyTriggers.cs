namespace SampleApp.DatabaseObjects.SQL
{
    public class SQLProductPollingDependencyTrigger : SQLDatabaseObject
    {
        public override string ExecuteSqlCreateString(string defaultSchema)
        {
            return
                $"SET ANSI_NULLS ON\n  " +
                $"GO\n " +
                $"SET QUOTED_IDENTIFIER ON\n  " +
                $"GO\n  " +
                $"CREATE TRIGGER [{defaultSchema}].[ProductsPolling]\n " +
                $"ON [{defaultSchema}].[Products]\n " +
                $"AFTER DELETE,UPDATE\n " +
                $"AS\n " +
                $"BEGIN\n " +
                $"\tSET NOCOUNT ON\n " +
                $"\tUPDATE [{defaultSchema}].[ncache_db_sync]\n " +
                $"\tSET modified=1\n " +
                $"\tFROM [{defaultSchema}].[ncache_db_sync]\n " +
                $"\tINNER JOIN deleted old\n " +
                $"\tON cache_key = '{defaultSchema}.[Products]:ProductID#' + " +
                    $"Cast((old.ProductID) AS VARCHAR)\n " +
                $"END\n " +
                $"GO\n " +
                $"ENABLE TRIGGER [{defaultSchema}].[ProductsPolling] ON " +
                    $"[{defaultSchema}].[Products]\n " +
                $"GO\n ";
        }

        public override string ExecuteSqlDropString(string defaultSchema)
        {
            return
                $"DROP TRIGGER IF EXISTS [{defaultSchema}].[ProductsPolling]\n " +
                $"GO\n ";
        }
    }

    public class SQLEmployeePollingDependencyTrigger : SQLDatabaseObject
    {
        public override string ExecuteSqlCreateString(string defaultSchema)
        {
            return 
            $"SET ANSI_NULLS ON\n " +
            $"GO\n " +
            $"SET QUOTED_IDENTIFIER ON\n " +
            $"GO\n " +
            $"CREATE TRIGGER [{defaultSchema}].[EmployeesPolling]\n " +
            $"ON [{defaultSchema}].[Employees]\n " +
            $"AFTER DELETE,UPDATE\n " +
            $"AS\n " +
            $"BEGIN\n " +
            $"\tSET NOCOUNT ON\n " +
            $"\tUPDATE [{defaultSchema}].[ncache_db_sync]\n " +
            $"\tSET modified=1\n " +
            $"\tWHERE cache_key = '{defaultSchema}.[Employees]:ALL'\n" +
            $"END\n " +
            $"GO\n " +
            $"ENABLE TRIGGER [{defaultSchema}].[EmployeesPolling] ON " +
                    $"[{defaultSchema}].[Employees]\n " +
            $"GO\n ";
        }

        public override string ExecuteSqlDropString(string defaultSchema)
        {
            return
                $"DROP TRIGGER IF EXISTS [{defaultSchema}].[EmployeesPolling]\n " +
                $"GO\n";
        }
    }


}
