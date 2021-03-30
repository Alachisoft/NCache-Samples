using NHibernate;
using NHibernate.Caches.NCache;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Sample.CustomerService.Maps;
using SampleApp.DatabaseObjects.SQL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SampleApp.NHibernateHelpers
{
    public sealed class NHibernateHelperSQL : NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;
        private static readonly object _locker = new object();

        private readonly string _connectionString;
        private readonly string _database;

        public NHibernateHelperSQL(
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;

            _database = new SqlConnection(connectionString).Database;
        }

        public override ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                lock (_locker)
                {
                    if (_sessionFactory == null)
                    {

                        var config = new Configuration();

                        config.Mappings(map =>
                        {
                            map.DefaultCatalog = _database;
                            map.DefaultSchema = "dbo";
                        });

                        config.DataBaseIntegration(db =>
                        {
                            db.LogFormattedSql = false;
                            db.LogSqlInConsole = false;
                            db.ConnectionString = _connectionString;
                            db.Timeout = 10;
                            db.Dialect<MsSql2005Dialect>();
                            db.Driver<SqlClientDriver>();
                            db.IsolationLevel = IsolationLevel.ReadCommitted;
                            db.ConnectionProvider<DriverConnectionProvider>();
                        });

                        config.Cache(cache =>
                        {
                            cache.DefaultExpiration = 600;
                            cache.RegionsPrefix = "sql-server-db";
                            cache.UseQueryCache = true;
                            cache.Provider<NCacheProvider>();
                            cache.QueryCacheFactory<NCacheQueryCacheFactory>();
                        });

                       

                        config.Properties.Add("cache.use_sliding_expiration", "true");

                        var mapper = new ModelMapper();
                        mapper.AddMappings(
                                    Assembly.GetAssembly(
                                                typeof(CustomersMap))
                                              .GetExportedTypes());
                        var mapping = 
                                mapper.CompileMappingForAllExplicitlyAddedEntities();

                        config.AddMapping(mapping);
                        config.AddAuxiliaryDatabaseObject(
                            new ServiceBrokerSettings());
                        config.AddAuxiliaryDatabaseObject(
                            new CustomerCountryByID_SP());
                        config.AddAuxiliaryDatabaseObject(
                            new CustomerContactNameAndPhoneByID_SP());
                        config.AddAuxiliaryDatabaseObject(
                            new SQLProductPollingDependencyTrigger());
                        config.AddAuxiliaryDatabaseObject(
                            new SQLEmployeePollingDependencyTrigger());

                        var schemaExport = new SchemaExport(config);
                        schemaExport.Create(true, true);

                        _sessionFactory = config.BuildSessionFactory();

                    }
                }
            }

            return _sessionFactory;
        }


        public override void CloseSessionFactory()
        {
            if (_sessionFactory != null)
            {
                lock (_locker)
                {
                    if (_sessionFactory != null)
                    {
                        _sessionFactory.Close();
                    }
                }
            }
        }

    }

}
