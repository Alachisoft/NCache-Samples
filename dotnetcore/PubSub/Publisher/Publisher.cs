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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    class Publisher
    {
        private static ICache _cache = null;
        private ITopic _topic;

        public static void Run()
        {
            try
            {
                Console.WriteLine("Starting 2 Publishers... Publishers will stop after publishing 30 messages each.");

                new Thread(() => RunPublisher<ElectronicsOrder>("ElectronicsOrders")).Start();
                new Thread(() => RunBulkPublisher<GarmentsOrder>("GarmentsOrders")).Start();
                new Thread(() => RunAsyncPublisher<GarmentsOrder>("GarmentsOrders")).Start();
                Console.WriteLine("Publishers Started.");


                Console.WriteLine("Press any key to delete the topics");
                Console.ReadLine();
                //Deletes the topic.
                DeleteTopic("ElectronicsOrders");
                DeleteTopic("GarmentsOrders");

                Console.WriteLine("TOPICs Deleted.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void RunPublisher<T>(string topicName) where T : Order, new()
        {
            InitializeCache();

            var publisher = new Publisher();
            publisher.PublishMessages<T>(topicName);
        }

        public static void RunBulkPublisher<T>(string topicName) where T : Order, new()
        {
            InitializeCache();

            var publisher = new Publisher();
            publisher.PublishBulkMessages<T>(topicName);
        }

        public static void RunAsyncPublisher<T>(string topicName) where T : Order, new()
        {
            InitializeCache();

            var publisher = new Publisher();
            publisher.PublishAsyncMessages<T>(topicName);
        }

        private void PublishMessages<T>(string topicName) where T : Order, new()
        {
            _topic = _cache.MessagingService.GetTopic(topicName);

            if (_topic == null)
                _topic = _cache.MessagingService.CreateTopic(topicName);
            _topic.MessageDeliveryFailure += MessageDeliveryFailure;

            //Generate 30 messages at the interval of 5 seconds.
            for (int i = 0; i < 30; i++)
            {
                T order = Order.GenerateOrder<T>();

                // Publishes the message with expiry.
                Message message = new Message(order, new TimeSpan(0, 0, 15));
                _topic.Publish(message, DeliveryOption.All, true);
                Console.WriteLine($"Message for {typeof(T).Name} OrderId: {order.OrderId} generated ");

                Thread.Sleep(5 * 1000);//Sleep for 5 seconds.
            }
        }

        private void PublishBulkMessages<T>(string topicName) where T : Order, new()
        {
            _topic = _cache.MessagingService.GetTopic(topicName);

            if (_topic == null)
                _topic = _cache.MessagingService.CreateTopic(topicName);
            _topic.MessageDeliveryFailure += MessageDeliveryFailure;

            for (int i = 0; i < 30; i++)
            {
                T order = Order.GenerateOrder<T>();
                var messageList = GenerateMessages(order);

                // publishes the bulk with notifications for delivery failure
                // returns all messages that have encountered an exception
                IDictionary<Message, Exception> messageExceptions = _topic.PublishBulk(messageList, true);

                // if any exceptions are encountered, throw to client
                if (messageExceptions.Count > 0)
                    throw new Exception($"{nameof(_topic.PublishBulk)} has encountered {messageExceptions.Count} exceptions");

                Console.WriteLine($"{messageList.Count} Messages for {typeof(T).Name} generated ");

                Thread.Sleep(5 * 1000);//Sleep for 5 seconds.

            }
        }

        private void PublishAsyncMessages<T>(string topicName) where T : Order, new()
        {
            _topic = _cache.MessagingService.GetTopic(topicName);

            if (_topic == null)
                _topic = _cache.MessagingService.CreateTopic(topicName);
            _topic.MessageDeliveryFailure += MessageDeliveryFailure;

            for (int i = 0; i < 30; i++)
            {
                T order = Order.GenerateOrder<T>();

                Message message = new Message(order, new TimeSpan(0, 0, 15));

                // Publish the order with delivery option set as all
                // and register message delivery failure
                Task task = _topic.PublishAsync(message, DeliveryOption.All, true);

                Console.WriteLine($"Message for {typeof(T).Name} OrderId: {order.OrderId} generated ");

                Thread.Sleep(5 * 1000);//Sleep for 5 seconds.

            }
        }

        public static void InitializeCache()
        {
            if (_cache != null)
                return;

            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }

        /// <summary>
        /// This method deletes the specified 'Topic' in the argumnet.
        /// </summary>
        /// <param name="topicName">Name of the topic to be deleted.</param>
        public static void DeleteTopic(string topicName)
        {
            if (topicName == null) throw new ArgumentNullException("topicName");

            // Deleting the topic.
            _cache.MessagingService.DeleteTopic(topicName);
        }

        /// <summary>
        /// This method will be invoked in the case of any failure in a message's delivery.
        /// </summary>
        public static void MessageDeliveryFailure(object sender, MessageFailedEventArgs args)
        {
            Console.WriteLine("Message not delivered. Reason: {0}", args.MessageFailureReason);
        }

        private List<Tuple<Message, DeliveryOption>> GenerateMessages<T>(T order) where T : Order, new()
        {
            // create dictionary for storing bulk
            List<Tuple<Message, DeliveryOption>> messageList = new List<Tuple<Message, DeliveryOption>>();

            for (int i = 0; i < 5; i++)
            {
                // Adds the message with expiry to bulk
                Message message = new Message(order, new TimeSpan(0, 0, 15));
                messageList.Add(new Tuple<Message, DeliveryOption>(message, DeliveryOption.Any));
            }
            return messageList;
        }
    }
}
