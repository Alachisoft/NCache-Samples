﻿// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Dependencies;
using System.Configuration;
using Alachisoft.NCache.Client;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that implements SQL DB dependency
    /// </summary>
	public class PollingDependency
	{
        private static ICache _cache;

        /// <summary>
        /// Executing this method will perform all the operations to configure SQL DB dependency
        /// </summary>
		public static void Run()
		{
			try
			{
				// Initialize cache
                InitializeCache();

                // Create and apply SQL DB dependency
                AddDependency();

                // Dispose the cache once done
				_cache.Dispose();
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
			}
		}

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The Cache Name cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = CacheManager.GetCache(cache);
        }

        /// <summary>
        /// This method configures SQLDB dependency
        /// </summary>
        private static void AddDependency()
        {
            // Scripts are provided to enable 
            // DBNotifications to NCache

            // Notification based DBDependency
            // JDBC connectionString                         

            string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
            if (String.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("The connection string cannot be null or empty.");
                return;
            }

            for (int i = 0; i < 10; i++)
            {
                string key = i + ":dbo.Products";
                // Cache key for product id 10 will be "10:dbo.Products"
                CacheDependency dependency = DBDependencyFactory.CreateSqlCacheDependency(connectionString, key);
                CacheItem cacheitem = new CacheItem(i);
                cacheitem.Dependency = dependency;


                _cache.Insert(key, cacheitem);
            }
            Console.WriteLine("Products added in the cache with SQL cache dependency.");

            // To Verify that the polling dependency works uncomment the following code.
            // The code waits for the user to update products with productId 6.
            // Once the item is updated in the database the cache item is invalidated and thus removed from the cache.

            //Console.WriteLine("Update product with id 6 in database and press enter...");
            //Console.Read();

            //// Demo is specified for cache_key 6:oldRow.Products 
            //if (_cache.Contains("6:oldRow.Products"))
            //{
            //    Console.WriteLine("Database dependency did not work as expected. " + "Check your connection string and sql command syntax.");
            //}
            //else
            //{
            //    Console.WriteLine("6:oldRow.Products removed due to SQL DB dependency.");
            //}
        }
	}
}