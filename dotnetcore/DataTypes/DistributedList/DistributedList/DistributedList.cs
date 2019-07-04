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
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using System.Collections.Generic;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the sample Distributed List
    /// </summary>
    public class DistributedList
    {
        private static ICache _cache;
        private static IList<Customer> _distributedList;
        private static string _listName = "DistributedList:Customers";

        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run ()
        {
            // Initialize cache
            InitializeCache();

            //Create or get a distributed list by name
            _distributedList = GetOrCreateList();

            // Adding items to distributed list
            AddObjectsToList();

            // Get a single object from distributed list by index
            Customer customer = GetObjectFromList(1);

            // Get the objects from distributed list
            IterateListObjects();

            // Modify the object and update in distributed list
            UpdateObjectsInList(0, customer);

            // Remove the existing object from distributed list by index
            RemoveObjectFromList(3);

            // Remove the existing object from distributed list by instance
            RemoveObjectFromList(customer);

            // Remove the distributed list from cache
            DeleteList();

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

            // Initialize an instance of the cache to begin performing operations.
            _cache = CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }

        /// <summary>
        /// Creates a distributed list to which instances of customers are to be added
        /// </summary>
        private static IList<Customer> GetOrCreateList ()
        {
            IDistributedList<Customer> distributedList = _cache.DataTypeManager.GetList<Customer>(_listName);

            if (distributedList == null)
            {
                DataTypeAttributes attributes = new DataTypeAttributes
                {
                    Expiration = new Expiration(ExpirationType.Absolute, new TimeSpan(0, 1, 0))
                };

                // Creating distributed list with absolute expiration of 1 minute
                distributedList = _cache.DataTypeManager.CreateList<Customer>(_listName, attributes);
            }

            return distributedList;
        }

        /// <summary>
        /// This method adds objects to distributed list
        /// </summary>
        private static void AddObjectsToList()
        {
            _distributedList.Add(new Customer
            {
                ContactName = "David Johnes",
                CompanyName = "Lonesome Pine Restaurant",
                ContactNo = "(1) 354-9768",
                Address = "Silicon Valley, Santa Clara, California",
            });
            _distributedList.Add(new Customer
            {
                ContactName = "Carlos Gonzalez",
                CompanyName = "LILA-Supermercado",
                ContactNo = "(9) 331-6954",
                Address = "Carrera 52 con Ave. Bolivar #65-98 Llano Largo",
            });
            _distributedList.Add(new Customer
            {
                ContactName = "Carlos Hernandez",
                CompanyName = "HILARION-Abastos",
                ContactNo = "(5) 555-1340",
                Address = "Carrera 22 con Ave. Carlos Soublette #8-35",
            });
            _distributedList.Add(new Customer
            {
                ContactName = "Elizabeth Brown",
                CompanyName = "Consolidated Holdings",
                ContactNo = "(171) 555-2282",
                Address = "Berkeley Gardens 12 Brewery",
            });
            _distributedList.Add(new Customer
            {
                ContactName = "Felipe Izquierdo",
                CompanyName = "LINO-Delicateses",
                ContactNo = "(8) 34-56-12",
                Address = "Ave. 5 de Mayo Porlamar",
            });

            // Print output on console.
            Console.WriteLine("\nObjects are added to distributed list.");
        }

        /// <summary>
        /// This method gets an object from distributed list specified by index
        /// </summary>
        /// <returns>Customer retrieved from distributed list 
        /// by index</returns>
        private static Customer GetObjectFromList (int index)
        {
            Customer cachedCustomer = _distributedList[index];

            // Print output on console
            Console.WriteLine("\nObject is fetched from distributed list");

            PrintCustomerDetails(cachedCustomer);

            return cachedCustomer;
        }

        /// <summary>
        /// This method gets objects from distributed list
        /// </summary>
        /// <returns>Returns instances of Customer retrieved from distributed list</returns>
        private static void IterateListObjects ()
        {
            Customer[] cachedCustomers = new Customer[_distributedList.Count];

            // Enumerate over all customer instances and assign them at respective indices
            var counter = 0;
            foreach (Customer customer in _distributedList)
            {
                cachedCustomers[counter++] = customer;
            }

            Console.WriteLine("\n{0} Objects are fetched from distributed list", counter);
        }

        /// <summary>
        /// This method updates object in distributed list
        /// </summary>
        /// <param name="index">Index of object to be updated at in distributed list</param>
        /// <param name="customer">Instance of Customer that will be updated in the distributed list</param>
        private static void UpdateObjectsInList (int index, Customer customer)
        {
            // Modify company name of customer
            customer.CompanyName = "Gourmet Lanchonetes";

            // Update object via indexer
            _distributedList[index] = customer;

            // Print output on console
            Console.WriteLine("\nObject is updated in distributed list.");
        }

        /// <summary>
        /// Remove an object in distributed list by index
        /// </summary>
        /// <param name="index">Index of object to be deleted from distributed list</param>
        private static void RemoveObjectFromList (int index)
        {
            // Remove the existing customer
            _distributedList.RemoveAt(index);

            // Print output on console
            Console.WriteLine("\nObject is removed from distributed list.");
        }

        /// <summary>
        /// Remove an object in distributed list by instance of object
        /// </summary>
        /// <param name="customer">Instance of object to be deleted from distributed list</param>
        private static void RemoveObjectFromList (Customer customer)
        {
            // Remove the existing customer
            // Expensive operation use carefully
            bool removed = _distributedList.Remove(customer);
            
            //print on console
            Console.WriteLine("\nObject is" + ((removed) ? " " : " not ") + "removed from distributed List.");

        }

        /// <summary>
        /// Removes distributed list from cache
        /// </summary>
        private static void DeleteList ()
        {
            // Remove list
            _cache.DataTypeManager.Remove(_listName);

            // Print detail on output
            Console.WriteLine("\nDistributed list successfully removed from cache.");
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
