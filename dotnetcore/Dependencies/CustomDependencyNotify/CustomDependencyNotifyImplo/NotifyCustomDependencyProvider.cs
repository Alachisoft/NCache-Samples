using Alachisoft.NCache.Runtime.Dependencies;
using Alachisoft.NCache.Runtime.CustomDependencyProviders;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples.Providers
{
    public class NotifyCustomDependencyProvider : ICustomDependencyProvider
    {
        private string providerName = "NotifyCustomDependencyProvider";
        private string cacheName;
        private string monitoredUri;
        private string authKey;
        private string databaseName;
        public string Name { get => providerName;  }

        /// <summary>
        /// Perform tasks like allocating resources or acquiring connections
        /// </summary>
        /// <param name="parameters">Startup paramters defined in the configuration</param>
        /// <param name="cacheId">Define for which cache provider is configured</param>
        public void Init(IDictionary<string, string> parameters, string cacheId)
        {
            cacheName = cacheId;

            if (parameters != null)
            {
                if (parameters.ContainsKey("EndPoint"))
                    monitoredUri = parameters["EndPoint"].ToString();
                else throw new Exception("Parameter EndPoint is missing. Unable to initialize provider");
                if (parameters.ContainsKey("MonitoredPrimaryKey"))
                    authKey = parameters["MonitoredPrimaryKey"].ToString();
                else throw new Exception("Parameter MonitoredPrimaryKey is missing. Unable to initialize provider");
                if (parameters.ContainsKey("MonitoredDbName"))
                    databaseName = parameters["MonitoredDbName"].ToString();
                else throw new Exception("Parameter MonitoredDbName is missing. Unable to initialize provider");

            }
        }

        /// <summary>
        /// Responsible for creating an extensible dependency object. 
        /// Key is passed as parameter.
        /// <param name="key">cache name</param>
        /// Key is passed as parameter.
        /// <param name="dependencyParameters">parameters to create extensible dependency instance</param>
        /// <returns>extensible dependency instance</returns>
        public ExtensibleDependency CreateDependency(string key, IDictionary<string, string> dependencyParameters)
        {
            string customerId="";
            string monitoredCollection = "";
            string leaseCollection = "";

            if (dependencyParameters != null)
            {
                if (dependencyParameters.ContainsKey("Key"))
                    customerId = dependencyParameters["Key"];
                else throw new Exception("Parameter Key is missing. Unable to instantiate Extensible dependency object");
                
                if (dependencyParameters.ContainsKey("MonitoredCollectionName"))
                    monitoredCollection = dependencyParameters["MonitoredCollectionName"];
                else throw new Exception("Parameter MonitoredCollectionName is missing. Unable to instantiate Extensible dependency object");

                if (dependencyParameters.ContainsKey("LeaseCollectionName"))
                    leaseCollection = dependencyParameters["LeaseCollectionName"];
                else throw new Exception("Parameter LeaseCollectionName is missing. Unable to instantiate Extensible dependency object");

                CosmosDbNotificationDependency<Customer> cosmosDbDependency = new CosmosDbNotificationDependency<Customer>(customerId,
                monitoredUri,
                authKey,
                databaseName,
                monitoredCollection,
                monitoredUri,
                authKey,
                databaseName,
                leaseCollection);

                return cosmosDbDependency;
            }
            else throw new Exception("Dependency parameters are null. Unable to instantiate extensible dependency object");

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
