using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Runtime.Exceptions;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    class DurableSubscriber
    {
        private static  ICache _cache;

        public static void Run()
        {
            try
            {
                InitializeCache();

                IDurableTopicSubscription electronicsSharedSubs = RunSubscriber("ElectronicsOrders", "ElectronicsSubscription", SubscriptionPolicy.Shared);
                IDurableTopicSubscription garmentsExclusiveSubs = RunSubscriber("GarmentsOrders", "GarmentsSubscription", SubscriptionPolicy.Exclusive);

                //Following patterns can be used;
                //* -	 represents zero or more characters. For example, ‘bl*’ finds TOPICs ‘black’, ‘blue’, and ‘blob’ etc.
                //? -	 represents one character. For example, ‘h?t’ finds ‘hot’, ‘hat’, and ‘hit’ etc.
                //[]-	 represents any single character within the brackets. For example, ‘h[oa]t’ finds hot and hat, but not hit.

                IDurableTopicSubscription allOrdersSubscription = RunPatternBasedSubscriber("*Orders", "AllOrdersSubscription");

                Console.WriteLine("Press any key to stop all subscribers...");
                Console.ReadLine();
                electronicsSharedSubs.UnSubscribe();
                garmentsExclusiveSubs.UnSubscribe();
                allOrdersSubscription.UnSubscribe();

            }
            catch (CacheException ex)
            {
                if (ex.ErrorCode == NCacheErrorCodes.SUBSCRIPTION_EXISTS)
                {
                    Console.WriteLine("Active Subscription with this name already exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IDurableTopicSubscription RunSubscriber(string topicName,string subscriptionName, SubscriptionPolicy policy)
        {
            ITopic topic = _cache.MessagingService.GetTopic(topicName);

            if (topic == null)
                topic = _cache.MessagingService.CreateTopic(topicName);

            //Creates durable subscription 
            //subscriptionName: User defined name for subscription
            //Subscription Policy: If subscription policy is shared , it means subscription can hold multiple active subscribers 
            //                     If subscription policy is exclusive , it means subscription can hold one active client only.
            return topic.CreateDurableSubscription(subscriptionName, policy, MessageReceivedCallback, new TimeSpan(1, 0, 0));
        }

        public static void InitializeCache()
        {
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

        public static IDurableTopicSubscription RunPatternBasedSubscriber(string topicPattern,string subscriptionName)
        {
            ITopic topic = _cache.MessagingService.GetTopic(topicPattern, Alachisoft.NCache.Runtime.Caching.Messaging.TopicSearchOptions.ByPattern);

            //Create durable subscriptions with an expiry of 1 hour on all the topics matching the specified pattern
            if (topic != null)
                return topic.CreateDurableSubscription(subscriptionName, SubscriptionPolicy.Shared, MessageReceivedCallbackPatternBased, new TimeSpan(1, 0, 0));

            return null;
        }

        /// <summary>
        /// This method will get invoked if a message is recieved by the subscriber.
        /// </summary>
        static void MessageReceivedCallback(object sender, MessageEventArgs args)
        {
            Console.WriteLine("Message Recieved for " + args.TopicName);
           
        }
        /// <summary>
        /// This method will get invoked if a message is recieved by the subscriber.
        /// </summary>
        static void MessageReceivedCallbackPatternBased(object sender, MessageEventArgs args)
        {
            Console.WriteLine("Message Recieved on Pattern Based subscription for " + args.TopicName);
            //Console.WriteLine(args.Message.Payload);
        }
    }
}
