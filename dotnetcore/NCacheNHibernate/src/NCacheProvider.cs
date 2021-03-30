using NHibernate.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NHibernate.Caches.NCache
{
    public class NCacheProvider : ICacheProvider
    {
        private static readonly ApplicationConfig applicationConfiguration;
        private static readonly Dictionary<string, CacheHandle> cacheHandles =
            new Dictionary<string, CacheHandle>();

        private static readonly object appConfigLocker = new object();

        static NCacheProvider()
        {
            applicationConfiguration =
                new ApplicationConfig();

            if (!(ConfigurationManager.GetSection("ncache") is ApplicationConfig config))
            {
                return;
            }

            applicationConfiguration = config;
        }

        public static void SetApplicationConfiguration(
            ConfigurationSettings settings)
        {
            lock (appConfigLocker)
            {
                foreach (var cacheConfig in settings.CacheConfigs)
                {
                    if (!applicationConfiguration.CacheConfigurations
                                    .ContainsKey(cacheConfig.CacheConfigId))
                    {
                        applicationConfiguration.CacheConfigurations.Add(
                                        cacheConfig.CacheConfigId,
                                        cacheConfig);
                    }
                }
                foreach (var regionConfig in settings.CacheRegions)
                {
                    if (!applicationConfiguration.CacheRegions
                                    .ContainsKey(regionConfig.RegionName))
                    {
                        applicationConfiguration.CacheRegions.Add(
                                        regionConfig.RegionName,
                                        regionConfig);
                    }
                }

                foreach (var queryDependency in settings.QueryDependencies)
                {
                    applicationConfiguration.QueryDependencies.Add(queryDependency);
                }

                foreach (var entityDependency in settings.EntityDependencies)
                {
                    applicationConfiguration.EntityDependencies.Add(entityDependency);
                } 
            }

        }


        internal static List<QueryDependencyConfiguration> GetQueryDependencyConfigs(
                                                                    string regionPrefix)
        {
            return applicationConfiguration.QueryDependencies
                                .Where(x => x.RegionPrefix == regionPrefix)
                                .ToList();

        }

        internal static List<DependencyConfig> GetEntityDependencyConfigs(
                                                        string regionPrefix,
                                                        string entityClassFullName)
        {
            return applicationConfiguration.EntityDependencies
                                .Where(x =>
                                        x.RegionPrefix == regionPrefix &&
                                        x.EntityClassFullName == entityClassFullName)
                                .ToList();
        }


        public static void ClearApplicationConfiguration()
        {
            applicationConfiguration.CacheConfigurations.Clear();
            applicationConfiguration.CacheRegions.Clear();

            foreach (var cacheHandle in cacheHandles)
            {
                cacheHandle.Value.Cache.Dispose();
            }

            cacheHandles.Clear();

            applicationConfiguration.QueryDependencies.Clear();

            applicationConfiguration.EntityDependencies.Clear();
        }


#pragma warning disable 618
        public ICache BuildCache(
            string regionName,
            IDictionary<string, string> properties)
#pragma warning restore 618
        {
            if (regionName == null)
            {
                regionName = String.Empty;
            }

            properties.TryGetValue(
                            Cfg.Environment.CacheRegionPrefix,
                            out var regionPrefix);

            var parsedRegionPrefix = string.IsNullOrWhiteSpace(regionPrefix) ?
                                                "nhibernate" :
                                                regionPrefix;

            RegionConfig regionConfig;


            if (applicationConfiguration.CacheRegions.ContainsKey(regionName))
            {
                regionConfig =
                                applicationConfiguration.CacheRegions[regionName];
            }
            else
            {
                regionConfig = BuildRegionConfiguration(
                                                regionName,
                                                parsedRegionPrefix);
            }

            var regionCacheId =
                regionConfig.CacheId;

            if (string.IsNullOrWhiteSpace(regionCacheId))
            {
                regionConfig.CacheId = GetDefaultCacheId(parsedRegionPrefix);
            }

            var regionCacheConfigId = $"{parsedRegionPrefix}:{regionConfig.CacheId}"; ;

            CacheConfig cacheConfig =
                applicationConfiguration.CacheConfigurations[regionCacheConfigId];

            if (!cacheHandles.ContainsKey(regionCacheConfigId))
            {
                lock (appConfigLocker)
                {
                    if (!cacheHandles.ContainsKey(regionCacheConfigId))
                    {
                        cacheHandles.Add(
                                            regionCacheConfigId,
                                            new CacheHandle(
                                                    cacheConfig));

                    }

                }
            }

            return new NCacheClient(
                        cacheHandles[regionCacheConfigId],
                        parsedRegionPrefix,
                        regionConfig,
                        properties);
        }

        private static RegionConfig BuildRegionConfiguration(
                                            string regionName,
                                            string regionPrefix)
        {
            var regionConfig = new RegionConfig();

            regionConfig.RegionName = regionName;

            return regionConfig;
        }

        private static string GetDefaultCacheId(
                                    string regionPrefix)
        {
            var cacheId = "";

            var cacheConfigs = applicationConfiguration.CacheConfigurations.Values
                                                .Where(x =>
                                                        x.RegionPrefix == regionPrefix)
                                                .ToList();
            if (cacheConfigs.Count == 0)
            {
                throw new Exception(
                            $"No cache configurations provided for {regionPrefix}");
            }
            else if (cacheConfigs.Count == 1)
            {
                cacheId = cacheConfigs[0].CacheId.Trim();
            }
            else if (cacheConfigs.Count > 1)
            {
                var defaultCacheConfigs = cacheConfigs
                                                .Where(x =>
                                                        x.IsDefault == true)
                                                .ToList();

                if (defaultCacheConfigs.Count > 1)
                {
                    throw new Exception(
                        "There should only be one default configuration" +
                        "for given regionPrefix");
                }
                else if (defaultCacheConfigs.Count == 0)
                {
                    throw new Exception(
                            "There should be exactly one cache configuration marked as " +
                            "default for a given region prefix in case of " +
                            "using multiple cache configurations");
                }
                else
                {
                    cacheId = defaultCacheConfigs[0].CacheId.Trim();
                }
            }

            return cacheId;
        }


        public long NextTimestamp()
        {
            return Timestamper.Next();
        }

        public void Start(IDictionary<string, string> properties)
        {
        }

        public void Stop()
        {
        }
    }
}
