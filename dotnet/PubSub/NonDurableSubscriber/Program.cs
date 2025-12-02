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
using Alachisoft.NCache.Runtime.Caching.Messaging;
using Alachisoft.NCache.Runtime.Exceptions;
using System;
using System.Configuration;

// ===============================================================================
// Non-durable subscribers only receive messages while they are active and
// connected. If a subscriber disconnects or the application stops, it will miss
// any messages published during its downtime. Use non-durable subscriptions for
// real-time, transient data streams (e.g. live dashboards or chat notifications)
// ===============================================================================

// Connect to a running cache and get cache handle for it
ICache? cache = GetCache();

//Topic names
string electronicsTopic = "ElectronicsOrders";
string garmentsTopic = "GarmentsOrders";

// Creating two non-durable topic subscriptions for "ElectronicsOrders" and "GarmentsOrders"
ITopicSubscription electronicsSubscription = CreateSubscription(electronicsTopic);
ITopicSubscription garmentsSubscription = CreateSubscription(garmentsTopic);

// Creating a pattern-based subscription for any topic which has "Orders" at the end of name (*Orders)
// Supported patterns:
//  '*'  → zero or more characters. Example: 'bl*' finds 'black', 'blue', 'blob'
//  '?'  → exactly one character. Example: 'h?t' finds 'hot', 'hat', 'hit'
//  '[]' → character set. Example: 'h[oa]t' finds 'hot' and 'hat', but not 'hit'
ITopicSubscription? allOrdersSubscription = CreatePatternSubscription("*Orders");

// Waits for user input to stop subscribers
Console.WriteLine("\nPress any key to unsubscribe from all topics...");
Console.ReadLine();

// Unsubscribing from all active topic subscriptions
electronicsSubscription?.UnSubscribe();
garmentsSubscription?.UnSubscribe();
allOrdersSubscription?.UnSubscribe();
Console.WriteLine("\nAll topic subscriptions have been unsubscribed successfully.");

// Dispose cache once all operations are complete
cache?.Dispose();

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to connect to a running cache and return cache handle for it 
/// </summary>
/// <returns>Cache handle for the connected cache</returns>
ICache? GetCache()
{
    // Get Cache name from configuration file
    string? cacheName = ConfigurationManager.AppSettings["CacheName"];

    // Validate cache name
    if (String.IsNullOrEmpty(cacheName))
    {
        Console.WriteLine("The CacheName cannot be null or empty.");
        return null;
    }

    // Connecting to a running cache and return a cache handle for it
    ICache cache = CacheManager.GetCache(cacheName);

    // Print output on console
    Console.WriteLine(string.Format("Cache '{0}' is connected.", cache));
    return cache;
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to create a non-durable subscriber for the specified topic
/// Non-durable subscriptions receive messages only while the subscriber is active
/// </summary>
/// <param name="topicName">Name of the topic to subscribe to</param>
/// <returns>Instance of ITopicSubscription representing the subscription</returns>
ITopicSubscription? CreateSubscription(string topicName)
{
    try
    {
        // Retrieve the topic from cache
        ITopic? topic = cache?.MessagingService.GetTopic(topicName);

        // Check if topic does not exists
        if (topic == null)
        {
            // Create a new topic
            topic = cache?.MessagingService.CreateTopic(topicName);
        }

        // Create a durable subscription
        ITopicSubscription subscription = topic.CreateSubscription(MessageReceivedCallback);

        // Print output on console
        Console.WriteLine($"\nNon-durable subscriber created for topic '{topicName}'.");

        // Return the created subscription
        return subscription;
    }
    catch (Exception ex)
    {
        // Handle other errors
        Console.WriteLine($"\nError creating subscriber for topic '{topicName}': {ex.Message}");

        // return null in case of error
        return null;
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to create a non-durable pattern-based subscription for all topics that match a given pattern
/// </summary>
/// <param name="pattern">Pattern used to match topic names</param>
/// <returns>Instance of ITopicSubscription if successful; otherwise null</returns>
ITopicSubscription? CreatePatternSubscription(string pattern)
{
    // Retrieve topics matching the specified pattern
    ITopic? patternTopic = cache.MessagingService.GetTopic(pattern, TopicSearchOptions.ByPattern);

    // Check if matching topics exist or not
    if (patternTopic != null)
    {
        // Create a pattern-based subscription
        ITopicSubscription subscription = patternTopic.CreateSubscription(PatternMessageReceivedCallback);

        // Print output on console
        Console.WriteLine($"\nPattern-based subscriber created for topics matching '{pattern}'.");

        // Return the created subscription
        return subscription;
    }
    else
    {
        // Print output on console if no matching topics found
        Console.WriteLine($"\nNo topics found matching pattern '{pattern}'.");

        // Return null if no matching topics found
        return null;
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Callback method invoked when a message is received for a specific topic subscription
/// </summary>
/// <param name="sender">Message source (publisher)</param>
/// <param name="args">Event arguments containing message and topic details</param>
void MessageReceivedCallback(object sender, MessageEventArgs args)
{
    // Display the topic name from which the message was received
    Console.WriteLine($"Message received for topic '{args.TopicName}'.");
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Callback method invoked when a message is received on a pattern-based subscription
/// </summary>
/// <param name="sender">Message source (publisher)</param>
/// <param name="args">Event arguments containing message and topic details</param>
void PatternMessageReceivedCallback(object sender, MessageEventArgs args)
{
    // Display the topic name from which the message was received
    Console.WriteLine($"Message received on pattern-based subscription for topic '{args.TopicName}'.");
}