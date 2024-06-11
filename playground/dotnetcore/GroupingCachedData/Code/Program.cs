using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;

namespace NCache.Playground.Samples.GroupingCachedData
{
  // Use the Tags feature of NCache to group items. First, use Tags while adding to the cache. Later, fetch by Tags

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Sample showing the Tags feature of NCache to logically group related data to easily fetch later.\n");

      // Connect to the cache
      string cacheName = args[0];

      Console.WriteLine($"Connecting to cache: {cacheName}");
      // Connect to the cache and return a cache handle
      var cache = CacheManager.GetCache(cacheName);
      Console.WriteLine($"Cache connected successfully: {cacheName}\n");

      var laptop = new Product { ProductID = 12001, Name = "Laptop", Price = 2000, Category = "Electronics" };
      var kindle = new Product { ProductID = 12002, Name = "Kindle", Price = 1000, Category = "Electronics" };
      var book = new Product { ProductID = 12003, Name = "Book", Price = 100, Category = "Stationery" };
      var smartPhone = new Product { ProductID = 12004, Name = "Smart Phone", Price = 1000, Category = "Electronics" };
      var mobilePhone = new Product { ProductID = 12005, Name = "Mobile Phone", Price = 200, Category = "Electronics" };

      var laptopCacheItem = new CacheItem(laptop) { Tags = new[] { new Tag("Technology"), new Tag("Gadgets") } };
      var kindleCacheItem = new CacheItem(kindle) { Tags = new[] { new Tag("Technology"), new Tag("Reading") } };
      var bookCacheItem = new CacheItem(book) { Tags = new[] { new Tag("Education"), new Tag("Reading") } };
      var smartPhoneCacheItem = new CacheItem(smartPhone) { Tags = new[] { new Tag("Technology"), new Tag("Communication") } };
      var mobilePhoneCacheItem = new CacheItem(mobilePhone) { Tags = new[] { new Tag("Technology"), new Tag("Communication") } };

      var products = new Dictionary<string, CacheItem>
      {
          { "12001", laptopCacheItem },
          { "12002", kindleCacheItem },
          { "12003", bookCacheItem },
          { "12004", smartPhoneCacheItem },
          { "12005", mobilePhoneCacheItem }
      };

      // Add all the products to the cache. 
      foreach (var product in products)
      {
        cache.Insert(product.Key, product.Value);
      }
      Console.WriteLine("Products added to the cache successfully.\n");

      var technologyItems = cache.SearchService.GetByTag<Product>(new Tag("Technology"));

      Console.WriteLine("Got Items from the cache by Tag \"Technology\"");
      PrintProductDetails(technologyItems);

      // Get all items from the cache with this Tag. readingItems is a list of key-value pairs
      var readingItems = cache.SearchService.GetByTag<Product>(new Tag("Reading"));

      Console.WriteLine("Got the Items from the cache by Tag \"Reading\"");
      PrintProductDetails(readingItems);

      // Create an array of tags
      Tag[] tags = new[] { new Tag("Technology"), new Tag("Reading") };

      // Remove all items from the cache with any of the tags
      cache.SearchService.RemoveByTags(tags, TagSearchOptions.ByAnyTag);
      Console.WriteLine("Removed Items by Tags \"Technology\" and \"Reading\"\n");

      Console.WriteLine("Sample completed successfully.");
    }

    static void PrintProductDetails(IEnumerable<KeyValuePair<string, Product>> items)
    {
      if (items != null && items.Any())
      {
        Console.WriteLine("ProductID, Name, Price, Category");
        foreach (var cacheItem in items)
        {
          var product = cacheItem.Value;
          Console.WriteLine($"- {product.ProductID}, {product.Name}, ${product.Price}, {product.Category}");
        }
      }
      else
      {
        Console.WriteLine("No items found...");
      }
      Console.WriteLine();
    }
  }

  [Serializable]
  public class Product
  {
    public int ProductID { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
  }
}