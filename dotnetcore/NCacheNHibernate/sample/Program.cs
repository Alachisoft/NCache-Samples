using NHibernate;
using NHibernate.Logging.Serilog;
using SampleApp.DatabaseSeed;
using SampleApp.NHibernateHelpers;
using Serilog;
using System;
using System.Configuration;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.AppSettings()
                            .CreateLogger();
            NHibernateLogger.SetLoggersFactory(new SerilogLoggerFactory());

            NHibernateHelper nhibernateHelper = null;

            try
            {
                var databaseType =
                        ConfigurationManager.AppSettings["database_type"];


                if (string.IsNullOrEmpty(databaseType))
                {
                    databaseType = "sql";
                }
                else
                {
                    databaseType = databaseType.ToLowerInvariant();
                }

                var connectionString =
                    ConfigurationManager.ConnectionStrings[databaseType]
                                                                .ConnectionString;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ConfigurationErrorsException(
                            "connection string can't be null");
                }

                if (databaseType == "sql")
                {
                    nhibernateHelper = new NHibernateHelperSQL(connectionString);
                }
                else if (databaseType == "oracle")
                {
                    nhibernateHelper = new NHibernateHelperOracle(connectionString);
                }
                else
                {
                    throw new ConfigurationErrorsException(
                        "databaseType must be either 'sql' or 'oracle'-case insensitive");
                }

                nhibernateHelper.GetSessionFactory();

                SeedData.Initialize(nhibernateHelper);
                var nhibernateRunner = new NHibernate(nhibernateHelper);
                nhibernateRunner.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (nhibernateHelper != null)
                {
                    nhibernateHelper.CloseSessionFactory();
                }
            }

        }
    }
}
