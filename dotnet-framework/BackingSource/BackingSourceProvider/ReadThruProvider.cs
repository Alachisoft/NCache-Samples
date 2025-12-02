// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Sample.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ===============================================================================
// NCache Read-Thru Provider is used to integrate NCache with an underlying data
// source. It allows NCache to automatically fetch data from the data source when
// a requested item is not found in the cache. This mechanism helps maintain data
// consistency between the cache and the data source, without the use of any extra
// code, while also improving application performance by reducing direct database
// access. Some use cases of Read-Thru Provider include:
//   1. Lazy Loading: Automatically loading data into the cache on-demand.
//   2. Data Synchronization: Keeping the cache updated with the latest data from
//      the data source.
//   3. Simplified Data Access: Abstracting the data retrieval logic from the
//      application code.
// ===============================================================================

namespace Alachisoft.NCache.Samples.BackingSourceProvider
{
    /// <summary>
    /// Contains methods used to read an object from the master data source and store in cache
    /// </summary>
    public class ReadThruProvider : IReadThruProvider
    {
        // Connection string for database
        private string _connectionString;

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to initialize the read-thru provider
        /// </summary>
        /// <param name="parameters">Startup parameters defined in the configuration</param>
        /// <param name="cacheName">Name of the cache</param>
        public void Init(IDictionary parameters, string cacheName)
        {
            // Retrieving connection string from parameters
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            // Getting connection string from parameters
            object connString = parameters["ConnectionString"];
            if (connString == null)
                throw new ArgumentException("Connection string not provided.");

            _connectionString = connString.ToString();
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to dispose the read-thru provider
        /// </summary>
        public void Dispose()
        {
            // Clearing connection string on dispose
            _connectionString = null;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load data from the source for a single key
        /// </summary>
        /// <param name="key">Key for which data is to be loaded</param>
        /// <returns>Loaded cache item</returns>
        public ProviderCacheItem LoadFromSource(string key)
        {
            // Loading customer record for a single key
            Customer customer = LoadCustomer(key);

            if (customer == null)
                return null;

            // Creating cache item and enabling resync option
            var cacheItem = new ProviderCacheItem(customer)
            {
                ResyncOptions = { ResyncOnExpiration = true }
            };

            // Returning prepared cache item
            return cacheItem;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load data from the source for multiple keys
        /// </summary>
        /// <param name="keys">Collection of keys for which data is to be loaded</param>
        /// <returns>Dictionary of loaded cache items</returns>
        public IDictionary<string, ProviderCacheItem> LoadFromSource(ICollection<string> keys)
        {
            // Initializing dictionary to hold loaded cache items
            Dictionary<string, ProviderCacheItem> providerItems = new Dictionary<string, ProviderCacheItem>();

            foreach (string key in keys)
            {
                // Loading customer record for the key
                Customer data = LoadCustomer(key);
                var item = new ProviderCacheItem(data)
                {
                    Expiration = new Expiration(ExpirationType.DefaultAbsolute),
                    Group = "customers"
                };

                // Adding loaded item to dictionary
                providerItems[key] = item;
            }

            // Returning all loaded items to cache
            return providerItems;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load data from the source for distributed data types
        /// </summary>
        /// <param name="key">Key for which data is to be loaded</param>
        /// <param name="dataType">Type of distributed data structure</param>
        /// <returns>Loaded provider data type item</returns>
        public ProviderDataTypeItem<IEnumerable> LoadDataTypeFromSource(string key, DistributedDataType dataType)
        {
            ProviderDataTypeItem<IEnumerable> providerItem = null;

            // Checking which data type is being requested and loading accordingly
            switch (dataType)
            {
                case DistributedDataType.Counter:
                    // Returning count of customers grouped by company name
                    long count = GetCustomerCountByCompanyName(key);
                    providerItem = new ProviderDataTypeItem<IEnumerable>(new List<long> { count });
                    break;

                case DistributedDataType.Dictionary:
                    // Loading all customers belonging to a specific city
                    IEnumerable<Customer> customersByCity = LoadCustomersByCity(key);

                    // Converting list of customers to a dictionary with CustomerID as key
                    Dictionary<string, Customer> dict = new Dictionary<string, Customer>();

                    foreach (Customer c in customersByCity)
                    {
                        if (!string.IsNullOrEmpty(c.CustomerID))
                            dict[c.CustomerID] = c;
                    }

                    // Wrapping dictionary in ProviderDataTypeItem
                    providerItem = new ProviderDataTypeItem<IEnumerable>(dict);
                    break;

                case DistributedDataType.List:
                    // Loading all customers belonging to a specific country
                    providerItem = new ProviderDataTypeItem<IEnumerable>(LoadCustomersFromCountry(key));
                    break;

                case DistributedDataType.Queue:
                    // Loading all customers linked with a specific order
                    IEnumerable<object> queueCustomers = LoadCustomerQueue();
                    providerItem = new ProviderDataTypeItem<IEnumerable>(queueCustomers);
                    break;

                case DistributedDataType.Set:
                    // Accepting keys with optional "OrdersSet:" prefix
                    string customerId = key;

                    // Defining prefix to identify order sets
                    const string prefix = "OrdersSet:";

                    // Removing prefix if present
                    if (customerId.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        customerId = customerId.Substring(prefix.Length);

                    // Loading order IDs for the specified customer
                    IEnumerable<object> orderIds = LoadOrderIDsByCustomer(customerId);

                    // Wrapping in a HashSet and returning as IEnumerable (provider signature expects IEnumerable)
                    HashSet<object> set = new HashSet<object>(new List<object>(orderIds));
                    providerItem = new ProviderDataTypeItem<IEnumerable>(set);
                    break;
            }

            return providerItem;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load a single customer record from the database
        /// </summary>
        /// <param name="customerId">ID of the customer to be loaded</param>
        /// <returns>Customer object or null if not found</returns>
        private Customer LoadCustomer(string customerId)
        {
            // SQL query to retrieve customer details by ID
            const string query = "SELECT CustomerID, CompanyName, ContactName, City, Country FROM Customers WHERE CustomerID = @CustomerID";

            // Creating SQL connection and command
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Opening connection and executing query
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // If a record is found, create and return Customer object
                    if (reader.Read())
                    {
                        Customer c = new Customer();
                        c.CustomerID = reader["CustomerID"].ToString();
                        c.CompanyName = reader["CompanyName"].ToString();
                        c.ContactName = reader["ContactName"].ToString();
                        c.City = reader["City"].ToString();
                        c.Country = reader["Country"].ToString();
                        return c;
                    }
                }


                // Closing connection after execution
                conn.Close();

            }

            return null;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load customers by city
        /// </summary>
        /// <param name="city">City name to filter customers</param>
        /// <returns>Enumerable of customer objects</returns>
        private IEnumerable<Customer> LoadCustomersByCity(string city)
        {
            // SQL query to retrieve customers by city
            const string query = "SELECT CustomerID, CompanyName, ContactName, City, Country FROM Customers WHERE City = @City";

            // Initializing list to hold customer records
            List<Customer> customers = new List<Customer>();

            // Creating SQL connection and command
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@City", city);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Iterating through result set and adding customer objects to the list
                    while (reader.Read())
                    {
                        Customer c = new Customer();
                        c.CustomerID = reader["CustomerID"].ToString();
                        c.CompanyName = reader["CompanyName"].ToString();
                        c.ContactName = reader["ContactName"].ToString();
                        c.City = reader["City"].ToString();
                        c.Country = reader["Country"].ToString();
                        customers.Add(c);
                    }
                }

                // Closing connection after execution
                conn.Close();
            }
            return customers;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load customers by country
        /// </summary>
        /// <param name="country">Country name to filter customers</param>
        /// <returns>Enumerable of customer objects</returns>
        private IEnumerable<object> LoadCustomersFromCountry(string country)
        {
            // SQL query to retrieve customers by country
            const string query = "SELECT CustomerID, CompanyName FROM Customers WHERE Country = @Country";

            List<object> customers = new List<object>();

            // Creating SQL connection and command for retrieving customers by country
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Adding country parameter
                cmd.Parameters.AddWithValue("@Country", country);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer c = new Customer();
                        c.CustomerID = reader["CustomerID"].ToString();
                        c.CompanyName = reader["CompanyName"].ToString();
                        customers.Add(c);
                    }
                }

                // Closing connection after execution
                conn.Close();
            }
            return customers;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load a batch of customers for demonstration of queue data type
        /// </summary>
        /// <returns>Enumerable of customer objects</returns>
        private IEnumerable<object> LoadCustomerQueue()
        {
            // SQL query to retrieve a batch of customers
            const string query = "SELECT TOP 10 CustomerID, CompanyName, ContactName, City, Country FROM Customers ORDER BY CustomerID";

            List<object> customers = new List<object>();

            // Creating SQL connection and command for retrieving customer batch
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer c = new Customer();
                        c.CustomerID = reader["CustomerID"].ToString();
                        c.CompanyName = reader["CompanyName"].ToString();
                        c.ContactName = reader["ContactName"].ToString();
                        c.City = reader["City"].ToString();
                        c.Country = reader["Country"].ToString();
                        customers.Add(c);
                    }
                }

                // Closing connection after execution
                conn.Close();
            }
            return customers;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load order IDs by customer
        /// </summary>
        /// <param name="customerId">Customer ID to filter order IDs</param>
        /// <returns>Enumerable of order IDs</returns>
        private IEnumerable<object> LoadOrderIDsByCustomer(string customerId)
        {
            // SQL query to retrieve order IDs by customer
            const string query = "SELECT OrderID FROM Orders WHERE CustomerID = @CustomerID";

            List<object> orderIds = new List<object>();

            // Creating SQL connection and command for retrieving order IDs by customer
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderIds.Add(reader["OrderID"]);
                    }
                }

                // Closing connection after execution
                conn.Close();
            }
            return orderIds;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to get customer count by company name
        /// </summary>
        /// <param name="companyName">Company name to filter customers</param>
        /// <returns>Count of customers</returns>
        private long GetCustomerCountByCompanyName(string companyName)
        {
            // SQL query to count customers by company name
            const string query = "SELECT COUNT(*) FROM Customers WHERE CompanyName = @CompanyName";

            // Creating SQL connection and command for retrieving customer count
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CompanyName", companyName);
                conn.Open();
                // Retrieving and returning the count result
                var result = Convert.ToInt64(cmd.ExecuteScalar());

                // Closing connection after execution
                conn.Close();

                return result;
            }
        }
    }
}
