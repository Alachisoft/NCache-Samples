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
using Alachisoft.NCache.Sample.Data;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

// ===============================================================================
// Publish/Subscribe (Pub/Sub) is a messaging feature provided by NCache.
// It allows applications to send and receive messages through topics 
// without being directly connected to each other. Some possible use cases of
// Pub/Sub in NCache include:
//   1. Sending real-time updates, such as order status, inventory changes, or
//      quote alerts.
//   2. Broadcasting notifications to multiple systems, microservices, or users
//   3. Updating dashboards or live data feeds automatically without polling
//   4. Facilitating communication between different components of a distributed
//      system.
//   5. Implementing event-driven architectures for decoupled and scalable systems
// This sample demonstrates publishing messages (synchronously, asynchronously,
// and in bulk) to topics. Corresponding Durable and Non-Durable Subscriber
// samples can receive these messages.
// ===============================================================================

namespace Publisher
{
    internal class Program
    {
        // Declaring cache handle for the connected cache
        static ICache cache;

        // Number of messages each publisher will publish
        const int MESSAGES_TO_PUBLISH = 30;

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

            // Setting topic names
            string electronicsTopic = "ElectronicsOrders";
            string garmentsTopic = "GarmentsOrders";

            // Creating tasks for publishers
            var electronicsOrder = new Task(() => RunPublisher<ElectronicsOrder>(electronicsTopic));
            var garmentsOrder = new Task(() => RunPublisher<ElectronicsOrder>(garmentsTopic));

            // Starting publisher tasks
            Console.WriteLine($"Starting a Publisher... Publisher will stop after publishing {MESSAGES_TO_PUBLISH} messages each.");
            electronicsOrder.Start();
            garmentsOrder.Start();
            Console.WriteLine("\nPublisher started successfully.");

            // Waiting for user input before cleaning up
            Console.WriteLine("\nPress any key to delete topics...");
            Console.ReadKey();

            // Deleting topics created for this sample
            DeleteTopic(electronicsTopic);
            DeleteTopic(garmentsTopic);

            Console.WriteLine("\nTopics deleted successfully.");

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
        /// Method to start publishing messages synchronously to a specific topic
        /// Each message is published individually with confirmation
        /// </summary>
        /// <typeparam name="T">Type of order being published</typeparam>
        /// <param name="topicName">Name of the topic to publish to</param>
        private static void RunPublisher<T>(string topicName) where T : Order, new()
        {
            // Creating or getting the topic if it already exists
            ITopic topic = GetOrCreateTopic(topicName);

            // Publishing set amount of messages on the topic
            for (int i = 0; i < MESSAGES_TO_PUBLISH; i++)
            {
                // Generating sample order and wrapping it in a message
                T order = Order.GenerateOrder<T>();
                Message message = new Message(order, TimeSpan.FromSeconds(15));

                try
                {
                    // Publishing message synchronously
                    topic.Publish(message, DeliveryOption.All, notifyDeliveryFailure: true);
                }
                catch
                {
                    // Handling topic deletion or unavailability
                    Console.WriteLine($"{topicName} no longer publishable (topic may have been deleted).");

                    // Exiting the publishing loop
                    break;
                }

                // Printing published message on console
                Console.WriteLine($"Message for {typeof(T).Name} OrderId: {order.OrderID} generated");

                // Waiting for 5 seconds before publishing the next message
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to retrieve an existing topic if available, or create a new one if not
        /// </summary>
        /// <param name="topicName">Name of the topic</param>
        /// <returns>Instance of ITopic</returns>
        private static ITopic GetOrCreateTopic(string topicName)
        {
            try
            {
                // Retrieving the topic from cache
                ITopic topic = cache.MessagingService.GetTopic(topicName);

                // Checking if topic does not exist
                if (topic == null)
                {
                    // Creating a new topic
                    topic = cache.MessagingService.CreateTopic(topicName);
                }

                // Returning the topic instance
                return topic;
            }
            catch (Exception ex)
            {
                // Printing output error on console
                Console.WriteLine($"\nError creating subscriber for topic '{topicName}': {ex.Message}");

                // Returning null in case of error
                return null;
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to delete a topic from the cache
        /// </summary>
        /// <param name="topicName">Name of the topic to delete</param>
        private static void DeleteTopic(string topicName)
        {
            // Validating topic name
            if (string.IsNullOrEmpty(topicName))
            {
                Console.WriteLine("Topic name cannot be null or empty.");
            }
            else
            {
                cache.MessagingService.DeleteTopic(topicName);
            }
        }


        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to generate a small batch of messages for bulk publishing
        /// </summary>
        /// <typeparam name="T">Type of order object contained in the messages</typeparam>
        /// <param name="order">Order object to wrap in each message</param>
        /// <returns>List of message and delivery option tuples for bulk publishing</returns>
        private static List<Tuple<Message, DeliveryOption>> GenerateMessages<T>(T order)
        {
            List<Tuple<Message, DeliveryOption>> messages = new List<Tuple<Message, DeliveryOption>>();

            for (int i = 0; i < 5; i++)
            {
                Message message = new Message(order, TimeSpan.FromSeconds(15));
                messages.Add(new Tuple<Message, DeliveryOption>(message, DeliveryOption.Any));
            }

            return messages;
        }

    }
}
