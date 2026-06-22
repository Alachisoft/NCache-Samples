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
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Sample.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

// ===============================================================================
// The Order Service is responsible for handling incoming orders, storing them 
// in a distributed list, and managing order-related events. It ensures that 
// order data is shared reliably using NCache and acts as a publisher to notify 
// the Inventory Service about new orders. Below are details about how Pub/Sub is
// used in this service:
//   1. Publishes "Order received" messages to notify the Inventory Service.
//   2. Subscribes to "out-of-stock" messages from the Inventory Service to stop
//     accepting further orders.
// ===============================================================================

// Defining constants
const string ORDER_LIST_NAME = "orders-list";
const string ORDER_TO_INVENTORY_TOPIC = "orders-topic";
const string INVENTORY_TO_ORDER_TOPIC = "inventory-stock-topic";

// Setting Up web application builder
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

// Fetching Distributed list for orders
var ordersList = cache.DataTypeManager.GetList<Order>(ORDER_LIST_NAME);

// Checking if the orders list exists
if (ordersList == null)
{
    // Creating a new distributed list for orders
    ordersList = cache.DataTypeManager.CreateList<Order>(ORDER_LIST_NAME);
}
else
{
    // Clearing the list on every startup
    ordersList.Clear();
}

// Initializing out-of-stock flag
bool outOfStock = false;

// Declaring topics for Pub/Sub
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
    // Handling topic creation errors
    Console.WriteLine($"Topic get/create error: {ex.Message}");
}

// Checking if inventoryToOrderTopic is not null
if (inventoryToOrderTopic != null)
{
    // Subscribing to "out-of-stock" messages from Inventory service
    inventoryToOrderTopic.CreateSubscription(OnInventoryMessageReceived);
}

// Callback to handle "out-of-stock" messages from Inventory service
void OnInventoryMessageReceived(object sender, MessageEventArgs args)
{
    try
    {
        // Extracting the message payload
        var msg = args.Message;

        // Checking if message is null
        if (msg == null) return;

        // Setting out-of-stock flag based on message payload
        outOfStock = msg.Payload.ToString() == "out-of-stock";
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error in Inventory->Order callback: {e.Message}");
    }
}

// Endpoint to place an order
app.MapPost("/orders", (Order order) =>
{
    // Checking if out-of-stock 
    if (outOfStock)
    {
        // Rejecting order if out-of-stock
        return Results.Problem("Product is out of stock.");
    }

    // Adding the order to distributed list
    ordersList.Add(order);

    // Checking if orderToInventoryTopic is not null
    if (orderToInventoryTopic != null)
    {
        // Creating "Order received" message with expiration time
        var orderMessage = new Message("Order received");
        orderMessage.ExpirationTime = TimeSpan.FromSeconds(5000);

        // Registering callback for message delivery failure
        orderToInventoryTopic.MessageDeliveryFailure += OnFailureMessageReceived;

        // Publishing the message to Inventory service
        orderToInventoryTopic.Publish(orderMessage, DeliveryOption.All, true);
    }

    // Returning success response
    return Results.Ok("Order placed successfully");
});

// Endpoint to clean up orders list
app.Map("/cleanup", () =>
{
    try
    {
        // Removing the orders list from the cache
        cache?.DataTypeManager.Remove(ORDER_LIST_NAME);

        // Returning success response
        return Results.Ok("Order list removed successfully.");
    }
    catch (Exception ex)
    {
        // Returning error response in case of failure
        return Results.Problem("Failed to remove order list.");
    }
});

// Callback to handle message delivery failure
void OnFailureMessageReceived(object sender, MessageEventArgs args)
{
    // Logging message delivery failure
    Console.WriteLine("Message delivery to inventory failed.");
}

// Running the web application
app.Run();