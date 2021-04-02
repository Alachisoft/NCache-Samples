using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Caches.NCache;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Sample.CustomerService.Maps;
using SampleApp.DatabaseObjects.Oracle;
using System;
using System.Data;
using System.Reflection;

namespace SampleApp.NHibernateHelpers
{
    public sealed class NHibernateHelperOracle : NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;
        private static readonly object _locker = new object();

        private readonly string _connectionString;

        public NHibernateHelperOracle(
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public override ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                lock (_locker)
                {
                    if (_sessionFactory == null)
                    {
                        NCacheProvider.ClearApplicationConfiguration();

                        var settings = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", false, true)
                            .Build();

                        var applicationConfiguration = new ConfigurationSettings();
                        settings
                            .GetSection("ncache:ApplicationConfig")
                            .Bind(applicationConfiguration);

                        NCacheProvider.SetApplicationConfiguration(applicationConfiguration);

                        var config = new Configuration();

                        config.Mappings(map =>
                        {
                            map.DefaultSchema = "BRAD";
                        });

                        config.DataBaseIntegration(db =>
                        {
                            db.LogFormattedSql = false;
                            db.LogSqlInConsole = false;
                            db.ConnectionString = _connectionString;
                            db.Timeout = 10;
                            db.Dialect<Oracle10gDialect>();
                            db.Driver<OracleManagedDataClientDriver>();
                            db.IsolationLevel = IsolationLevel.ReadCommitted;
                            db.ConnectionProvider<DriverConnectionProvider>();
                        });

                        config.Cache(cache =>
                        {
                            cache.DefaultExpiration = 600;
                            cache.RegionsPrefix = "oracle-db";
                            cache.UseQueryCache = true;
                            cache.Provider<NCacheProvider>();
                            cache.QueryCacheFactory<NCacheQueryCacheFactory>();
                        });

                        config.Properties.Add("cache.use_sliding_expiration", "true");

                        var mapper = new ModelMapper();
                        mapper.AddMappings(Assembly.GetAssembly(typeof(CustomersMap)).GetExportedTypes());
                        var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
                        config.AddMapping(mapping);

                        config.AddAuxiliaryDatabaseObject(
                            new CustomerCountryByID_SP());
                        config.AddAuxiliaryDatabaseObject(
                            new OracleProductPollingDependencyTrigger());
                        config.AddAuxiliaryDatabaseObject(
                            new OracleEmployeePollingDependencyTrigger());

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
                _sessionFactory.Close();
            }
        }

    }
}
