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
using Alachisoft.NCache.Sample.Data;
using System.Configuration;

// ===============================================================================
// NCache allows you to perform bulk operations on the cache using bulk APIs. The
// following bulk operations are performed using bulk APIs in this sample:
//   1. Adding multiple items to cache in bulk
//   2. Updating multiple items in cache in bulk
//   3. Getting multiple items from cache in bulk
//   4. Removing multiple items from cache in bulk
// ===============================================================================

// Connect to a running cache and get cache handle for it 
ICache cache = GetCache();

// Validating cache handle
if (cache == null)
{
    Console.Write("Cache connection could not be established. Exiting the sample.");
    return;
}

// Generating multiple customers and their keys that will be used in this sample
Customer[] customers = CreateNewCustomers();
string[] custKeys = GetKeys(customers);

// Generating hashmap of the keys and products
IDictionary<string, CacheItem> customerItems = GetCacheItemDictionary(custKeys, customers);

// Adding multiple items in cache using bulk API
AddMultipleObjectsToCache(customerItems);

// Updating multiple items in cache using bulk API
UpdateMultipleObjectsInCache(custKeys, customerItems);

// Getting multiple items from cache
GetMultipleObjectsFromCache(custKeys);

// Removing the existing objects using bulk API
RemoveMultipleObjectsFromCache(custKeys);

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
/// Method to create an array of customers to be used in this sample
/// </summary>
/// <returns>Array of Customers</returns>
Customer[] CreateNewCustomers()
{
    // Creating sample customers
    Customer[] customers = new Customer[4];
    customers[0] = new Customer() { CustomerID = "C001", ContactName = "John", CompanyName = "MNO Solutions" };
    customers[1] = new Customer() { CustomerID = "C002", ContactName = "Sam", CompanyName = "IJK Digital" };
    customers[2] = new Customer() { CustomerID = "C003", ContactName = "Pamela", CompanyName = "XYZ Global" };
    customers[3] = new Customer() { CustomerID = "C004", ContactName = "Steve", CompanyName = "DEF Tech" };

    // Returning the array of customers
    return customers;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to generate list of keys for provided customer array
/// </summary>
/// <param name="customer">Array of customers</param>
/// <returns>Array of keys</returns>
string[] GetKeys(Customer[] customers)
{
    // Creating array for keys
    string[] keys = new string[customers.Length];

    // Populating keys array
    for (int i = 0; i < customers.Length; i++)
    {
        keys[i] = string.Format("Customer:{0}", customers[i].CustomerID);
    }

    // Returning the keys array
    return keys;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to generate hashmap of cache items and keys 
/// </summary>
/// <param name="keys">List of keys to generate hashmap</param>
/// <param name="customers">List of customers to generate hashmap</param>
/// <returns>Dictionary of Cache Items and Key</returns>
IDictionary<string, CacheItem> GetCacheItemDictionary(string[] keys, Customer[] customers)
{
    // Creating dictionary of Cache Items and keys
    IDictionary<string, CacheItem> cacheItems = new Dictionary<string, CacheItem>();

    // Populating dictionary of Cache Items and keys
    for (int i = 0; i < customers.Length; i++)
    {
        CacheItem cacheItem = new CacheItem(customers[i]);
        cacheItems.Add(keys[i], cacheItem);
    }

    // Returning the dictionary of Cache Items and keys
    return cacheItems;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to add multiple objects in the cache using bulk API
/// </summary>
/// <param name="items">Dictionary of keys and items to add</param>
void AddMultipleObjectsToCache(IDictionary<string, CacheItem> items)
{
    // Adding items using bulk API
    IDictionary<string, Exception> result = cache.AddBulk(items);

    // Checking for any failures
    if (result.Count == 0)
    {
        Console.WriteLine("All items are successfully added to cache.");
    }
    else
    {
        Console.WriteLine("Some items failed to add:");
        foreach (var entry in result)
        {
            Console.WriteLine($"Key: {entry.Key}, Error: {entry.Value.Message}");
        }
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to update multiple objects in the cache using bulk API
/// </summary>
/// <param name="keys">List of keys</param>
/// <param name="items">Dictionary of keys and items to update</param>
void UpdateMultipleObjectsInCache(string[] keys, IDictionary<string, CacheItem> items)
{
    // Modifying the items before updating
    items[keys[0]].GetValue<Customer>().CompanyName = "ABC Corp";
    items[keys[1]].GetValue<Customer>().CompanyName = "QRS Enterprises";

    // Updating multiple items in cache using bulk API
    IDictionary<string, Exception> result = cache.InsertBulk(items);

    // Checking for any failures
    if (result.Count == 0)
    {
        Console.WriteLine("All items are successfully updated in cache.");
    }
    else
    {
        Console.WriteLine($"{result.Count} item(s) failed to update:");
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to read multiple objects from the cache using bulk API
/// </summary>
/// <param name="keys">List of string keys to fetch data from cache</param>
void GetMultipleObjectsFromCache(string[] keys)
{
    // Fetching multiple existing Items from cache
    IDictionary<string, Customer> retrievedItems = cache.GetBulk<Customer>(keys);
    Console.WriteLine("Fetching a bulk of items from cache.");

    // Displaying the fetched items on console
    foreach (KeyValuePair<string, Customer> item in retrievedItems)
    {
        Console.WriteLine($"Key: {item.Key}, ID: {item.Value.CustomerID}, Company Name: {item.Value.CompanyName}");
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to remove multiple objects from the cache using bulk API
/// </summary>
/// <param name="keys">List of string keys to be removed from cache</param>
void RemoveMultipleObjectsFromCache(string[] keys)
{
    // Removing multiple items from cache
    cache?.RemoveBulk(keys);
    Console.WriteLine("Items deleted from cache.");
}