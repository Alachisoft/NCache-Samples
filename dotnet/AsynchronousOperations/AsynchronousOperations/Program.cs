// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System.Configuration;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Sample.Data;

// ===============================================================================
// NCache allows you to perform asynchronous operations on the cache using async
// APIs. Following are the asynchronous operations demonstrated in this sample:
//   1. Connecting to a running cache
//   2. Adding an object to cache asynchronously
//   3. Updating an existing object in cache asynchronously
//   4. Deleting an object from cache asynchronously
// ===============================================================================

// Connect to a running cache and get cache handle for it 
ICache? cache = GetCache();

// Creating a simple customer object and key for this sample
Customer customer = CreateNewCustomer();
string key = GetKey(customer);

// Adding customer in cache asynchronously
await AddToCacheAsync(key, customer);

// Modifying the customer and updating in cache
await UpdateInCacheAsync(key, customer);

// Removing the existing customer asynchronously
await RemoveFromCacheAsync(key);

// Dispose the cache once done
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
    Console.WriteLine(string.Format("Cache '{0}' is connected.", cacheName));
    return cache;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to add customer to cache using async API
/// </summary>
/// <param name="key">Key of customer to be added in cache</param>
/// <param name="customer">Instance of Customer that will be added to cache</param>
/// <returns>Task representing the async operation</returns>
async Task AddToCacheAsync(string key, Customer customer)
{
    // Adding item asynchronously
    await cache.AddAsync(key, customer).ContinueWith(task => OnItemAdded(key, task));
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to update customer in cache using async API
/// </summary>
/// <param name="key">Key of customer to be updated in cache</param>
/// <param name="customer">Instance of Customer that will be updated in the cache</param>
/// <returns>Task representing the async operation</returns>
async Task UpdateInCacheAsync(string key, Customer customer)
{
    // Changing the customer detail
    customer.CompanyName = "Gourmet Lanchonettes";

    // Updating the item in cache asynchronously
    await cache.InsertAsync(key, customer).ContinueWith(task => OnItemUpdate(key, task));
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to delete customer from the cache using async API
/// </summary>
/// <param name="key">Key of customer to be removed from cache</param>
/// <returns>Task representing the async operation</returns>
async Task RemoveFromCacheAsync(string key)
{
    // Removing the existing customer asynchronously
    await cache.RemoveAsync<Customer>(key).ContinueWith(task => OnItemRemoved(key, task));
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Callback method executed each time an AddAsync operation completes
/// </summary>
/// <param name="key">Key of the item added</param>
/// <param name="task">Task representing the async operation</param>
void OnItemAdded(string key, Task<CacheItemVersion> task)
{
    // Checking if the task is completed successfully
    if (task.Status == TaskStatus.RanToCompletion)
    {
        // Printing output on console
        Console.WriteLine("\nObject is added to cache.");

        // Fetching and printing the added customer from cache
        Customer cachedCustomer = cache.Get<Customer>(key);
        PrintCustomerDetails(cachedCustomer);
    }

    // Checking if the task is faulted
    if (task.Status == TaskStatus.Faulted)
    {
        Console.WriteLine(string.Format("Failed to add key '{0}' in cache.", key));
    }

    // Checking if there is any exception occurred during the async operation
    if (task.Exception != null)
    {
        Console.WriteLine(string.Format("Failed to add key '{0}' in cache due to {1}.", key, task.Exception.ToString()));
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Callback method executed each time InsertAsync operation completes
/// </summary>
/// <param name="key">Key of the item added</param>
/// <param name="task">Task representing the async operation</param>
void OnItemUpdate(string key, Task<CacheItemVersion> task)
{
    // Checking if the task is completed successfully
    if (task.Status == TaskStatus.RanToCompletion)
    {
        // Printing output on console
        Console.WriteLine("\nObject is updated to cache.");

        // Fetching and printing the updated customer from cache
        Customer cachedCustomer = cache.Get<Customer>(key);
        PrintCustomerDetails(cachedCustomer);
    }

    // Checking if the task is faulted
    if (task.Status == TaskStatus.Faulted)
    {
        Console.WriteLine(string.Format("Failed to update key '{0}' in cache.", key));
    }

    // Checking if there is any exception occurred during the async operation
    if (task.Exception != null)
    {
        Console.WriteLine(string.Format("Failed to update key '{0}' in cache due to {1}.", key, task.Exception.ToString()));
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Callback method executed each time RemoveAsync operation completes
/// </summary>
/// <param name="key">Key of the item added</param>
/// <param name="task">Task representing the async operation</param>
void OnItemRemoved(string key, Task<Customer> task)
{
    // Checking if the task is completed successfully
    if (task.Status == TaskStatus.RanToCompletion)
    {
        // Printing output on console
        Console.WriteLine("\nObject is removed from cache.");

        // Trying to fetch and print the removed customer from cache
        Customer cachedCustomer = cache.Get<Customer>(key);
        PrintCustomerDetails(cachedCustomer);
    }

    // Checking if the task is faulted
    if (task.Status == TaskStatus.Faulted)
    {
        Console.WriteLine(string.Format("Failed to remove key '{0}' in cache.", key));
    }

    // Checking if any exception occurred during the async operation
    if (task.Exception != null)
    {
        Console.WriteLine(string.Format("Failed to remove key '{0}' in cache due to {1}.", key, task.Exception.ToString()));
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to generate an instance of Customer to be used in this sample
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
/// <param name="customer"> Instance of Customer to generate a key</param>
/// <returns>Key for the customer object</returns>
string GetKey(Customer customer)
{
    return string.Format("Customer:{0}", customer.CustomerID);
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to print details of customer object
/// </summary>
/// <param name="customer">Customer instance whose attributes are to be printed</param>
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