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
using Alachisoft.NCache.Licensing.DOM;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Events;
using Alachisoft.NCache.Sample.Data;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

// ===============================================================================
// Distributed Queue is a powerful distributed data structure which allows
// multiple clients to enqueue and dequeue items in a thread-safe manner. Some
// possible use cases of distributed queue include:
//   1. Task Scheduling: Managing and distributing tasks among multiple nodes.
//   2. Load Balancing: Distributing jobs evenly across multiple servers/services.
//   3. Message Queuing: Enabling asynchronous message processing between entities
//      (e.g., microservices, applications, users, etc.).
//   4. Event Processing: Handling real-time events in received order.
//   5. Data Streaming: Buffering and managing continuous data streams between
//      systems across distributed environments.
// Distributed Queues are designed to handle high concurrency and provide fault
// tolerance, making them suitable for large-scale distributed applications.
// ===============================================================================

namespace DQueue
{
    public class Program
    {
        // Cache handle for the connected cache
        static ICache _cache;

        // Distributed queue to be used in sample       
        static IDistributedQueue<Customer> _queue;

        static void Main(string[] args)
        {
            // Getting cache handle
            _cache = GetCache();

            // Name of the distributed queue to be used in sample
            string queueName = "DQueue:Customers";

            // Creating or getting a distributed queue by name
            _queue = GetOrCreateQueue(queueName);

            // Registering events for enqueue and dequeue operations on distributed queue
            RegisterEvents(_queue);

            // Enqueue items in the distributed queue
            Enqueue(_queue);

            // Dequeue from distributed queue
            Dequeue(_queue);

            // Displaying customers from distributed queue
            DisplayQueue(_queue);

            // Clearing the distributed queue
            ClearQueue(_queue);

            // Unregistering events for enqueue and dequeue operations on distributed queue
            UnregisterEvents(_queue);

            // Removing the distributed queue from _cache
            RemoveQueueFromCache(queueName);

            // Dispose the _cache once done
            _cache?.Dispose();
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to connect to a running _cache and return _cache handle for it 
        /// </summary>
        /// <returns>Cache handle for the connected _cache</returns>
        private static ICache GetCache()
        {
            string cacheName = ConfigurationManager.AppSettings["CacheName"];

            if (String.IsNullOrEmpty(cacheName))
            {
                Console.WriteLine("The CacheName cannot be null or empty.");
                return null;
            }

            // Connecting to a running _cache and return a _cache handle for it
            _cache = CacheManager.GetCache(cacheName);

            // Printing output on console
            Console.WriteLine(string.Format("\nCache '{0}' is connected.", cacheName));
            return _cache;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to create a distributed queue from queue name if it does not exist,
        /// otherwise get the existing distributed queue
        /// </summary>
        /// <param name="queueName">Name of the distributed queue</param>
        /// <returns>Distributed queue containing customers</returns>
        private static IDistributedQueue<Customer> GetOrCreateQueue(string queueName)
        {
            // Trying to get existing distributed queue
            IDistributedQueue<Customer> distributedQueue = _cache.DataTypeManager.GetQueue<Customer>(queueName);

            // Checking if distributed queue does not exist
            if (distributedQueue == null)
            {
                // Defining data type attributes for distributed queue with expiration settings
                DataTypeAttributes attributes = new DataTypeAttributes
                {
                    // Setting absolute expiration of 1 minute
                    Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(1))
                };

                // Creating distributed queue with absolute expiration of 1 minute
                distributedQueue = _cache.DataTypeManager.CreateQueue<Customer>(queueName, attributes);
            }

            // Returning the distributed queue
            return distributedQueue;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to register event notifications for enqueue and dequeue operations on Distributed Queue
        /// </summary>
        /// <param name="distributedQueue">Distributed queue on which operation would be performed</param>
        private static void RegisterEvents(IDistributedQueue<Customer> distributedQueue)
        {
            // Registering event notifications for enqueue and dequeue operations
            distributedQueue.RegisterNotification(EventCallBack, EventType.ItemAdded |
                EventType.ItemRemoved, DataTypeEventDataFilter.None);
            Console.WriteLine("Distributed queue events registered successfully.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to generate sample customers and enqueue them in the specified distributed queue
        /// </summary>
        /// <param name="distributedQueue">Distributed queue on which operation would be performed</param>
        private static void Enqueue(IDistributedQueue<Customer> queue)
        {
            Console.WriteLine("Enqueue Customers in Distributed Queue");

            // Generating sample customers with data
            List<Customer> customers = new List<Customer>()
            {
                new Customer
                {
                    ContactName = "Maria Anders",
                    CompanyName = "Alfreds Futterkiste",
                    ContactNo = "(030) 0074321",
                    Address = "Obere Str. 57, Berlin",
                },
                new Customer
                {
                    ContactName = "Ana Trujillo",
                    CompanyName = "Ana Trujillo Emparedados y helados",
                    ContactNo = "(5) 555-4729",
                    Address = "Avda. de la Constitución 2222, México D.F.",
                },
                new Customer
                {
                    ContactName = "Antonio Moreno",
                    CompanyName = "Antonio Moreno Taquería",
                    ContactNo = "(5) 555-3932",
                    Address = "Mataderos 2312, México D.F.",
                },
                new Customer
                {
                    ContactName = "Thomas Hardy",
                    CompanyName = "Around the Horn",
                    ContactNo = "(171) 555-7788",
                    Address = "120 Hanover Sq., London",
                },
                new Customer
                {
                    ContactName = "Hanna Moos",
                    CompanyName = "Blauer See Delikatessen",
                    ContactNo = "(0521) 555-9933",
                    Address = "Forsterstr. 57, Mannheim",
                },
            };

            // Iterating through customers to enqueue them in the distributed queue
            foreach (Customer customer in customers)
            {
                // Enqueue customer in the distributed queue
                queue.Enqueue(customer);

                // Adding a delay for ItemAdded event processing
                Task.Delay(500).Wait();
            }

            // Printing output on console.
            Console.WriteLine("Customers are enqueued in the distributed queue");
            Console.WriteLine();
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to see the first customer from Distributed Queue without removing it
        /// </summary>
        /// <param name="distributedQueue">Distributed queue on which operation would be performed</param>
        private static void PeekFromQueue(IDistributedQueue<Customer> distributedQueue)
        {
            Console.WriteLine("Peek Customer From Distributed Queue");

            // Fetching the first customer in the distributed queue without removing it
            Customer customer = distributedQueue.Peek();

            // Printing customer details on console
            PrintCustomerDetails(customer);
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to dequeue customer from Distributed Queue
        /// </summary>
        /// <param name="distributedQueue">Distributed queue on which operation would be performed</param>
        private static void Dequeue(IDistributedQueue<Customer> distributedQueue)
        {
            // Dequeue the first customer from the distributed list and storing it in a variable
            Customer customer = distributedQueue.Dequeue();

            // Printing the customer details on console
            Console.WriteLine("Dequeue customer from Distributed Queue:");
            PrintCustomerDetails(customer);

            // Peeking the first customer in the distributed queue after dequeue operation
            PeekFromQueue(distributedQueue);
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to display customers enqueued in the specified Distributed Queue
        /// </summary>
        /// <param name="distributedQueue">Distributed queue on which operation would be performed</param>
        private static void DisplayQueue(IDistributedQueue<Customer> distributedQueue)
        {
            // Checking if distributed queue has any objects
            if (distributedQueue.Count > 0)
            {
                // Printing number of objects in distributed queue on console
                Console.WriteLine("{0} Customers are enqueued:", distributedQueue.Count);

                // Iterating through distributed queue to display customer details
                foreach (Customer customer in distributedQueue)
                {
                    // Printing customer details on console
                    PrintCustomerDetails(customer);
                }
            }
            else
            {
                // Print output on console
                Console.WriteLine("Distributed Queue is empty.");
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to clear all the customers in the Distributed Queue
        /// </summary>
        /// <param name="distributedQueue">Distributed queue on which operation would be performed</param>
        private static void ClearQueue(IDistributedQueue<Customer> distributedQueue)
        {
            // Clearing the distributed queue
            distributedQueue.Clear();

            // Printing output on console
            Console.WriteLine("Distributed queue has been cleared.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to unregister event notifications for enqueue and dequeue operations on Distributed Queue
        /// </summary>
        /// <param name="distributedQueue">Distributed queue on which operation would be performed</param>
        private static void UnregisterEvents(IDistributedQueue<Customer> distributedQueue)
        {
            // Unregistering event notifications for enqueue and dequeue operations
            distributedQueue.UnRegisterNotification(EventCallBack, EventType.ItemAdded | EventType.ItemRemoved);
            Console.WriteLine("Distributed queue events unregistered successfully.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to remove distributed queue from _cache
        /// </summary>
        /// <param name="queueName">Name of the distributed queue</param>
        private static void RemoveQueueFromCache(string queueName)
        {
            // Removing queue from _cache
            _cache?.DataTypeManager.Remove(queueName);

            // Printing output on console
            Console.WriteLine("Distributed queue successfully removed from _cache.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Event callback method to handle events for Distributed Queue
        /// </summary>
        /// <param name="collectionName">Name of the distributed queue</param>
        /// <param name="e">Event arguments</param>
        private static void EventCallBack(string collectionName, DataTypeEventArg e)
        {
            // Handling events based on event type
            switch (e.EventType)
            {
                // Event type for item added to distributed queue
                case EventType.ItemAdded:
                    // In real world scenarios, we would dequeue the item here to process it
                    // For demonstration, we will just peek the item
                    Console.WriteLine("Event: An item was added to the Distributed Queue.");
                    PeekFromQueue(_queue);
                    break;
                // Event type for item removed from distributed queue
                case EventType.ItemRemoved:
                    Console.WriteLine("Event: An item was removed from the Distributed Queue.");
                    break;
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to print the details of customer on console
        /// </summary>
        /// <param name="customer">Customer instance whose attributes are to be printed</param>
        private static void PrintCustomerDetails(Customer customer)
        {
            if (customer == null) return;

            Console.WriteLine("Customer details are as follows: ");
            Console.WriteLine("ContactName: " + customer.ContactName);
            Console.WriteLine("CompanyName: " + customer.CompanyName);
            Console.WriteLine("Contact No: " + customer.ContactNo);
            Console.WriteLine("Address: " + customer.Address);
            Console.WriteLine();
        }
    }
}