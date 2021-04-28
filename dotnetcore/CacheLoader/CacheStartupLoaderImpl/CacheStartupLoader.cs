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
using System.Collections.Generic;
using Alachisoft.NCache.Client;
using SampleData;

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
        private string _connectionString;
        private string _query;
        private static ICache _cache;

        /// <summary>
        /// Initialization of the data source from which data can be loaded...
        /// </summary>
        /// <param name="parameters">Cache startup must pass the parameters for initialization</param>
        /// <param name="cacheName">Cache ID</param>
        public void Init(IDictionary<string, string> parameters, string cacheName)
        {
            if (parameters == null && parameters.Count == 0)
                return;

            _connectionString = parameters.Keys.Contains("conn-string") ? parameters["conn-string"] as string : null;
            _query = "SELECT OrderID, OrderDate,  ShipName, ShipAddress, ShipCity, ShipCountry FROM Orders";

            _cache = CacheManager.GetCache(cacheName);
        }

        /// <summary>
        /// Dispose any external resources
        /// </summary>
        public void Dispose()
        {
        }

        public object LoadDatasetOnStartup(string dataset)
        {
            IList<object> loadDatasetAtStartup;

            if (string.IsNullOrEmpty(dataset))
                throw new InvalidOperationException("Invalid dataset.");

            switch (dataset.ToLower())
            {
                case "products":
                    loadDatasetAtStartup = FetchProductsFromDataSouce();
                    break;
                case "suppliers":
                    loadDatasetAtStartup = FetchSuppliersFromDataSouce();
                    break;
                default:
                    throw new InvalidOperationException("Invalid Dataset.");
            }

            string[] keys = GetKeys(loadDatasetAtStartup);
            IDictionary<string, CacheItem> cacheData = GetCacheItemDictionary(keys, loadDatasetAtStartup);
            _cache.InsertBulk(cacheData);

            object userContext = DateTime.Now;
            return userContext;
        }

        public static IDictionary<string, CacheItem> GetCacheItemDictionary(string[] keys, IList<Object> value)
        {
            IDictionary<string, CacheItem> items = new Dictionary<string, CacheItem>();
            CacheItem cacheItem = null;

            for (int i = 0; i < value.Count - 1; i++)
            {
                cacheItem = new CacheItem(value[i]);
                items.Add(keys[i], cacheItem);
            }

            return items;
        }

        public string[] GetKeys(IList<object> objects)
        {
            string[] keys = new string[objects.Count];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = objects[i].GetType() == typeof(Product) ? $"ProductId:{(objects[i] as Product).Id}" : $"SupplierId:{(objects[i] as Supplier).Id}";
            }

            return keys;
        }

        private IList<object> FetchProductsFromDataSouce()
        {
            string Query = "select * from Products where UnitsInStock > 0";
            return ExecuteQuery(Query, "products");

        }

        private IList<object> FetchSuppliersFromDataSouce()
        {
            string Query = "select * from Suppliers";
            return ExecuteQuery(Query, "Suppliers");

        }

        private IList<object> ExecuteQuery(string Query, string dataSet)
        {
            IList<object> Data;

            using (SqlConnection myConnection = new SqlConnection(_connectionString))
            {
                SqlCommand oCmd = new SqlCommand(Query, myConnection);

                myConnection.Open();

                using (var reader = oCmd.ExecuteReader())
                {
                    Data = GetData(reader, dataSet);
                }

                myConnection.Close();
            }
            return Data;
        }

        private IList<object> GetData(SqlDataReader sqlDataReader, string dataSet)
        {
            IList<object> dataList = new List<object>();
            while (sqlDataReader.Read())
            {
                if (string.Compare(dataSet, "suppliers", true) == 0)
                {
                    Supplier supplier = new Supplier()
                    {
                        Id = Convert.ToInt32(sqlDataReader["SupplierID"]),
                        CompanyName = sqlDataReader["CompanyName"].ToString(),
                        ContactName = sqlDataReader["ContactName"].ToString(),
                        Address = sqlDataReader["Address"].ToString()
                    };
                    dataList.Add(supplier);

                }
                if (string.Compare(dataSet, "Products", true) == 0)
                {
                    Product product = new Product()
                    {
                        Name = sqlDataReader["ProductName"].ToString(),
                        Id = Convert.ToInt32(sqlDataReader["ProductID"]),
                        UnitsAvailable = Convert.ToInt32(sqlDataReader["UnitsInStock"]),
                        UnitPrice = Convert.ToInt32(sqlDataReader["UnitPrice"])
                    };
                    dataList.Add(product);

                }
            }
            return dataList;
        }

        public object RefreshDataset(string dataset, object userContext)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, RefreshPreference> GetDatasetsToRefresh(IDictionary<string, object> userContexts)
        {
            throw new NotImplementedException();
        }
    }
}
