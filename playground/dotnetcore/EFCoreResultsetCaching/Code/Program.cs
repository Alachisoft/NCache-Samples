using Alachisoft.NCache.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace NCache.Playground.Samples.EFCoreResultsetCaching
{
  // Use NCache integration with EF Core (thru Extension Methods) to seamlessly cache EF Core query resultset.
  // Next time, EF Core fetches the resultset from the cache and saves an expensive database trip.
  // We're using an SQLite in-memory database (with "Code First" approach) that goes away when the process ends.

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Sample showing EF Core Resultset caching with NCache to save expensive database trips.\n");

      using (var dbContext = new CustomersbContext(args[0]))
      {
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();

        // Cache the entire query resultset as one item in the cache
        var cachingOptionsAlfki = new CachingOptions { StoreAs = StoreAs.Collection };
        cachingOptionsAlfki.SetAbsoluteExpiration(DateTime.Now.AddMinutes(5));

        // .FromCache() Extension Method fetches the resultset from the cache. If not found,
        // it caches it automatically. We are caching with 5min expiration.

        // This is the first call so resultset is obtained from the database
        var alFKICustomer = dbContext.Customers.Where(c => c.CustomerID == "ALFKI")
                            .FromCache(cachingOptionsAlfki).FirstOrDefault();

        Console.WriteLine("Loaded customer ALFKI from the database and cached...5min expiration");

        if (alFKICustomer != null)
          PrintCustomerDetails(alFKICustomer);

        // This is the second call to .FromCache() so resultset is fetched from the cache
        alFKICustomer = dbContext.Customers.Where(c => c.CustomerID == "ALFKI")
                        .FromCache(cachingOptionsAlfki).FirstOrDefault();

        if (alFKICustomer != null)
        {
          Console.WriteLine("Found and retrieved customer ALFKI from the cache...");
          PrintCustomerDetails(alFKICustomer);
        }

        Console.WriteLine("Sample completed successfully.");
      }
    }

    static void PrintCustomerDetails(Customer customer)
    {
      if (customer != null)
      {
        Console.WriteLine("CustomerID, Company, City, Country");
        Console.WriteLine($"- {customer.CustomerID}, {customer.CompanyName}, {customer.City}, {customer.Country}");
      }
      Console.WriteLine();
    }
  }

  public class CustomersbContext : DbContext
  {
    public string _cacheName;
    public DbSet<Customer> Customers { get; set; }

    public CustomersbContext(string cacheName)
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
      // Seed dummy data for demonstration purposes
      modelBuilder.Entity<Customer>().HasData(
          new Customer { CustomerID = "ALFKI", CompanyName = "Alfreds Futterkiste", City = "Berlin", Country = "Germany" },
          new Customer { CustomerID = "GREAL", CompanyName = "Great Lakes Food Market", City = "Eugene", Country = "USA" }
      );
    }
  }

  [Serializable]
  public class Customer
  {
    public string CustomerID { get; set; }
    public string CompanyName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
  }
}