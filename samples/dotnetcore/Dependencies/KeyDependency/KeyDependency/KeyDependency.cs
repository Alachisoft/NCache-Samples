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
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Dependencies;
using Alachisoft.NCache.Sample.Data;
using System.Configuration;
using Alachisoft.NCache.Client;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// This class demostrate key based dependency
    /// </summary>
	public class KeyDependency
	{
        private static ICache _cache;

        /// <summary>
        /// Executing this method will perform all the operations to use key based dependency
        /// </summary>
		public static void Run()
		{
			// Initialize cache
            InitializeCache();

            // Add a single key dependency and modify parent object to see if dependent is removed
            AddSinglekeyDependency();

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
            _cache = CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }

        /// <summary>
        /// This method demonstrate a single key dependency 
        /// </summary>
        private static void AddSinglekeyDependency()
        {
            // Key dependency makes an item to get removed when
            // it's accosiated item is removed or has it's value changed.

            // Generate a new instance of customer
            Customer customer = new Customer { CustomerID= "Customer:76", ContactName = "David Johnes", CompanyName = "Lonesome Pine Restaurant", ContactNo = "12345-6789", Address = "Silicon Valley, Santa Clara, California"};
            
            // Add customer in cache
            _cache.Add(customer.CustomerID, customer);

            Console.WriteLine("\nItem 'Customer:DavidJohnes' is added to cache.");

            // Generate a new instance of order
            Order order = new Order { OrderID = 10248, OrderDate = DateTime.Parse("1996-08-16 00:00:00.000"), ShipAddress = "Carrera 22 con Ave. Carlos Soublette #8-35" };
            
            // Generate an instance of key dependency.
            CacheDependency dependency = new Alachisoft.NCache.Runtime.Dependencies.KeyDependency(customer.CustomerID);
	    // create CacheItem to with your desired object
            CacheItem cacheItem = new CacheItem(order);
	    // add key dependency
            cacheItem.Dependency = dependency;
            // Add order in cache with dependency on the customer added before
            _cache.Add("Order:"+order.OrderID, cacheItem);
            
            Console.WriteLine("Item 'Order:10248' dependent upon 'Customer:DavidJohnes' is added to cache.");
            
            // Note that an item can be dependent on multiple items in cache this can be done like this
            // CacheDependency multipleDependency = new Alachisoft.NCache.Runtime.Dependencies.KeyDependency(new string[] { "Customer:DavidJohnes", "Product:52" })

            // To Verify that key dependency is working uncomment the following code.
            // Any modification in the dependent item will cause invalidation of the dependee item.
            // Thus the item will be removed from cache.

            //// Delete customer from cache
            _cache.Remove(customer.CustomerID);

            //// Print output on console
            //Console.WriteLine("Item 'Customer:DavidJohnes' is deleted from cache.");

            //object value = _cache.Get<object>("Order:10248");

            //// value should be null
            //if (value == null)
            //{
            //    Console.WriteLine("Dependent item 'Order:10248' is successfully removed from cache.");
            //}
            //else
            //{
            //    Console.WriteLine("Error while removing the dependent items from cache.");
            //}
        }
	}
}
