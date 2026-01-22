using System.Collections;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;

namespace NCache.Playground.Samples.SqlQueryCacheLookup
{
  // Use SQL to query for cached data in NCache based on object attributes and Tags instead of just keys.
  // NCache SQL query requires indexes to be created on all objects being searched. One way is 
  // thru cache configuration. Second is through code using "Custom Attributes" to annotate object fields
  // and create index on them automatically when they're added to the cache.
  // In this sample, we're using "Custom Attributes" to create index on Product object.

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Sample showing SQL queries to find data in NCache based on object attributes instead of keys.\n");

      string cacheName = args[0];

      Console.WriteLine($"Connecting to cache: {cacheName}");
      // Connect to the cache and return a cache handle
      var cache = CacheManager.GetCache(cacheName);
      Console.WriteLine($"Cache connected successfully: {cacheName}\n");

      // Adds all the products to the cache. This automatically creates indexes on various
      // attributes of Product object by using "Custom Attributes".
      AddSampleData(cache);

      Console.WriteLine("Find products with Category IN (\"Electronics\", \"Stationery\") AND Price < $2000");

      // $VALUE$ keyword means the entire object instead of individual attributes that are also possible
      string sql = $"SELECT $VALUE$ FROM NCache.Playground.Samples.SqlQueryCacheLookup.Product WHERE Category IN (?, ?) AND Price < ?";

      var sqlCommand = new QueryCommand(sql);

      ArrayList catParamList = new ArrayList();
      catParamList.Add("Electronics");
      catParamList.Add("Stationery");

      sqlCommand.Parameters.Add("Category", catParamList);
      sqlCommand.Parameters.Add("Price", new Decimal(2000));

      // ExecuteReader returns ICacheReader with the query resultset
      using (var reader = cache.SearchService.ExecuteReader(sqlCommand))
      {
        var fetchedProducts = new List<Product>();
        if (reader.FieldCount > 0)
        {
          while (reader.Read())
          {
            // GetValue() with $VALUE$ keyword returns the entire object instead of just one column
            var result = reader.GetValue<Product>("$VALUE$");

            fetchedProducts.Add(result);
          }
        }
        PrintProducts(fetchedProducts);
      }

      Console.WriteLine("Find all \"phone\" products (using LIKE operator) AND (Tag = \"Technology\" OR Tag = \"Gadgets\")");
      sql = $"SELECT $VALUE$ FROM NCache.Playground.Samples.SqlQueryCacheLookup.Product WHERE Name LIKE ? AND ($Tag$ = ? OR $Tag$ = ?)";

      sqlCommand = new QueryCommand(sql);

      ArrayList tagList = new ArrayList();
      tagList.Add("Technology");
      tagList.Add("Gadgets");

      sqlCommand.Parameters.Add("Name", "*Phone*");
      sqlCommand.Parameters.Add("$Tag$", tagList);

      using (var reader = cache.SearchService.ExecuteReader(sqlCommand))
      {
        var fetchedProducts = new List<Product>();
        if (reader.FieldCount > 0)
        {
          while (reader.Read())
          {
            var result = reader.GetValue<Product>("$VALUE$");
            fetchedProducts.Add(result);
          }
        }
        PrintProducts(fetchedProducts);
      }

      Console.WriteLine("GROUP BY query: Get a count of product by category with Price < $3000");
      sql = $"SELECT Category, COUNT(*) FROM NCache.Playground.Samples.SqlQueryCacheLookup.Product WHERE Price < ? GROUP BY Category";

      sqlCommand = new QueryCommand(sql);

      sqlCommand.Parameters.Add("Price", new Decimal(3000));

      using (var reader = cache.SearchService.ExecuteReader(sqlCommand))
      {
        Console.WriteLine("Category, Count");
        if (reader.FieldCount > 0)
        {
          while (reader.Read())
          {
            var category = reader.GetValue<string>("Category");
            var count = reader.GetValue<int>("COUNT()");
            Console.WriteLine($"- {category}, {count}");
          }
        }
      }

      Console.WriteLine("\nDelete Items by Tags \"Technology\" and \"Reading\"");
      ArrayList removeTagsList = new ArrayList();
      removeTagsList.Add("Technology");
      removeTagsList.Add("Reading");

      sql = "DELETE FROM NCache.Playground.Samples.SqlQueryCacheLookup.Product WHERE $Tag$ IN (?, ?)";
      sqlCommand = new QueryCommand(sql);
      sqlCommand.Parameters.Add("$Tag$", removeTagsList);
      var itemAffected = cache.SearchService.ExecuteNonQuery(sqlCommand);
      Console.WriteLine($"{itemAffected} items removed from the cache.\n");

      Console.WriteLine("Sample completed successfully.");
    }

    static void AddSampleData(ICache cache)
    {
      Console.WriteLine("Adding products to the cache with tags...");

      var laptop = new Product { ProductID = 13001, Name = "Laptop", Price = 2000, Category = "Electronics" };
      var kindle = new Product { ProductID = 13002, Name = "Kindle", Price = 1000, Category = "Electronics" };
      var book = new Product { ProductID = 13003, Name = "Book", Price = 100, Category = "Stationery" };
      var smartPhone = new Product { ProductID = 13004, Name = "Smart Phone", Price = 1000, Category = "Electronics" };
      var mobilePhone = new Product { ProductID = 13005, Name = "Mobile Phone", Price = 200, Category = "Electronics" };

      var laptopCacheItem = new CacheItem(laptop) { Tags = new[] { new Tag("Technology"), new Tag("Gadgets") } };
      var kindleCacheItem = new CacheItem(kindle) { Tags = new[] { new Tag("Technology"), new Tag("Reading") } };
      var bookCacheItem = new CacheItem(book) { Tags = new[] { new Tag("Education"), new Tag("Reading") } };
      var smartPhoneCacheItem = new CacheItem(smartPhone) { Tags = new[] { new Tag("Technology"), new Tag("Communication") } };
      var mobilePhoneCacheItem = new CacheItem(mobilePhone) { Tags = new[] { new Tag("Technology"), new Tag("Communication") } };

      var products = new Dictionary<string, CacheItem>
      {
          { "13001", laptopCacheItem },
          { "13002", kindleCacheItem },
          { "13003", bookCacheItem },
          { "13004", smartPhoneCacheItem },
          { "13005", mobilePhoneCacheItem }
      };

      // Adds all the products to the cache. This automatically creates indexes on various
      // attributes of Product object by using "Custom Attributes".
      foreach (var product in products)
      {
        cache.Insert(product.Key, product.Value);
      }

      var productValues = products.Values.Select(cacheItem => cacheItem.GetValue<Product>());
      PrintProducts(productValues);
    }

    static void PrintProducts(IEnumerable<Product> products)
    {
      Console.WriteLine("ProductID, Name, Price, Category");
      foreach (var product in products)
      {
        Console.WriteLine($"- {product.ProductID}, {product.Name}, ${product.Price}, {product.Category}");
      }
      Console.WriteLine();
    }
  }

  [Serializable]
  public class Product
  {
    public int ProductID { get; set; }
    [QueryIndexed]
    public string Name { get; set; }
    [QueryIndexed]
    public decimal Price { get; set; }
    [QueryIndexed]
    public string Category { get; set; }
  }
}