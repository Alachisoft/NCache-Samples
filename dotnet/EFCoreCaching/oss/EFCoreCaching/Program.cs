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
using Alachisoft.NCache.EntityFrameworkCore;
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Caching;
using EFCoreCaching.Models;
using EFCoreCaching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ===============================================================================
// Entity Framework Core is a widely used ORM for .NET applications. NCache
// provides seamless integration with Entity Framework Core, enabling developers
// to leverage distributed caching capabilities within their EF Core applications.
// This integration automatically improves performance without major code changes.
// It supports LINQ and asynchronous operations, offering configurable options
// like expiration and cache invalidation to ensure data consistency. Some use
// cases for caching with Entity Framework Core using NCache include:
//   1. Caching user configuration settings to reduce database load and improve
//      response time.
//   2. Caching product catalogs in e-commerce applications for faster access and
//      scalability.
//   3. Caching lookup or reference data to minimize repetitive database queries.
//   4. Caching results of complex or expensive queries to enhance application
//      performance.
//   5. Caching reports, dashboards, or analytics data to speed up retrieval time.
// ===============================================================================

// Creating DbContext instance
var db = new NorthwindContext();

// QUERY CACHING examples using EF Core with NCache
// Retrieving data from database as well as cache
await GetCustomerAsync(db);
await GetCustomerOrdersAsync(db);
await GetAllCustomersAsync(db);
TestCacheHitVsMiss(db);


/// -------------------------------------------------------------------------------
/// <summary>
/// Method to return default cache options for queries, with optional storage mode
/// </summary>
CachingOptions DefaultOptions(StoreAs storeAs = StoreAs.Collection)
    => new CachingOptions
    {
        QueryIdentifier = null,
        CreateDbDependency = false,
        StoreAs = storeAs,
        Priority = CacheItemPriority.Default
    };

/// <summary>
/// Tests cache hit vs miss by running same query twice
/// and measuring time difference
/// </summary>
void TestCacheHitVsMiss(NorthwindContext db)
{
    var options = DefaultOptions();
    var sw = System.Diagnostics.Stopwatch.StartNew();

    // First call - cache miss, hits DB
    var result1 = (from o in db.Customers.AsNoTracking()
                   where o.Country == "France"
                   select o)
                 .FromCache(options)
                 .ToList();

    sw.Stop();
    Console.WriteLine($"\nSync: France orders - First call (DB): {result1.Count} orders in {sw.ElapsedMilliseconds}ms");

    sw.Restart();

    // Second call - should be cache hit
    var result2 = (from o in db.Customers.AsNoTracking()
                   where o.Country == "France"
                   select o)
                 .FromCache(options)
                 .ToList();

    sw.Stop();
    Console.WriteLine($"Sync: France orders - Second call (cache): {result2.Count} orders in {sw.ElapsedMilliseconds}ms");
}

/// <summary>
/// Fetch all customers - tests basic collection caching
/// </summary>
async Task GetAllCustomersAsync(NorthwindContext db)
{
    var options = DefaultOptions();

    var result = await (from c in db.Customers.AsNoTracking()
                        select c)
               .FromCacheAsync(options);

    Console.WriteLine("\nAsync: All customers from cache:");
    foreach (var c in result)
        Console.WriteLine($"{c.CustomerId,-10} {c.CompanyName,-40} {c.Country}");
}



/// -------------------------------------------------------------------------------
/// <summary>
/// Method to retrieve a Customer from NCache using FromCacheAsync
/// </summary>
/// <param name="db">Instance of Northwind db context</param>
async Task GetCustomerAsync(NorthwindContext db)
{
    var options = DefaultOptions();

    // Fetching specific customer (ALFKI) from cache asynchronously
    // This fetches data from cache instead of db as it was previously loaded in GetCustomer
    // Note: We can also use FromCache here for synchronous operation
    var result = await (from c in db.Customers.AsNoTracking()
                        where c.CustomerId == "ALFKI"
                        select c)
               .FromCacheAsync(options);

    Console.WriteLine("\nAsync: Customer fetched from db or cache:");
    foreach (var c in result)
        Console.WriteLine($"{c.CustomerId} - {c.CompanyName}");
}

/// -------------------------------------------------------------------------------
/// <summary>
/// Method to retrieve orders for a specific customer using cached queries asynchronously
/// </summary>
/// <param name="db">Instance of Northwind db context</param>
async Task GetCustomerOrdersAsync(NorthwindContext db)
{
    var options = DefaultOptions();

    // Fetching orders for customer (ALFKI) from cache asynchronously
    // This fetches data from cache instead of db as it was previously loaded in GetCustomerOrders
    // Note: We can also use FromCache here for synchronous operation
    var result = await (from o in db.Orders.AsNoTracking()
                        where o.CustomerId == "ALFKI"
                        select o)
                .FromCacheAsync(options);

    Console.WriteLine("\nAsync: Orders fetched from db for ALFKI:");
    foreach (var o in result)
        Console.WriteLine($"OrderID: {o.OrderId}, Date: {o.OrderDate}, ShipName: {o.ShipName}");
}
