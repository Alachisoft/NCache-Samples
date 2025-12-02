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
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Sample.Data;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackingSource
{
    class Program
    {
        private static ICache cache;
        private static string readThruProvider;
        private static string writeThruProvider;

        static void Main(string[] args)
        {
            // Connect to a running cache and get cache handle for it
            cache = GetCache();

            // Fetch config values for providers once and reuse globally
            readThruProvider = ConfigurationManager.AppSettings["ReadThruProviderName"] ?? "SqlReadThruProvider";
            writeThruProvider = ConfigurationManager.AppSettings["WriteThruProviderName"] ?? "SqlWriteThruProvider";

            // Fetching a customer using ReadThru
            Customer customer = FetchCustomerUsingReadThru("ALFKI");

            // Updating the fetched customer using WriteThru
            UpdateCustomerUsingWriteThru(customer);

            // Demonstrating counter for company customer count
            DemonstrateCounter();

            // Demonstrating distributed data types
            DemonstrateDictionary("London");

            // Demonstrating distributed list for customers of a country
            DemonstrateList("UK");

            // Demonstrating distributed queue for customers
            DemonstrateQueue();

            // Demonstrating distributed hash set for order IDs of a customer
            DemonstrateHashSet(customer);

            // Dispose cache
            cache.Dispose();
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to connect to a running cache and return cache handle for it 
        /// </summary>
        /// <returns>Cache handle for the connected cache</returns>
        private static ICache GetCache()
        {
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

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch a customer using ReadThru
        /// </summary>
        /// <param name="customerId">Unique customer ID</param>
        /// <returns>Customer object if found, null otherwise</returns>
        private static Customer FetchCustomerUsingReadThru(string customerId)
        {
            Console.WriteLine("Fetching customer using ReadThru...");

            // Attempting to get customer from cache; if not found, ReadThru will fetch from DB
            Customer customer = cache.Get<Customer>(
                customerId,
                new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
            );

            // Printing fetched customer details
            if (customer != null)
            {
                Console.WriteLine("Customer fetched using ReadThru:");
                PrintCustomer(customer);
            }
            else
            {
                Console.WriteLine("Customer not found.\n");
            }

            return customer;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to update a customer using WriteThru
        /// </summary>
        /// <param name="customer">Customer object to be updated</param>
        private static void UpdateCustomerUsingWriteThru(Customer customer)
        {
            // Validating customer object
            if (customer == null)
                return;

            Console.WriteLine("Updating customer using WriteThru...");

            // Modifying customer contact name
            customer.ContactName = "Updated Contact - Demo";
            CacheItem item = new CacheItem(customer);

            // Inserting updated customer back into cache; WriteThru will update DB as well
            cache.Insert(
                customer.CustomerID,
                item,
                new WriteThruOptions(WriteMode.WriteThru, writeThruProvider)
            );

            Console.WriteLine("Customer updated successfully via WriteThru.\n");
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate distributed counter for company customer count
        /// </summary>
        private static void DemonstrateCounter()
        {
            Console.WriteLine("Demonstrating Distributed Counter...");

            // Obtaining counter instance from cache
            ICounter counter = cache.DataTypeManager.GetCounter(
                "North/South",
                new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
            );

            Console.WriteLine("Current Counter Value: " + counter.Value);
            counter.Increment();
            Console.WriteLine("New Counter Value: " + counter.Value + "\n");
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate distributed dictionary for customers of a city
        /// </summary>
        /// <param name="city">City name for which customers are to be fetched</param>
        private static void DemonstrateDictionary(string city)
        {
            Console.WriteLine(string.Format("Distributed Dictionary for city '{0}'...", city));

            // Obtaining distributed dictionary instance from cache
            var dict = cache.DataTypeManager.GetDictionary<string, object>(
                city,
                new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
            );

            // Validating dictionary
            if (dict == null)
            {
                Console.WriteLine("No dictionary found.\n");
                return;
            }

            // Displaying retrieved customers
            Console.WriteLine("Retrieved " + dict.Count + " customers.");

            foreach (var pair in dict)
            {
                PrintCustomer((Customer)pair.Value);
            }

            Console.WriteLine("Updating first dictionary record...");

            dict.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, writeThruProvider);

            foreach (var key in dict.Keys)
            {
                // Update the contact name of the customer
                Customer cust = (Customer)dict[key];
                cust.ContactName += " [Edited]";
                dict[key] = cust;
                break;
            }

            Console.WriteLine("Dictionary updated.\n");
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate distributed list for customers of a country
        /// </summary>
        /// <param name="country">Country name for which customers are to be fetched</param>
        private static void DemonstrateList(string country)
        {
            Console.WriteLine(string.Format("Distributed List for '{0}'...", country));

            // Obtaining distributed list instance from cache
            var list = cache.DataTypeManager.GetList<object>(
                country,
                new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
            );

            // Validating list
            if (list == null)
            {
                Console.WriteLine("No list found.\n");
                return;
            }

            // Displaying retrieved customers
            Console.WriteLine("Retrieved " + list.Count + " customers.");

            foreach (Customer c in list)
            {
                PrintCustomer(c);
            }

            Console.WriteLine("Updating list...");

            // Updating first record in list using WriteThru
            if (list.Count > 0)
            {
                // Updating the company name of the first customer
                list.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, writeThruProvider);
                Customer first = (Customer)list[0];
                first.CompanyName += " [Updated]";
                list[0] = first;
            }

            Console.WriteLine("List updated.\n");
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate distributed queue for customers
        /// </summary>
        private static void DemonstrateQueue()
        {
            Console.WriteLine("Distributed Queue...");

            // Obtaining distributed queue instance from cache
            var queue = cache.DataTypeManager.GetQueue<object>(
                "CustomerQueue",
                new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
            );

            // Validating queue
            if (queue == null)
            {
                Console.WriteLine("Queue empty.\n");
                return;
            }

            // Displaying retrieved customers
            Console.WriteLine("Queue contains: " + queue.Count);

            foreach (Customer c in queue)
            {
                PrintCustomer(c);
            }

            // Enqueuing a new customer using WriteThru
            Console.WriteLine("Adding new customer to queue...");
            queue.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, writeThruProvider);

            // Creating a new customer object
            Customer newCust = new Customer();
            newCust.CustomerID = "NEW01";
            newCust.ContactName = "Queue Demo";
            newCust.CompanyName = "QueueCo";
            newCust.City = "New York";
            newCust.Country = "USA";

            queue.Enqueue(newCust);

            Console.WriteLine("Customer enqueued.\n");
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate distributed hash set for order IDs of a customer
        /// </summary>
        /// <param name="customer">Customer whose order IDs are to be fetched</param>
        private static void DemonstrateHashSet(Customer customer)
        {
            // Validating customer object
            if (customer == null)
                return;

            Console.WriteLine("Distributed HashSet...");

            // Unique key for the customer's order ID set
            string setKey = "OrdersSet:" + customer.CustomerID;

            // Remove existing set to demonstrate fresh ReadThru fetch
            cache.Remove(setKey);

            // Obtaining distributed hash set instance from cache
            var hashSet = cache.DataTypeManager.GetHashSet<int>(
                setKey,
                new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
            );

            // Validating hash set
            if (hashSet == null)
            {
                Console.WriteLine("No hashset found.\n");
                return;
            }

            // Displaying retrieved order IDs
            Console.WriteLine("HashSet contains " + hashSet.Count + " items:");
            foreach (int id in hashSet)
                Console.Write(id + "\t");

            Console.WriteLine("\n");
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to print customer details to console
        /// </summary>
        /// <param name="customer">Customer object whose details are to be printed</param>
        private static void PrintCustomer(Customer customer)
        {
            // Validating customer object
            if (customer == null)
                return;

            // Helper function to handle null or empty strings
            Func<string, string> Safe = delegate (string v)
            {
                return string.IsNullOrEmpty(v) ? "N/A" : v;
            };

            Console.WriteLine("---------------------------------------");
            Console.WriteLine("CustomerID : " + Safe(customer.CustomerID));
            Console.WriteLine("ContactName: " + Safe(customer.ContactName));
            Console.WriteLine("CompanyName: " + Safe(customer.CompanyName));
            Console.WriteLine("City       : " + Safe(customer.City));
            Console.WriteLine("Country    : " + Safe(customer.Country));
            Console.WriteLine("---------------------------------------\n");
        }
    }
}
