namespace SampleApp.DatabaseObjects.Oracle
{
    public class CustomerCountryByID_SP : OracleDatabaseObject
    {
        public override string ExecuteSqlCreateString(string defaultSchema)
        {
            return
                $"CREATE OR REPLACE PROCEDURE {defaultSchema}.GETCOUNTRYBYCUSTOMERID(\n" +
                $"\tCUSTOMERID IN VARCHAR2,\n" +
                $"\tCOUNTRY OUT VARCHAR2)\n " +
                $"AS\n" +
                $"BEGIN\n" +
                $"\tSELECT {defaultSchema}.\"Customers\".\"Country\"\n" +
                $"\tINTO COUNTRY\n" +
                $"\tFROM {defaultSchema}.\"Customers\"\n" +
                $"\tWHERE {defaultSchema}.\"Customers\".\"CustomerID\" = CUSTOMERID;\n" +
                $"END;\n";
        }

        public override string ExecuteSqlDropString(string defaultSchema)
        {
            return
                $"DROP PROCEDURE {defaultSchema}.GETCOUNTRYBYCUSTOMERID;";
        }
    }
}
