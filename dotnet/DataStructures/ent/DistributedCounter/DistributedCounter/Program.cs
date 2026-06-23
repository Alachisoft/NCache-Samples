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
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Runtime.Caching;

// ===============================================================================
// Distributed Counter is a powerful data structure provided by NCache for
// high-concurrency scenarios where multiple clients or application servers need
// to increment, decrement, or read a shared numeric value atomically and with low
// latency. Some possible use cases of distributed counters include:
//   1. Counting page views or clicks on a website
//   2. Tracking inventory levels in an e-commerce application    
//   3. Implementing rate limiting for APIs or services
//   4. Managing distributed locks or semaphores
//   5. Real-time analytics and monitoring
// Distributed counters are designed to be highly available and fault-tolerant.
// ===============================================================================

// Connect to a running cache and get cache handle for it 
ICache cache = GetCache();

// Validating cache handle
if (cache == null)
{
    Console.Write("Cache connection could not be established. Exiting the sample.");
    return;
}

// Name of the distributed counter to be used in sample
string counterName = "DistributedCounter:11001";

// Creating or getting counter
ICounter counter = GetOrCreateCounter(counterName);

// Setting counter value
long counterValue = counter.SetValue(10);
Console.WriteLine("Counter value set to {0}.", counterValue);

// Incrementing counter value
counterValue = counter.Increment();
Console.WriteLine("Counter value incremented by 1.");
Console.WriteLine("New Counter value is {0}.", counterValue);

// Decrementing counter value
counterValue = counter.Decrement();
Console.WriteLine("Counter value decremented by 1.");
Console.WriteLine("New Counter value is {0}.", counterValue);

// Incrementing counter value by 5
counterValue = counter.IncrementBy(5);
Console.WriteLine("Counter value incremented by {0}.", 5);
Console.WriteLine("New Counter value is {0}.", counterValue);

// Decrementing counter by value
counterValue = counter.DecrementBy(2);
Console.WriteLine("Counter value decremented by {0}.", 2);
Console.WriteLine("New Counter value is {0}.", counterValue);

// Removing the distributed counter from cache
cache.DataTypeManager.Remove(counterName);

// Dispose the cache once done
cache.Dispose();

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to connect to a running cache and return cache handle for it 
/// </summary>
/// <returns>Cache handle for the connected cache</returns>
ICache? GetCache()
{
    string? cacheName = ConfigurationManager.AppSettings["CacheName"];

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
/// Method to retrieve the distributed counter from Cache,
/// if the counter does not exist, it is created and returned
/// </summary>
/// <param name="counterName">Name of counter to be used to make counter</param>
/// <returns>Instance of ICounter</returns>
ICounter GetOrCreateCounter(string counterName)
{
    // Retrieving the counter from cache
    ICounter counter = cache.DataTypeManager.GetCounter(counterName);

    // Checking if counter exists or not
    if (counter == null)
    {
        // Creating attributes with expiration for counter
        DataTypeAttributes attributes = new DataTypeAttributes
        {
            Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(1))
        };

        // Creating distributed counter with absolute expiration
        // to modify cache properties of the counter,
        // provide an instance of DataTypeAttributes in the second parameter
        counter = cache.DataTypeManager.CreateCounter(counterName, attributes);
    }
    return counter;
}