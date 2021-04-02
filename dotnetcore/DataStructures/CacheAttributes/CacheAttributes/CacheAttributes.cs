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
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Events;
using System.Threading;
using Alachisoft.NCache.Runtime;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality for all the common cacheoperations on Distributed DataTypes taking List as Datatype
    /// </summary>
    public class CacheAttributes
    {
        private static ICache _cache;
        private static readonly string _datatype = "DistributedList:Customers";
        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run ()
        {
            // Initialize cache
            InitializeCache();

            // Create distributed list with Different Datatype Attributes
            CreateListWithDataTypeAttributes();

            // Create customers object
            Customer[] customers = GetCustomers();

            // Adding items to Distributed Datatype
            AddObjectsToDatatype(customers);

            //Register For Notification on datatype
            RegisterNotification();

            // Get a single object from distributed list by index
            Customer customer = GetObjectFromList(1);

            // Modify the object and update in distributed list to trigger Notification Callback
            UpdateObjectsInList(0);

            // Remove the existing object from distributed list by index to Trigger Notification Callback
            RemoveObjectFromList(3);

            //Sleep for 5 seconds 
            Thread.Sleep(5000);

            //Register For Notifications 
            UnRegisterNotification();

            //Lock the list for 1 Minute
            LockDatatype();

            //UnLock the list
            UnLockList();

            // Remove the distributed list from cache
            RemoveList();

            // Dispose the cache once done
            _cache.Dispose();
        }

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache ()
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Client.CacheManager.GetCache(cache);

            //Clear the Cache
            _cache.Clear();
            // Print output on console.
            Console.WriteLine(string.Format("\nCache {0} is initialized.", cache));
        }


        /// <summary>
        /// This method acquire lock on Distributed datatype
        /// </summary>
        private static void LockDatatype ()
        {
            // Get Distributed datatype
            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);

            // Acquire Lock
            bool locked = list.Lock(new TimeSpan(0, 1, 0));

            // Print output on console.
            Console.WriteLine("\nDistributed Datatype: {0} " + ((locked) ? "Locked" : "Cannot be Locked"), _datatype);

        }

        /// <summary>
        ///  This method UnLock List Datastructure
        /// 
        /// </summary>
        private static void UnLockList ()
        {
            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);

            //Unlock the Distributed Datatype
            list.Unlock();

            //Print output on console.
            Console.WriteLine("\nDistributed Datatype:{0} Unlocked", _datatype);

        }


        /// <summary>
        ///  This method unregister notification on Distributed Datatype
        /// </summary>
        /// 
        private static void UnRegisterNotification ()
        {
            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);

            //unregister notifications for all Event types
            list.UnRegisterNotification(OnDataNotificationCallback,
                EventType.ItemAdded | EventType.ItemRemoved | EventType.ItemUpdated);

            //Print output on console.
            Console.WriteLine("\nNotifications unregistered on Distributed Datatype: {0}", _datatype);
        }
        /// <summary>
        /// This method register notifications for Add,Update and Remove on List
        /// </summary>
        private static void RegisterNotification ()
        {

            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);

            //register callback for notification when item is added , removed or Updated in Distributed Datatype with DataFilter as None 
            list.RegisterNotification(new DataTypeDataNotificationCallback(OnDataNotificationCallback),
                EventType.ItemAdded | EventType.ItemRemoved | EventType.ItemUpdated
                , DataTypeEventDataFilter.None);

            //Print output on console.
            Console.WriteLine("\nNotifications are registered on Dictionary: {0}", _datatype);
        }

        /// <summary>
        /// This is method is trigered when Add,Update or Remove operation is applied on list
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="collectionEventArgs"></param>
        public static void OnDataNotificationCallback (string collectionName, DataTypeEventArg collectionEventArgs)
        {
            lock (_datatype)
            {

                Console.WriteLine("\nNotification Received");

                if (collectionEventArgs.EventType == EventType.ItemUpdated)
                {
                    Console.WriteLine("Item Updated in Datatype: {0}", collectionName);
                }
                else if (collectionEventArgs.EventType == EventType.ItemAdded)
                {
                    Console.WriteLine("Item added in  Datatype: {0}", collectionName);
                }
                else
                {
                    Console.WriteLine("Item Removed from datatype: {0}", collectionName);
                }
            }
        }



        /// <summary>
        /// Creates a distributed list to which instances of customers are to be added
        /// </summary>
        /// be the key against which distributed list is to be stored.</param>
        private static void CreateListWithDataTypeAttributes ()
        {

            //Attribute NamedTags Dictionary
            NamedTagsDictionary namedTags = new NamedTagsDictionary();
            namedTags.Add("Customers", "Loyal");
            //Expiration
            Expiration expiration = new Expiration(ExpirationType.Absolute, new TimeSpan(0, 1, 0));
            //with Tag Frequent
            Tag tag = new Tag("Frequent");

            //Group for distributed datatype
            string group = "Potential";

            // Item priority Default
            CacheItemPriority cacheItemPriority = CacheItemPriority.Default;

            // Resync Operation is disabled
            ResyncOptions resyncOptions = new ResyncOptions(false);

            DataTypeAttributes attributes = new DataTypeAttributes
            {
                Expiration = expiration,
                NamedTags = namedTags,
                Group = group,
                ResyncOptions = resyncOptions,
                Priority = cacheItemPriority

            };

            // Creating distributed list with the defined attributes
            _cache.DataTypeManager.CreateList<Customer>(_datatype, attributes);
        }

        /// <summary>
        /// This method adds objects to distributed list
        /// </summary>
        /// <param name="customer"> Instances of Customer that will be added to 
        /// distributed list</param>
        private static void AddObjectsToDatatype (Customer[] customers)
        {
            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);
            //Add Bulk into Distributed List
            list.AddRange(customers);

            // Print output on console.
            Console.WriteLine("\nObjects are added to distributed list.");
        }

        /// <summary>
        /// This method gets an object from distributed list specified by index
        /// </summary>
        /// <returns>Returns instance of Customer retrieved from distributed list 
        /// by index</returns>
        private static Customer GetObjectFromList (int index)
        {
            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);
            Customer cachedCustomer = list[index];

            // Print output on console.
            Console.WriteLine("\nObject is fetched from distributed list");

            PrintCustomerDetails(cachedCustomer);

            return cachedCustomer;
        }


        /// <summary>
        /// This method updates object in distributed list
        /// </summary>
        /// <param name="index">Index of object to be updated at in distributed list</param>
        /// <param name="customer">Instance of Customer that will be updated in the distributed list</param>
        private static void UpdateObjectsInList (int index)
        {

            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);
            // Modify company name of customer

            Customer customer = list[index];

            customer.CompanyName = "Gourmet Lanchonetes";

            // Update object via indexer
            list[index] = customer;

            // Print output on console.
            Console.WriteLine("\nObject is updated in distributed list.");
        }

        /// <summary>
        /// Remove an object in distributed list by index
        /// </summary>
        /// <param name="index">Index of object to be deleted from distributed list</param>
        private static void RemoveObjectFromList (int index)
        {
            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);
            // Remove the existing customer
            list.RemoveAt(index);

            // Print output on console.
            Console.WriteLine("\nObject is removed from distributed list.");
        }

        /// <summary>
        /// Remove an object in distributed list by instance of object
        /// </summary>
        /// <param name="customer">Instance of object to be deleted from distributed list</param>
        private static void RemoveObjectFromList (Customer customer)
        {
            IDistributedList<Customer> list = _cache.DataTypeManager.GetList<Customer>(_datatype);
            // Remove the existing customer
            // Expensive operation use carefully
            bool removed = list.Remove(customer);

            //Print output on console.
            Console.WriteLine("\nObject is" + ((removed) ? " " : " not ") + "removed from distributed List.");

        }

        /// <summary>
        /// Removes distributed list from cache
        /// </summary>
        private static void RemoveList ()
        {
            // Remove list
            _cache.DataTypeManager.Remove(_datatype);

            // Print output on console.
            Console.WriteLine("\nDistributed list successfully removed from cache.");
        }

        /// <summary>
        /// Generates instances of Customer to be used in this sample
        /// </summary>
        /// <returns> returns instances of Customer </returns>
        private static Customer[] GetCustomers ()
        {
            Customer[] customers = new Customer[5];

            customers[0] = new Customer
            {
                ContactName = "David Johnes",
                CompanyName = "Lonesome Pine Restaurant",
                ContactNo = "(1) 354-9768",
                Address = "Silicon Valley, Santa Clara, California",
            };
            customers[1] = new Customer
            {
                ContactName = "Carlos Gonzalez",
                CompanyName = "LILA-Supermercado",
                ContactNo = "(9) 331-6954",
                Address = "Carrera 52 con Ave. Bolivar #65-98 Llano Largo",
            };
            customers[2] = new Customer
            {
                ContactName = "Carlos Hernandez",
                CompanyName = "HILARION-Abastos",
                ContactNo = "(5) 555-1340",
                Address = "Carrera 22 con Ave. Carlos Soublette #8-35",
            };
            customers[3] = new Customer
            {
                ContactName = "Elizabeth Brown",
                CompanyName = "Consolidated Holdings",
                ContactNo = "(171) 555-2282",
                Address = "Berkeley Gardens 12 Brewery",
            };
            customers[4] = new Customer
            {
                ContactName = "Felipe Izquierdo",
                CompanyName = "LINO-Delicateses",
                ContactNo = "(8) 34-56-12",
                Address = "Ave. 5 de Mayo Porlamar",
            };

            return customers;
        }

        /// <summary>
        /// Generates a string key for specified customer
        /// </summary>
        /// <param name="customer"> Instance of Customer to generate a key</param>
        /// <returns> returns a key </returns>
        private static string GetKey (Customer customer)
        {
            return string.Format("Customer:{0}", customer.ContactName);
        }

        /// <summary>
        /// This method prints details of customer type.
        /// </summary>
        /// <param name="customer"></param>
        private static void PrintCustomerDetails (Customer customer)
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
    }
}
