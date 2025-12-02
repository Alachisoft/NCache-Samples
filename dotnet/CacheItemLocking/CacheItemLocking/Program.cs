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
using System;
using System.Configuration;

// ===============================================================================
// Cache Item Locking is a mechanism that prevents multiple clients from modifying
// the same cache item simultaneously. When an item is locked by a client, other
// clients are prevented from modifying that item until the lock is released. This
// ensures data consistency and integrity in a distributed caching environment.
// Some possible use cases of Cache Item Locking include:
//   1. Preventing race conditions when multiple clients attempt concurrent
//      updates on the same data.
//   2. Ensuring data integrity during critical operations.
//   3. Coordinating access to shared resources in a distributed system.
//   4. Implementing optimistic concurrency control to avoid lost updates.
//   5. Managing transactions in a distributed cache environment.
// ===============================================================================

// Connect to a running cache and get cache handle for it
ICache? cache = GetCache();

// Creating a sample customer and key for demonstration
Customer customer = CreateNewCustomer();
string key = $"Customer:{customer.CustomerID}";

// Creating new lock handle and one second timespan for locking cache item
LockHandle lockHandle = new LockHandle();
TimeSpan timeSpan = TimeSpan.FromSeconds(1);

// Adding customer item in cache
AddInCache(key, customer);

// Fetching customer from cache and locking the item (GET Operation)
customer = cache.Get<Customer>(key, acquireLock: true, timeSpan, ref lockHandle);
Console.WriteLine($"Customer with key: {key} fetched from cache and locked via GET operation, lock id: {lockHandle.LockId}");

// Changing customer contact number
customer.ContactNo = "11983-4678";
Console.WriteLine("Customer contact number changed");

// Updating customer and unlocking it as well (INSERT Operation)
cache.Insert(key, new CacheItem(customer), lockHandle, releaseLock: true);
Console.WriteLine($"Customer with key: {key} updated in cache and unlocked via INSERT operation.");

// Explicitly acquiring a lock on item in cache (LOCK Operation)
bool isLocked = cache.Lock(key, timeSpan, out lockHandle);
Console.WriteLine($"Attempting to acquire lock on customer with key: {key} via explicit Lock operation.");

// Checking if the acquired lock was successfully
if (isLocked)
{
    Console.WriteLine($"Lock acquired via explicit Lock operation, updated lock id: {lockHandle.LockId}");
}
else
{
    Console.WriteLine("Unable to acquire lock as item is already locked.");
}

// Releasing the lock on the item using the lock handle
cache?.Unlock(key, lockHandle);
Console.WriteLine($"Customer with key: {key} unlocked via explicit Unlock operation.");

// Removing the item from cache for cleanup
cache?.Remove(key);

// Dispose cache once done
cache?.Dispose();

/// ------------------------------------------------------------------------------
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
    Console.WriteLine(string.Format("Cache '{0}' is connected.", cache));
    return cache;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to generate a new customer working as sample data
/// </summary>
/// <returns>Instance of Customer</returns>
Customer CreateNewCustomer()
{
    return new Customer
    {
        CustomerID = "DAVJO",
        ContactName = "David Jones",
        CompanyName = "Lonesome Pine Restaurant",
        ContactNo = "78495-1534",
        Address = "Silicon Valley, Santa Clara, California",
    };
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to add customer in the cache
/// </summary>
/// <param name="key">Key of the item to be added</param>
/// <param name="customer">Customer object to add in cache</param>
void AddInCache(string key, Customer customer)
{
    try
    {
        // Adding customer in cache
        cache?.Add(key, customer);
        Console.WriteLine($"\nCustomer with Key: {key} added in cache.");
    }
    catch (Exception ex)
    {
        // Triggers if item with same key is already present in cache
        Console.WriteLine("\nError occurred during Add operation: " + ex.Message);
    }
}