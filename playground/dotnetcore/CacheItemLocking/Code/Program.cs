using Alachisoft.NCache.Client;

namespace NCache.Playground.Samples.CacheItemLocking
{
  // Locking an item prevents other users from accessing it while you update it.
  // You can lock with cache.GetCacheItem() or cache.Lock() and unlock with cache.Insert() or cache.Unlock()
  // NOTE: Locking API is for application level locking. NCache already has its own internal locking
  // for concurrency and thread-safe operations.

  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Sample showing item level lock and unlock in NCache.\n");
      string cacheName = args[0];

      Console.WriteLine($"Connecting to cache: {cacheName}\n");
      // Connect to the cache and return a cache handle
      var cache = CacheManager.GetCache(cacheName);
      Console.WriteLine($"Connected to Cache successfully: {cacheName}\n");

      Product prod1 = new Product { ProductID = 11001, Name = "Laptop", Price = 1000, Category = "Electronics" };

      string prod1Key = prod1.ProductID.ToString();
      Console.WriteLine("Adding product 11001 to the cache...");

      // add a product to the cache. if it already exists, the operation will fail
      cache.Insert(prod1Key, prod1);
      Console.WriteLine("Product added successfully:");
      PrintProductDetails(prod1);

      // Get product 11001 and lock it in the cache
      Console.WriteLine("Get and lock product 11001 in the cache...");
      LockHandle lockHandle = null;

      // Passing "acquireLock:" flag also locks the item and "lockTimeout:" releases the lock after 10sec
      var lockedCacheItem = cache.GetCacheItem(prod1Key, acquireLock: true, lockTimeout: TimeSpan.FromSeconds(10), ref lockHandle);
      if (lockedCacheItem != null)
      {
        Console.WriteLine("Product 11001 retrieved and locked successfully:");
        var retrievedProduct = lockedCacheItem.GetValue<Product>();
        PrintProductDetails(retrievedProduct);

        // Change the price of product 11001
        retrievedProduct.Price = 2000;

        // Insert() updates the item and releases the lock
        cache.Insert(prod1Key, lockedCacheItem, lockHandle, releaseLock: true);
        Console.WriteLine("Updated price of product 11001 and released the lock simultaneously in the cache...");

        PrintProductDetails(retrievedProduct);
      }
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