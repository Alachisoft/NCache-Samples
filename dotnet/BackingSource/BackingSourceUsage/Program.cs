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
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Client.DataTypes.Collections;

// ===============================================================================
// NCache backing source is a powerful feature that allows seamless integration of
// NCache with external data sources. This mechanisms ensures data consistency
// between the cache and the database. Some simple use cases include:
//   1. Caching frequently accessed data to reduce database load.
//   2. Ensuring data consistency between cache and database.
//   3. Simplifying application logic by abstracting data retrieval and storage.
// ===============================================================================

// Connect to a running cache and get cache handle for it
ICache? cache = GetCache();

// Fetch config values for providers once and reuse globally
string readThruProvider = ConfigurationManager.AppSettings["ReadThruProviderName"] ?? "SqlReadThruProvider";
string writeThruProvider = ConfigurationManager.AppSettings["WriteThruProviderName"] ?? "SqlWriteThruProvider";

// Fetching a customer using ReadThru
Customer? customer = FetchCustomerUsingReadThru("ALFKI");

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
cache?.Dispose();

/// ------------------------------------------------------------------------------------------
/// <summary>
/// Method to connect to a running cache and return cache handle for it 
/// </summary>
/// <returns>Cache handle for the connected cache</returns>
ICache? GetCache()
{
    // Getting cache name from configuration file
    string? cacheName = ConfigurationManager.AppSettings["CacheName"];

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
Customer? FetchCustomerUsingReadThru(string customerId)
{
    Console.WriteLine("Fetching customer using ReadThru...");

    // Attempting to get customer from cache; if not found, ReadThru will fetch from DB
    Customer? customer = cache.Get<Customer>(
        customerId,
        new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
    );

    // Printing fetched customer details
    if (customer != null)
    {
        Console.WriteLine("Customer fetched via ReadThru and cached.\n");
        PrintCustomer(customer);
    }
    else
    {
        Console.WriteLine("Customer not found in DB.\n");
    }

    // Returning fetched customer
    return customer;
}

/// ------------------------------------------------------------------------------------------
/// <summary>
/// Method to update a customer using WriteThru
/// </summary>
/// <param name="customer">Customer object to be updated</param>
void UpdateCustomerUsingWriteThru(Customer? customer)
{
    // Validating customer object
    if (customer == null) return;

    Console.WriteLine("Updating customer contact name using WriteThru...");

    // Modifying customer contact name
    customer.ContactName = "Updated Contact - Demo";
    CacheItem item = new CacheItem(customer);

    // Inserting updated customer back into cache; WriteThru will update DB as well
    cache?.Insert(
        customer.CustomerID,
        item,
        new WriteThruOptions(WriteMode.WriteThru, writeThruProvider)
    );

    // Printing updated customer details
    Console.WriteLine("Customer updated in DB and cache.\n");
}

/// ------------------------------------------------------------------------------------------
/// <summary>
/// Method to demonstrate distributed counter for company customer count
/// </summary>
void DemonstrateCounter()
{
    Console.WriteLine("Demonstrating Distributed Counter...");

    // Obtaining counter instance from cache
    ICounter counter = cache.DataTypeManager.GetCounter(
        "North/South",
        new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
    );

    // Displaying current counter value
    Console.WriteLine($"Current Counter Value: {counter.Value}");
    counter.Increment();
    Console.WriteLine($"Counter incremented. New Value: {counter.Value}\n");
}

/// ------------------------------------------------------------------------------------------
/// <summary>
/// Method to demonstrate distributed dictionary for customers of a city
/// </summary>
/// <param name="city">City name for which customers are to be fetched</param>
void DemonstrateDictionary(string city)
{
    Console.WriteLine($"Demonstrating Distributed Dictionary for city '{city}'...");

    // Obtaining distributed dictionary instance from cache
    var dict = cache.DataTypeManager.GetDictionary<string, object>(
        city,
        new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
    );

    // Validating dictionary
    if (dict == null)
    {
        Console.WriteLine($"No customers found for city '{city}'.\n");
        return;
    }

    // Displaying retrieved customers
    Console.WriteLine($"Retrieved {dict.Count} customers from '{city}'.");
    foreach (var pair in dict)
        PrintCustomer((Customer)pair.Value);

    // Updating first record in dictionary using WriteThru
    Console.WriteLine("Updating first record using WriteThru...");
    foreach (var key in dict.Keys)
    {
        // Update the contact name of the customer
        var cust = (Customer)dict[key];
        cust.ContactName += " [Edited]";
        dict.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, writeThruProvider);
        dict[key] = cust;
        break;
    }

    // Indicate completion
    Console.WriteLine("Dictionary updated successfully.\n");
}

/// ------------------------------------------------------------------------------------------
/// <summary>
/// Method to demonstrate distributed list for customers of a country
/// </summary>
/// <param name="country">Country name for which customers are to be fetched</param>
void DemonstrateList(string country)
{
    Console.WriteLine($"Demonstrating Distributed List for '{country}'...");

    // Obtaining distributed list instance from cache
    var list = cache.DataTypeManager.GetList<object>(
        country,
        new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
    );

    // Validating list
    if (list == null)
    {
        Console.WriteLine($"No customers found for '{country}'.\n");
        return;
    }

    // Displaying retrieved customers
    Console.WriteLine($"Retrieved {list.Count} customers from '{country}'.");
    foreach (Customer c in list)
        PrintCustomer(c);

    Console.WriteLine("Updating first record using WriteThru...");
    // Updating first record in list using WriteThru
    if (list.Count > 0)
    {
        // Updating the company name of the first customer
        list.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, writeThruProvider);
        var first = (Customer)list[0];
        first.CompanyName += " [Updated]";
        list[0] = first;
    }

    Console.WriteLine("List updated successfully.\n");
}

/// ------------------------------------------------------------------------------------------
/// <summary>
/// Method to demonstrate distributed queue for customers
/// </summary>
void DemonstrateQueue()
{
    Console.WriteLine("Demonstrating Distributed Queue...");

    // Obtaining distributed queue instance from cache
    var queue = cache.DataTypeManager.GetQueue<object>(
        "CustomerQueue",
        new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
    );

    // Validating queue
    if (queue == null)
    {
        Console.WriteLine("No queue data found.\n");
        return;
    }

    // Displaying retrieved customers
    Console.WriteLine($"Queue contains {queue.Count} customers.");
    foreach (Customer c in queue)
        PrintCustomer(c);

    // Enqueuing a new customer using WriteThru
    Console.WriteLine("Enqueuing new customer using WriteThru...");
    queue.WriteThruOptions = new WriteThruOptions(WriteMode.WriteThru, writeThruProvider);

    // Creating a new customer object
    Customer newCust = new Customer
    {
        CustomerID = "NEW01",
        ContactName = "Queue Demo Contact",
        CompanyName = "QueueCo",
        Country = "USA",
        City = "New York",
        Address = "123 Demo St",
        PostalCode = "10001",
        ContactNo = "111-222-3333",
        Fax = "111-222-3334"
    };

    // Enqueue the new customer
    queue.Enqueue(newCust);
    Console.WriteLine("Customer added to queue.\n");
}

/// ------------------------------------------------------------------------------------------
/// <summary>
/// Method to demonstrate distributed hash set for order IDs of a customer
/// </summary>
/// <param name="customer">Customer whose order IDs are to be fetched</param>
void DemonstrateHashSet(Customer? customer)
{
    // Validating customer object
    if (customer == null) return;

    Console.WriteLine("Demonstrating Distributed HashSet...");

    // Unique key for the customer's order ID set
    string setKey = $"OrdersSet:{customer.CustomerID}";

    // Remove existing set to demonstrate fresh ReadThru fetch
    cache?.Remove(setKey);

    // Obtaining distributed hash set instance from cache
    var hashSet = cache.DataTypeManager.GetHashSet<int>(
        setKey,
        new ReadThruOptions(ReadMode.ReadThru, readThruProvider)
    );

    // Validating hash set
    if (hashSet == null)
    {
        Console.WriteLine($"No order IDs found for customer '{customer.CustomerID}'.\n");
        return;
    }

    // Displaying retrieved order IDs
    Console.WriteLine($"Retrieved {hashSet.Count} order IDs for '{customer.CustomerID}':");
    foreach (int id in hashSet)
        Console.Write(id + "\t");
    Console.WriteLine("\n");
}

/// ------------------------------------------------------------------------------------------
/// <summary>
/// Method to print customer details to console
/// </summary>
/// <param name="customer">Customer object whose details are to be printed</param>
void PrintCustomer(Customer customer)
{
    // Validating customer object
    if (customer == null) return;

    // Helper function to handle null or empty strings
    static string Safe(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "N/A" : value;

    Console.WriteLine("---------------------------------------------");
    Console.WriteLine($"CustomerID : {Safe(customer.CustomerID)}");
    Console.WriteLine($"ContactName: {Safe(customer.ContactName)}");
    Console.WriteLine($"CompanyName: {Safe(customer.CompanyName)}");
    Console.WriteLine($"City       : {Safe(customer.City)}");
    Console.WriteLine($"Country    : {Safe(customer.Country)}");
    Console.WriteLine("---------------------------------------------\n");
}