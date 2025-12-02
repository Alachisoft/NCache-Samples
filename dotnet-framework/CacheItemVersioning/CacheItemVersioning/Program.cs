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
using System.Configuration;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Sample.Data;

// ===============================================================================
// Cache item versioning allows you to track changes to cache items over time by
// associating a version number with each item. This version number is incremented
// each time the item is updated in the cache. Some use cases of this include:
//   1. Concurrency Control: Ensuring that updates to cache items are based on
//      the most recent version, preventing lost updates.
//   2. Change Tracking: Keeping track of changes made to cache items over time.
//   3. Data Synchronization: Synchronizing cache items with external data sources
//      by comparing version numbers.
//   4. Conflict Resolution: Resolving conflicts when multiple clients attempt to
//      update the same cache item simultaneously.
//   5. Auditing: Maintaining a history of changes made to cache items for
//      auditing purposes.
// ===============================================================================

namespace CacheItemVersioning
{
    internal class Program
    {
        // Connect to a running cache and get cache handle for it 
        static ICache _cache = GetCache();

        static void Main(string[] args)
        {
            // Creating instance of Customer and key to use in this sample
            Customer customer = CreateNewCustomer();
            string key = GetKey(customer);

            // Adding item in cache and fetching the cache item version
            CacheItemVersion itemVersion = AddToCache(key, customer);

            // Updating item in cache if cache item version matches and getting latest cache item version
            CacheItemVersion latestItemVersion = UpdateInCache(key, customer, itemVersion);

            // Fetching item from cache using old version.
            GetItemWithVersion(key, itemVersion);

            // Fetching item from cache using latest version.
            GetItemWithVersion(key, latestItemVersion);

            // Removing item from cache using latest version
            RemoveWithVersion(key, latestItemVersion);

            // Dispose the cache once done
            _cache.Dispose();
        }

        /// ------------------------------------------------------------------------------
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

            // Connecting to a running cache and return a cache handle for it
            ICache cache = CacheManager.GetCache(cacheName);

            // Printing output on console
            Console.WriteLine(string.Format("Cache '{0}' is connected.", cacheName));
            return cache;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to generate instance of Customer to be used in this sample
        /// </summary>
        /// <returns>Instance of Customer</returns>
        static Customer CreateNewCustomer()
        {
            // Creating an instance of Customer and populating data
            Customer customer = new Customer
            {
                CustomerID = "SCOPR",
                ContactName = "Scott Prince",
                CompanyName = "Lonesome Pine Restaurant",
                ContactNo = "25632-5646",
                Address = "95-A Barnes Road Wallingford, CT",
            };

            // Returning customer instance
            return customer;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to generate a string key for specified customer
        /// </summary>
        /// <param name="customer">Instance of Customer to generate a key</param>
        /// <returns>Key for the specified customer</returns>
        static string GetKey(Customer customer)
        {
            return string.Format("Customer:{0}", customer.CustomerID);
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to add object in the cache and return cache item version
        /// </summary>
        /// <param name="key">Key to be added in cache</param>
        /// <param name="customer">Instance of Customer that will be inserted in the cache</param>
        /// <returns>Cache item version</returns>
        static CacheItemVersion AddToCache(string key, Customer customer)
        {
            // Adding item in cache
            CacheItemVersion cacheItemVersion = _cache.Add(key, customer);

            // Printing output on console
            Console.WriteLine("Item is added to cache.");

            // Returning cache item version
            return cacheItemVersion;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to update object in the cache if cache item version matches 
        /// </summary>
        /// <param name="key">Key to be updated in cache</param>
        /// <param name="customer">Instance of Customer that will be updated in the cache</param>
        /// <param name="currentVersion">Instance of CacheItemVersion to verify item is updated in cache</param>
        /// <returns>Updated cache item version</returns>
        static CacheItemVersion UpdateInCache(string key, Customer customer, CacheItemVersion currentVersion)
        {
            // Changing the customer details
            customer.CompanyName = "Gourmet Lanchonettes";

            // Creating a CacheItem from customer instance and assigning current Item Version
            CacheItem cacheItem = new CacheItem(customer);
            cacheItem.Version = currentVersion;

            // Updating item in cache if current version matches with the version in cache
            CacheItemVersion newVersion = _cache.Insert(key, cacheItem);

            // Checking if item version has changed
            if (currentVersion.Version != newVersion.Version)
            {
                // Printing output on console
                Console.WriteLine("Item has changed since last time it was fetched.");
            }

            // Returning updated cache item version
            return newVersion;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch and display object from cache if the provided cache item version matches latest in cache
        /// </summary>
        /// <param name="key">Key of item</param>
        /// <param name="version">Instance of CacheItemVersion</param>
        static void GetItemWithVersion(string key, CacheItemVersion version)
        {
            // Fetching item from cache if cache item version is newer than provided version
            Customer customer = _cache.GetIfNewer<Customer>(key, ref version);

            // Checking if item is null
            if (customer == null)
            {
                // This means item version provided is the latest in cache
                Console.WriteLine("Specified item version is latest.");
            }
            else
            {
                // This means item version provided is obsolete and item is fetched from cache
                Console.WriteLine("Specified item version is obsolete. Fetched item from cache:");
                PrintCustomerDetails(customer);
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to delete an object in the cache using cache item version
        /// </summary>
        /// <param name="key"> String key to be removed from cache</param>
        /// <param name="cacheItemVersion"> Instance of CacheItemVersion to remove item from cache if item version is same in cache</param>
        static void RemoveWithVersion(string key, CacheItemVersion cacheItemVersion)
        {
            // Removing object from cache if cache item version matches in cache
            _cache.Remove(key, null, cacheItemVersion);

            // Printing output on console
            Console.WriteLine("Customer is removed from cache.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to print details of customer type
        /// </summary>
        /// <param name="customer">Customer instance whose attributes are to be printed</param>
        static void PrintCustomerDetails(Customer customer)
        {
            Console.WriteLine("Customer Details are as follows: ");
            Console.WriteLine("ContactName: " + customer.ContactName);
            Console.WriteLine("CompanyName: " + customer.CompanyName);
            Console.WriteLine("Contact No: " + customer.ContactNo);
            Console.WriteLine("Address: " + customer.Address);
            Console.WriteLine();
        }
    }
}
