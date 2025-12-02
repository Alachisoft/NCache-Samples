// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.CacheLoader;
using Alachisoft.NCache.Sample.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

// ===============================================================================
// NCache Loader Refresher is mechanism to load and refresh data into NCache from
// a data source. The Loader is invoked on cache startup to populate the cache
// with initial data. The Refresher is invoked at runtime to refresh the data
// in cache based on the changes in the data source. Loaders and refreshers are
// particularly useful in scenarios where data consistency and performance are
// critical, such as in e-commerce applications, financial systems and real-time
// analytics platforms. Some use cases of NCache Loader/Refresher include:
//   1. Loading and refreshing lookup or reference data (e.g., country codes, tax
//      rates) into cache on startup and at regular intervals.
//   2. Loading currency exchange rates on cache startup and refreshing them at
//      scheduled intervals.
//   3. Loading product catalogs on startup and updating prices or availability
//      periodically.
//   4. Caching web images or static content on startup and refreshing them when
//      updated.
//   5. Populating latest news headlines or other frequently changing content into
//      cache at regular intervals.
// NOTE: For the sample to work, the library project DLL needs to be deployed to
// NCache Cache Loader/Refresher. This can be done by the following:
//  - Enable Cache Loader/Refresher is in the configuration.
//  - Add the library file of this project as assembly name.
//  - Add a parameter named "ConnectionString" with the connection string of your
//    database as value.
//  - Add "products" and "suppliers" as datasets (leave options as default).
//  - Deploy the library to the Cache Loader.
// ===============================================================================

namespace LoaderAndRefresher
{
    /// <summary>
    /// Implementation of <see cref="ICacheLoader"/> to load and refresh datasets into NCache from a data source
    /// </summary>
    public class LoaderRefresher : ICacheLoader
    {
        // Declaring static variable to hold cache handle
        private static ICache _cache;

        // Declaring variable to hold connection string for the database
        private string _connectionString;

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method implementation of <see cref="ICacheLoader"/> to initialize the loader/refresher with parameters and cache name
        /// </summary>
        /// <param name="parameters">Dictionary of parameters used for initializing loader/refresher</param>
        /// <param name="cacheName">Name of cache on which loader/refresher is deployed</param>
        public void Init(IDictionary<string, string> parameters, string cacheName)
        {
            // Extracting connection string from parameters
            if (parameters != null && parameters.Count > 0)
                _connectionString = parameters.ContainsKey("ConnectionString")
                    ? parameters["ConnectionString"] as string : string.Empty;

            // Connecting to a running cache and return a cache handle for it
            _cache = CacheManager.GetCache(cacheName);
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method implementation of <see cref="ICacheLoader"/> to load the specified dataset from data source into cache on startup
        /// </summary>
        /// <param name="dataSet">Name of dataset</param>
        /// <returns>Context object to track dataset last refresh time</returns>
        public object LoadDatasetOnStartup(string dataSet)
        {
            // Creating list to hold the dataset to be loaded
            IList<object> dataset = null;

            // Fetching dataset based on the provided name
            switch (dataSet.ToLower())
            {
                // Fetching products dataset
                case "products":
                    dataset = GetProducts();
                    break;
                // Fetching suppliers dataset
                case "suppliers":
                    dataset = GetSuppliers();
                    break;
            }

            // Inserting the dataset into cache if not null
            if (dataset != null)
            {
                IDictionary<string, CacheItem> cacheData = GetCacheItemDictionary(dataset);

                // Inserting the dataset into cache in bulk
                _cache?.InsertBulk(cacheData);
            }

            // Returning current time as user context for tracking last refresh time
            object userContext = DateTime.Now;
            return userContext;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method implementation of <see cref="ICacheLoader"/> to refresh the specified dataset
        /// </summary>
        /// <param name="dataSet">Name of dataset ("products" or "suppliers")</param>
        /// <param name="userContext">Object returned from <see cref="LoadDatasetOnStartup"/> or last refresh</param>
        /// <returns>Updated user context after refresh</returns>
        public object RefreshDataset(string dataSet, object userContext)
        {
            // Variable to hold last refresh time
            DateTime? lastRefreshTime;

            // Refreshing dataset based on the provided name
            switch (dataSet.ToLower())
            {
                // Refreshing products dataset
                case "products":
                    lastRefreshTime = userContext as DateTime?;
                    var productsNeedToRefresh = FetchUpdatedProducts(lastRefreshTime);
                    foreach (var obj in productsNeedToRefresh)
                    {
                        var product = obj as Product;
                        if (product == null) continue;

                        string key = $"ProductId:{product.ProductID}";
                        CacheItem cacheItem = new CacheItem(product);
                        var version = _cache?.Insert(key, cacheItem);
                    }
                    break;
                // Refreshing suppliers dataset
                case "suppliers":
                    lastRefreshTime = userContext as DateTime?;
                    var suppliersNeedToRefresh = FetchUpdatedSuppliers(lastRefreshTime);
                    foreach (var obj in suppliersNeedToRefresh)
                    {
                        var supplier = obj as Supplier;
                        if (supplier == null) continue;
                        string key = $"SupplierId:{supplier.SupplierID}";
                        CacheItem cacheItem = new CacheItem(supplier);
                        _cache?.Insert(key, cacheItem);
                    }
                    break;
                // Handling invalid dataset names
                default:
                    lastRefreshTime = null;
                    break;
            }

            // Updating user context with current time after refresh
            userContext = DateTime.Now;

            return userContext;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to determine which datasets need refreshing
        /// </summary>
        /// <param name="userContexts">Dictionary of dataset names and their last refresh times</param>
        /// <returns>Dictionary of dataset names and <see cref="RefreshPreference"/></returns>
        public IDictionary<string, RefreshPreference> GetDatasetsToRefresh(IDictionary<string, object> userContexts)
        {
            // Dictionary to hold datasets that need refreshing
            IDictionary<string, RefreshPreference> datasetsNeedToRefresh = new Dictionary<string, RefreshPreference>();

            // Variables for last refresh time and update status
            DateTime? lastRefreshTime;
            bool datasetHasUpdated;

            // Checking each dataset for updates
            foreach (var dataSet in userContexts.Keys)
            {
                // Determining if the dataset has been updated since last refresh
                switch (dataSet.ToLower())
                {
                    // Checking for products dataset updates
                    case "products":
                        lastRefreshTime = userContexts[dataSet] as DateTime?;
                        datasetHasUpdated = IsProductUpdated(dataSet, lastRefreshTime);
                        if (datasetHasUpdated)
                        {
                            datasetsNeedToRefresh.Add(dataSet, RefreshPreference.RefreshNow);
                        }
                        break;
                    // Checking for suppliers dataset updates
                    case "suppliers":
                        lastRefreshTime = userContexts[dataSet] as DateTime?;
                        datasetHasUpdated = IsSupplierUpdated(dataSet, lastRefreshTime);
                        if (datasetHasUpdated)
                        {
                            datasetsNeedToRefresh.Add(dataSet, RefreshPreference.RefreshNow);
                        }
                        break;
                    // Handling invalid dataset names
                    default:
                        lastRefreshTime = null;
                        break;
                }
            }

            // Returning the datasets that need refreshing
            return datasetsNeedToRefresh;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method implementation of <see cref="ICacheLoader"/> to dispose unmanaged resources
        /// </summary>
        public void Dispose()
        {
            // Disposing cache once done
            _cache?.Dispose();
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to create a dictionary of <see cref="CacheItem"/> from keys and objects
        /// </summary>
        /// <param name="values">Objects to store</param>
        /// <returns>Dictionary of key and <see cref="CacheItem"/></returns>
        public IDictionary<string, CacheItem> GetCacheItemDictionary(IList<object> values)
        {
            // Preparing dictionary of cache items
            IDictionary<string, CacheItem> items = new Dictionary<string, CacheItem>();

            // Populating the dictionary with cache items
            CacheItem cacheItem = null;
            for (int i = 0; i < values.Count; i++)
            {
                // Creating a new cache item for each object and adding to dictionary
                cacheItem = new CacheItem(values[i]);

                // Generating key based on object type
                string key = values[i].GetType() == typeof(Product) ? $"ProductId:{(values[i] as Product).ProductID}"
                    : $"SupplierId:{(values[i] as Supplier).SupplierID}";

                // Adding the cache item to the dictionary with the generated key
                items.Add(key, cacheItem);
            }

            // Returning the dictionary of cache items
            return items;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch products from the data source
        /// </summary>
        /// <returns>List of products</returns>
        private IList<object> GetProducts()
        {
            // Query to select products with units in stock
            string query = "select * from Products where UnitsInStock > 0";

            // Executing the query and retrieving the data
            var data = ExecuteQuery<Product>(query);

            // Returning the list of products
            return data;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch suppliers from the data source
        /// </summary>
        /// <returns>List of suppliers</returns>
        private IList<object> GetSuppliers()
        {
            // Query to select all suppliers
            string query = "select * from Suppliers";

            // Executing the query and retrieving the data
            var data = ExecuteQuery<Supplier>(query);

            // Returning the list of suppliers
            return data;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to check if the products dataset has been updated since the last refresh time
        /// </summary>
        /// <param name="dataSet">Name of dataset</param>
        /// <param name="dateTime">Last refresh time</param>
        /// <returns>True if updated, otherwise false</returns>
        private bool IsProductUpdated(string dataSet, object dateTime)
        {
            // Checking for updates in products dataset
            bool result = false;
            string query = $"select count(*) from dbo.Products where LastModify > '{dateTime as DateTime?}'";
            result = ExecuteAggregateQuery(query) > 0;

            return result;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to check if the suppliers dataset has been updated since the last refresh time
        /// </summary>
        /// <param name="dataSet">Name of dataset</param> 
        /// <param name="dateTime">Last refresh time</param>
        /// <returns>True if updated, otherwise false</returns>
        private bool IsSupplierUpdated(string dataSet, object dateTime)
        {
            // Checking for updates in suppliers dataset
            bool result = false;
            string query = $"select count(*) from dbo.Suppliers where LastModify > '{dateTime as DateTime?}'";
            result = ExecuteAggregateQuery(query) > 0;

            return result;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch updated products since the last refresh time
        /// </summary>
        /// <param name="dateTime">Last refresh time</param>
        /// <returns>List of updated products</returns>
        private IList<object> FetchUpdatedProducts(object dateTime)
        {
            // Query to select products modified after the given dateTime
            string query = $"select * from dbo.Products where LastModify > '{dateTime as DateTime?}'";

            // Executing the query and retrieving the data
            var data = ExecuteQuery<Product>(query);

            // Returning the list of updated products
            return data;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch updated suppliers since the last refresh time
        /// </summary>
        /// <param name="dateTime">Last refresh time</param>
        /// <returns>List of updated suppliers</returns>
        private IList<object> FetchUpdatedSuppliers(object dateTime)
        {
            // Query to select suppliers modified after the given dateTime
            string query = $"select * from dbo.Suppliers where LastModify > '{dateTime as DateTime?}'";

            // Executing the query and retrieving the data
            var data = ExecuteQuery<Supplier>(query);

            // Returning the list of updated suppliers
            return data;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to execute an aggregate query and return the integer result
        /// </summary>
        /// <param name="Query">query to execute</param>
        /// <returns>Integer result of the aggregate query</returns>
        private int ExecuteAggregateQuery(string Query)
        {
            // Variable to hold the result
            int result = 0;

            // Executing the query and retrieving the result
            using (SqlConnection myConnection = new SqlConnection(_connectionString))
            {
                // Creating command
                SqlCommand oCmd = new SqlCommand(Query, myConnection);

                // Opening connection and executing reader
                myConnection.Open();
                using (var reader = oCmd.ExecuteReader())
                {
                    // Reading the result from the data reader
                    while (reader.Read())
                    {
                        // Converting the result to integer
                        result = Convert.ToInt32(reader[0]);
                    }
                }
                // Closing the connection
                myConnection.Close();
            }

            // Returning the aggregate result
            return result;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to execute a query and retrieve data as a list of objects
        /// </summary>
        /// <param name="Query">query to execute</param>
        /// <returns>List of objects retrieved from the query</returns>
        private IList<object> ExecuteQuery<T>(string Query)
        {
            // Variable to hold the result set
            IList<object> Data;

            // Executing the query and retrieving the data
            using (SqlConnection myConnection = new SqlConnection(_connectionString))
            {
                // Creating command
                SqlCommand oCmd = new SqlCommand(Query, myConnection);

                // Opening the connection
                myConnection.Open();

                // Executing reader and mapping data to objects
                using (var reader = oCmd.ExecuteReader())
                {
                    // Getting data from the reader
                    Data = GetData<T>(reader);
                }

                // Closing the connection
                myConnection.Close();
            }

            // Returning the list of objects
            return Data;
        }

        /// -------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to map data from data reader to Product or Supplier objects
        /// </summary>
        /// <param name="sqlDataReader">data reader</param>
        /// <returns>List of mapped objects</returns>
        private IList<object> GetData<T>(SqlDataReader sqlDataReader)
        {
            // List to hold the mapped objects
            IList<object> dataList = new List<object>();

            // Reading data from the data reader
            while (sqlDataReader.Read())
            {
                // Mapping data to Supplier objects
                if (typeof(T) == typeof(Supplier))
                {
                    // Creating and populating Supplier object
                    Supplier supplier = new Supplier()
                    {
                        SupplierID = Convert.ToInt32(sqlDataReader["SupplierID"]),
                        CompanyName = sqlDataReader["CompanyName"].ToString(),
                        ContactName = sqlDataReader["ContactName"].ToString(),
                        Address = sqlDataReader["Address"].ToString()
                    };

                    // Adding the Supplier object to the list
                    dataList.Add(supplier);
                }

                // Mapping data to Product objects
                if (typeof(T) == typeof(Product))
                {
                    // Creating and populating Product object
                    Product product = new Product()
                    {
                        ProductName = sqlDataReader["ProductName"].ToString(),
                        ProductID = Convert.ToInt32(sqlDataReader["ProductID"]),
                        UnitsAvailable = Convert.ToInt32(sqlDataReader["UnitsInStock"]),
                        UnitPrice = Convert.ToInt32(sqlDataReader["UnitPrice"])
                    };

                    // Adding the Product object to the list
                    dataList.Add(product);
                }
            }
            // Returning the list of mapped objects
            return dataList;
        }
    }
}