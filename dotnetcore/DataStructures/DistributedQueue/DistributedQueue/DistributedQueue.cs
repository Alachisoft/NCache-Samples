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
using System.Collections.Generic;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the sample
    /// </summary>
    public class DistributedQueue
    {
        private static ICache _cache;
        private static IDistributedQueue<Customer> _distributedQueue;
        private static readonly string QueueName = "DistributedQueue:Customers";

        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run()
        {
            // Initialize cache
            InitializeCache();

            //Create or get a distributed queue by name
            _distributedQueue = GetOrCreateQueue();

            // Adding items to distributed queue
            Console.WriteLine("\n--- Enqueue Customers to Queue ---");
            EnqueueInQueue();
            Console.WriteLine();

            // Get the first object from distributed queue
            Console.WriteLine("\n--- Peek Customer From Queue ---");
            PeekFromQueue();
            Console.WriteLine();

            // Dequeue from distributed queue
            Console.WriteLine("\n--- Dequeue customer from Distributed Queue ---");
            DequeueFromQueue();
            Console.WriteLine();

            // Display customers from distributed queue
            Console.WriteLine("\n--- Display Customers From Queue ---");
            DisplayFromQueue(true);
            Console.WriteLine();

            // clear distributed queue
            Console.WriteLine("\n--- Clear Distributed Queue ---");
            ClearQueue();
            Console.WriteLine();

            // Remove the distributed queue from cache
            Console.WriteLine("\n--- Remove Distributed Queue from Cache --- ");
            DeleteQueueFromCache();
            Console.WriteLine();

            // Dispose the cache once done
            _cache.Dispose();
        }

        #region Queue functions

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
        /// Creates a distributed queue to which customers are to be added
        /// </summary>
        private static IDistributedQueue<Customer> GetOrCreateQueue()
        {
            IDistributedQueue<Customer> distributedQueue = _cache.DataTypeManager.GetQueue<Customer>(QueueName);

            if(distributedQueue == null)
            {
                DataTypeAttributes attributes = new DataTypeAttributes
                {
                    Expiration = new Expiration(ExpirationType.Absolute, new TimeSpan(0, 1, 0))
                };

                // Creating distributed queue with absolute expiration of 1 minute
                distributedQueue = _cache.DataTypeManager.CreateQueue<Customer>(QueueName,attributes);
            }

            return distributedQueue;
        }

        /// <summary>
        /// Add customer to distributed queue
        /// </summary>
        private static void EnqueueInQueue()
        {
            _distributedQueue.Enqueue(new Customer
            {
                ContactName = "David Johnes",
                CompanyName = "Lonesome Pine Restaurant",
                ContactNo = "(1) 354-9768",
                Address = "Silicon Valley, Santa Clara, California",
            });
            _distributedQueue.Enqueue(new Customer
            {
                ContactName = "Carlos Gonzalez",
                CompanyName = "LILA-Supermercado",
                ContactNo = "(9) 331-6954",
                Address = "Carrera 52 con Ave. Bolivar #65-98 Llano Largo",
            });
            _distributedQueue.Enqueue(new Customer
            {
                ContactName = "Carlos Hernandez",
                CompanyName = "HILARION-Abastos",
                ContactNo = "(5) 555-1340",
                Address = "Carrera 22 con Ave. Carlos Soublette #8-35",
            });
            _distributedQueue.Enqueue(new Customer
            {
                ContactName = "Elizabeth Brown",
                CompanyName = "Consolidated Holdings",
                ContactNo = "(171) 555-2282",
                Address = "Berkeley Gardens 12 Brewery",
            });
            _distributedQueue.Enqueue(new Customer
            {
                ContactName = "Felipe Izquierdo",
                CompanyName = "LINO-Delicateses",
                ContactNo = "(8) 34-56-12",
                Address = "Ave. 5 de Mayo Porlamar",
            });

            // Print output on console.
            Console.WriteLine("Objects are added to queue.");
        }

        /// <summary>
        /// Display customers from Distributed Queue
        /// </summary>
        /// <param name="showResults">whether elements of queue should be printed</param>
        private static void DisplayFromQueue(bool showResults = false)
        {
            // create array to store queue members
            Customer[] cachedCustomers = new Customer[_distributedQueue.Count];
            // create index variable to iterate array
            int index = 0;

            // iterate over each member in queue
            foreach (Customer customer in _distributedQueue)
            {
                // add current element to array
                cachedCustomers[index++] = customer;
                if (showResults)
                    PrintCustomerDetails(customer);
            }

            // Print output on console
            if (showResults)
                Console.WriteLine();
            Console.WriteLine("{0} Objects are fetched from distributed queue", index);
        }

        /// <summary>
        /// See first customer from Distributed Queue
        /// </summary>
        private static void PeekFromQueue()
        {
            // store next customer for displaying
            Customer nextCustomer = _distributedQueue.Peek();

            // print customer details on output
            PrintCustomerDetails(nextCustomer);
        }

        /// <summary>
        /// dequeue customer from Distributed Queue
        /// </summary>
        private static void DequeueFromQueue()
        {
            // store Dequeued customer in a variable
            Customer customer = _distributedQueue.Dequeue();

            // print customer details on output
            PrintCustomerDetails(customer);

            // print next customer details on output
            Console.WriteLine("Next object to Dequeue: ");
            PrintCustomerDetails(_distributedQueue.Peek());
        }

        /// <summary>
        /// clear Distributed Queue
        /// </summary>
        private static void ClearQueue()
        {
            // clear queue
            _distributedQueue.Clear();

            Console.WriteLine("Queue is clear");
        }

        /// <summary>
        /// Removes distributed queue from cache
        /// </summary>
        private static void DeleteQueueFromCache()
        {
            // Remove queue from cache
            _cache.DataTypeManager.Remove(QueueName);

            Console.WriteLine("Distributed queue successfully removed from cache.");
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Generates a string key for specified customer
        /// </summary>
        /// <param name="customer"> Instance of Customer to generate a key</param>
        /// <returns> returns a key </returns>
        private static string GetKey(Customer customer)
        {
            return string.Format("Customer:{0}", customer.ContactName);
        }

        /// <summary>
        /// This method prints details of customer type.
        /// </summary>
        /// <param name="customer"></param>
        private static void PrintCustomerDetails(Customer customer)
        {
            if (customer == null) return;

            Console.WriteLine();
            Console.WriteLine("Customer Details are as follows: ");
            Console.WriteLine("ContactName: " + customer.ContactName);
            Console.WriteLine("CompanyName: " + customer.CompanyName);
            Console.WriteLine("Contact No: " + customer.ContactNo);
            Console.WriteLine("Address: " + customer.Address);
            Console.WriteLine();
        }

        #endregion
    }
}
