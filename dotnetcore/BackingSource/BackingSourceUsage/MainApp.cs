using System;
using System.Collections.Generic;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Client.DataTypes.Collections;

namespace Alachisoft.NCache.Samples
{
    class MainApp
    {
        /// <summary>
        /// instance of cache on which operations are performed
        /// </summary>
        private ICache _cache;

        #region properties being used to fetch and store user data
        public string CustomerID { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        #endregion

        #region global provider names
        private string ReadThruProviderName { get; set; }
        private string WriteThruProviderName { get; set; }
        #endregion

        #region Display Functions

        /// <summary>
        /// Displays customer data
        /// </summary>
        /// <param name="customer">object containing customer details</param>
        public void DisplayCustomerData(Customer customer)
        {
            Console.WriteLine("Customer ID: " + (CustomerID = customer.CustomerID));
            Console.WriteLine("Contact Name: " + (ContactName = customer.ContactName));
            Console.WriteLine("Contact Number: " + (Phone = customer.ContactNo));
            Console.WriteLine("Fax: " + (Fax = customer.Fax));
            Console.WriteLine("Company: " + (CompanyName = customer.CompanyName));
            Console.WriteLine("Address: " + (Address = customer.Address));
            Console.WriteLine("City: " + (City = customer.City));
            Console.WriteLine("Postal Code: " + (PostalCode = customer.PostalCode));
            Console.WriteLine("Country: " + (Country = customer.Country));

        }

        /// <summary>
        /// displays list objects
        /// </summary>
        /// <param name="distributedList">List to display</param>
        private void DisplayListData(IDistributedList<object> distributedList)
        {
            Console.WriteLine("Result count: " + distributedList.Count);
            foreach (Customer listCustomer in distributedList)
            {
                Console.WriteLine("--");
                DisplayCustomerData(listCustomer);
            }
        }

        /// <summary>
        /// displays dictionary objects
        /// </summary>
        /// <param name="distributedDictionary">Dictionary to display</param>
        public void DisplayDictionaryData(IDistributedDictionary<string, object> distributedDictionary)
        {
            Console.WriteLine("Result count: " + distributedDictionary.Count);
            foreach (KeyValuePair<string, object> pair in distributedDictionary)
            {
                Console.WriteLine("--");
                DisplayCustomerData((Customer)pair.Value);
            }
        }

        /// <summary>
        /// displays queued objects
        /// </summary>
        /// <param name="distributedQueue">Queue to display</param>
        private void DisplayQueueData(IDistributedQueue<object> distributedQueue)
        {
            Console.WriteLine("Result count: " + distributedQueue.Count);
            foreach (Customer queueCustomer in distributedQueue)
            {
                Console.WriteLine("--");
                DisplayCustomerData(queueCustomer);
            }
        }

        /// <summary>
        /// displays hashset objects
        /// </summary>
        /// <param name="distributedHashSet">hashset to display</param>
        private void DisplayHashSetData(IDistributedHashSet<int> distributedHashSet)
        {
            distributedHashSet.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName);
            Console.WriteLine("Result count: " + distributedHashSet.Count);
            Console.WriteLine("--");
            foreach (int num in distributedHashSet)
            {
                Console.Write(num + "\t");
            }
            Console.WriteLine("\n--");
        }

        #endregion

        #region Methods to Modify or Add to Data Structures

        /// <summary>
        /// Modifies counter value
        /// </summary>
        /// <param name="counter"></param>
        public void ModifyCounterValue(ICounter counter)
        {
            counter.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName);
            Console.Write("Enter New Counter Value: ");
            long value = Convert.ToInt64(Console.ReadLine());
            counter.SetValue(value);
            Console.WriteLine();
            Console.WriteLine("New Counter Value is: " + counter.Value);
        }

        /// <summary>
        /// Modifies first entry of list
        /// </summary>
        /// <param name="distributedList"></param>
        private void ModifyListValue(IDistributedList<object> distributedList)
        {
            distributedList.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName);
            InputDataFromUser();
            distributedList[0] = new Customer()
            {
                CustomerID = CustomerID,
                ContactName = ContactName,
                CompanyName = CompanyName,
                Address = Address,
                City = City,
                Country = Country,
                PostalCode = PostalCode,
                ContactNo = Phone,
                Fax = Fax,
            };
        }

        /// <summary>
        /// modify dictionary entry by CustomerID
        /// </summary>
        /// <param name="distributedDictionary"></param>
        public void ModifyDictionaryValue(IDistributedDictionary<string, object> distributedDictionary)
        {
            distributedDictionary.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName);
            InputDataFromUser();
            distributedDictionary[CustomerID] = new Customer()
            {
                CustomerID = CustomerID,
                ContactName = ContactName,
                CompanyName = CompanyName,
                Address = Address,
                City = City,
                Country = Country,
                PostalCode = PostalCode,
                ContactNo = Phone,
                Fax = Fax,
            };
        }

        /// <summary>
        /// remove dictionary entry by CustomerID
        /// </summary>
        /// <param name="distributedDictionary"></param>
        public void RemoveDictionaryValue(IDistributedDictionary<string, object> distributedDictionary)
        {
            distributedDictionary.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName);
            distributedDictionary.Remove(CustomerID);
        }

        /// <summary>
        /// remove queue entry by Customer
        /// </summary>
        /// <param name="distributedList"></param>
        public void RemoveQueueValue(IDistributedQueue<object> distributedQueue)
        {
            distributedQueue.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName);
            distributedQueue.Dequeue();
        }

        /// <summary>
        /// remove list entry by Customer
        /// </summary>
        /// <param name="distributedList"></param>
        public void RemoveListValue(IDistributedList<object> distributedList)
        {
            distributedList.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName);
            distributedList.Remove(new Customer()
            {
                CustomerID = CustomerID,
            });
        }

        /// <summary>
        /// Enqueue data to Queue. Similar methods for other data structures can be written in the same way
        /// </summary>
        /// <param name="distributedQueue"></param>
        private void AddDataToQueue(IDistributedQueue<object> distributedQueue)
        {
            distributedQueue.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName);
            InputDataFromUser();
            distributedQueue.Enqueue(new Customer()
            {
                CustomerID = CustomerID,
                ContactName = ContactName,
                CompanyName = CompanyName,
                Address = Address,
                City = City,
                Country = Country,
                PostalCode = PostalCode,
                ContactNo = Phone,
                Fax = Fax,
            });
        }


        #endregion

        #region Input Methods

        /// <summary>
        /// Take main menu input from user
        /// </summary>
        /// <returns></returns>
        public MainMenu InputMainMenu()
        {
            ConsolePause();

            Console.WriteLine("Main Menu");
            Console.WriteLine();
            Console.WriteLine("1 - Get Customer Detail");
            Console.WriteLine("2 - Get Customer Count by Company Name");
            Console.WriteLine("3 - Get Customers by City Name");
            Console.WriteLine("4 - Get Customers by Country Name");
            Console.WriteLine("5 - Get Customers by a Specific Order");
            Console.WriteLine("6 - Get Order IDs of a customer");
            Console.WriteLine();
            Console.Write("Enter Option: ");

            try
            {
                // convert input to integer
                int choice = Convert.ToInt32(Console.ReadLine());

                ConsolePause();

                // validate input; ensure the input is between required numbers
                if (choice >= 1 && choice < 7)
                    return (MainMenu) choice;
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// take customer data input from user
        /// </summary>
        public void InputDataFromUser()
        {
            Console.WriteLine("Enter the following information");

            string input;
            Console.Write("Customer ID: ");
            CustomerID = ((input = Console.ReadLine()) != "") ? input : CustomerID;
            Console.Write("Contact Name: ");
            ContactName = ((input = Console.ReadLine()) != "") ? input : ContactName;
            Console.Write("Company Name: ");
            CompanyName = ((input = Console.ReadLine()) != "") ? input : CompanyName;
            Console.Write("Address: ");
            Address = ((input = Console.ReadLine()) != "") ? input : Address;
            Console.Write("City: ");
            City = ((input = Console.ReadLine()) != "") ? input : City;
            Console.Write("Country: ");
            Country = ((input = Console.ReadLine()) != "") ? input : Country;
            Console.Write("Postal Code: ");
            PostalCode = ((input = Console.ReadLine()) != "") ? input : PostalCode;
            Console.Write("Phone Number: ");
            Phone = ((input = Console.ReadLine()) != "") ? input : Phone;
            Console.Write("Fax Number: ");
            Fax = ((input = Console.ReadLine()) != "") ? input : Fax;
        }

        #endregion

        /// <summary>
        /// Insert customer data to cache
        /// </summary>
        public void StoreNewCustomerData()
        {
            Customer customer = new Customer();
            customer.CustomerID = CustomerID;
            customer.ContactName = ContactName;
            customer.CompanyName = CompanyName;
            customer.Address = Address;
            customer.City = City;
            customer.Country = Country;
            customer.PostalCode = PostalCode;
            customer.ContactNo = Phone;
            customer.Fax = Fax;

            try
            {
                _cache.Insert(customer.CustomerID.ToUpper(), new CacheItem(customer), new WriteThruOptions(WriteMode.WriteThru, WriteThruProviderName));

                Console.WriteLine("Customer information updated successfuly");
            }
            catch(Exception e)
            {
                Console.WriteLine("\n" + "Error: " + e.Message);
            }
        }

        /// <summary>
        /// method for functioning
        /// </summary>
        public void Run()
        {
            // initialize cache before any operations
            InitializeCache();

            ReadThruProviderName = "SqlReadThruProvider";
            WriteThruProviderName = "SqlWriteThruProvider";

            // initialize customer object
            Customer customer = null;
            ICounter counter = null;
            IDistributedDictionary<string, object> distributedDictionary;
            IDistributedList<object> distributedList;
            IDistributedQueue<object> distributedQueue;
            IDistributedHashSet<int> distributedHashSet;

            bool isRunning = true;

            while (isRunning)
            {
                // clearing cache to see readthru/writethru changes
                _cache.Clear();

                // take input according to main menu
                switch (InputMainMenu())
                {
                    // 1 - Get Customer Detail
                    case MainMenu.GetCacheItem:

                        // take customer ID from user
                        Console.Write("Enter Customer ID: ");
                        CustomerID = Convert.ToString(Console.ReadLine());

                        customer = _cache.Get<Customer>(CustomerID, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));

                        if (customer != null)
                        {
                            // display details of entered customer ID
                            Console.WriteLine("\n" + "Fetched Customer Details [readthru]: ");

                            DisplayCustomerData(customer);

                            ConsolePause();

                            Console.WriteLine("Enter any detail to update (press enter on any value to skip) [writethru]");
                            // take new customer data from user
                            InputDataFromUser();
                            // update existing customer with new data
                            StoreNewCustomerData();

                            // show newly fetched customer details
                            Console.WriteLine("\n" + "Fetched Customer Details [cache]: ");
                            customer = _cache.Get<Customer>(CustomerID);
                            DisplayCustomerData(customer);
                        }
                        else
                        {
                            Console.WriteLine("\nCustomer with provided ID does not exist.");
                        }

                        break;
                    // 2 - Get Customer Count by Company Name
                    case MainMenu.GetDistributedCounter:
                        
                        // input company name from user
                        Console.Write("Enter Company Name: ");
                        CompanyName = Convert.ToString(Console.ReadLine());

                        // fetch customers with same company name
                        counter = _cache.DataTypeManager.GetCounter(CompanyName, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));

                        if (counter.Value != 0)
                        {
                            Console.WriteLine("Customers working in company are: " + counter.Value);

                            if (ConsolePause().Equals(ConsoleKey.Escape))
                                break;

                            // modify counters
                            ModifyCounterValue(counter);
                        }
                        else
                        {
                            Console.WriteLine("\nCustomer with provided CompanyName does not exist.");
                        }
                        break;
                    // 3 - Get Customers by City Name
                    case MainMenu.GetDistributedDictionary:

                        // take city name input from user
                        Console.Write("Enter City Name: ");
                        City = Convert.ToString(Console.ReadLine());

                        // fetch customers with same city name
                        distributedDictionary = _cache.DataTypeManager.GetDictionary<string, object>(City, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));

                        if (distributedDictionary != null)
                        {
                            DisplayDictionaryData(distributedDictionary);

                            if (!ConsolePause(false).Equals(ConsoleKey.Escape))
                            {
                                // modify any dictionary record
                                Console.WriteLine("\n" + "Modify Any Record [writethru]");
                                ModifyDictionaryValue(distributedDictionary);
                                Console.WriteLine("\n" + "Data Modified");

                                // view changed data
                                Console.WriteLine("\n" + "Fetched Customer Details [cache]: ");
                                distributedDictionary = _cache.DataTypeManager.GetDictionary<string, object>(City);
                                DisplayDictionaryData(distributedDictionary);
                            }

                            if (!ConsolePause(false).Equals(ConsoleKey.Escape))
                            {
                                // remove entry from dictionary
                                Console.WriteLine("\n" + "Removing Dictionary Entry");
                                // take customer ID input from user
                                Console.Write("Enter Customer ID: ");
                                CustomerID = Convert.ToString(Console.ReadLine());
                                RemoveDictionaryValue(distributedDictionary);
                                Console.WriteLine("\n" + "Dictionary Entry Removed");

                                // view changed data
                                Console.WriteLine("\n" + "Fetched Customer Details [readthru]: ");
                                distributedDictionary = _cache.DataTypeManager.GetDictionary<string, object>(City, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));
                                DisplayDictionaryData(distributedDictionary);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nCustomer with provided City does not exist.");
                        }
                        break;
                    // 4 - Get Customers by Country Name
                    case MainMenu.GetDistributedList:
                        // take country name input from user
                        Console.Write("Enter Country Name: ");
                        Country = Convert.ToString(Console.ReadLine());

                        // fetch customers with same country name
                        distributedList = _cache.DataTypeManager.GetList<object>(Country, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));

                        if (distributedList != null)
                        {
                            DisplayListData(distributedList);

                            if (ConsolePause(false).Equals(ConsoleKey.Escape))
                            {
                                // modify first entry of list
                                Console.WriteLine("\nModify First Record [writethru]: ");
                                ModifyListValue(distributedList);

                                // view changed data
                                Console.WriteLine("\n" + "Fetched Customer Details [cache]: ");
                                distributedList = _cache.DataTypeManager.GetList<object>(Country);
                                DisplayListData(distributedList);
                            }

                            if (!ConsolePause(false).Equals(ConsoleKey.Escape))
                            {
                                // remove entry from dictionary
                                Console.WriteLine("\n" + "Removing List Entry");
                                // take customer ID input from user
                                Console.Write("Enter Customer ID: ");
                                CustomerID = Convert.ToString(Console.ReadLine());
                                RemoveListValue(distributedList);
                                Console.WriteLine("\n" + "List Entry Removed");

                                // view changed data
                                Console.WriteLine("\n" + "Fetched Customer Details [readthru]: ");
                                distributedDictionary = _cache.DataTypeManager.GetDictionary<string, object>(City, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));
                                DisplayDictionaryData(distributedDictionary);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nCustomer with provided Country does not exist.");
                        }
                        break;
                    // 5 - Get Customers by a Specific Order
                    case MainMenu.GetDistributedQueue:
                        // take sorting field input from user
                        Console.Write("Enter Sorting Field Name: ");
                        string sort = Convert.ToString(Console.ReadLine());

                        distributedQueue = null;
                        try
                        {
                            // fetch customers sorted by sorting field
                            distributedQueue = _cache.DataTypeManager.GetQueue<object>(sort, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));
                        }
                        catch(Alachisoft.NCache.Runtime.Exceptions.OperationFailedException e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        if (distributedQueue != null)
                        {
                            DisplayQueueData(distributedQueue);

                            if (!ConsolePause(false).Equals(ConsoleKey.Escape))
                            {

                                // Add a new customer
                                Console.WriteLine("\nAdd new customer [writethru]: ");
                                AddDataToQueue(distributedQueue);

                                // view changed data
                                Console.WriteLine("\n" + "Fetched Customer Details [cache]: ");
                                distributedQueue = _cache.DataTypeManager.GetQueue<object>(sort);
                                DisplayQueueData(distributedQueue);
                            }

                            if (!ConsolePause(false).Equals(ConsoleKey.Escape))
                            {
                                // remove entry from dictionary
                                Console.WriteLine("\n" + "Dequeuing Queue Entry");
                                // take customer ID input from user
                                Console.Write("Enter Customer ID: ");
                                CustomerID = Convert.ToString(Console.ReadLine());
                                RemoveQueueValue(distributedQueue);
                                Console.WriteLine("\n" + "Queue Entry Dequeued");

                                // view changed data
                                Console.WriteLine("\n" + "Fetched Customer Details [readthru]: ");
                                distributedDictionary = _cache.DataTypeManager.GetDictionary<string, object>(City, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));
                                DisplayDictionaryData(distributedDictionary);
                            }
                        }
                        break;
                    // 6 - Get Order IDs of a customer
                    case MainMenu.GetDistributedHashSet:
                        // take customer ID input from user
                        Console.Write("Enter Customer ID: ");
                        CustomerID = Convert.ToString(Console.ReadLine());

                        // fetch order IDs of a customer
                        distributedHashSet = _cache.DataTypeManager.GetHashSet<int>(CustomerID, new ReadThruOptions(ReadMode.ReadThru, ReadThruProviderName));

                        if (distributedHashSet != null)
                        {
                            DisplayHashSetData(distributedHashSet);
                        }
                        else
                        {
                            Console.WriteLine("\nCustomer with provided ID does not exist.");
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// initialize cache
        /// </summary>
        private void InitializeCache()
        {
            string cacheId = System.Configuration.ConfigurationManager.AppSettings["CacheID"];

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

        #region Helper Functions

        public ConsoleKey ConsolePause(bool clearScreen = true)
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue (or escape to skip additional functioning)...");
            ConsoleKey key = Console.ReadKey().Key;
            if(clearScreen)
                Console.Clear();
            return key;
        }

        public enum MainMenu
        {
            GetCacheItem = 1,
            GetDistributedCounter = 2,
            GetDistributedDictionary = 3,
            GetDistributedList = 4,
            GetDistributedQueue = 5,
            GetDistributedHashSet = 6
        }

        #endregion
    }
}