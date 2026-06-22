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
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Caching.Messaging;
using Alachisoft.NCache.Runtime.Exceptions;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;

// ===============================================================================
// Non-durable subscribers only receive messages while they are active and
// connected. If a subscriber disconnects or the application stops, it will miss
// any messages published during its downtime. Use non-durable subscriptions for
// real-time, transient data streams (e.g. live dashboards or chat notifications)
// ===============================================================================

namespace NonDurableSubscriber
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

            // Creating two non-durable topic subscriptions for "ElectronicsOrders" and "GarmentsOrders"
            ITopicSubscription electronicsSubscription = CreateSubscription(electronicsTopic);
            ITopicSubscription garmentsSubscription = CreateSubscription(garmentsTopic);

            // Waiting for user input to stop subscribers
            Console.WriteLine("\nPress any key to unsubscribe from all topics...");
            Console.ReadLine();

            // Unsubscribing from all active topic subscriptions
            electronicsSubscription.UnSubscribe();
            garmentsSubscription.UnSubscribe();
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
        /// Method to create a non-durable subscriber for the specified topic
        /// Non-durable subscriptions receive messages only while the subscriber is active
        /// </summary>
        /// <param name="topicName">Name of the topic to subscribe to</param>
        /// <returns>Instance of ITopicSubscription representing the subscription</returns>
        private static ITopicSubscription CreateSubscription(string topicName)
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

                // Creating a durable subscription
                ITopicSubscription subscription = topic.CreateSubscription(MessageReceivedCallback);

                // Printing output on console
                Console.WriteLine($"\nNon-durable subscriber created for topic '{topicName}'.");

                // Returning the created subscription
                return subscription;
            }
            catch (Exception ex)
            {
                // Handling other errors
                Console.WriteLine($"\nError creating subscriber for topic '{topicName}': {ex.Message}");

                // Returning null in case of error
                return null;
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to create a non-durable pattern-based subscription for all topics that match a given pattern
        /// </summary>
        /// <param name="pattern">Pattern used to match topic names</param>
        /// <returns>Instance of ITopicSubscription if successful; otherwise null</returns>
        private static ITopicSubscription CreatePatternSubscription(string pattern)
        {
            // Retrieving topics matching the specified pattern
            ITopic patternTopic = cache.MessagingService.GetTopic(pattern);

            // Checking if matching topics exist or not
            if (patternTopic != null)
            {
                // Creating a pattern-based subscription
                ITopicSubscription subscription = patternTopic.CreateSubscription(PatternMessageReceivedCallback);

                // Printing output on console
                Console.WriteLine($"\nPattern-based subscriber created for topics matching '{pattern}'.");

                // Returning the created subscription
                return subscription;
            }
            else
            {
                // Printing output on console if no matching topics found
                Console.WriteLine($"\nNo topics found matching pattern '{pattern}'.");

                // Returning null if no matching topics found
                return null;
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Callback method invoked when a message is received for a specific topic subscription
        /// </summary>
        /// <param name="sender">Message source (publisher)</param>
        /// <param name="args">Event arguments containing message and topic details</param>
        private static void MessageReceivedCallback(object sender, MessageEventArgs args)
        {
            // Displaying the topic name from which the message was received
            Console.WriteLine($"Message received for topic '{args.TopicName}'.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Callback method invoked when a message is received on a pattern-based subscription
        /// </summary>
        /// <param name="sender">Message source (publisher)</param>
        /// <param name="args">Event arguments containing message and topic details</param>
        private static void PatternMessageReceivedCallback(object sender, MessageEventArgs args)
        {
            // Displaying the topic name from which the message was received
            Console.WriteLine($"Message received on pattern-based subscription for topic '{args.TopicName}'.");
        }

    }
}
