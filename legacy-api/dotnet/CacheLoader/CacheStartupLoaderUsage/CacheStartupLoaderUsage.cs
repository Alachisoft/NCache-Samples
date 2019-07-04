// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Web.Caching;
using System;
using System.Collections;
using System.Configuration;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the sample
    /// </summary>
    public class CacheStartupLoaderUsage
    {
        private static Cache _cache;

        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run()
        {
            // Initialize the cache
            InitializeCache();

            // Read data from cache
            ReadObjectsFromCache();
        }

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache()
        {
            string cacheId = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cacheId))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Web.Caching.NCache.InitializeCache(cacheId);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cacheId));
        }

        /// <summary>
        /// This method iterates over cache
        /// </summary>
        private static void ReadObjectsFromCache()
        {
            // Total item count in the cache
            // Count should be equal to total orders in database
            long count = _cache.Count;
            Console.WriteLine("\nCache Count: " + count);

            Order cachedOrder = null;

            // Order Key is order id, reteriving initial some orders from cache.
            foreach (DictionaryEntry cacheEntry in _cache)
            {
                // Try to get the order again, getting non-existing items returns null
                try
                {
                    cachedOrder = (Order)_cache.Get(cacheEntry.Key as string) as Order;
                    printOrderDetails(cachedOrder);

                }
                catch (Exception ex) { /*handle exception here.*/}
            }
        }

        /// <summary>
        /// This method prints detials of order type.
        /// </summary>
        /// <param name="customer"></param>
        private static void printOrderDetails(Order cachedOrder)
        {
            if (cachedOrder == null) return;

            Console.WriteLine();
            Console.WriteLine("Order Details are as follows: ");
            Console.WriteLine("ID: " + cachedOrder.OrderID);
            Console.WriteLine("OrderDate: " + cachedOrder.OrderDate);
            Console.WriteLine("ShipName: " + cachedOrder.ShipName);
            Console.WriteLine("ShipAddress: " + cachedOrder.ShipAddress);
            Console.WriteLine("ShipCity: " + cachedOrder.ShipCity);
            Console.WriteLine("ShipCountry: " + cachedOrder.ShipCountry);
            
            Console.WriteLine();
        }
    }
}
