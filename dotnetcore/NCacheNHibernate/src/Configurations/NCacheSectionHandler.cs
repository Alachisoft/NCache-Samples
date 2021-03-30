using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace NHibernate.Caches.NCache
{
    public class NCacheSectionHandler : IConfigurationSectionHandler
    {
        public object Create(
            object parent,
            object configContext,
            XmlNode section)
        {
            var applicationConfig =
                new ApplicationConfig();

            var node = section.SelectNodes("cache-configurations");

            if (node.Count > 1)
            {
                throw new ConfigurationErrorsException(
                    $"There can only be one cache-configurations subsection");
            }

            if (node.Count == 1)
            {
                var cacheConfigurationNodes = node[0].SelectNodes("cache-configuration");

                if (cacheConfigurationNodes.Count == 0)
                {
                    throw new ConfigurationErrorsException(
                        "cache-configurations subsection is empty");
                }

                foreach (XmlNode cacheConfigurationNode in cacheConfigurationNodes)
                {
                    AddCacheConfig(
                        cacheConfigurationNode,
                        applicationConfig);
                }
            }


            node = section.SelectNodes("cache-regions");

            if (node.Count > 1)
            {
                throw new ConfigurationErrorsException(
                    $"There can only be one cache-regions subsection");
            }

            if (node.Count == 1)
            {
                var cacheRegionNodes = node[0].SelectNodes("cache-region");

                if (cacheRegionNodes.Count == 0)
                {
                    throw new ConfigurationErrorsException(
                        "cache-regions subsection is empty");
                }

                foreach (XmlNode cacheRegionNode in cacheRegionNodes)
                {
                    AddCacheRegion(
                        cacheRegionNode,
                        applicationConfig);
                }
            }

            node = section.SelectNodes("query-dependencies");

            if (node.Count > 1)
            {
                throw new ConfigurationErrorsException(
                    $"There can only be one query-dependencies subsection");
            }

            if (node.Count == 1)
            {
                var queryDependencyNodes = node[0].SelectNodes("query-dependency");

                if (queryDependencyNodes.Count == 0)
                {
                    throw new ConfigurationErrorsException(
                        "query-dependencies subsection is empty");
                }

                foreach (XmlNode queryDependencyNode in queryDependencyNodes)
                {
                    AddQueryDependency(
                        queryDependencyNode,
                        applicationConfig);
                }
            }

            node = section.SelectNodes("entity-dependencies");

            if (node.Count > 1)
            {
                throw new ConfigurationErrorsException(
                    $"There can only be one entity-dependencies subsection");
            }

            if (node.Count == 1)
            {
                var entityDependencyNodes = node[0].SelectNodes("entity-dependency");

                if (entityDependencyNodes.Count == 0)
                {
                    throw new ConfigurationErrorsException(
                        "entity-dependencies subsection is empty");
                }

                foreach (XmlNode entityDependencyNode in entityDependencyNodes)
                {
                    AddEntityDependency(
                        entityDependencyNode,
                        applicationConfig);
                }
            }

            return applicationConfig;
        }

        private static void AddCacheConfig(
            XmlNode cacheConfigurationNode,
            ApplicationConfig applicationConfig)
        {
            CacheConfig cacheConfig = new CacheConfig();

            var regionPrefix =
                    GetString(cacheConfigurationNode, "region-prefix", false);

            if (!string.IsNullOrWhiteSpace(regionPrefix))
            {
                cacheConfig.RegionPrefix = regionPrefix;
            }

            cacheConfig.CacheId =
                GetString(cacheConfigurationNode, "cache-name", true);

            if (applicationConfig.CacheConfigurations.ContainsKey(
                                                        cacheConfig.CacheConfigId))
            {
                throw new ConfigurationErrorsException(
                    $"Multiple cache configurations with identical cache config id " +
                    $"{cacheConfig.CacheConfigId} given");
            }

            var isDefault =
                    GetBoolean(cacheConfigurationNode, "is-default", false);

            if (isDefault.HasValue)
            {
                cacheConfig.IsDefault = isDefault.Value;
            }

            cacheConfig.ConnectionTimeout =
                    GetDouble(cacheConfigurationNode, "connection-timout");

            cacheConfig.KeepAliveInterval =
                    GetDouble(cacheConfigurationNode, "keep-alive-interval");

            cacheConfig.EnableKeepAlive =
                    GetBoolean(cacheConfigurationNode, "enable-keep-alive");

            cacheConfig.CommandRetryInterval =
                    GetDouble(cacheConfigurationNode, "command-retry-interval");

            cacheConfig.CommandRetries =
                    GetInteger(cacheConfigurationNode, "command-retries");

            cacheConfig.RetryConnectionDelay =
                    GetDouble(cacheConfigurationNode, "retry-connection-delay");

            cacheConfig.RetryInterval =
                    GetDouble(cacheConfigurationNode, "retry-interval");

            cacheConfig.ConnectionRetries =
                    GetInteger(cacheConfigurationNode, "connection-retries");

            cacheConfig.EnableClientLogs =
                    GetBoolean(cacheConfigurationNode, "enable-client-logs");

            cacheConfig.ClientRequestTimeout =
                    GetDouble(cacheConfigurationNode, "client-request-timeout");

            cacheConfig.LoadBalance =
                    GetBoolean(cacheConfigurationNode, "load-balance");

            cacheConfig.AppName =
                    GetString(cacheConfigurationNode, "app-name");

            cacheConfig.ClientBindIP =
                    GetString(cacheConfigurationNode, "client-bind-ip");

            cacheConfig.LogLevel =
                    GetNCacheLogLevel(cacheConfigurationNode, "log-level");

            cacheConfig.Mode =
                    GetNCacheIsolationLevel(cacheConfigurationNode, "mode");

            cacheConfig.SyncMode =
                    GetNCacheClientSyncMode(cacheConfigurationNode, "sync-mode");

            var user =
                    GetString(cacheConfigurationNode, "user");

            var password =
                    GetString(cacheConfigurationNode, "password");

            if ((string.IsNullOrWhiteSpace(user) &&
                !string.IsNullOrWhiteSpace(password)) ||
                (!string.IsNullOrWhiteSpace(user) &&
                 string.IsNullOrWhiteSpace(password)))
            {
                throw new ConfigurationErrorsException(
                    $"User name or password is not given with the other given");
            }
            else if (
                !string.IsNullOrWhiteSpace(user) &&
                !string.IsNullOrWhiteSpace(password))
            {
                cacheConfig.UserCredentials = new NCacheCredentials(user, password);
            }

            var serverNodesRoot =
                cacheConfigurationNode.SelectNodes("servers");

            if (serverNodesRoot.Count > 1)
            {
                throw new ConfigurationErrorsException(
                    $"A cache-configuration section can have only 1 servers section");
            }

            if (serverNodesRoot.Count == 1)
            {
                var serverNodes =
                    serverNodesRoot[0].SelectNodes("server");

                if (serverNodes.Count == 0)
                {
                    throw new ConfigurationErrorsException(
                        "servers subsection empty");
                }
                else
                {
                    Dictionary<string, int?> serverDict =
                        new Dictionary<string, int?>();

                    foreach (XmlNode serverNode in serverNodes)
                    {
                        var ipAddress =
                                GetString(serverNode, "server-ip", true);
                        var port =
                                GetInteger(serverNode, "port");

                        if (!serverDict.ContainsKey(ipAddress))
                        {
                            serverDict.Add(ipAddress, port);
                        }
                        else
                        {
                            throw new ConfigurationErrorsException(
                                $"Multiple servers with identical ip addresses given.");
                        }
                    }

                    cacheConfig.ServerList = GetNCacheServerInfoList(serverDict);
                }
            }

            applicationConfig.CacheConfigurations.Add(
                                            cacheConfig.CacheConfigId,
                                            cacheConfig);

        }

        private static void AddCacheRegion(
            XmlNode cacheRegionNode,
            ApplicationConfig applicationConfig)
        {
            RegionConfig regionConfig =
                                new RegionConfig();

            regionConfig.RegionName =
                GetString(cacheRegionNode, "region-name", true);

            if (applicationConfig.CacheRegions.ContainsKey(
                                                    regionConfig.RegionName))
            {
                throw new ConfigurationErrorsException(
                    $"Multiple cache region configurations with identical region-name " +
                    $"{regionConfig.RegionName} given");
            }

            var cacheId =
                    GetString(cacheRegionNode, "cache-id", false);

            if (!string.IsNullOrWhiteSpace(cacheId))
            {
                regionConfig.CacheId = cacheId;
            }

            var expiration =
                    GetTimeSpan(cacheRegionNode, "expiration-period", true);

            if (expiration.HasValue)
            {
                regionConfig.Expiration = expiration.Value;
            }

            var useSliding =
                    GetBoolean(cacheRegionNode, "use-sliding-expiration");

            if (useSliding.HasValue)
            {
                regionConfig.UseSlidingExpiration = useSliding.Value;
            }

            var cacheKeyPrefix =
                    GetString(cacheRegionNode, "cache-key-prefix");

            if (!string.IsNullOrWhiteSpace(cacheKeyPrefix))
            {
                regionConfig.CacheKeyPrefix = cacheKeyPrefix;
            }

            var lockKeyTimeout =
                    GetTimeSpan(cacheRegionNode, "lock-key-timout", true);

            if (lockKeyTimeout.HasValue)
            {
                regionConfig.LockKeyTimeout = lockKeyTimeout.Value;
            }

            var lockAcquireTimeout =
                    GetTimeSpan(cacheRegionNode, "lock-acquire-timeout", true);

            if (lockAcquireTimeout.HasValue)
            {
                regionConfig.LockAcquireTimeout = lockAcquireTimeout.Value;
            }

            var lockRetryTimes =
                    GetInteger(cacheRegionNode, "lock-retry-times");

            if (lockRetryTimes.HasValue)
            {
                regionConfig.LockRetryTimes = lockRetryTimes.Value;
            }

            var lockMaxRetryDelay =
                    GetTimeSpan(cacheRegionNode, "lock-max-retry-delay");

            if (lockMaxRetryDelay.HasValue)
            {
                regionConfig.LockMaxRetryDelay = lockMaxRetryDelay.Value;
            }

            var lockMinRetryDelay =
                    GetTimeSpan(cacheRegionNode, "lock-min-retry-delay");

            if (lockMinRetryDelay.HasValue)
            {
                regionConfig.LockMinRetryDelay = lockMinRetryDelay.Value;
            }

            var circuitBreakerExceptionsAllowed =
                    GetInteger(
                        cacheRegionNode,
                        "circuit-breaker-exceptions-allowed");

            if (circuitBreakerExceptionsAllowed.HasValue)
            {
                regionConfig.CircuitBreakerExceptionsAllowed =
                                    circuitBreakerExceptionsAllowed.Value;
            }

            var circuitBreakerDurationOfBreak =
                    GetInteger(
                        cacheRegionNode,
                        "circuit-breaker-open-duration");

            if (circuitBreakerDurationOfBreak.HasValue)
            {
                regionConfig.CircuitBreakerDurationOfBreak =
                                    circuitBreakerDurationOfBreak.Value;
            }

            applicationConfig.CacheRegions.Add(
                                        regionConfig.RegionName,
                                        regionConfig);
        }

        private static void AddQueryDependency(
            XmlNode queryDependencyNode,
            ApplicationConfig applicationConfig)
        {
            var isPollingDependencyUsed =
                    GetBoolean(queryDependencyNode, "polling-used", true);

            var regionPrefix =
                    GetString(queryDependencyNode, "region-prefix", false);

            if (string.IsNullOrWhiteSpace(regionPrefix))
            {
                regionPrefix = "nhibernate";
            }

            if (!isPollingDependencyUsed.HasValue ||
                    isPollingDependencyUsed.Value == false)
            {
                var queryCommandDependencyConfiguration =
                    new QueryCommandDependencyConfiguration();

                queryCommandDependencyConfiguration.RegionPrefix =
                                                            regionPrefix;

                queryCommandDependencyConfiguration.QualifiedTableName =
                        GetString(queryDependencyNode, "qualified-table-name", true);

                queryCommandDependencyConfiguration.DatabaseType =
                        GetDatabaseType(queryDependencyNode, "database-type");

                var columnNameNodesRoot = queryDependencyNode.SelectNodes("column-names");

                if (columnNameNodesRoot.Count == 0)
                {
                    throw new ConfigurationErrorsException(
                        $"Each query command dependency section must have atleast 1 " +
                        $"column-names subsection");
                }
                else if (columnNameNodesRoot.Count > 1)
                {
                    throw new ConfigurationErrorsException(
                        $"Each query command dependency section can have at most 1 " +
                        $"column-names subsection");
                }
                else
                {
                    var columnNameNodes = columnNameNodesRoot[0].SelectNodes("column-name");

                    if (columnNameNodes.Count == 0)
                    {
                        throw new ConfigurationErrorsException(
                                        $"Each column-names section must have atleast 1 " +
                                        $"column-name subsection");
                    }

                    List<string> columnNames = new List<string>(columnNameNodes.Count);

                    foreach (XmlNode columnNameNode in columnNameNodes)
                    {
                        columnNames.Add(
                                        GetString(columnNameNode, "name", true));
                    }

                    queryCommandDependencyConfiguration.KeyColumnNames = columnNames.ToArray();

                    applicationConfig.QueryDependencies
                        .Add(queryCommandDependencyConfiguration);
                }
            }
            else
            {
                var queryTableDependencyConfiguration =
                    new QueryTableDependencyConfiguration();

                queryTableDependencyConfiguration.RegionPrefix =
                                                            regionPrefix;

                queryTableDependencyConfiguration.QualifiedTableName =
                        GetString(queryDependencyNode, "qualified-table-name", true);

                queryTableDependencyConfiguration.DatabaseType =
                        GetDatabaseType(queryDependencyNode, "database-type");

                applicationConfig.QueryDependencies
                        .Add(queryTableDependencyConfiguration);
            }
        }

        private static void AddEntityDependency(
            XmlNode entityDependencyNode,
            ApplicationConfig applicationConfig)
        {
            var dependencyId =
                GetInteger(entityDependencyNode, "dependency-id", true);

            var regionPrefix =
                GetString(entityDependencyNode, "region-prefix", false);

            if (string.IsNullOrWhiteSpace(regionPrefix))
            {
                regionPrefix = "nhibernate";
            }

            if (applicationConfig.EntityDependencies.Any(
                                            x => x.DependencyID == dependencyId &&
                                                 x.RegionPrefix == regionPrefix))
            {
                throw new ConfigurationErrorsException(
                    $"Multiple dependencies in dependencies section with same " +
                    $"dependency id {dependencyId.Value} and regionPrefix " +
                    $"{regionPrefix}");
            }

            var entityClassFullName =
                GetString(entityDependencyNode, "entity-class-full-name", true);

            var databaseDependencyType =
                GetDependencyType(entityDependencyNode, "dependency-type");

            if (databaseDependencyType == DatabaseDependencyType.Oledb)
            {
                var tableName =
                        GetString(entityDependencyNode, "qualified-table-name", true);

                var dbCacheKey =
                        GetString(entityDependencyNode, "db-cache-key", true);

                var databaseType =
                        GetDatabaseType(entityDependencyNode, "database-type");

                var dependency = new OleDbDependencyConfig
                {
                    DependencyID = dependencyId.Value,
                    RegionPrefix = regionPrefix,
                    EntityClassFullName = entityClassFullName,
                    QualifiedTableName = tableName,
                    DbCacheKey = dbCacheKey,
                    DatabaseType = databaseType
                };

                applicationConfig.EntityDependencies.Add(dependency);
            }
            else
            {
                var sqlStatement =
                        GetString(entityDependencyNode, "sql-command", true);

                var isStoredProcedure =
                        GetBoolean(entityDependencyNode, "is-stored-procedure");

                CommandDependencyConfig dependency = null;
                if (databaseDependencyType == DatabaseDependencyType.Sql)
                {
                    dependency = new SQLDependencyConfig
                    {
                        DependencyID = dependencyId.Value,
                        RegionPrefix = regionPrefix,
                        EntityClassFullName = entityClassFullName,
                        SQLStatement = sqlStatement,
                        IsStoredProcedure = !isStoredProcedure.HasValue ?
                                                                false :
                                                                isStoredProcedure.Value
                    };
                }
                else
                {
                    dependency = new OracleDependencyConfig
                    {
                        DependencyID = dependencyId.Value,
                        EntityClassFullName = entityClassFullName,
                        SQLStatement = sqlStatement,
                        IsStoredProcedure = !isStoredProcedure.HasValue ?
                                                                false :
                                                                isStoredProcedure.Value
                    };
                }

                var paramNodesRoot = entityDependencyNode.SelectNodes("command-params");

                if (paramNodesRoot.Count > 1)
                {
                    throw new ConfigurationErrorsException(
                        $"Oracle or SQL dependency can only have at most 1 command-params " +
                        $"section");
                }
                else if (paramNodesRoot.Count == 1)
                {
                    var commandParamsNodes =
                            paramNodesRoot[0].SelectNodes("command-param");

                    if (commandParamsNodes.Count == 0)
                    {
                        throw new ConfigurationErrorsException(
                            "command-params subsection is empty");
                    }

                    foreach (XmlNode commandParamsNode in commandParamsNodes)
                    {
                        var inputNodes = commandParamsNode.SelectNodes("input");

                        if (inputNodes.Count > 1)
                        {
                            throw new ConfigurationErrorsException(
                            "A command-param section can only have 1 input");
                        }
                        else if (inputNodes.Count == 1)
                        {
                            var inputParameterName =
                                    GetString(inputNodes[0], "param-name", true);
                            var inputParameterType =
                                    GetString(inputNodes[0], "param-db-type", true);

                            dependency.PrimaryKeyInputParameter = inputParameterName;
                            dependency.PrimaryKeyDbType = inputParameterType;
                        }

                        var outputNodesRoot = commandParamsNode.SelectNodes("outputs");

                        if (outputNodesRoot.Count > 1)
                        {
                            throw new ConfigurationErrorsException(
                            "A command-param section can only have 1 outputs section");
                        }
                        else if (outputNodesRoot.Count == 1)
                        {
                            var outputNodes = outputNodesRoot[0].SelectNodes("output");

                            if (outputNodes.Count == 0)
                            {
                                throw new ConfigurationErrorsException(
                                                    "outputs section is empty");
                            }
                            else
                            {
                                foreach (XmlNode outputNode in outputNodes)
                                {
                                    var outputParameterName =
                                        GetString(outputNode, "param-name", true);
                                    var outputParameterType =
                                        GetString(outputNode, "param-db-type", true);

                                    dependency.OutputParametersAndDbTypes.Add(
                                                                outputParameterName,
                                                                outputParameterType);
                                }
                            }
                        }

                    }
                }

                applicationConfig.EntityDependencies.Add(dependency);
            }
        }

        private static string GetString(
            XmlNode node,
            string attributeName,
            bool isRequired = false)
        {
            var attribute =
                node.Attributes[attributeName];

            if (attribute == null ||
                string.IsNullOrWhiteSpace(attribute.Value))
            {
                if (isRequired)
                {
                    throw new ConfigurationErrorsException(
                        $"{attributeName} is required in the {node.Name} section");
                }

                return null;
            }
            else
            {
                return attribute.Value.Trim();
            }
        }

        private static int? GetInteger(
            XmlNode node,
            string attributeName,
            bool isRequired = false)
        {
            var attribute =
                node.Attributes[attributeName];

            if (attribute == null ||
                string.IsNullOrWhiteSpace(attribute.Value))
            {
                if (isRequired)
                {
                    throw new ConfigurationErrorsException(
                        $"{attributeName} is required in the {node.Name} section");
                }

                return null;
            }
            else
            {
                int result;

                if (int.TryParse(attribute.Value.Trim(), out result))
                {
                    if (result < 0)
                    {
                        throw new ConfigurationErrorsException(
                            $"{attributeName} < 0 in the {node.Name} section");
                    }

                    return result;
                }
                else
                {
                    throw new ConfigurationErrorsException(
                            $"{attributeName} in the {node.Name} section " +
                            $"not a valid integer value");
                }
            }
        }

        private static bool? GetBoolean(
            XmlNode node,
            string attributeName,
            bool isRequired = false)
        {
            var attribute =
                node.Attributes[attributeName];

            if (attribute == null ||
                string.IsNullOrWhiteSpace(attribute.Value))
            {
                if (isRequired)
                {
                    throw new ConfigurationErrorsException(
                        $"{attributeName} is required in the {node.Name} section");
                }

                return null;
            }
            else
            {
                bool result;

                if (bool.TryParse(attribute.Value.Trim(), out result))
                {
                    return result;
                }
                else
                {
                    throw new ConfigurationErrorsException(
                            $"{attributeName} in the {node.Name} section " +
                            $"not a valid boolean value");
                }
            }
        }

        private static double? GetDouble(
            XmlNode node,
            string attributeName,
            bool isRequired = false)
        {
            var attribute =
                node.Attributes[attributeName];

            if (attribute == null ||
                string.IsNullOrWhiteSpace(attribute.Value))
            {
                if (isRequired)
                {
                    throw new ConfigurationErrorsException(
                        $"{attributeName} is required in the {node.Name} section");
                }

                return null;
            }
            else
            {
                double result;

                if (double.TryParse(attribute.Value.Trim(), out result))
                {
                    if (result < 0)
                    {
                        throw new ConfigurationErrorsException(
                            $"{attributeName} < 0 in the {node.Name} section");
                    }

                    return result;
                }
                else
                {
                    throw new ConfigurationErrorsException(
                            $"{attributeName} in the {node.Name} section " +
                            $"not a valid double value");
                }
            }
        }

        private static TimeSpan? GetTimeSpan(
            XmlNode node,
            string attributeName,
            bool isInSeconds = false)
        {
            double? result = GetDouble(node, attributeName);

            if (result.HasValue)
            {
                if (isInSeconds)
                {
                    return TimeSpan.FromSeconds(result.Value);
                }
                else
                {
                    return TimeSpan.FromMilliseconds(result.Value);
                }
            }

            return null;
        }

        private static NCacheLogLevel? GetNCacheLogLevel(
            XmlNode node,
            string attributeName)
        {
            var attribute =
                node.Attributes[attributeName];

            if (attribute == null ||
                string.IsNullOrWhiteSpace(attribute.Value))
            {
                return null;
            }

            var stringValue = attribute.Value.Trim();

            if (stringValue.Equals("info",
                        StringComparison.InvariantCultureIgnoreCase))
            {
                return NCacheLogLevel.Info;
            }
            else if (stringValue.Equals("error",
                        StringComparison.InvariantCultureIgnoreCase))
            {
                return NCacheLogLevel.Error;
            }
            else if (stringValue.Equals("debug",
                        StringComparison.InvariantCultureIgnoreCase))
            {
                return NCacheLogLevel.Debug;
            }
            else
            {
                throw new ConfigurationErrorsException(
                            $"{attributeName} in the {node.Name} section " +
                            $"not a valid NCache log level value");
            }
        }

        private static NCacheIsolationLevel? GetNCacheIsolationLevel(
            XmlNode node,
            string attributeName)
        {
            var attribute =
                node.Attributes[attributeName];

            if (attribute == null ||
                string.IsNullOrWhiteSpace(attribute.Value))
            {
                return null;
            }

            var stringValue = attribute.Value.Trim();

            if (stringValue.Equals("default",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return NCacheIsolationLevel.Default;
            }
            else if (stringValue.Equals("inproc",
                        StringComparison.InvariantCultureIgnoreCase))
            {
                return NCacheIsolationLevel.InProc;
            }
            else if (stringValue.Equals("outproc",
                        StringComparison.InvariantCultureIgnoreCase))
            {
                return NCacheIsolationLevel.Outproc;
            }
            else
            {
                throw new ConfigurationErrorsException(
                            $"{attributeName} in the {node.Name} section " +
                            $"not a valid NCache isolation level value");
            }
        }

        private static NCacheClientSyncMode? GetNCacheClientSyncMode(
            XmlNode node,
            string attributeName)
        {
            var attribute =
                node.Attributes[attributeName];

            if (attribute == null ||
                string.IsNullOrWhiteSpace(attribute.Value))
            {
                return null;
            }

            var stringValue = attribute.Value.Trim();

            if (stringValue.Equals("optimistic",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return NCacheClientSyncMode.Optimistic;
            }
            else if (stringValue.Equals("pessimistic",
                        StringComparison.InvariantCultureIgnoreCase))
            {
                return NCacheClientSyncMode.Pessimistic;
            }
            else
            {
                throw new ConfigurationErrorsException(
                            $"{attributeName} in the {node.Name} section " +
                            $"not a valid NCache client sync mode value");
            }
        }

        private static List<NCacheServerInfo> GetNCacheServerInfoList(
            IDictionary<string, int?> serverInfos)
        {
            var serverList =
                new List<NCacheServerInfo>(serverInfos.Count);

            foreach (var serverInfo in serverInfos)
            {
                if (!serverInfo.Value.HasValue)
                {
                    serverList.Add(new NCacheServerInfo(serverInfo.Key.Trim()));
                }
                else
                {
                    serverList.Add(new NCacheServerInfo(
                                                serverInfo.Key.Trim(),
                                                serverInfo.Value.Value));
                }
            }

            return serverList;
        }

        private static DatabaseDependencyType GetDependencyType(
            XmlNode node,
            string attributeName)
        {
            var result = GetString(node, attributeName, true);

            if (result.Equals(
                            "sql",
                            StringComparison.InvariantCultureIgnoreCase))
            {
                return DatabaseDependencyType.Sql;
            }
            else if (result.Equals(
                                "orcl",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return DatabaseDependencyType.Oracle;
            }
            else if (result.Equals(
                                "oledb",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return DatabaseDependencyType.Oledb;
            }
            else
            {
                throw new ConfigurationErrorsException(
                    $"Dependency type not valid. Should be either sql, orcl or " +
                    $"oledb case-insensitive");
            }
        }


        private static DatabaseType GetDatabaseType(
            XmlNode node,
            string attributeName)
        {
            var result = GetString(node, attributeName);

            if (string.IsNullOrWhiteSpace(result))
            {
                return DatabaseType.Sql;
            }

            if (result.Equals(
                            "sql",
                            StringComparison.InvariantCultureIgnoreCase))
            {
                return DatabaseType.Sql;
            }
            else if (result.Equals(
                                "orcl",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return DatabaseType.Oracle;
            }
            else
            {
                throw new ConfigurationErrorsException(
                    $"Database type not valid. Should be either sql or orcl case-insensitive");
            }
        }
    }
}
