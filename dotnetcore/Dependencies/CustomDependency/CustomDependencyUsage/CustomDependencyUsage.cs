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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            // Update data in database and wait for clean interval
            UpdateUnitsInStockInDatabase(keys);

            // Get objects from cache
            GetObjectsFromCache(keys);
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
            _connectionString = ConfigurationManager.AppSettings["connectionString"];

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
                item.Dependency = new CacheDependency(new Dependency(products[i].Id, _connectionString));
                items[i] = item;
            }

            IDictionary<string, CacheItem> cacheItems = GetCacheItemDictionary(keys, items);

            _cache.AddBulk(cacheItems);

            // Print output on console
            Console.WriteLine("Items are added to Cache.");

            return keys;
        }

        /// <summary>
        /// Loads objects from the datasource.
        /// </summary>
        /// <returns>List of products </returns>
        private static List<Product> LoadProductsFromDatabase()
        {
            List<Product> products = new List<Product>();
            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                "Select ProductID, ProductName, UnitsInStock" +
                " From Products where ProductID < 16");

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();

                product.Id = (int)reader["ProductID"];
                product.Name = reader["ProductName"].ToString();
                product.UnitsAvailable = Convert.ToInt32(reader["UnitsInStock"].ToString());

                products.Add(product);
            }
            reader.Close();

            return products;
        }

        /// <summary> 
        /// Updates the quantity in the database 
        /// </summary>
        /// <param name="productID"></param>
        private static void UpdateUnitsInStockInDatabase(string[] keys)
        {
            int quantity = 0;

            for (int i = 0; i < keys.Length; i++)
            {
                SqlCommand cmd = _connection.CreateCommand();

                cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                    "Update Products " +
                    "Set UnitsInStock = {0} " +
                    "Where ProductID = {1}",
                    quantity, keys[i]);

                cmd.ExecuteNonQuery();
            }

            // Print output on console
            Console.WriteLine("\nItems are modified in database.");

            // Wait for clean interval 
            Thread.Sleep(15000);
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
                Product cachedCustomer = _cache.Get<Product>(keys[i]);
                if (cachedCustomer == null)
                {
                    Console.WriteLine(string.Format("Item with key {0} is removed from cache ", keys[i]));
                }
            }
        }
    }
}
