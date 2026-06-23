// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Client;
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Caching.Messaging;
using Alachisoft.NCache.Runtime.Exceptions;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;

// ===============================================================================
// Durable subscribers are **persistent** and can recover messages published while
// they were offline. Once reconnected, they receive all pending messages that
// match their subscription. Use durable subscriptions for critical or
// time-sensitive systems like order processing, payment workflows etc.
// ===============================================================================

namespace DurableSubscriber
{
    internal class Program
    {
        // Declaring cache handle for the connected cache
        static ICache cache;

        static void Main(string[] args)
        {
            // Getting cache handle
            cache = GetCache();

            // Validating cache handle
            if (cache == null)
            {
                Console.WriteLine("Cache connection failed. Exiting the sample.");
                return;
            }

            // Declaring topic names
            string electronicsTopic = "ElectronicsOrders";
            string garmentsTopic = "GarmentsOrders";

            // Declaring subscription names
            string electronicSubscription = "ElectronicsSubscription";
            string garmentSubscription = "GarmentsSubscription";

            // Creating durable topic subscriptions for "ElectronicsOrders" (Shared) and "GarmentsOrders" (Exclusive)
            IDurableTopicSubscription electronicsSharedSubs = CreateDurableSubscription(electronicsTopic, electronicSubscription, SubscriptionPolicy.Shared);
            IDurableTopicSubscription garmentsExclusiveSubs = CreateDurableSubscription(garmentsTopic, garmentSubscription, SubscriptionPolicy.Exclusive);

            // Creating a pattern-based durable subscription for any topic which has "Orders" at the end of name (*Orders)
            // Patterns supported:
            // '*'  → zero or more characters. e.g., 'bl*' finds 'black', 'blue', 'blob'
            // '?'  → one character. e.g., 'h?t' finds 'hot', 'hat', 'hit'
            // '[]' → character set. e.g., 'h[oa]t' finds 'hot', 'hat', not 'hit'
            IDurableTopicSubscription allOrdersSubscription = CreatePatternSubscription("*Orders", "AllOrdersSubscription");

            // Waiting for user input to stop subscribers
            Console.WriteLine("\nPress any key to stop all subscribers...");
            Console.ReadLine();

            // Unsubscribing from all active topic subscriptions
            electronicsSharedSubs.UnSubscribe();
            garmentsExclusiveSubs.UnSubscribe();
            allOrdersSubscription.UnSubscribe();
            Console.WriteLine("\nAll topic subscriptions have been unsubscribed successfully.");

            // Dispose the cache once done
            cache.Dispose();
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to connect to a running cache and return cache handle for it 
        /// </summary>
        /// <returns>Cache handle for the connected cache</returns>
        private static ICache GetCache()
        {
            string cacheName = ConfigurationManager.AppSettings["CacheName"];

            if (String.IsNullOrEmpty(cacheName))
            {
                Console.WriteLine("The CacheName cannot be null or empty.");
                return null;
            }

            // Trying to connect to cache
            try
            {
                // Connecting to a running cache and return a cache handle for it
                ICache cache = CacheManager.GetCache(cacheName);

                // Printing output on console
                Console.WriteLine(string.Format("Cache '{0}' is connected.", cacheName));

                // Returning the connected cache handle
                return cache;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to cache: " + ex.Message);
                return null;
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to create a durable subscriber for the specified topic
        /// </summary>
        /// <param name="topicName">Name of the topic to subscribe to</param>
        /// <param name="subscriptionName">Name of the durable subscription</param>
        /// <param name="policy">Subscription policy (Shared or Exclusive)</param>
        /// <returns>Instance of IDurableTopicSubscription</returns>
        private static IDurableTopicSubscription CreateDurableSubscription(string topicName, string subscriptionName, SubscriptionPolicy policy)
        {
            try
            {
                // Retrieving the topic from cache
                ITopic topic = cache.MessagingService.GetTopic(topicName);

                // Checking if topic does not exists
                if (topic == null)
                {
                    // Creating a new topic
                    topic = cache.MessagingService.CreateTopic(topicName);
                }

                // Creating a durable subscription with 1-hour expiration
                IDurableTopicSubscription subscription = topic.CreateDurableSubscription(
                    subscriptionName,
                    policy,
                    MessageReceivedCallback,
                    TimeSpan.FromHours(1)
                );

                // Printing output on console
                Console.WriteLine($"\nDurable subscriber '{subscriptionName}' created on topic '{topicName}' with policy '{policy}'.");

                // Returning the created subscription
                return subscription;
            }
            catch (CacheException ex) when (ex.ErrorCode == NCacheErrorCodes.SUBSCRIPTION_EXISTS)
            {
                // Handling case where subscription with same name already exists
                Console.WriteLine($"\nActive subscription '{subscriptionName}' already exists for topic '{topicName}'.");
                return null;
            }
            catch (Exception ex)
            {
                // Handling other errors
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
        private static IDurableTopicSubscription CreatePatternSubscription(string topicPattern, string subscriptionName)
        {
            // Retrieving topics matching the specified pattern
            ITopic topic = cache.MessagingService.GetTopic(topicPattern, TopicSearchOptions.ByPattern);

            // Checking if topic exists or not
            if (topic != null)
            {
                // Creating durable subscription with 1-hour expiration
                IDurableTopicSubscription subscription = topic.CreateDurableSubscription(
                    subscriptionName,
                    SubscriptionPolicy.Shared,
                    MessageReceivedCallbackPatternBased,
                    TimeSpan.FromHours(1)
                );

                // Printing output on console
                Console.WriteLine($"\nPattern-based durable subscriber '{subscriptionName}' created for topics matching '{topicPattern}'.");

                // Returning the created subscription
                return subscription;
            }
            else
            {
                // Printing if topics were not found
                Console.WriteLine($"\nNo topics found matching pattern '{topicPattern}'.");

                // Returning null if no matching topics found
                return null;
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Callback method invoked when a message is received by the subscriber
        /// </summary>
        /// <param name="sender">Message sender</param>
        /// <param name="args">Message event arguments</param>
        private static void MessageReceivedCallback(object sender, MessageEventArgs args)
        {
            // Printing message details on console
            Console.WriteLine($"Message received for topic '{args.TopicName}'.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Callback method invoked when a message is received by a pattern-based subscription
        /// </summary>
        /// <param name="sender">Message sender</param>
        /// <param name="args">Message event arguments</param>
        private static void MessageReceivedCallbackPatternBased(object sender, MessageEventArgs args)
        {
            // Printing message details on console
            Console.WriteLine($"Message received on pattern-based subscription for topic '{args.TopicName}'.");
        }
    }
}
