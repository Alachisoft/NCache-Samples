using Alachisoft.NCache.Client;
using Alachisoft.NCache.Client.DataTypes.Collections;

namespace NCache.Playground.Samples.DataStructures
{
  // Use NCache distributed data structures (List and Queue in this sample). NCache provides
  // Set, Dictionary, and Counter data structures as well. These data structures are distributed in the cluster
  // for scalability and accessible to all clients.

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Sample showing distributed data structures in NCache (List, Queue) for more powerful data management.\n");

      var cacheName = args[0];

      Console.WriteLine($"Connecting to cache: {cacheName}\n");
      // Connect to the cache and return a cache handle
      var cache = CacheManager.GetCache(cacheName);
      Console.WriteLine($"Connected to Cache successfully: {cacheName}\n");

      Console.WriteLine("Create a List data structure and add products 'Electronics' to it...");

      // Get the list if it already exists in the cache
      IDistributedList<Product> productList = cache.DataTypeManager.GetList<Product>("productList");

      if (productList == null)
      {
        // Create a List data structure in the cache and then add products to it
        productList = cache.DataTypeManager.CreateList<Product>("productList");
      }

      // These products are directly added to the cache inside the List data structure when you add them below
      productList.Add(new Product { ProductID = 15001, Name = "Laptop", Price = 2000, Category = "Electronics" });
      productList.Add(new Product { ProductID = 15002, Name = "Kindle", Price = 1000, Category = "Electronics" });
      productList.Add(new Product { ProductID = 15004, Name = "Smart Phone", Price = 1000, Category = "Electronics" });
      productList.Add(new Product { ProductID = 15005, Name = "Mobile Phone", Price = 200, Category = "Electronics" });

      PrintProductList(productList);

      Console.WriteLine("Create a Queue data structure and add orders to it...");

      // Get the queue if it already exists in the cache
      IDistributedQueue<Order> orderQueue = cache.DataTypeManager.GetQueue<Order>("orderQueue");

      if (orderQueue == null)
      {
        // Create a Queue data structure and then add orders to it by maintaining their sequence
        orderQueue = cache.DataTypeManager.CreateQueue<Order>("orderQueue");
      }

      // These orders are directly added to the cache inside the Queue data structure when you enqueue them below
      orderQueue.Enqueue(new Order { OrderID = 16248, Customer = "Vins et alcools Chevalier", OrderDate = new DateTime(1997, 7, 4), ShipVia = "Federal Shipping" });
      orderQueue.Enqueue(new Order { OrderID = 16249, Customer = "Toms Spezialitäten", OrderDate = new DateTime(1997, 7, 5), ShipVia = "Speedy Express" });
      orderQueue.Enqueue(new Order { OrderID = 16250, Customer = "Hanari Carnes", OrderDate = new DateTime(1997, 7, 7), ShipVia = "United Package" });
      orderQueue.Enqueue(new Order { OrderID = 16251, Customer = "Victuailles en stock", OrderDate = new DateTime(1997, 7, 8), ShipVia = "Speedy Express" });

      PrintOrderQueue(orderQueue);

      Console.WriteLine("Fetch the list and print all items from the List data structure...");

      // Fetch the List data structure from the cache and then access all items in it
      var fetchedProductList = cache.DataTypeManager.GetList<Product>("productList");

      PrintProductList(fetchedProductList);

      Console.WriteLine("Process all orders from the Queue one by one in a sequence\n");

      // Fetch the Queue data structure from the cache and then process all orders one by one in a sequence
      var fetchedOrderQueue = cache.DataTypeManager.GetQueue<Order>("orderQueue");
      while (fetchedOrderQueue.Count > 0)
      {
        var order = orderQueue.Dequeue();
        Console.WriteLine("Process order : " + order.OrderID);
        PrintOrder(order);
      }

      // Removing all items from the list
      productList.RemoveRange(0, productList.Count);

      Console.WriteLine("Removed products from the List data structure.\n");

      Console.WriteLine("Sample completed successfully.");
    }

    static void PrintProductList(IList<Product> productList)
    {
      Console.WriteLine("ProductID, Name, Price, Category, Tags");
      foreach (var product in productList)
      {
        Console.WriteLine($"- {product.ProductID}, {product.Name}, ${product.Price}, {product.Category}");
      }
      Console.WriteLine();
    }

    static void PrintOrderQueue(IDistributedQueue<Order> orderQueue)
    {
      Console.WriteLine("OrderID, Customer, Order Date, Ship Via");
      foreach (var order in orderQueue)
      {
        Console.WriteLine($"- {order.OrderID}, {order.Customer}, {order.OrderDate.ToShortDateString()}, {order.ShipVia}");
      }
      Console.WriteLine();
    }

    static void PrintOrder(Order order)
    {
      Console.WriteLine("OrderID, Customer, Order Date, Ship Via");
      Console.WriteLine($"- {order.OrderID}, {order.Customer}, {order.OrderDate.ToShortDateString()}, {order.ShipVia} \n");
    }
  }

  class Product
  {
    public int ProductID { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
  }

  class Order
  {
    public int OrderID { get; set; }
    public string Customer { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShipVia { get; set; }
  }
}