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
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Sample.Data;
using System.Configuration;

// Connect to a running cache and get cache handle for it 
ICache? cache = GetCache();

// ===============================================================================
// Tags allow you to logically group different cached items together based on some
// something common among them. Tags provide a many to many relationship between
// cached items and tags. You can assign the same tag to multiple cached items and
// you can assign multiple tags to a single cached item. Some use cases include:
//   1. Grouping related items (e.g., products by category, region, or supplier)
//      for easier retrieval.
//   2. Fast filtering and searching of cached items based on tag associations.
//   3. Performing bulk operations such as invalidation or removal of all items
//      under a specific tag.
//   4. Associating cached items with multiple dimensions, such as "Brand:Apple"
//      and "Type:Electronics" to support multidimensional querying.
//   5. Implementing cache dependency or invalidation strategies — e.g. removing
//      all items tagged with a supplier when that entity changes in the database.
// Tags are a powerful feature that can help you manage and organize your cached
// data more effectively.
// ===============================================================================

// Creating sample products for tags demonstration
var laptop = new Product { ProductID = 12001, ProductName = "Laptop", Category = "Electronics" };
var book = new Product { ProductID = 12003, ProductName = "Book", Category = "Stationery" };
var kindle = new Product { ProductID = 12002, ProductName = "Kindle", Category = "Electronics" };
var smartPhone = new Product { ProductID = 12004, ProductName = "Smart Phone", Category = "Electronics" };
var mobilePhone = new Product { ProductID = 12005, ProductName = "Mobile Phone", Category = "Electronics" };

// Adding tags to sample products
var laptopCacheItem = new CacheItem(laptop) { Tags = new[] { new Tag("Technology"), new Tag("Gadgets") }, NamedTags = new NamedTagsDictionary() };
var bookCacheItem = new CacheItem(book) { Tags = new[] { new Tag("Education"), new Tag("Reading") }, NamedTags = new NamedTagsDictionary() };
var kindleCacheItem = new CacheItem(kindle) { Tags = new[] { new Tag("Technology"), new Tag("Reading") }, NamedTags = new NamedTagsDictionary() };
var smartPhoneCacheItem = new CacheItem(smartPhone) { Tags = new[] { new Tag("Technology"), new Tag("Communication") }, NamedTags = new NamedTagsDictionary() };
var mobilePhoneCacheItem = new CacheItem(mobilePhone) { Tags = new[] { new Tag("Technology"), new Tag("Communication") }, NamedTags = new NamedTagsDictionary() };

// Creating the products dictionary
var products = new Dictionary<string, CacheItem>
 {
     { $"Product:{laptop.ProductID}", laptopCacheItem },
     { $"Product:{book.ProductID}", bookCacheItem },
     { $"Product:{kindle.ProductID}", kindleCacheItem },
     { $"Product:{smartPhone.ProductID}", smartPhoneCacheItem },
     { $"Product:{mobilePhone.ProductID}", mobilePhoneCacheItem },
 };

// Adding all the products to the cache in bulk. 
cache?.InsertBulk(products);
Console.WriteLine("Products added in the cache with Tags successfully.\n");

// Fetching products by "Technology" tag and printing the results
IDictionary<string, Product> technologyItems = cache.SearchService.GetByTag<Product>(new Tag("Technology"));
Console.WriteLine("Fetch Items from the cache by Tag \"Technology\"");
PrintProductDetails(technologyItems);

// Fetching keys by "Gadgets" tag and printing the results
ICollection<string> keysByTag = cache.SearchService.GetKeysByTag(new Tag("Gadgets"));
Console.WriteLine("Fetch Keys from the cache by Tag \"Gadgets\"");
PrintKeys(keysByTag);

// Creating an array of tags for multi-tag search
Tag[] tags = new[] { new Tag("Technology"), new Tag("Reading") };

// Fetching items from the cache that match any of the specified tags (e.g. Technology or Reading)
IDictionary<string, Product> productsByAnyTag = cache.SearchService.GetByTags<Product>(tags, TagSearchOptions.ByAnyTag);
Console.WriteLine("Fetch Items from the cache by \"Technology\" or \"Reading\"\n");
PrintProductDetails(productsByAnyTag);

// Removing items from the cache that match all of the specified tags (e.g. Technology and Reading)
cache.SearchService.RemoveByTags(tags, TagSearchOptions.ByAllTags);
Console.WriteLine("Removed Items by Tags \"Technology\" and \"Reading\"\n");

// ===============================================================================
// Named Tags allow you to define custom attributes for the data that you are
// caching. This could either be where your data is unstructured, meaning not
// defined in a class, or where you want to add additional attributes to your
// class without modifying it. Some use cases include:
//   1. Storing metadata about cached items, such as creation date, last accessed
//      date, or expiration date.
//   2. Enabling advanced search and filtering capabilities based on Named Tags.
//   3. Implementing custom business logic based on Named Tags values.
//   4. Categorizing cached items for easier grouping and bulk operations, such as
//      invalidating all items of a certain type.
//   5. Tagging items with user or session identifiers to support personalized
//      caching and quick retrieval per user/session.
// Named Tags provide a flexible way to enhance the functionality of your cache
// data store.
// ===============================================================================

// Creating sample image items for Named Tags demonstration
var imgSunset = new CacheItem("001_sunset.jpg");
var imgBeach = new CacheItem("002_beach.jpg");
var imgMountain = new CacheItem("003_mountain.jpg");
var imgNight = new CacheItem("004_night.jpg");
var imgForest = new CacheItem("005_forest.jpg");

// Adding NamedTags to sample image items
imgSunset.NamedTags = new NamedTagsDictionary();
imgSunset.NamedTags.Add("Resolution", "1080p");
imgSunset.NamedTags.Add("SizeMB", 2.3);
imgBeach.NamedTags = new NamedTagsDictionary();
imgBeach.NamedTags.Add("Resolution", "4K");
imgBeach.NamedTags.Add("SizeMB", 5.1);
imgMountain.NamedTags = new NamedTagsDictionary();
imgMountain.NamedTags.Add("Resolution", "720p");
imgMountain.NamedTags.Add("SizeMB", 1.8);
imgNight.NamedTags = new NamedTagsDictionary();
imgNight.NamedTags.Add("Resolution", "4K");
imgNight.NamedTags.Add("SizeMB", 4.7);
imgForest.NamedTags = new NamedTagsDictionary();
imgForest.NamedTags.Add("Resolution", "1080p");
imgForest.NamedTags.Add("SizeMB", 3.0);
imgForest.NamedTags.Add("ColorProfile", "sRGB");

// Creating the image items dictionary
var images = new Dictionary<string, CacheItem>
{
    { "Image:001", imgSunset },
    { "Image:002", imgBeach },
    { "Image:003", imgMountain },
    { "Image:004", imgNight },
    { "Image:005", imgForest }
};

// Adding images in cache with NamedTags. 
cache.AddBulk(images);
Console.WriteLine("Images added in cache with NamedTags");

// Fetching and printing images having at least 5MB size
PrintImagesWithMinSize(5.0);

// Removing NamedTag "ColorProfile" from imgForest cache item and updating the item in cache
imgForest.NamedTags.Remove("ColorProfile");
cache.Insert($"Image:005", imgForest);

// Deleting cached images with NamedTag "Resolution" set as "720p"
DeleteImagesWithLowRes();

// Removing all cache items used in this sample demonstration
cache.RemoveBulk(products.Keys);
cache.RemoveBulk(images.Keys);

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
    Console.WriteLine(string.Format("Cache '{0}' is connected.", cacheName));
    return cache;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to print product dictionary to console
/// </summary>
/// <param name="items">Dictionary of cache keys and their corresponding Product objects</param>
void PrintProductDetails(IEnumerable<KeyValuePair<string, Product>> items)
{
    // Printing product details
    if (items != null && items.Any())
    {
        // Iterating through each product and printing its details
        Console.WriteLine("ProductID, Name, Category");
        foreach (var cacheItem in items)
        {
            var product = cacheItem.Value;
            Console.WriteLine($"- {product.ProductID}, {product.ProductName}, {product.Category}");
        }
    }
    else
    {
        // Printing message if no products found
        Console.WriteLine("No products found...");
    }
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to print list of keys on console
/// </summary>
/// <param name="keys">Collection of keys</param>
void PrintKeys(ICollection<string> keys)
{
    // Printing keys on console
    if (keys != null && keys.Any())
    {
        // Iterating through each key and printing it
        Console.WriteLine("Keys:");
        foreach (var key in keys)
        {
            Console.WriteLine($"- {key}");
        }
    }
    else
    {
        // Printing message if no keys found
        Console.WriteLine("No keys found...");
    }
    Console.WriteLine();
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to print cache images having NamedTag "SizeMB" with minimum provided value
/// </summary>
/// <param name="SizeMB">The minimum size to filter images</param>
void PrintImagesWithMinSize(double SizeMB)
{
    // Creating query to get cache items with NamedTag "SizeMB"
    string query = "SELECT $VALUE$ FROM $Text$ WHERE SizeMB >= ?";
    QueryCommand sizeQuery = new QueryCommand(query);

    // Setting "SizeMB" parameter in query
    sizeQuery.Parameters.Add("SizeMB", SizeMB);

    // Executing the query to get images with minimum size and getting the result set
    Console.WriteLine($"Fetching the images having minimum {SizeMB}MB size...");
    ICacheReader result = cache.SearchService.ExecuteReader(sizeQuery, true);

    // Proceeding only if the query returned an open result set
    if (!result.IsClosed)
    {
        // Iterating through each record in the result set
        while (result.Read())
        {
            Console.WriteLine($"- Image: {result.GetValue<string>("$VALUE$")}");
        }
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to remove images from cache having NamedTag "Resolution" value set to "720p"
/// </summary>
void DeleteImagesWithLowRes()
{
    // Creating query to delete cached images with NamedTag "Resolution" set as "720p"
    string query = "DELETE FROM $Text$ WHERE Resolution = ?";
    var deleteQuery = new QueryCommand(query);

    // Setting "Resolution" parameter in query
    deleteQuery.Parameters.Add("Resolution", "720p");

    // Executing the delete query and getting number of rows removed
    Console.WriteLine($"Removing the images having Resolution set as 720p...");
    int rowsRemoved = cache.SearchService.ExecuteNonQuery(deleteQuery);

    // Printing output on console
    if (rowsRemoved == 0)
        Console.WriteLine("No images removed");
    else
        Console.WriteLine($"Removed {rowsRemoved} image(s) that have resolution set to 720p.");
}