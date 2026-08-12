using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;

namespace NCache.Playground.Samples.PubSubMessaging
{
  // Use NCache Pub/Sub Messaging in this sample. In NCache, you can create multiple Topics
  // and against each topic create multiple publishers and subscribers. In this sample,
  // 2 parallel Async publisher tasks publish on the same topic and 2 subscribers receive
  // messages from this topic. All messages use DeliveryOption. Any so only one subscriber
  // gets each message. DeliveryOption.All is available but not used.

  class Program
  {
    static string topicName = "ORDERS";
    static readonly object consoleLock = new object();
    static async Task Main(string[] args)
    {
      Console.WriteLine("Sample showing NCache Pub/Sub Messaging for reliable real-time messaging between publishers and subscribers.\n");

      var cacheName = args[0];

      Console.WriteLine($"Connecting to cache: {cacheName}\n");
      // Connect to the cache and return a cache handle
      var cache = CacheManager.GetCache(cacheName);
      Console.WriteLine($"Connected to Cache successfully: {cacheName}\n");

      Console.WriteLine($"Create a Topic {topicName} ...\n");

      // Create a Topic in NCache.
      var topic = cache.MessagingService.CreateTopic(topicName);

      Console.WriteLine("Register 2 subscribers for Topic " + topicName + "\n");

      // Register subscribers to this Topic
      var subscription1 = topic.CreateSubscription(MessageReceived_Subscription1);
      var subscription2 = topic.CreateSubscription(MessageReceived_Subscription2);

      // Create first asynchronous publisher task to publish 2 messages
      var publisher1Task = new Task(async () =>
      {
        var message1 = new Message
              (
                  new Order() { OrderID = 17348, Customer = "Vins et alcools Chevalier", OrderDate = DateTime.Parse("04-Jul-1997"), ShipVia = "Federal Shipping" }
              );

        var message2 = new Message
              (
                  new Order() { OrderID = 17350, Customer = "Hanari Carnes", OrderDate = DateTime.Parse("07-Jul-1997"), ShipVia = "United Package" }
              );

        PrintMessage(message1, "Publisher 1", null);
              // "Any" means that only one subscriber will receive this message
              // Another option is "All" where all subscribers recieve the message
        topic.Publish(message1, DeliveryOption.Any);

              // wait for 1sec before publishing again
        Thread.Sleep(1000);
        PrintMessage(message2, "Publisher 1", null);

        topic.Publish(message2, DeliveryOption.Any);
      });

      // Create second asynchronous publisher task to publish 2 messages
      var publisher2Task = new Task(async () =>
      {
        var message1 = new Message
              (
                  new Order() { OrderID = 17349, Customer = "Toms Spezialitäten", OrderDate = DateTime.Parse("05-Jul-1997"), ShipVia = "Speedy Express" }
              );

        var message2 = new Message
              (
                  new Order() { OrderID = 17351, Customer = "Victuailles en stock", OrderDate = DateTime.Parse("08-Jul-1997"), ShipVia = "Speedy Express" }
              );

        PrintMessage(message1, "Publisher 2", null);

              // "Any" means that only one subscriber will receive this message
              // Another option is "All" where all subscribers recieve the message
        topic.Publish(message1, DeliveryOption.Any);

              // wait for 1sec before publishing again
        Thread.Sleep(1000);
        PrintMessage(message2, "Publisher 2", null);

        topic.Publish(message2, DeliveryOption.Any);
      });

      // Now start both Async publisher tasks so each can publish 2 messages in parallel
      publisher1Task.Start();
      publisher2Task.Start();

      // Wait for 5sec
      Thread.Sleep(5000);

      // Waiting for both publisher tasks to complete so we can end the sample
      await Task.WhenAll(publisher1Task, publisher2Task);

      subscription1.UnSubscribe();
      subscription2.UnSubscribe();

      Console.WriteLine("Sample completed successfully.");
    }

    private static void MessageReceived_Subscription1(object sender, MessageEventArgs args)
    {
      PrintMessage(args.Message, null, "Subscriber 1");
    }

    private static void MessageReceived_Subscription2(object sender, MessageEventArgs args)
    {
      PrintMessage(args.Message, null, "Subscriber 2");
    }

    static void PrintMessage(IMessage message, string sender, string receiver)
    {
      var order = message.Payload as Order;

      lock (consoleLock)
      {
        if (!string.IsNullOrEmpty(sender)) Console.WriteLine($"{sender} publishes a message for Topic {topicName} ...");
        else if (!string.IsNullOrEmpty(receiver)) Console.WriteLine($"{receiver} receives message ...");

        Console.WriteLine("OrderID, Customer, Order Date, Ship Via");
        Console.WriteLine($"- {order.OrderID}, {order.Customer}, {order.OrderDate.ToString("dd-MMM-yyyy")}, {order.ShipVia} \n");
      }
    }
  }

  [Serializable]
  public class Order
  {
    public int OrderID { get; set; }
    public string Customer { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShipVia { get; set; }
  }
}