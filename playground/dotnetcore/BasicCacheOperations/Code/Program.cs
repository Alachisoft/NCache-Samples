using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;

namespace NCache.Playground.Samples.BasicCacheOperations
{
  // Basic caching operations (CRUD) as an intro to NCache API

  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Sample showing basic caching operations (CRUD) in NCache, along with expiration.\n");

      string cacheName = args[0];

      Console.WriteLine($"Connecting to cache: {cacheName}\n");
      // Connect to the cache and return a cache handle
      var cache = CacheManager.GetCache(cacheName);
      Console.WriteLine($"Connected to Cache successfully: {cacheName}\n");

      // Add product 10001 to the cache with a 5-min expiration
      Product prod1 = new Product { ProductID = 10001, Name = "Laptop", Price = 1000, Category = "Electronics" };

      string prod1Key = prod1.ProductID.ToString();
      var productCacheItem = new CacheItem(prod1);
      productCacheItem.Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(5));

      // Add to the cache with 5min absolute expiration (TTL). Item expires automatically
      cache.Add(prod1Key, productCacheItem);

      Console.WriteLine("Product 10001 added successfully (5min expiration):");
      PrintProductDetails(prod1);

      // Get from the cache. If not found, a null is returned
      Product retrievedprod1 = cache.Get<Product>(prod1Key);
      if (retrievedprod1 != null)
      {
        Console.WriteLine("Product 10001 retrieved successfully:");
        PrintProductDetails(retrievedprod1);
      }

      retrievedprod1.Price = 3000;

      // Update product in the cache. If not found, it is added
      productCacheItem = new CacheItem(retrievedprod1);
      productCacheItem.Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(5));
      cache.Insert(prod1Key, productCacheItem);

      Console.WriteLine("Price for Product 10001 updated successfully:");
      PrintProductDetails(retrievedprod1);

      // Remove the item from the cache
      cache.Remove(prod1Key);
      Console.WriteLine($"Deleted the product {prod1Key} from the cache...\n");

      Console.WriteLine("Sample completed successfully.");
    }

    private static void PrintProductDetails(Product product)
    {
      Console.WriteLine("ProductID, Name, Price, Category");
      Console.WriteLine($"- {product.ProductID}, {product.Name}, {product.Price}, {product.Category} \n");
    }
  }

  // Sample class for Product
  [Serializable]
  public class Product
  {
    public int ProductID { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
  }
}