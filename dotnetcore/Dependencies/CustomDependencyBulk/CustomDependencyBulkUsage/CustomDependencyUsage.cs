// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Runtime.Dependencies;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the sample
    /// </summary>
    public class CustomDependencyUsage
    {
        private static ICache _cache;
        private static SqlConnection _connection = null;
        private static string _connectionString;

        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run()
        {
            // Initialize cache 
            InitializeCache();

            // Initialize database connection
            InitializeDatabaseConnection();

            // Add data to cache with custom dependency
            string[] keys = AddDataIntoCache();

            // Update unit price of some products and wait for clean interval
            UpdateProducts();

            // Get objects from cache
            GetObjectsFromCache(keys);

            // Undo the update
            UndoUpdateOnProducts();

            // Dispose the cache once done
            _cache.Dispose();
        }

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Client.CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }

        /// <summary>
        /// This method initializes the database connection
        /// </summary>
        private static void InitializeDatabaseConnection()
        {
            _connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            if (String.IsNullOrEmpty(_connectionString))
            {
                Console.WriteLine("The connectionString cannot be null or empty.");
                return;
            }

            _connection = new SqlConnection(_connectionString);
            _connection.Open();

            // Print output on console
            Console.WriteLine("\nDatabase connection is initialized.");
        }

        /// <summary>
        /// This method Adds objects to cache
        /// </summary>
        /// <returns>List of keys added to cache </returns>
        private static string[] AddDataIntoCache()
        {
            List<Product> products = LoadProductsFromDatabase();
            CacheItem[] items = new CacheItem[products.Count];
            string[] keys = new String[products.Count];
            for (int i = 0; i < products.Count; i++)
            {
                keys[i] = products[i].Id.ToString();
                CacheItem item = new CacheItem(products[i]);
                
                IDictionary<string, string> param = new Dictionary<string, string>();
                param.Add("ProductID", products[i].Id.ToString());
                param.Add("UnitsAvailable", products[i].UnitsAvailable.ToString());
                param.Add("ConnectionString", _connectionString);
                CustomDependency customDependency = new CustomDependency(ConfigurationManager.AppSettings["ProviderName"], param);
                item.Dependency = customDependency;
                items[i] = item;
            }

            IDictionary<string, CacheItem> cacheItems = GetCacheItemDictionary(keys, items);

            var res = _cache.InsertBulk(cacheItems);

            // Print output on console
            Console.WriteLine("\nItems are added to Cache.");

            return keys;
        }

        /// <summary>
        /// Loads objects from the datasource.
        /// </summary>
        /// <returns>List of orders </returns>
        private static List<Product> LoadProductsFromDatabase()
        {
            List<Product> products = new List<Product>();
            SqlCommand command = new SqlCommand(
                 "select ProductID, ProductName, UnitsInStock " +
                "from Products"
                , _connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();
                product.Id = Convert.ToInt32(reader["ProductID"].ToString());
                product.Name = (reader["ProductName"].ToString());
                int units = 0;
                Int32.TryParse(reader["UnitsInStock"].ToString(), out units);
                product.UnitsAvailable = units;

                products.Add(product);

            }
            reader.Close();

            return products;
        }

        /// <summary>
        /// Generates hashmap of Cache Items and keys 
        /// </summary>
        /// <param name="keys"> List of keys to generate hashmap</param>
        /// <param name="cacheItems"> List of cache items to generate hashmap</param>
        /// <returns> returns Dictionary of Cache Items and keys </returns>
        private static IDictionary<string, CacheItem> GetCacheItemDictionary(string[] keys, CacheItem[] cacheItems)
        {
            IDictionary<string, CacheItem> items = new Dictionary<string, CacheItem>();

            for (int i = 0; i < cacheItems.Length; i++)
            {
                items.Add(keys[i], cacheItems[i]);
            }

            return items;
        }

        /// <summary>
        /// This method gets objects from the cache
        /// </summary>
        /// <param name="key"> String keys to get objects from cache </param>
        private static void GetObjectsFromCache(string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                Product cachedProduct = _cache.Get<Product>(keys[i]);
                if (cachedProduct == null)
                {
                    Console.WriteLine(string.Format("Removed item with id : {0}", keys[i]));
                }
            }
        }

        /// <summary>
        /// Update unit available in stock of some products
        /// </summary>
        private static void UpdateProducts()
        {
            SqlCommand command = new SqlCommand(
                "UPDATE Products " +
                "set UnitsInStock = 0 " +
                "WHERE ProductID in (14,15,16,17,18,19,20)"
                , _connection);

            command.ExecuteNonQuery();

            // Print output on console
            Console.WriteLine("\nItems modified in database.");

            // Wait for clean interval 
            Thread.Sleep(20000);
        }

        /// <summary>
        /// Undo the update on products
        /// </summary>
        private static void UndoUpdateOnProducts()
        {
            SqlCommand command = new SqlCommand(
                "UPDATE Products " +
                "set UnitsInStock = 30 " +
                "WHERE ProductID in (14,15,16,17,18,19,20)",
                _connection);

            command.ExecuteNonQuery();
        }
    }
}
