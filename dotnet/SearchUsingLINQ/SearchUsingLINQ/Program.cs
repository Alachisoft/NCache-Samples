// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Linq;
using Alachisoft.NCache.Sample.Data;

// ===============================================================================
// NCache LINQ provides seamless integration of Language Integrated Query (LINQ) 
// with distributed cached data. This allows cached objects to be filtered via
// LINQ query just like data from in-memory collections or relational databases,
// without bringing all data to the client. Some use cases of LINQ include:
//  1. Filtering products based on attributes like price, category, or discount.
//  2. Retrieving only required fields instead of entire objects from cache.
//  3. Performing tag-based or named-tag-based searches for analytics or reports.
//  4. Implementing dynamic search features using strongly-typed queries.
//  5. Enhancing retrieval performance by leveraging indexed LINQ queries.
// NCache LINQ supports a wide range of LINQ query operators, including filtering,
// projection, sorting, grouping, and aggregation. It translates LINQ queries into
// efficient cache operations, minimizing data transfer and leveraging the
// distributed nature of NCache.
// ===============================================================================

// Connect to a running cache and get cache handle for it
ICache? cache = GetCache();

// Generating and inserting sample products into cache for demonstration
AddSampleData();

// Running queries
RunQueries();

// Dispose the cache once all operations are complete
cache?.Dispose();

/// ------------------------------------------------------------------------------
/// <summary>
/// Connects to a running cache and returns its handle
/// </summary>
ICache? GetCache()
{
    string? cacheName = ConfigurationManager.AppSettings["CacheName"];

    if (string.IsNullOrEmpty(cacheName))
    {
        Console.WriteLine("The CacheName cannot be null or empty.");
        return null;
    }

    // Connecting to a running cache
    ICache cache = CacheManager.GetCache(cacheName);

    // Printing output on console
    Console.WriteLine(string.Format("\nCache '{0}' is connected.", cache));
    return cache;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to generate sample products and add them to the cache
/// </summary>
void AddSampleData()
{
    try
    {
        Console.WriteLine("\nLoading products into cache...");

        // These products match the ones usually used in SearchUsingSQL
        Product[] products = new Product[]
        {
            new Product { ProductID = 1001, ProductName = "Chai", Category = "1", UnitPrice = 18, UnitsAvailable = 39 },
            new Product { ProductID = 1002, ProductName = "Chang", Category = "1", UnitPrice = 19, UnitsAvailable = 17 },
            new Product { ProductID = 1003, ProductName = "Aniseed Syrup", Category = "2", UnitPrice = 10, UnitsAvailable = 13 },
            new Product { ProductID = 1004, ProductName = "Chef Anton's Cajun", Category = "2", UnitPrice = 2, UnitsAvailable = 2 },
            new Product { ProductID = 1005, ProductName = "Chef Anton's Gumbo", Category = "2", UnitPrice = 21, UnitsAvailable = 0 },
            new Product { ProductID = 1006, ProductName = "Grandma's Boysenberry", Category = "3", UnitPrice = 25, UnitsAvailable = 120 },
            new Product { ProductID = 1007, ProductName = "Uncle Bob's Organic Dried Pears", Category = "7", UnitPrice = 30, UnitsAvailable = 15 },
            new Product { ProductID = 1008, ProductName = "Northwoods Cranberry Sauce", Category = "2", UnitPrice = 4, UnitsAvailable = 5 },
            new Product { ProductID = 1009, ProductName = "Mishi Kobe Niku", Category = "4", UnitPrice = 97, UnitsAvailable = 29 },
            new Product { ProductID = 1010, ProductName = "Ikura", Category = "4", UnitPrice = 31, UnitsAvailable = 4 }
        };

        foreach (var p in products)
        {
            string key = "Product:" + p.ProductID;
            cache?.Insert(key, p);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading cache: " + ex.Message);
    }
}


/// -------------------------------------------------------------------------------
/// <summary>
/// Method to execute LINQ 
/// </summary>
void RunQueries()
{
    Console.WriteLine("\nRunning various LINQ queries on cached data...");

    IQueryable<Product> products = new NCacheQuery<Product>(cache);

    // Fetching all products where UnitsAvailable < 5
    var lowStockProducts =
        from product in products
        where product.UnitsAvailable < 5
        select product;

    PrintQueryResults("Products with UnitsAvailable < 5", lowStockProducts);

    // Fetching all products where Category = '4'
    var cat4Products =
        from product in products
        where product.Category == "4"
        select product;

    PrintQueryResults("Products in Category 4", cat4Products);

    // Fetching all products where UnitsAvailable < 10 AND UnitPrice < 10
    var lowPriceProducts =
        from product in products
        where product.UnitsAvailable < 10 && product.UnitPrice < 10
        select product;

    PrintQueryResults("Products with UnitsAvailable < 10 and UnitPrice < 10", lowPriceProducts);
}


/// -------------------------------------------------------------------------------
/// <summary>
/// Method to print query results
/// </summary>
void PrintQueryResults(string description, IQueryable<Product> result)
{
    Console.WriteLine();
    Console.WriteLine("-----------------------------------------------");
    Console.WriteLine(description);
    Console.WriteLine("-----------------------------------------------");

    if (result != null && result.Any())
    {
        foreach (Product p in result)
            Console.WriteLine("ProductID: " + p.ProductID);
    }
    else
    {
        Console.WriteLine("No records found.");
    }

    Console.WriteLine();
}