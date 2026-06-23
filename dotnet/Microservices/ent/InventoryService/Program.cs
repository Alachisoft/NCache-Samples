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
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Runtime.Caching;

// ===============================================================================
// The Inventory Service tracks stock count using a distributed counter and
// manages inventory events. It ensures consistent stock updates across services
// using NCache. Below are details about how Pub/Sub is used in this service:
//   1. Subscribes to "Order received" messages from the Order Service to
//      decrement stock.
//   2. Publishes "out-of-stock" messages when stock reaches zero to notify
//      the Order Service.
// ===============================================================================

// Defining constants
const string STOCK_COUNTER_NAME = "stock-counter";
const string ORDER_TO_INVENTORY_TOPIC = "orders-topic";
const string INVENTORY_TO_ORDER_TOPIC = "inventory-stock-topic";
const int INIT_STOCK_VALUE = 3;

// Setting up the web application builder
var builder = WebApplication.CreateBuilder(args);

// Loading configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Building the app
var app = builder.Build();

// Getting cache name from configuration
var cacheName = app.Configuration["NCache:CacheName"];

// Connect to cache
ICache cache = CacheManager.GetCache(cacheName);

// Messaging service to publish and subscribe to topics
var messaging = cache.MessagingService;

// Fetching stock counter
ICounter retrievedCounter = cache.DataTypeManager.GetCounter(STOCK_COUNTER_NAME);

// Checking if counter exists
if (retrievedCounter == null)
{
    // Creating a new counter since it doesn't exist
    retrievedCounter = cache.DataTypeManager.CreateCounter(STOCK_COUNTER_NAME, INIT_STOCK_VALUE);
}
else
{
    // Setting initial counter value
    retrievedCounter.SetValue(INIT_STOCK_VALUE);
}

// Declaring topic variables
ITopic? orderToInventoryTopic = null;
ITopic? inventoryToOrderTopic = null;

try
{
    // Getting or creating topics for communication
    orderToInventoryTopic = messaging.GetTopic(ORDER_TO_INVENTORY_TOPIC) ?? messaging.CreateTopic(ORDER_TO_INVENTORY_TOPIC);
    inventoryToOrderTopic = messaging.GetTopic(INVENTORY_TO_ORDER_TOPIC) ?? messaging.CreateTopic(INVENTORY_TO_ORDER_TOPIC);
}
catch (Exception ex)
{
    // Printing topic creation errors
    Console.WriteLine($"Topic get/create error: {ex.Message}");
}

// Notifying “out of stock” if stock is initially 0
if (inventoryToOrderTopic != null && retrievedCounter.Value == 0)
{
    var msg = new Message("out-of-stock");
    inventoryToOrderTopic.Publish(msg, DeliveryOption.All, true);
}

// Subscribing to Order topic to listen for received orders
if (orderToInventoryTopic != null)
{
    orderToInventoryTopic.CreateSubscription(OnOrderReceived);
}

// Callback method to handle "order received" messages from Order service
void OnOrderReceived(object sender, MessageEventArgs args)
{
    try
    {
        // Extracting the message
        var msg = args.Message;

        // Checking for null message
        if (msg == null) return;

        // Decrementing stock counter on each order received
        long newStockValue = retrievedCounter.Decrement();

        // If stock reaches zero, notify via inventory topic
        if (newStockValue <= 0)
        {
            // Creating and publishing out-of-stock message
            var orderMessage = new Message("out-of-stock");
            inventoryToOrderTopic.Publish(orderMessage, DeliveryOption.All, true);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in Order->Inventory callback: {ex.Message}");
    }
}

// Endpoint to get the current stock value
app.MapGet("/stock", () =>
{
    var value = retrievedCounter.Value;
    return Results.Ok(new { stock = value });
});

// Endpoint to clean up the stock counter from the cache
app.MapDelete("/cleanup", () =>
{
    try
    {
        // Removing the stock counter from the cache
        cache?.DataTypeManager.Remove(STOCK_COUNTER_NAME);
        return Results.Ok("Stock counter removed successfully.");
    }
    catch (Exception ex)
    {
        return Results.Problem("Failed to remove stock counter.");
    }
});

// Running the application
app.Run();