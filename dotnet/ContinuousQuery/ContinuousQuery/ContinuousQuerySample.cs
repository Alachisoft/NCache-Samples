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
using System.Collections;
using Alachisoft.NCache.Runtime.Events;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Sample.Data;
using System.Configuration;
using System.Threading;

namespace Alachisoft.NCache.Samples
{
	public class ContinuousQuerySample
	{
        static ICache _cache;

		public static void Run()
		{
		    // Initialize new cache
            InitializeCache();

            // Generate instance of Customer that will be used in different operations in this sample
            Customer customer = CreateNewCustomer();
            string key = GetKey(customer);

            // Adding item to cache
            AddObjectToCache(key, customer);

            // Build Query Command
            QueryCommand queryCommand = BuildQueryCommand();

            // Register continuous query on cache
            ContinuousQuery continuousQuery = RegisterQuery(queryCommand);

            // Query keys in cache as per criteria
            QueryKeysInCache(queryCommand);

            // Query data in cache as per criteria
            QueryDataInCache(queryCommand);

            // Update an item in cache that is within query criteria to raise data modification event
            UpdateObjectInCache(key, customer);

            // Delete the existing object
            DeleteObjectFromCache(key);

            // Unregister query 
            UnRegisterQuey(continuousQuery);
          
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
        /// This method adds object in the cache
        /// </summary>
        /// <param name="key"> String key to be added in cache </param>
        /// <param name="customer"> Instance of Customer that will be inserted in the cache </param>
        private static void AddObjectToCache(string key, Customer customer)
        {
            // Add new customer object in cache
            _cache.Add(key, customer);

            // Print output on console
            Console.WriteLine("\nItem is added to cache.");
        }

        /// <summary>
        /// Build Query to execute
        /// </summary>
        /// <returns>returns instance of QueryCommand</returns>
        private static QueryCommand BuildQueryCommand()
        {
            string query = "SELECT $VALUE$ FROM Alachisoft.NCache.Sample.Data.Customer WHERE Country = ?";

            QueryCommand queryCommand= new QueryCommand(query);

            queryCommand.Parameters.Add("Country", "USA");

            return queryCommand;
        }

        /// <summary>
        /// Register continuous query on cache
        /// </summary>
        /// <returns> returns instance of registered continuous query instance </returns>
        private static ContinuousQuery RegisterQuery(QueryCommand queryCommand)
        {

            ContinuousQuery continuousQuery = new ContinuousQuery(queryCommand);

            // Add query notifications
            continuousQuery.RegisterNotification(new QueryDataNotificationCallback(QueryDataModified), EventType.ItemAdded, EventDataFilter.None);

            // Item update notification with DataWithMetadata as event data filter to recieve modifed item on updation
            continuousQuery.RegisterNotification(new QueryDataNotificationCallback(QueryDataModified), EventType.ItemUpdated, EventDataFilter.DataWithMetadata);

            // Item delete notification
            continuousQuery.RegisterNotification(new QueryDataNotificationCallback(QueryDataModified), EventType.ItemRemoved, EventDataFilter.None);

            // Register continuous query on server
            _cache.MessagingService.RegisterCQ(continuousQuery);

            // Print output on console
            Console.WriteLine("\nContinuous Query is registered.");

            return continuousQuery;
        }

        /// <summary>
        /// This method queries cached keys
        /// </summary>
        /// <param name="customer"> Instance of Query Command to query cached items</param>
        private static void QueryKeysInCache(QueryCommand queryCommand)
        {
            // Getting keys via query
            //ICacheReader cacheReader = _cache.ExecuteReaderCQ(continuousQuery, false);
            ICacheReader cacheReader = _cache.SearchService.ExecuteReader(queryCommand, false);
            if (!cacheReader.IsClosed)
            {
                // Print output on console
                Console.WriteLine("\nFollowing keys are fetched with Continuous Query.");

                while (cacheReader.Read())
                {
                    string key = cacheReader.GetValue<string>(0);

                    Console.WriteLine("Key: " + key);

                    // A second call to cache to fetch the customer
                    //Customer cachedCustomer = (Customer)_cache.Get(key);
                }
            }
        }

        /// <summary>
        /// This method queries cached data
        /// </summary>
        /// <param name="customer"> Instance of registered continuous query to query cached items</param>
        private static void QueryDataInCache(QueryCommand queryCommand)
        {
            // Getting keyvalue pair via query
            // Much faster way ... avoids round trip to cache
            //ICacheReader cacheReader = _cache.ExecuteReaderCQ(continuousQuery, true);
            ICacheReader cacheReader = _cache.SearchService.ExecuteReader(queryCommand, true);
            if (!cacheReader.IsClosed)
            {
                // Print output on console
                Console.WriteLine("\nFollowing items are fetched with Continuous Query.");

                while (cacheReader.Read())
                {
                    var asd = cacheReader.GetValue<object>(0);
                    Customer customerFound = cacheReader.GetValue<Customer>(1);
                    PrintCustomerDetails(customerFound);
                }
            }
        }

        /// <summary>
        /// This method updates cached item that is within query criteria to raise an data modification event
        /// </summary>
        /// <param name="key"> String key to be updated in cache</param>
        /// <param name="customer"> Instance of registered continuous query to query cached items</param>
        private static void UpdateObjectInCache(string key, Customer customer)
        {
            // Modifiying and re-insert the item
            customer.CompanyName = "Gourmet Lanchonetes";

            // This should invoke query RegisterUpdateNotification 
            _cache.Insert(key, customer);

            // Print output on console
            Console.WriteLine("\nItem is updated in cache.");

            // Wait for the callback.
            Thread.Sleep(1000);
        }

        /// <summary>
        /// This method unregisteres continuous query on cache
        /// </summary>
        /// <param name="customer"> Instance of registered continuous </param>
        private static void UnRegisterQuey(ContinuousQuery continuousQuery)
        {
            // UnRegister the continuous query on server
            _cache.MessagingService.UnRegisterCQ(continuousQuery);

            // Print output on console
            Console.WriteLine("\nContinuous Query is unregistered.");
        }

        /// <summary>
        /// Delete an object in the cache
        /// </summary>
        /// <param name="key"> String key to be deleted from cache</param>
        private static void DeleteObjectFromCache(string key)
        {
            // Remove the existing customer
            _cache.Remove(key);

            // Print output on console
            Console.WriteLine("\nObject is deleted from cache.");

            // Wait for the callback.
            Thread.Sleep(1000);
        }

        /// <summary>
        /// This method handles data modification callbacks
        /// </summary>
        /// <param name="key"> Modifed string key </param>
        /// <param name="key"> Metada about updated item in cache</param>
        private static void QueryDataModified(string @string, CQEventArg cQEventArg)
        {
            // Print output on console
            Console.WriteLine("\nContinuous Query data modification event is received from cache.");

            switch (cQEventArg.EventType)
            {
                case EventType.ItemAdded: Console.WriteLine(@string + " is added to cache.");
                    break;

                case EventType.ItemRemoved: Console.WriteLine(@string + " is removed from cache.");
                    break;

                case EventType.ItemUpdated: 
                    Console.WriteLine(@string + " is updated in cache.");
                    if (cQEventArg.Item != null)
                    {
                        PrintCustomerDetails(cQEventArg.Item.GetValue<Customer>());
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Generates instance of Customer to be used in this sample
        /// </summary>
        /// <returns> returns instance of Customer </returns>
        private static Customer CreateNewCustomer()
        {
            return new Customer
            {
                CustomerID = "JONMA",
                ContactName = "John Mathew",
                CompanyName = "Lonesome Pine Restaurant",
                ContactNo = "25564-4546",
                Address = "92-A Barnes Road, WA, Ct.",
                Country = "USA"
            };
        }
        
        /// <summary>
        /// Generates a string key for specified customer
        /// </summary>
        /// <param name="customer"> Instance of Customer to generate a key</param>
        /// <returns> returns a key </returns>
        private static string GetKey(Customer customer)
        {
            return string.Format("Customer:{0}", customer.CustomerID);
        }

        /// <summary>
        /// This method prints detials of customer type.
        /// </summary>
        /// <param name="customer"></param>
        private static void PrintCustomerDetails(Customer customer)
		{
             Console.WriteLine();
             Console.WriteLine("Customer Details are as follows: ");
             Console.WriteLine("ContactName: " + customer.ContactName);
             Console.WriteLine("CompanyName: " + customer.CompanyName);
             Console.WriteLine("Contact No: " + customer.ContactNo);
             Console.WriteLine("Address: " + customer.Address);
             Console.WriteLine("Country: " + customer.Country);
             Console.WriteLine();
		}
	}
}