// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using Alachisoft.NCache.Runtime.CacheLoader;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Sample.Data;
using System.Data;
using System.Data.SqlClient;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Implementation of Cache Loader
    /// 
    /// It executes at the startup of the Cache if configured.
    /// Primary purpose is to load the Cache with the data on startup.
    /// 
    /// This implementation of cache loader is implemented to run cache loader on multiple nodes 
    /// depending on "distribution hints" in this sample we are using "ShipVia" column of "Orders"
    /// table of "northwind" database. There are three shippers in northwind, data for different 
    /// shippers are loaded through different distribution hint.
    /// 
    /// </summary>
    public class CacheStartupLoader : ICacheLoader
    {
        private SqlConnection _connection;
        private string _connectionString;
        private string _query;
        private string _hint = string.Empty;

        /// <summary>
        /// Initialization of the data source from which data can be loaded...
        /// </summary>
        /// <param name="parameters">Cache startup must pass the parameters for initialization</param>
        /// <param name="cacheId">Cache ID</param>
        public void Init(System.Collections.IDictionary parameters, string cacheId)
        {
            if (parameters == null && parameters.Count == 0)
                return;

            _connectionString = parameters.Contains("conn-string") ? parameters["conn-string"] as string : null;
            _hint = parameters.Contains("distributionhint") ? parameters["distributionhint"] as string : "";
            _query = "SELECT OrderID, OrderDate,  ShipName, ShipAddress, ShipCity, ShipCountry FROM Orders";

            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Load the Next Chunk of data from the database
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public LoaderResult LoadNext(object userContext)
        {
            return LoadOrdersFromSource(userContext);
        }

        /// <summary>
        /// Load the Orders from the database
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private LoaderResult LoadOrdersFromSource(object userContext)
        {
            string condition;
            switch (_hint.ToLower())
            {
                case "shipper1":
                    condition = " where ShipVia = 1";
                    return LoadOrders(condition, userContext);
                case "shipper2":
                    condition = " where ShipVia = 2";
                    return LoadOrders(condition, userContext);
                case "shipper3":
                    condition = " where ShipVia = 3";
                    return LoadOrders(condition, userContext);
                default:
                    condition = "";
                    return LoadOrders(condition, userContext);
            }
        }

        /// <summary>
        /// Load the Orders from the database based on condition from the database.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private LoaderResult LoadOrders(string condition, object userContext)
        {
            LoaderResult result = new LoaderResult();
            result.UserContext = userContext;
            if (_connection != null)
            {
                SqlCommand command = new SqlCommand(_query + condition, _connection);
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Read and construct Order
                    Order order = new Order();
                    order.OrderID = int.Parse(reader[0].ToString());
                    order.OrderDate = Convert.ToDateTime(reader[1].ToString());
                    order.ShipName = reader[2] as string;
                    order.ShipAddress = reader[3] as string;
                    order.ShipCity = reader[4] as string;
                    order.ShipCountry = reader[5] as string;
                    // Create a Cache item
                    // You must create ProviderCacheItem before adding to the resultant data...
                    ProviderCacheItem cacheItem = new ProviderCacheItem(order);

                    // Add to Loader data...
                    result.Data.Add(new System.Collections.Generic.KeyValuePair<string, ProviderItemBase>( order.OrderID.ToString(), cacheItem));
                }
            }
            result.HasMoreData = false;
            return result;
        }

        /// <summary>
        /// Dispose any external resources
        /// </summary>
        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
