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

// ===============================================================================
// NCache allows you to perform basic caching operations using synchronous APIs.
// The following synchronous operations are demonstrated in this sample:
//   1. Connecting to a running cache
//   2. Adding an object to cache
//   3. Getting an object from cache
//   4. Updating an existing object in cache
//   5. Deleting an object from cache
// ===============================================================================

// Connect to a running cache and get cache handle for it 
ICache? cache = GetCache();

// Validating cache handle
if (cache == null)
{
    Console.Write("Cache connection could not be established. Exiting the sample.");
    return;
}

// Creating a customer object and key that will be used in this sample
Customer customer = CreateNewCustomer();
string key = GetKey(customer);

// Adding customer to cache synchronously
AddToCache(key, customer);

// Getting the customer from cache
customer = GetFromCache(key);

// Modifying the customer and updating in cache
UpdateInCache(key, customer);

// Deleting the existing customer from cache
RemoveFromCache(key);

// Dispose the cache once done
cache?.Dispose();

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to connect to a running cache and return cache handle for it 
/// </summary>
/// <returns>Cache handle for the connected cache</returns>
ICache GetCache()
{
    // Getting cache name from configuration file
    string? cacheName = ConfigurationManager.AppSettings["CacheName"];

    // Validating cache name
    if (String.IsNullOrEmpty(cacheName))
    {
        Console.WriteLine("The CacheName cannot be null or empty.");
        return null;
    }

    // Trying to connect to cache
    try
    {
        // Connecting to a running cache and return a cache handle for it
        ICache cache = CacheManager.GetCache(cacheName);

        // Printing output on console
        Console.WriteLine(string.Format("Cache '{0}' is connected.", cacheName));

        // Returning cache handle
        return cache;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error connecting to cache '{cacheName}': {ex.Message}");
        return null;
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to add object in the cache using synchronous API
/// </summary>
/// <param name="key">String key to be added in cache</param>
/// <param name="customer">Instance of Customer that will be added to cache</param>
void AddToCache(string key, Customer customer)
{
    // Creating time span for setting expiration interval
    TimeSpan expirationInterval = TimeSpan.FromMinutes(1);

    // Creating expiration interval with absolute type to assign to cache item
    Expiration expiration = new Expiration(ExpirationType.Absolute);
    expiration.ExpireAfter = expirationInterval;

    //Populating cache item and adding new expiration
    CacheItem item = new CacheItem(customer);
    item.Expiration = expiration;

    // Adding cacheitem to cache with an absolute expiration of 1 minute
    try
    {
        cache?.Add(key, item);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error adding item to cache: " + ex.Message);
    }
    
    // Print output on console
    Console.WriteLine("\nObject is added to cache.");
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to get customer from the cache using synchronous api
/// </summary>
/// <param name="key">Key of customer</param>
/// <returns>Instance of Customer retrieved from cache</returns>
Customer GetFromCache(string key)
{
    // Fetching the customer from cache
    Customer cachedCustomer = cache.Get<Customer>(key);
    Console.WriteLine("\nObject is fetched from cache");

    // Printing output on console
    PrintCustomerDetails(cachedCustomer);
    return cachedCustomer;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to modify customer and update it in cache
/// </summary>
/// <param name="key">Key of the item</param>
/// <param name="customer">Instance of Customer</param>
void UpdateInCache(string key, Customer customer)
{
    // Changing customer object
    customer.CompanyName = "Gourmet Lanchonettes";

    // Creating expiration time of 30 seconds
    TimeSpan expirationInterval = TimeSpan.FromSeconds(30);

    // Creating expiration interval with sliding type
    Expiration expiration = new Expiration(ExpirationType.Sliding);
    expiration.ExpireAfter = expirationInterval;

    // Creating cache item with required data and adding expiration
    CacheItem item = new CacheItem(customer);
    item.Expiration = expiration;

    // Updating the item in cache
    cache?.Insert(key, item);

    // Printing output on console
    Console.WriteLine("\nObject is updated in cache.");
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to delete the customer from cache
/// </summary>
/// <param name="key">Key of customer to be deleted from cache</param>
void RemoveFromCache(string key)
{
    // Removing the existing customer
    cache?.Remove(key);

    // Printing output on console
    Console.WriteLine("\nObject is removed from cache.");
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to generate instance of Customer to be used in this sample
/// </summary>
/// <returns>Instance of Customer</returns>
Customer CreateNewCustomer()
{
    return new Customer
    {
        CustomerID = "DAVJO",
        ContactName = "David Johnes",
        CompanyName = "Lonesome Pine Restaurant",
        ContactNo = "12345-6789",
        Address = "Silicon Valley, Santa Clara, California",
    };
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to generate a string key for specified customer
/// </summary>
/// <param name="customer">Instance of Customer</param>
/// <returns>Key for the specified customer</returns>
string GetKey(Customer customer)
{
    return string.Format("Customer:{0}", customer.CustomerID);
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to print the details of customer object on the console
/// </summary>
/// <param name="customer">Instance of Customer</param>
void PrintCustomerDetails(Customer customer)
{
    // Validating customer object
    if (customer == null) return;

    // Printing customer details on console
    Console.WriteLine();
    Console.WriteLine("Customer Details are as follows: ");
    Console.WriteLine("ContactName: " + customer.ContactName);
    Console.WriteLine("CompanyName: " + customer.CompanyName);
    Console.WriteLine("Contact No.: " + customer.ContactNo);
    Console.WriteLine("Address: " + customer.Address);
    Console.WriteLine();
}