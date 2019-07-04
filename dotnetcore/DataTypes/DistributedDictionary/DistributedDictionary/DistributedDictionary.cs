using System;
using System.Configuration;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;
using System.Collections.Generic;

namespace Alachisoft.NCache.Samples
{
    class DistributedDictionary
    {
        private static ICache _cache;
        private static string _dictionaryName = "DistributedDictionary:Customers";
        private static IDictionary<string, Customer> _distributedDictionary;
        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run ()
        {
            // Initialize cache
            InitializeCache();

            // Create or get a distributed Dictionary by name
            _distributedDictionary = GetOrCreateDictionary(_dictionaryName);

            // Adding items to distributed Dictionary
            AddObjectsToDictionary();

            // Get a single object from distributed Dictionary by key
            Customer customer = GetObjectFromDictionary("customer1");

            // Print output on console 
            PrintCustomerDetails(customer);

            // Find Customer with the Contact No in distributed Dictionary
            FindCustomerInDictionary("(9) 331-6954");

            // Modify the object and update in distributed Dictionary
            UpdateObjectsInDictionary("customer1");

            // Remove the existing object from distributed Dictionary by Key
            RemoveObjectFromDictionary("customer2");
            
            // get object from Dictionary with key
            customer = GetObjectFromDictionary("customer1");
            
            // Remove the existing object from distributed Dictionary by Key Value Pair
            RemoveObjectFromDictionary("customer1", customer);

            // Delete the distributed Dictionary from cache
            DeleteDictionary();

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
            _cache = Alachisoft.NCache.Client.CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }

        /// <summary>
        /// Creates a distributed Dictionary to which instances of customers are to be added
        /// </summary>
        /// <param name="DictionaryName">The name of Dictionary. This name is ultimately going to 
        /// be the key against which distributed Dictionary is to be stored.</param>
        private static IDictionary<string, Customer> GetOrCreateDictionary (string DictionaryName)
        {
            IDistributedDictionary<string, Customer> dictionary = _cache.DataTypeManager.GetDictionary<string, Customer>(_dictionaryName);

            if (dictionary == null)
            {

                //Add expiration details 
                DataTypeAttributes attributes = new DataTypeAttributes
                {
                    Expiration = new Expiration(ExpirationType.Absolute, new TimeSpan(0, 1, 0))
                };

                // Creating distributed Dictionary with absolute expiration of 1 minute
                dictionary = _cache.DataTypeManager.CreateDictionary<string, Customer>(DictionaryName, attributes);
            }

            return dictionary;
        }


        /// <summary>
        /// This method adds Customer objects to distributed Dictionary
        /// </summary>
        private static void AddObjectsToDictionary ()
        {
            _distributedDictionary.Add("customer1", new Customer
            {
                CustomerID = "customer1",
                ContactName = "David Johnes",
                CompanyName = "Lonesome Pine Restaurant",
                ContactNo = "(1) 354-9768",
                Address = "Silicon Valley, Santa Clara, California",
            });

            _distributedDictionary.Add("customer2", new Customer
            {
                CustomerID = "customer2",
                ContactName = "Carlos Gonzalez",
                CompanyName = "LILA-Supermercado",
                ContactNo = "(9) 331-6954",
                Address = "Carrera 52 con Ave. Bolivar #65-98 Llano Largo",
            });

            _distributedDictionary.Add("customer3", new Customer
            {
                CustomerID = "customer3",
                ContactName = "Carlos Hernandez",
                CompanyName = "HILARION-Abastos",
                ContactNo = "(5) 555-1340",
                Address = "Carrera 22 con Ave. Carlos Soublette #8-35",
            });
            _distributedDictionary.Add("customer4", new Customer
            {
                CustomerID = "customer4",
                ContactName = "Elizabeth Brown",
                CompanyName = "Consolidated Holdings",
                ContactNo = "(171) 555-2282",
                Address = "Berkeley Gardens 12 Brewery",
            });
            _distributedDictionary.Add("customer5", new Customer
            {
                CustomerID = "custome5",
                ContactName = "Felipe Izquierdo",
                CompanyName = "LINO-Delicateses",
                ContactNo = "(8) 34-56-12",
                Address = "Ave. 5 de Mayo Porlamar",
            });
            // Print output on console
            Console.WriteLine("\n{0} Objects are added to distributed Dictionary.", _distributedDictionary.Count);
        }

        /// <summary>
        /// This method gets an object from distributed Dictionary specified by index
        /// </summary>
        /// <returns>Returns instance of Customer retrieved from distributed Dictionary 
        /// by index</returns>
        private static Customer GetObjectFromDictionary (string key)
        {
            Customer cachedCustomer;

            //Get object from Distributed Dictionary with key
            bool fetched = _distributedDictionary.TryGetValue(key, out cachedCustomer);

            Console.WriteLine("\nObject is" + ((fetched) ? "" : " not") + " fetched from distributed Dictionary");

            return cachedCustomer;
        }

        /// <summary>
        /// This method iterate objects of the distributed Dictionary
        /// <param name="contactNo">contactNo of object to be Find in distributed Dictionary</param>  
        /// </summary>
        private static void FindCustomerInDictionary (string contactNo)
        {
            Console.WriteLine("Finding object(s) in distributed Dictionary");
            // Enumerate over all customer instances and find customer with given Contact No
            var counter = 0;
            foreach (Customer customer in _distributedDictionary.Values)
            {
                if (customer.ContactNo.Equals(contactNo))
                {
                    PrintCustomerDetails(customer);
                    counter++;
                }
            }

            // Print output on console
            Console.WriteLine("\n{0} Object(s) are fetched from distributed Dictionary", counter);
        }

        /// <summary>
        /// This method updates object in distributed Dictionary
        /// </summary>
        /// <param name="key">key of object to be updated at in distributed Dictionary</param>       
        private static void UpdateObjectsInDictionary (string key)
        {
            //Get Object from dictionaray
            Customer customer = GetObjectFromDictionary(key);

            // Modify company name of customer
            customer.CompanyName = "Gourmet Lanchonetes";

            // Update object via key
            _distributedDictionary[key] = customer;

            // Print output on console.
            Console.WriteLine("\nObject is updated in distributed Dictionary.");
        }

        /// <summary>
        /// Remove an object in distributed Dictionary by key
        /// </summary>
        /// <param name="key">key of object to be deleted from distributed Dictionary</param>
        private static void RemoveObjectFromDictionary (string key)
        {
            // Remove the existing customer
            bool removed = _distributedDictionary.Remove(key);

            // Print output on console.
            Console.WriteLine("\nObject is" + ((removed) ? " " : " not ") + "removed from distributed Dictionary.");
        }

        /// <summary>
        /// Remove an object from distributed Dictionary by instance and Key of object
        /// </summary>
        /// <param name="customer">Instance of object to be deleted from distributed Dictionary</param>
        /// <param name="key"> Key of object to be deleted from distributed Dictionary</param>

        private static void RemoveObjectFromDictionary (string key, Customer customer)
        {
            IDistributedDictionary<string, Customer> dictionary = _cache.DataTypeManager.GetDictionary<string, Customer>(_dictionaryName);
            
            // Remove the existing customer
            // Expensive operation use carefully
            bool removed = dictionary.Remove(new KeyValuePair<string, Customer>(key, customer));

            //Print output on console.
            Console.WriteLine("\nObject is" + ((removed) ? " " : " not ") + "removed from distributed Dictionary.");
        }

        /// <summary>
        /// Removes distributed Dictionary from cache
        /// </summary>
        private static void DeleteDictionary ()
        {
            // Remove Dictionary from the Cache
            _cache.DataTypeManager.Remove(_dictionaryName);

            // Print output on console.
            Console.WriteLine("\nDistributed Dictionary successfully removed from cache.");
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
