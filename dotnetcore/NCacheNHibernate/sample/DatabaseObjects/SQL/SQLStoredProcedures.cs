namespace SampleApp.DatabaseObjects.SQL
{
    public class CustomerCountryByID_SP : SQLDatabaseObject
    {
        public override string ExecuteSqlCreateString(string defaultSchema)
        {
            return
                $"SET ANSI_NULLS ON\n " +
                $"GO\n " +
                $"SET QUOTED_IDENTIFIER ON\n " +
                $"GO\n " +
                $"CREATE PROCEDURE [{defaultSchema}].[GetCountryByCustomerID]\n " +
                $"@customerId nvarchar(255)\n " +
                $"AS\n " +
                $"SELECT Country\n " +
                $"FROM [{defaultSchema}].[Customers]\n" +
                $"WHERE CustomerID = @customerId;\n " +
                $"GO\n";
        }

        public override string ExecuteSqlDropString(string defaultSchema)
        {
            return
                $"DROP PROCEDURE [{defaultSchema}].[GetCountryByCustomerID]";
        }
    }

    public class CustomerContactNameAndPhoneByID_SP : SQLDatabaseObject
    {
        public override string ExecuteSqlCreateString(string defaultSchema)
        {
            return 
                $"SET ANSI_NULLS ON\n" +
                $"GO\n" +
                $"SET QUOTED_IDENTIFIER ON" +
                $"\nGO\n" +
                $"CREATE PROCEDURE[{defaultSchema}].[GetContactNameAndPhoneByCustomerID]" +
                $"\n\t@cId nvarchar(255)," +
                $"\n\t@contactName nvarchar(255) OUT," +
                $"\n\t@phone nvarchar(255) OUT" +
                $"\nAS\n" +
                $"SELECT @contactName = ContactName, @phone = Phone\n" +
                $"FROM[{defaultSchema}].[Customers]\n" +
                $"WHERE CustomerID = @cId;\n" +
                $"GO";
    }

        public override string ExecuteSqlDropString(string defaultSchema)
        {
            return $"DROP PROCEDURE [{defaultSchema}].[GetContactNameAndPhoneByCustomerID]";
        }
    }
}
