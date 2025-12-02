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
// Durable subscribers are **persistent** and can recover messages published while
// they were offline. Once reconnected, they receive all pending messages that
// match their subscription. Use durable subscriptions for critical or
// time-sensitive systems like order processing, payment workflows etc.
// ===============================================================================

// Connect to a running cache and get cache handle for it
ICache? cache = GetCache();

//Topic names
string electronicsTopic = "ElectronicsOrders";
string garmentsTopic = "GarmentsOrders";

//Subscription names
string electronicSubscription = "ElectronicsSubscription";
string garmentSubscription = "GarmentsSubscription";

// Creating durable topic subscriptions for "ElectronicsOrders" (Shared) and "GarmentsOrders" (Exclusive)
IDurableTopicSubscription? electronicsSharedSubs = CreateDurableSubscription(electronicsTopic, electronicSubscription, SubscriptionPolicy.Shared);
IDurableTopicSubscription? garmentsExclusiveSubs = CreateDurableSubscription(garmentsTopic, garmentSubscription, SubscriptionPolicy.Exclusive);

// Creating a pattern-based durable subscription for any topic which has "Orders" at the end of name (*Orders)
// Patterns supported:
// '*'  → zero or more characters. e.g., 'bl*' finds 'black', 'blue', 'blob'
// '?'  → one character. e.g., 'h?t' finds 'hot', 'hat', 'hit'
// '[]' → character set. e.g., 'h[oa]t' finds 'hot', 'hat', not 'hit'
IDurableTopicSubscription? allOrdersSubscription = CreatePatternSubscription("*Orders", "AllOrdersSubscription");

// Waits for user input to stop subscribers
Console.WriteLine("\nPress any key to stop all subscribers...");
Console.ReadLine();

// Unsubscribing from all active topic subscriptions
electronicsSharedSubs?.UnSubscribe();
garmentsExclusiveSubs?.UnSubscribe();
allOrdersSubscription?.UnSubscribe();
Console.WriteLine("\nAll topic subscriptions have been unsubscribed successfully.");

// Dispose the cache once done
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
/// Method to create a durable subscriber for the specified topic
/// </summary>
/// <param name="topicName">Name of the topic to subscribe to</param>
/// <param name="subscriptionName">Name of the durable subscription</param>
/// <param name="policy">Subscription policy (Shared or Exclusive)</param>
/// <returns>Instance of IDurableTopicSubscription</returns>
IDurableTopicSubscription? CreateDurableSubscription(string topicName, string subscriptionName, SubscriptionPolicy policy)
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

        // Create a durable subscription with 1-hour expiration
        IDurableTopicSubscription subscription = topic.CreateDurableSubscription(
            subscriptionName,
            policy,
            MessageReceivedCallback,
            TimeSpan.FromHours(1)
        );

        // Print output on console
        Console.WriteLine($"\nDurable subscriber '{subscriptionName}' created on topic '{topicName}' with policy '{policy}'.");

        // Return the created subscription
        return subscription;
    }
    catch (CacheException ex) when (ex.ErrorCode == NCacheErrorCodes.SUBSCRIPTION_EXISTS)
    {
        // Handle case where subscription with same name already exists
        Console.WriteLine($"\nActive subscription '{subscriptionName}' already exists for topic '{topicName}'.");
        return null;
    }
    catch (Exception ex)
    {
        // Handle other errors
        Console.WriteLine($"\nError creating subscriber for topic '{topicName}': {ex.Message}");
        return null;
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Method to create a pattern-based durable subscription for all topics matching a given pattern
/// </summary>
/// <param name="topicPattern">Pattern to match topic names</param>
/// <param name="subscriptionName">Name of the durable subscription</param>
/// <returns>Instance of IDurableTopicSubscription</returns>
IDurableTopicSubscription? CreatePatternSubscription(string topicPattern, string subscriptionName)
{
    // Retrieve topics matching the specified pattern
    ITopic? topic = cache?.MessagingService.GetTopic(topicPattern, TopicSearchOptions.ByPattern);

    // Check if topic exists or not
    if (topic != null)
    {
        // Create durable subscription with 1-hour expiration
        IDurableTopicSubscription subscription = topic.CreateDurableSubscription(
            subscriptionName,
            SubscriptionPolicy.Shared,
            MessageReceivedCallbackPatternBased,
            TimeSpan.FromHours(1)
        );

        // Print output on console
        Console.WriteLine($"\nPattern-based durable subscriber '{subscriptionName}' created for topics matching '{topicPattern}'.");

        // Return the created subscription
        return subscription;
    }
    else 
    {
        // No matching topics found
        Console.WriteLine($"\nNo topics found matching pattern '{topicPattern}'.");

        // Return null if no matching topics found
        return null;
    }
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Callback method invoked when a message is received by the subscriber
/// </summary>
/// <param name="sender">Message sender</param>
/// <param name="args">Message event arguments</param>
void MessageReceivedCallback(object sender, MessageEventArgs args)
{
    // Print message details on console
    Console.WriteLine($"Message received for topic '{args.TopicName}'.");
}

/// ------------------------------------------------------------------------------
/// <summary>
/// Callback method invoked when a message is received by a pattern-based subscription
/// </summary>
/// <param name="sender">Message sender</param>
/// <param name="args">Message event arguments</param>
void MessageReceivedCallbackPatternBased(object sender, MessageEventArgs args)
{
    // Print message details on console
    Console.WriteLine($"Message received on pattern-based subscription for topic '{args.TopicName}'.");
}
