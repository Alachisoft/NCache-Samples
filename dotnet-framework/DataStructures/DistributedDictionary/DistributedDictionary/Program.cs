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
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using Alachisoft.NCache.Sample.Data;



// ===============================================================================
// Distributed Counter is a powerful data structure provided by NCache for
// high-concurrency scenarios where multiple clients or application servers need
// to increment, decrement, or read a shared numeric value atomically and with low
// latency. Some possible use cases of distributed counters include:
//   1. Counting page views or clicks on a website
//   2. Tracking inventory levels in an e-commerce application    
//   3. Implementing rate limiting for APIs or services
//   4. Managing distributed locks or semaphores
//   5. Real-time analytics and monitoring
// Distributed counters are designed to be highly available and fault-tolerant,
// ===============================================================================

namespace DistributedCounter
{
    public class Program
    {
        // Cache handle for the connected cache
        static ICache _cache;

        static void Main(string[] args)
        {
            // Getting cache handle
            _cache = GetCache();

            // Names of two dictionaries
            string topSellingName = "Products:TopSelling";
            string newArrivalsName = "Products:NewArrivals";

            // Creating or Getting Distributed Dictionary for Top Selling products
            IDistributedDictionary<string, Product> topSelling = CreateOrGetDict(topSellingName);

            // Creating or Getting Distributed Dictionary for New Arrivals products
            IDistributedDictionary<string, Product> newArrivals = CreateOrGetDict(newArrivalsName);

            // Adding products to the Top Selling dictionary
            AddTopSellingProducts(topSelling);

            // Adding products to the New Arrivals dictionary
            AddNewArrivalProducts(newArrivals);

            // Fetching and displaying products from Top Selling dictionary
            Console.WriteLine("Displaying Top Selling Products:");
            PrintProducts(topSellingName);

            // Fetching and displaying products from New Arrivals dictionary
            Console.WriteLine("Displaying New Arrival Products:");
            PrintProducts(newArrivalsName);

            // Updating a product in the Top Selling dictionary
            Console.WriteLine("Updating Prod:1003 in Top Selling Products");

            // Fetching existing product from the dictionary
            if (topSelling.TryGetValue("Prod:1003", out var existingProduct))
            {
                // Modifying the product's stock
                existingProduct.UnitsInStock = 15;

                // Saving the updated product back to the dictionary
                topSelling["Prod:1003"] = existingProduct;
            }

            // Removing a product from the Top Selling dictionary
            topSelling.Remove("Prod:1005");
            Console.WriteLine($"Removed Prod:1005 from {topSellingName}");

            // Displaying updated Top Selling products
            Console.WriteLine("Displaying Updated Top Selling Products");
            PrintProducts(topSellingName);

            // Deleting both distributed dictionaries from cache
            DeleteDictionary(topSellingName);
            DeleteDictionary(newArrivalsName);

            // Dispose the cache once done
            _cache.Dispose();
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to connect to a running cache and return cache handle for it 
        /// </summary>
        /// <returns>Cache handle for the connected cache</returns>
        private static ICache GetCache()
        {
            string cacheName = ConfigurationManager.AppSettings["CacheName"];

            if (String.IsNullOrEmpty(cacheName))
            {
                Console.WriteLine("The CacheName cannot be null or empty.");
                return null;
            }

            // Connecting to a running cache and return a cache handle for it
            _cache = CacheManager.GetCache(cacheName);

            // Printing output on console
            Console.WriteLine(string.Format("\nCache '{0}' is connected.", cacheName));
            return _cache;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to create a distributed Dictionary to which instances of orders are to be added
        /// </summary>
        /// <param name="DictionaryName">The name of Dictionary as key</param>
        /// <returns>Instance of IDistributedDictionary</returns>
        private static IDistributedDictionary<string, Product> CreateOrGetDict(string DictionaryName)
        {
            // Trying to get existing distributed Dictionary from cache
            IDistributedDictionary<string, Product> dictionary =
                _cache.DataTypeManager.GetDictionary<string, Product>(DictionaryName);

            // Checking if dictionary already exists
            if (dictionary == null)
            {
                // Defining attributes for distributed Dictionary
                DataTypeAttributes attributes = new DataTypeAttributes
                {
                    // Setting absolute expiration of 1 day for this dictionary
                    Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromDays(1))
                };

                // Creating a new distributed Dictionary in cache
                dictionary = _cache.DataTypeManager.CreateDictionary<string, Product>(DictionaryName, attributes);
            }

            // Returning the distributed Dictionary
            return dictionary;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Adds a list of top selling products into the given distributed dictionary
        /// </summary>
        /// <param name="dict">Distributed Dictionary to add top selling products to</param>
        private static void AddTopSellingProducts(IDistributedDictionary<string, Product> dict)
        {
            // Creating list of top selling products
            List<Product> topSellingProducts = new List<Product>()
            {
                new Product { ProductID = 1001, ProductName = "Macbook M3", Category = "Electronics", UnitPrice = 1500, UnitsInStock = 20 },
                new Product { ProductID = 1002, ProductName = "Wireless Headphones", Category = "Accessories", UnitPrice = 99, UnitsInStock = 200 },
                new Product { ProductID = 1003, ProductName = "Sneakers Classic", Category = "Footwear", UnitPrice = 79, UnitsInStock = 80 },
                new Product { ProductID = 1004, ProductName = "4K LED TV", Category = "Electronics", UnitPrice = 800, UnitsInStock = 50 },
                new Product { ProductID = 1005, ProductName = "Espresso Coffee Maker", Category = "Home Appliances", UnitPrice = 120, UnitsInStock = 100 }
            };

            // Iterating through the list
            foreach (Product product in topSellingProducts)
            {
                // Adding each product to the distributed dictionary
                dict.Add("Prod:" + product.ProductID, product);
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Adds a list of newly arrived products into the given distributed dictionary
        /// </summary>
        /// <param name="dict">Dictionary to add new arrival products to</param>
        private static void AddNewArrivalProducts(IDistributedDictionary<string, Product> dict)
        {
            // Creating list of new arrival products
            List<Product> newArrivalProducts = new List<Product>()
            {
                new Product { ProductID = 2001, ProductName = "Classic Denim Jacket", Category = "Clothing", UnitPrice = 65, UnitsInStock = 150 },
                new Product { ProductID = 2002, ProductName = "VR Gaming Set", Category = "Gaming", UnitPrice = 450, UnitsInStock = 60 },
                new Product { ProductID = 2003, ProductName = "Smart Home Speaker", Category = "Electronics", UnitPrice = 120, UnitsInStock = 90 },
                new Product { ProductID = 2004, ProductName = "Electric Scooter", Category = "Transportation", UnitPrice = 300, UnitsInStock = 40 },
                new Product { ProductID = 2005, ProductName = "Noise-Cancelling Earbuds", Category = "Accessories", UnitPrice = 150, UnitsInStock = 110 }
            };

            // Iterating through the list
            foreach (Product product in newArrivalProducts)
            {
                // Adding each product to the distributed dictionary
                dict.Add("Prod:" + product.ProductID, product);
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to print details of product
        /// </summary>
        /// <param name="dictName">Name of distributed Dictionary whose values are to be printed</param>
        private static void PrintProducts(string dictName)
        {
            // Getting distributed Dictionary from cache
            var dict = _cache.DataTypeManager.GetDictionary<string, Product>(dictName);

            // Checking if dictionary is null or empty
            if (dict == null || dict.Count == 0)
            {
                Console.WriteLine($"\nProduct List ({dictName}) is empty or removed.");
                return;
            }

            // Printing header on console
            Console.WriteLine($"\nProduct List ({dictName}) contains {dict.Count} product(s):");

            // Iterating through the dictionary and printing product details
            foreach (var item in dict)
            {
                // Printing product details on console
                Console.WriteLine($"  - {item.Key} | Name: {item.Value.ProductName}, Price: {item.Value.UnitPrice}, Stock: {item.Value.UnitsInStock}");
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to remove distributed Dictionary from cache
        /// </summary>
        /// <param name="dictionaryName">The name of distributed Dictionary to be removed from cache</param>
        private static void DeleteDictionary(string dictionaryName)
        {
            // Removing Dictionary from the Cache
            _cache.DataTypeManager.Remove(dictionaryName);

            // Printing output on console.
            Console.WriteLine("Distributed Dictionary successfully removed from cache.");
        }


    }
}
