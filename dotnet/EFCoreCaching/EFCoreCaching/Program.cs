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
// NOTE: Before running this sample, ensure that the Northwind entities are
// indexed in your configured NCache instance.
//   See NCache Manager -> Advanced Settings -> Query Indexes.
// ===============================================================================

// Creating DbContext instance
var db = new NorthwindContext();

// QUERY CACHING examples using EF Core with NCache
// Retrieving data from database as well as cache
await GetCustomerAsync(db);
await GetCustomerOrdersAsync(db);

// REFERENCE DATA CACHING examples using EF Core with NCache
// Load entire data sets into cache first
LoadReferenceDataFromDB(db);

// Now run EF Core Queries on this cached data sets
await SearchSuppliersInCacheAsync(db);
await SearchDiscontinuedProductsInCacheAsync(db);

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

/// -------------------------------------------------------------------------------
/// <summary>
/// Method to fetch data regarding suppliers and products from database
/// </summary>
/// <param name="db">Instance of Northwind db context</param>
void LoadReferenceDataFromDB(NorthwindContext db)
{
    var options = DefaultOptions(StoreAs.SeparateEntities);

    //// Loading all suppliers into cache
    var suppliers = (from c in db.Suppliers select c).LoadIntoCache(options).ToList();

    //// Loading all products into cache
    var products = (from c in db.Products select c).LoadIntoCache(options).ToList();

    // Confirming data load
    if (suppliers.Any() && products.Any())
    {
        Console.WriteLine("\nSuppliers and products loaded into cache successfully.");
    }
    else
    {
        Console.WriteLine("\nFailed to load suppliers and products into cache.");
    }
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

/// -------------------------------------------------------------------------------
/// <summary>
/// Method to retrieve suppliers with caching and expiration settings asynchronously
/// </summary>
/// <param name="db">Instance of Northwind db context</param>
async Task SearchSuppliersInCacheAsync(NorthwindContext db)
{
    var options = DefaultOptions();
    options.SetSlidingExpiration(TimeSpan.FromMinutes(30));

    // Fetching suppliers only from cache asynchronously (no DB hit)
    // Note: We can also use FromCacheOnly here for synchronous operation
    var result = await (from s in db.Suppliers
                        where s.SupplierId <= 10
                        select s)
                .FromCacheOnlyAsync();

    Console.WriteLine("\nAsync: Fetching suppliers from cache only where SupplierId<=10:");
    foreach (var s in result)
        Console.WriteLine($"{s.SupplierId,-5} {s.CompanyName,-40} {s.Phone}");
}

/// -------------------------------------------------------------------------------
/// <summary>
/// Method to retrieve discontinued products using separate entity storage asynchronously
/// </summary>
/// <param name="db">Instance of Northwind db context</param>
async Task SearchDiscontinuedProductsInCacheAsync(NorthwindContext db)
{
    var options = DefaultOptions(StoreAs.SeparateEntities);

    // Fetching discontinued products only from cache asynchronously (no DB hit)
    // Note: We can also use FromCacheOnly here for synchronous operation
    var discontinued = await (from p in db.Products
                              where p.Discontinued == true
                              select p)
                     .FromCacheOnlyAsync();

    Console.WriteLine("\nAsync: Fetching discontinued products from cache only:");
    foreach (var p in discontinued)
        Console.WriteLine($"{p.ProductId,-5} {p.ProductName,-40} {p.QuantityPerUnit}");
}