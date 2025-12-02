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
using Alachisoft.NCache.Runtime.Events;
using Alachisoft.NCache.Sample.Data;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;
using System.Threading.Tasks;

// ===============================================================================
// NCache Events are used to get notifications about changes happening in the
// cache. Events can be Cache-Level; which notify about changes happening to any
// item in the cache, or Item-Level; which notify about changes happening to a
// specific cached item identified by its key. Some possible use cases of NCache
// events include:
//   1. Keeping external systems in sync with cache changes.        
//   2. Implementing reactive programming models where actions are triggered based
//      on cache events.
//   3. Monitoring cache usage patterns and item lifecycle for analytics.
//   4. Implementing custom cache eviction or refresh strategies based on item
//      access patterns.
//   5. Building real-time applications that respond to data changes instantly.
// NOTE: Event notifications must be enabled in NCache manager for events to work
// ===============================================================================

namespace Events
{
    public class Program
    {
        // Cache handle for the connected cache
        static ICache _cache;

        static void Main(string[] args)
        {
            // Getting cache handle
            _cache = GetCache();

            // Creating sample product and key for demonstration
            Product product = new Product { ProductID = 1, ProductName = "Laptop", Category = "Electronics", UnitPrice = 500, UnitsInStock = 10 };
            string key = $"Product:{product.ProductID}";

            // ===============================================================================
            // CACHE-LEVEL EVENTS: Events that notify about changes happening to any item in
            // the cache. These events include ItemAdded, ItemRemoved, and ItemUpdated.
            // ===============================================================================

            // Registering events for all cache item events: add, remove, and update
            CacheEventDescriptor _eventDescriptor = RegisterCacheEvents();

            // Adding an item to the cache which will trigger a cache-level event notification for ItemAdded
            _cache.Add(key, product);

            // Updating an item in the cache which will trigger a cache-level event notification for ItemUpdated
            product.UnitsInStock = 10;
            _cache.Insert(key, product);

            // Removing an item from cache which will trigger a cache-level event notification for ItemRemoved
            _cache.Remove(key);

            // Pausing for 5 seconds to allow time for cache events to be received
            Task.Delay(5000).Wait();

            // Un-registering cache events to stop receiving further event updates
            UnRegisterCacheEvents(_eventDescriptor);

            // ===============================================================================
            // ITEM-LEVEL EVENTS: Events that notify about changes happening to a specific
            // cached item identified by its key. These events include ItemUpdated and
            // ItemRemoved. The ItemAdded event is not applicable for item-level events since
            // the item does not exist in the cache prior to being added.
            // ===============================================================================

            // Adding an item to the cache which will trigger item-level event notifications for ItemUpdated and ItemRemoved
            _cache.Add(key, product);

            // Registering item-level notifications for a specific key
            RegisterItemEvents(key);

            // Updating item to trigger ItemUpdated notification on this key
            product.UnitPrice = 700;
            _cache.Insert(key, product);

            // Pausing for 5 seconds to allow time for notifications to be received
            Task.Delay(5000).Wait();

            // Un-registering ItemUpdated notifications on this key
            UnRegisterItemEvents(key, EventType.ItemUpdated);

            // Removing item to trigger ItemRemoved notification on this key
            _cache.Remove(key);

            // Pausing for 5 seconds to allow time for notifications to be received
            Task.Delay(5000).Wait();

            // Un-registering ItemRemoved notifications on this key
            UnRegisterItemEvents(key, EventType.ItemRemoved);

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

            // Connecting to a running cache and return a cache handle for it
            _cache = CacheManager.GetCache(cacheName);

            // Printing output on console
            Console.WriteLine(string.Format("\nCache '{0}' is connected.", cacheName));
            return _cache;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to register cache level notifications for items added, removed and updated
        /// </summary>
        /// <returns>Cache event descriptor</returns>
        private static CacheEventDescriptor RegisterCacheEvents()
        {
            // Registering a cache-level notification callback for item add, remove, and update events
            CacheEventDescriptor eventDescriptor = _cache.MessagingService.RegisterCacheNotification(new CacheDataNotificationCallback(CacheDataModified),
                                                   EventType.ItemAdded | EventType.ItemRemoved | EventType.ItemUpdated,
                                                   EventDataFilter.Metadata);

            // Print output on console
            Console.WriteLine("Cache Notification " + ((eventDescriptor.IsRegistered) ? "Registered successfully" : "Not Registered"));

            // Return event descriptor
            return eventDescriptor;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to register notifications for a specific cached item against the specified key
        /// Event will be triggered if item updated or removed
        /// </summary>
        /// <param name="key">Key of the item to register notifications for</param>
        private static void RegisterItemEvents(string key)
        {
            // Creating a callback method to handle item-level notifications for this key
            CacheDataNotificationCallback notificationCallback = new CacheDataNotificationCallback(KeyNotificationMethod);

            // Registering key-specific notification callback for item update and remove events
            _cache.MessagingService.RegisterCacheNotification(key, notificationCallback,
                EventType.ItemRemoved | EventType.ItemUpdated, EventDataFilter.Metadata);
            // Print output on console
            Console.WriteLine("Event Register for Key:{0}", key);
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to unregister cache level notifications
        /// </summary>
        /// <param name="eventDescriptor">CacheEventDescriptor instance on which unregisteration is to be performed</param>
        private static void UnRegisterCacheEvents(CacheEventDescriptor eventDescriptor)
        {
            // Un-registering cache-level notifications using the event descriptor
            _cache.MessagingService.UnRegisterCacheNotification(eventDescriptor);

            // Print output on console
            Console.WriteLine("Cache-level Notification " + "Unregistered successfully");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to unregister item level notifications
        /// </summary>
        /// <param name="key">Item key for which notifications should be unregistered</param>
        /// <param name="eventType">The specific event type to unregister notifications for</param>
        private static void UnRegisterItemEvents(string key, EventType eventType)
        {
            // Un-registering cache-level notifications using the event descriptor
            _cache.MessagingService.UnRegisterCacheNotification(key, new CacheDataNotificationCallback(KeyNotificationMethod), eventType);

            // Print output on console
            Console.WriteLine($"Item-level notification for event '{eventType}' on key '{key}' unregistered successfully.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Callback method for handling cache-level notifications triggered when any item is added,
        /// updated or removed from cache
        /// </summary>
        /// <param name="key">key against which callback was fired</param>
        /// <param name="cacheEventArgs">Event arguments</param>
        private static void CacheDataModified(string key, CacheEventArg cacheEventArgs)
        {
            // Variable to hold event type text
            string eventText = string.Empty;

            // Determine event type
            switch (cacheEventArgs.EventType)
            {
                case EventType.ItemAdded: eventText = "added"; break;
                case EventType.ItemUpdated: eventText = "updated"; break;
                case EventType.ItemRemoved: eventText = "removed"; break;
            }

            // Print output on console
            Console.WriteLine($"Cache level notification: Item with key '{key}' was {eventText} from the cache.");
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Callback method for handling item-level notifications when the item is updated or removed from cache
        /// </summary>
        /// <param name="key">Key against which callback was fired</param>
        /// <param name="cacheEventArgs">Event arguments</param>
        private static void KeyNotificationMethod(string key, CacheEventArg cacheEventArgs)
        {
            switch (cacheEventArgs.EventType)
            {

                case EventType.ItemRemoved:
                    Console.WriteLine("Key: " + key + " is removed from the cache");
                    break;
                case EventType.ItemUpdated:
                    Console.WriteLine("Key: " + key + " is updated in the cache");
                    break;
            }
        }

    }
}
