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
using System.Configuration;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the sample
    /// </summary>
    public class LoaderAndRefresherUsage
    {
        private static ICache _cache;

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
            _cache = CacheManager.GetCache(cacheId);

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

            Supplier cachedSupplier = null;

            // Order Key is supplier id, reteriving initial some orders from cache.
            foreach (DictionaryEntry cacheEntry in _cache)
            {
                // Try to get the supplier again, getting non-existing items returns null
                try
                {
                    cachedSupplier = _cache.Get<Supplier>(cacheEntry.Key as string) as Supplier;
                    printOrderDetails(cachedSupplier);

                }
                catch (Exception ex) { /*handle exception here.*/}
            }
        }

        /// <summary>
        /// This method prints detials of supplier type.
        /// </summary>
        /// <param name="cachedSupplier"></param>
        private static void printOrderDetails(Supplier cachedSupplier)
        {
            if (cachedSupplier == null) return;

            Console.WriteLine();
            Console.WriteLine("Supplier Details are as follows: ");
            Console.WriteLine("ID: " + cachedSupplier.Id);
            Console.WriteLine("CompanyName: " + cachedSupplier.CompanyName);
            Console.WriteLine("ContactName: " + cachedSupplier.ContactName);
            Console.WriteLine("Address: " + cachedSupplier.Address);
            
            Console.WriteLine();
        }
    }
}
