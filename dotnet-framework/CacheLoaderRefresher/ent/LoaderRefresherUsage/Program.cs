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
using Alachisoft.NCache.Sample.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

// ===============================================================================
// NCache Loader/Refresher is mechanism to load and refresh data into NCache from
// a data source. The Loader is invoked on cache startup to populate the cache
// with initial data. The Refresher is invoked at runtime to refresh the data in
// cache based on the changes in the data source. Loaders and refreshers are
// particularly useful in scenarios where data consistency and performance are
// critical, such as in e-commerce applications, financial systems and real-time
// analytics platforms. Some use cases of NCache Loader/Refresher include:
//   1. Loading and refreshing lookup or reference data (e.g., country codes, tax
//      rates) into cache.
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

namespace LoaderRefresherUsage
{
    internal class Program
    {
        // Declaring the cache handle for the connected cache
        static ICache cache;

        static void Main(string[] args)
        {
            // Connecting to a running cache and get cache handle for it 
            cache = GetCache();

            // Validating cache handle
            if (cache == null)
            {
                Console.WriteLine("Cache connection failed. Exiting the sample.");
                return;
            }

            // Displaying cache items loaded from database on startup
            DisplayCacheItems();
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to connect to a running cache and return cache handle for it 
        /// </summary>
        /// <returns>Cache handle for the connected cache</returns>
        static ICache GetCache()
        {
            // Getting cache name from configuration file
            string cacheName = ConfigurationManager.AppSettings["CacheName"];

            // Validating cache name
            if (String.IsNullOrEmpty(cacheName))
            {
                Console.WriteLine("The CacheName cannot be null or empty.");
                return null;
            }

            // Trying to connect to cache
            try
            {
                // Connecting to a running cache and return a cache handle for it
                ICache cache = CacheManager.GetCache(cacheName);

                // Printing output on console
                Console.WriteLine(string.Format("Cache '{0}' is connected.", cacheName));

                // Returning the connected cache handle
                return cache;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to cache: " + ex.Message);
                return null;
            }
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to iterate over all cache items inserted by loader and refresher and print their details
        /// </summary>
        static void DisplayCacheItems()
        {
            // Printing total number of items currently in the cache
            Console.WriteLine($"\nCache Count: {cache.Count}");

            // Declaring dictionaries to collect items
            var products = new Dictionary<int, Product>();
            var suppliers = new Dictionary<int, Supplier>();

            // Iterating over all items in the cache using DictionaryEntry
            foreach (DictionaryEntry entry in cache)
            {
                // Extracting the cache key as a string
                string key = entry.Key as string;

                try
                {
                    // Fetching the object from cache by its key
                    // Using pattern matching to determine the type
                    switch (cache.Get<object>(key))
                    {
                        // If object is a Supplier, print supplier details
                        case Supplier s:
                            suppliers[s.SupplierID] = s;
                            break;

                        // If object is a Product, print product details
                        case Product p:
                            products[p.ProductID] = p;
                            break;

                        // Handling any other unexpected types
                        default:
                            Console.WriteLine($"Unknown type in cache for key: {key}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Catching and displaying any errors that occur during fetching
                    Console.WriteLine($"Error retrieving object for key {key}: {ex.Message}");
                }
            }

            // Printing all products
            Console.WriteLine("\nList of Products in cache: ");
            foreach (var p in products.OrderBy(x => x.Key))
            {
                PrintProductDetails(p.Value);
            }

            // Printing all suppliers
            Console.WriteLine("\nList of Suppliers in cache: ");
            foreach (var s in suppliers.OrderBy(x => x.Key))
            {
                PrintSupplierDetails(s.Value);
            }
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to print details of supplier type
        /// </summary>
        /// <param name="cachedSupplier">Supplier instance to be printed</param>
        static void PrintSupplierDetails(Supplier cachedSupplier)
        {
            if (cachedSupplier == null) return;

            Console.Write($"ID: {cachedSupplier.SupplierID}");
            Console.Write($"\tCompanyName: {cachedSupplier.CompanyName}");
            Console.Write($"\tContactName: {cachedSupplier.ContactName}");
            Console.WriteLine($"\tAddress: {cachedSupplier.Address}");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to print details of product type
        /// </summary>
        /// <param name="cachedProduct">Product instance to be printed</param>
        static void PrintProductDetails(Product cachedProduct)
        {
            if (cachedProduct == null) return;

            Console.Write($"ID: {cachedProduct.ProductID}");
            Console.Write($"\tUnitsAvailable: {cachedProduct.UnitsAvailable}");
            Console.Write($"\tUnitPrice: {cachedProduct.UnitPrice}");
            Console.WriteLine($"\tName: {cachedProduct.ProductName}");
        }
    }
}
