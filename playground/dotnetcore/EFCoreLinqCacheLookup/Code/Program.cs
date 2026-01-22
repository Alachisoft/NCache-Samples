using Alachisoft.NCache.EntityFrameworkCore;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace NCache.Playground.Samples.EFCoreCacheLookup
{
  // Use NCache integration with EF Core (thru Extension Methods) to load reference data into the cache
  // and later search the cache thru EF Core LINQ queries. This is not resultset caching.
  // We're using an SQLite in-memory database (with "Code First" approach) that goes away when the process ends.

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Sample showing NCache EF Core Extension Methods to load data in the cache and then use LINQ to query for it.\n");

      using (var dbContext = new ProductsDbContext(args[0]))
      {
        dbContext.Database.EnsureCreated();

        Console.WriteLine("LoadIntoCache: Adding all products to the cache thru EF Core Extension Method...");

        // Cache each object separately
        var cachingOptions = new CachingOptions() { StoreAs = StoreAs.SeperateEntities };

        // This method should use LINQ to load all products. Currently, it is using EF Core mapping to the Product table
        // We need to show how to use LINQ here even though all products will be loaded
        // p => true is a simple expression that returns true for every element in the collection.
        var products = dbContext.Products.Where(p => true).LoadIntoCache(cachingOptions).ToList();

        PrintProducts(products);

        Console.WriteLine("Find products with Category IN (\"Electronics\", \"Stationery\") AND price < $2000");

        // Creating an array of categories for filtering products
        var categoryFilter = new[] { "Electronics", "Stationery" };

        var fetchedProducts = dbContext.Products
            .Where(p => categoryFilter.Contains(p.Category) && p.Price < 2000)
            .FromCacheOnly()
            .ToList();

        PrintProducts(fetchedProducts);

        Console.WriteLine("Find all \"phone\" products (using LIKE operator) AND (Category = \"Electronics\" OR Category = \"Stationery\")");
        var keyword = "Phone";
        fetchedProducts = dbContext.Products
            .Where(p => p.Name.Contains(keyword) && (p.Category == "Electronics" || p.Category == "Stationery"))
            .FromCacheOnly()
            .ToList();

        PrintProducts(fetchedProducts);

        Console.WriteLine("Sample completed successfully.");
      }
    }

    static void PrintProducts(List<Product> products)
    {
      Console.WriteLine("ProductID, Name, Price, Category");
      foreach (var product in products)
      {
        Console.WriteLine($"- {product.ProductID}, {product.Name}, ${product.Price}, {product.Category}");
      }

      Console.WriteLine();
    }
  }

  public class ProductsDbContext : DbContext
  {
    public string _cacheName;
    public DbSet<Product> Products { get; set; }

    public ProductsDbContext(string cacheName)
    {
        _cacheName = cacheName;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      string cacheId = _cacheName;

      Console.WriteLine($"Connecting to cache: {cacheId}\n");
      NCacheConfiguration.Configure(cacheId, DependencyType.Other);
      Console.WriteLine($"Connected to Cache successfully: {cacheId}\n");

      // Configure in-memory database using SQLite provider
      var connection = new SqliteConnection("Data Source=:memory:");
      connection.Open();
      optionsBuilder.UseSqlite(connection);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Product>().HasData(
          new Product { ProductID = 14001, Name = "Laptop", Price = 2000, Category = "Electronics" },
          new Product { ProductID = 14002, Name = "Kindle", Price = 1000, Category = "Electronics" },
          new Product { ProductID = 14003, Name = "Book", Price = 100, Category = "Stationery" },
          new Product { ProductID = 14004, Name = "Smart Phone", Price = 1000, Category = "Electronics" },
          new Product { ProductID = 14005, Name = "Mobile Phone", Price = 200, Category = "Electronics" }
      );
    }
  }

  [Serializable]
  public class Product
  {
    public int ProductID { get; set; }

    [QueryIndexed]
    public string Name { get; set; }
    [QueryIndexed]
    public int Price { get; set; }
    [QueryIndexed]
    public string Category { get; set; }
  }
}