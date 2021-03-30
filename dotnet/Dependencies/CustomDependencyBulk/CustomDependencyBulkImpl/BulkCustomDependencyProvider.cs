﻿using Alachisoft.NCache.Runtime.CustomDependencyProviders;
using Alachisoft.NCache.Runtime.Dependencies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples.Providers
{
    public class BulkCustomDependencyProvider : ICustomDependencyProvider
    {
        private string providerName = "BulkCustomDependencyProvider";
        private string cacheName;
        public string Name { get => providerName;  }


        /// <summary>
        /// Perform tasks like allocating resources or acquiring connections
        /// </summary>
        /// <param name="parameters">Startup paramters defined in the configuration</param>
        /// <param name="cacheId">Define for which cache provider is configured</param>
        public void Init(IDictionary<string, string> parameters, string cacheId)
        {
            cacheName = cacheId;
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
            int units = 0;
            string connectionString = "";

            if (dependencyParameters != null)
            {
                if (dependencyParameters.ContainsKey("ProductID"))
                    productId = Int32.Parse(dependencyParameters["ProductID"]);
                else throw new Exception("ProductID parameter is missing. Unable to instantiate Extensible dependency object");
                if (dependencyParameters.ContainsKey("UnitsAvailable"))
                    units = Int32.Parse(dependencyParameters["UnitsAvailable"]);
                else throw new Exception("UnitsAvailable parameter is missing. Unable to instantiate Extensible dependency object");
                if (dependencyParameters.ContainsKey("ConnectionString"))
                    connectionString = dependencyParameters["ConnectionString"];
                else throw new Exception("ConnectionString parameter is missing. Unable to instantiate Extensible dependency object");

                ProductDependency dependency = new ProductDependency(connectionString, productId, units);
                return dependency;

                 
            }

            else throw new Exception("Dependency parameters are not specified. Unable to instantiate extensible dependency object");

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
