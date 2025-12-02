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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Sample.Data;

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

namespace Alachisoft.NCache.Samples.BackingSource
{
    /// <summary>
    /// Contains methods used to read an object from the master data source and store in cache
    /// </summary>
    public class SqlReadThruProvider : IReadThruProvider
    {
        // Connection string to connect to the database
        private string? _connectionString;

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
                throw new ArgumentNullException(nameof(parameters));

            // Getting connection string from parameters
            object? connString = parameters["ConnectionString"];
            _connectionString = connString?.ToString() ??
                throw new ArgumentException("Connection string not provided in parameters.");
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
            var customer = LoadCustomer(key);
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
            var providerItems = new Dictionary<string, ProviderCacheItem>();

            // Iterating through provided keys and loading data for each
            foreach (string key in keys)
            {
                // Loading customer record for the key
                var data = LoadCustomer(key);
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
        public ProviderDataTypeItem<IEnumerable>? LoadDataTypeFromSource(string key, DistributedDataType dataType)
        {
            ProviderDataTypeItem<IEnumerable>? providerItem = null;

            // Checking which data type is being requested and loading accordingly
            switch (dataType)
            {
                case DistributedDataType.Counter:
                    // Returning count of customers grouped by company name
                    providerItem = new ProviderDataTypeItem<IEnumerable>(GetCustomerCountByCompanyName(key));
                    break;

                case DistributedDataType.Dictionary:
                    // Loading all customers belonging to a specific city
                    var customers = LoadCustomersByCity(key);
                    if (customers == null)
                        return null;

                    // Converting list of customers to a dictionary with CustomerID as key
                    var dict = new Dictionary<string, Customer>();
                    foreach (var c in customers)
                    {
                        if (c?.CustomerID != null)
                            dict[c.CustomerID] = c;
                    }

                    // Wrapping dictionary in ProviderDataTypeItem
                    providerItem = new ProviderDataTypeItem<IEnumerable>((IEnumerable)dict);
                    break;

                case DistributedDataType.List:
                    // Loading all customers belonging to a specific country
                    providerItem = new ProviderDataTypeItem<IEnumerable>(LoadCustomersFromCountry(key));
                    break;

                case DistributedDataType.Queue:
                    // Loading all customers linked with a specific order
                    var data = LoadCustomerQueue();
                    if (data == null || !data.Any())
                        return null;

                    providerItem = new ProviderDataTypeItem<IEnumerable>((IEnumerable)data);
                    break;

                case DistributedDataType.Set:
                    {
                        // Accepting keys with optional "OrdersSet:" prefix
                        string customerId = key;

                        // Defining prefix to identify order sets
                        const string prefix = "OrdersSet:";

                        // Removing prefix if present
                        if (customerId.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                            customerId = customerId.Substring(prefix.Length);

                        // Loading order IDs for the specified customer
                        var orderIds = LoadOrderIDsByCustomer(customerId);
                        if (orderIds == null)
                            return null;

                        // Wrapping in a HashSet and returning as IEnumerable (provider signature expects IEnumerable)
                        ISet<object> hashSet = new HashSet<object>(orderIds);
                        providerItem = new ProviderDataTypeItem<IEnumerable>((IEnumerable)hashSet);
                        break;
                    }

            }

            // Returning loaded provider item
            return providerItem;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load a single customer record from the database
        /// </summary>
        /// <param name="customerId">ID of the customer to be loaded</param>
        /// <returns>Customer object or null if not found</returns>
        private Customer? LoadCustomer(string customerId)
        {
            // SQL query to retrieve customer details by ID
            const string query = "SELECT CustomerID, CompanyName, ContactName, City, Country FROM Customers WHERE CustomerID = @CustomerID";

            // Creating SQL connection and command
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CustomerID", customerId);

            // Opening connection and executing query
            conn.Open();
            using var reader = cmd.ExecuteReader();

            // If a record is found, create and return Customer object
            if (reader.Read())
            {
                return new Customer
                {
                    CustomerID = reader["CustomerID"].ToString(),
                    CompanyName = reader["CompanyName"].ToString(),
                    ContactName = reader["ContactName"].ToString(),
                    City = reader["City"].ToString(),
                    Country = reader["Country"].ToString()
                };
            }
            // Closing connection after execution
            conn.Close();

            // If no record is found, return null
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
            var customers = new List<Customer>();

            // Creating SQL connection and command
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@City", city);

            // Opening connection and executing query
            conn.Open();
            using var reader = cmd.ExecuteReader();

            // Iterating through result set and adding customer objects to the list
            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerID = reader["CustomerID"].ToString(),
                    CompanyName = reader["CompanyName"].ToString(),
                    ContactName = reader["ContactName"].ToString(),
                    City = reader["City"].ToString(),
                    Country = reader["Country"].ToString()
                });
            }
            // Closing connection after execution
            conn.Close();

            // Returning the list of customers
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

            // Creating SQL connection and command for retrieving customers by country
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            // Adding country parameter
            cmd.Parameters.AddWithValue("@Country", country);

            // Opening connection and executing query
            conn.Open();
            using var reader = cmd.ExecuteReader();

            // Creating list to hold customer records
            List<object> customers = new List<object>();

            // Iterating through result set and adding customer objects to the list
            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerID = reader["CustomerID"].ToString(),
                    CompanyName = reader["CompanyName"].ToString()
                });
            }

            // Closing connection after execution
            conn.Close();

            // Returning the list of customers
            return customers;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to load customers linked with a specific order
        /// </summary>
        /// <param name="orderId">Order ID to filter customers</param>
        /// <returns>Enumerable of customer objects</returns>
        private IEnumerable<object> LoadCustomersByOrder(string orderId)
        {
            // SQL query to retrieve customers linked with a specific order
            const string query = "SELECT c.CustomerID, c.CompanyName FROM Customers c " +
                                 "INNER JOIN Orders o ON c.CustomerID = o.CustomerID " +
                                 "WHERE o.OrderID = @OrderID";

            // Creating SQL connection and command for retrieving customers linked with an order
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            // Adding order ID parameter
            cmd.Parameters.AddWithValue("@OrderID", orderId);

            // Opening connection and executing query
            conn.Open();
            using var reader = cmd.ExecuteReader();

            // Creating list to hold customer records
            List<object> customers = new List<object>();

            // Iterating through result set and adding customer objects to the list
            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerID = reader["CustomerID"].ToString(),
                    CompanyName = reader["CompanyName"].ToString()
                });
            }

            // Closing connection after execution
            conn.Close();

            // Returning the list of customers
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

            // Creating SQL connection and command for retrieving order IDs by customer
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            // Adding customer ID parameter
            cmd.Parameters.AddWithValue("@CustomerID", customerId);

            // Opening connection and executing query
            conn.Open();
            using var reader = cmd.ExecuteReader();

            // Creating list to hold order IDs
            List<object> orderIds = new List<object>();

            // Iterating through result set and adding order IDs to the list
            while (reader.Read())
            {
                orderIds.Add(reader["OrderID"]);
            }
            // Closing connection after execution
            conn.Close();

            // Returning the list of order IDs
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
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            // Adding company name parameter
            cmd.Parameters.AddWithValue("@CompanyName", companyName);

            // Opening connection and executing scalar query
            conn.Open();

            // Retrieving and returning the count result
            var result = Convert.ToInt64(cmd.ExecuteScalar());

            // Closing connection after execution
            conn.Close();

            return result;
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

            // Creating SQL connection and command for retrieving customer batch
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            // Opening connection and executing query
            conn.Open();
            using var reader = cmd.ExecuteReader();

            List<Customer> customers = new List<Customer>();

            // Iterating through result set and adding customer objects to the list
            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerID = reader["CustomerID"].ToString(),
                    CompanyName = reader["CompanyName"].ToString(),
                    ContactName = reader["ContactName"].ToString(),
                    City = reader["City"].ToString(),
                    Country = reader["Country"].ToString()
                });
            }

            // Closing connection after execution
            conn.Close();

            // Returning the list of customers
            return customers;
        }
    }
}
