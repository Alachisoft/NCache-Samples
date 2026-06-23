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

using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Sample.Data;
using System;
using System.Collections.Generic;
using System.Configuration;

// ===============================================================================
// This sample demonstrates how to perform SQL-like queries on cached data in
// NCache. You can use queries to filter, search, or project specific data
// efficiently. Some possible use cases of SearchUsingSQL in NCache include:
//   1. Filtering items based on attributes like price, category, or warranty.
//   2. Retrieving only required fields instead of fetching entire objects to
//      optimize performance.
//   3. Running tag-based or named-tag-based searches for analytics, reporting,
//      or dashboards.
//   4. Implementing dynamic search features in applications (e.g., user-driven
//      queries).
//   5. Enhancing data retrieval performance using indexed queries on cached data.
// ===============================================================================

// Connect to a running cache and return a cache handle for it
ICache cache = GetCache();

// Validating cache handle
if (cache == null)
{
    Console.Write("Cache connection could not be established. Exiting the sample.");
    return;
}

// Generating and inserting sample products into cache for demonstration
AddSampleData();

// Querying using Namedtags
QueryByNamedTags(true);

// Performing queries using defined indexes
QueryByPriceIndex(100);

// Performing queries using projection
QuerySelectedFields(100);

// Dispose the cache once all operations are complete
cache.Dispose();

/// ------------------------------------------------------------------------------
/// <summary>
/// Connects to a running cache and returns its handle
/// </summary>
ICache GetCache()
{
    string? cacheName = ConfigurationManager.AppSettings["CacheName"];

    if (string.IsNullOrEmpty(cacheName))
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
/// Method to generate sample products and add them to the cache with associated tags and named tags
/// </summary>
void AddSampleData()
{
    // Creating sample products for demonstration
    var laptop = new Product { ProductID = 13001, ProductName = "Laptop", UnitPrice = 2000, Category = "Electronics" };
    var kindle = new Product { ProductID = 13002, ProductName = "Kindle", UnitPrice = 1000, Category = "Electronics" };
    var book = new Product { ProductID = 13003, ProductName = "Book", UnitPrice = 100, Category = "Stationery" };
    var smartPhone = new Product { ProductID = 13004, ProductName = "Smart Phone", UnitPrice = 1000, Category = "Electronics" };
    var mobilePhone = new Product { ProductID = 13005, ProductName = "Mobile Phone", UnitPrice = 200, Category = "Electronics" };

    // Creating CacheItems with Tags and Named Tags from sample products
    var laptopCacheItem = new CacheItem(laptop) { Tags = new[] { new Tag("Technology"), new Tag("Gadgets") }, NamedTags = new NamedTagsDictionary() };
    var kindleCacheItem = new CacheItem(kindle) { Tags = new[] { new Tag("Technology"), new Tag("Reading") }, NamedTags = new NamedTagsDictionary() };
    var bookCacheItem = new CacheItem(book) { Tags = new[] { new Tag("Education"), new Tag("Reading") }, NamedTags = new NamedTagsDictionary() };
    var smartPhoneCacheItem = new CacheItem(smartPhone) { Tags = new[] { new Tag("Technology"), new Tag("Communication") }, NamedTags = new NamedTagsDictionary() };
    var mobilePhoneCacheItem = new CacheItem(mobilePhone) { Tags = new[] { new Tag("Technology"), new Tag("Communication") }, NamedTags = new NamedTagsDictionary() };

    // Adding NamedTags to CacheItems
    laptopCacheItem.NamedTags.Add("Discount", 20);
    laptopCacheItem.NamedTags.Add("Warranty", true);
    bookCacheItem.NamedTags.Add("Discount", 10);
    bookCacheItem.NamedTags.Add("Warranty", false);
    kindleCacheItem.NamedTags.Add("Discount", 15);
    kindleCacheItem.NamedTags.Add("Warranty", false);
    smartPhoneCacheItem.NamedTags.Add("Discount", 25);
    smartPhoneCacheItem.NamedTags.Add("Warranty", true);
    mobilePhoneCacheItem.NamedTags.Add("Discount", 5);
    mobilePhoneCacheItem.NamedTags.Add("Warranty", true);

    // Creating a dictionary of products to be added to cache
    var products = new Dictionary<string, CacheItem>
      {
          { $"Product:{laptop.ProductID}", laptopCacheItem },
          { $"Product:{kindle.ProductID}", kindleCacheItem },
          { $"Product:{book.ProductID}", bookCacheItem },
          { $"Product:{smartPhone.ProductID}", smartPhoneCacheItem },
          { $"Product:{mobilePhone.ProductID}", mobilePhoneCacheItem }
      };

    // Adding all the products to the cache
    Console.WriteLine("Adding products to the cache with tags...");
    cache?.InsertBulk(products);

    // Fetching and printing added products on console
    var productValues = products.Values.Select(cacheItem => cacheItem.GetValue<Product>());
    PrintProducts(productValues);
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to print products to the console
/// </summary>
/// <param name="products">Products whose details are to be printed</param>
void PrintProducts(IEnumerable<Product> products)
{
    Console.WriteLine("ProductID, Name, Price, Category");
    foreach (var product in products)
    {
        Console.WriteLine($"- {product.ProductID}, {product.ProductName}, ${product.UnitPrice}, {product.Category}");
    }
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to demonstrate how to query items from NCache using Named Tags and print them
/// </summary>
/// <param name="warranty">Warranty value to filter products</param>
void QueryByNamedTags(bool warranty)
{
    // Defining SQL-like query on the 'Supplier' named tag
    string query = "SELECT $Value$ FROM Alachisoft.NCache.Sample.Data.Product WHERE Warranty = ?";

    // Creating query command and specifying parameter value
    QueryCommand queryCommand = new QueryCommand(query);
    queryCommand.Parameters.Add("Warranty", warranty);

    // Executing query and get results through ICacheReader
    using ICacheReader reader = cache!.SearchService.ExecuteReader(queryCommand);

    int count = 0;

    // Reading through the result set
    while (reader.Read())
    {
        Product product = reader.GetValue<Product>(1);
        PrintProductDetails(product);
        count++;
    }

    // Printing query results
    Console.WriteLine($"{count} cache items fetched using query on Named Tags.\n");
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to demonstrate how to query cached items based on indexed attributes and print them
/// </summary>
/// <param name="unitPrice">Unit price value to filter products</param>
void QueryByPriceIndex(decimal unitPrice)
{
    // Query can be applied on primitive attributes or on indexed fields
    string query = "SELECT $Value$ FROM Alachisoft.NCache.Sample.Data.Product WHERE UnitPrice > ?";

    // Creating query command and setting parameter
    QueryCommand queryCommand = new QueryCommand(query);
    queryCommand.Parameters.Add("UnitPrice", unitPrice);

    // Executing query
    using ICacheReader reader = cache!.SearchService.ExecuteReader(queryCommand);

    int count = 0;

    // Iterating over the results and displaying each product
    while (reader.Read())
    {
        Product product = reader.GetValue<Product>(1);
        PrintProductDetails(product);
        count++;
    }

    // Printing query summary
    Console.WriteLine($"{count} cache items fetched using query on indexed attribute (UnitPrice).\n");
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to demonstrate querying using Projection, fetching only selected attributes and printing the items
/// </summary>
/// <param name="unitPrice">Unit price value to filter products</param>
void QuerySelectedFields(decimal unitPrice)
{
    // Projection query returns only specified fields instead of entire object
    string query = "SELECT ProductName, Category FROM Alachisoft.NCache.Sample.Data.Product WHERE UnitPrice > ?";

    // Creating query command and specifying parameter
    QueryCommand queryCommand = new QueryCommand(query);
    queryCommand.Parameters.Add("UnitPrice", unitPrice);

    // Executing query and retrieving results
    using ICacheReader reader = cache!.SearchService.ExecuteReader(queryCommand);

    int count = 0;

    // Reading projected fields from the result set
    while (reader.Read())
    {
        Console.WriteLine("Name:     " + reader.GetValue<string>("ProductName"));
        Console.WriteLine("Category: " + reader.GetValue<string>("Category"));
        Console.WriteLine();
        count++;
    }

    // Printing output on console
    Console.WriteLine($"{count} cache items fetched using projection query.\n");
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to print product details to the console
/// </summary>
/// <param name="product">Product object whose details are to be printed</param>
void PrintProductDetails(Product product)
{
    Console.WriteLine("Id:        " + product.ProductID);
    Console.WriteLine("Name:      " + product.ProductName);
    Console.WriteLine("Class:     " + product.ClassName);
    Console.WriteLine("Category:  " + product.Category);
    Console.WriteLine("UnitPrice: " + product.UnitPrice);
    Console.WriteLine();
}
