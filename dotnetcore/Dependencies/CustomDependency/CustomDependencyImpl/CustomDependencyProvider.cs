using Alachisoft.NCache.Runtime.Dependencies;
using Alachisoft.NCache.Runtime.CustomDependencyProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples.Providers
{
    public class CustomDependencyProvider : ICustomDependencyProvider
    {
        private string providerName = "CustomDependencyProvider";
        private string cacheName;
        public string Name { get => providerName; }

        /// <summary>
        /// Perform tasks like allocating resources or acquiring connections
        /// </summary>
        /// <param name="parameters">>Startup paramters defined in the configuration</param>
        /// <param name="cacheName">Define for which cache provider is configured</param>
        public void Init(IDictionary<string, string> parameters, string cacheName)
        {
            this.cacheName = cacheName;
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
            int productId = 0;
            string connectionString = "";

            if (dependencyParameters != null)
            {
                if (dependencyParameters.ContainsKey("ProductID"))
                    productId = Int32.Parse(dependencyParameters["ProductID"]);
                else
                    throw new Exception("ProductID paramerter is missing. Unable to instantiate Extensible dependency object");

                if (dependencyParameters.ContainsKey("ConnectionString"))
                    connectionString = dependencyParameters["ConnectionString"];
                else
                    throw new Exception("ConnectionString paramerter is missing. Unable to instantiate Extensible dependency object");

                Dependency dependency = new Dependency(productId, connectionString);
                return dependency;
            }
            else throw new Exception("DependencyParameters are null. Cannot instantiate Extensible dependency object");

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
