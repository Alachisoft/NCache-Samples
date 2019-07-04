// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Configuration;
using Alachisoft.NCache.Runtime.Events;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Web.Caching;

namespace Alachisoft.NCache.Samples
{
    public class Events
    {
        private static Cache _cache;
        private static CacheEventDescriptor _eventDescriptor;

        public static void Run()
        {
            // Initialize cache
            InitializeCache();

            // Event notifications must be enabled in NCache manager -> Options for events to work
            RegisterCacheNotification();

            // Generate a new instance of product
            string key = "Item:1";
            Product product1 = GenerateProduct(1);

            // Add item in cache
            AddItem(key, product1);

            // Key based notifications allows item notifications for particular items in 
            CacheDataNotificationCallback notificationCallback = new CacheDataNotificationCallback(KeyNotificationMethod);
            _cache.RegisterCacheNotification(key, notificationCallback, EventType.ItemAdded | EventType.ItemRemoved | EventType.ItemUpdated,
                                            EventDataFilter.Metadata);
            // Update item to trigger key based notification.
            UpdateItem(key, product1);


            // Delete item to trigger key based notification.
            DeleteItem(key);

            UnRegisterCacheNotification();

            // Custom Events need to be registered
            RegisterCustomEvent();

            // Generate a new instance of product
            Product product2 = GenerateProduct(2);

            // Raise the custom event, it is triggered with all who have Registered for Custom events 
            RaiseCustomEvent(key, product2);

            // Dispose cache once done
            _cache.Dispose();
        }

        /// <summary>
        /// This method initializes the cache.
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheId"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The Cache Name cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Web.Caching.NCache.InitializeCache(cache);

            Console.WriteLine("Cache initialized successfully");
        }

        /// <summary>
        /// Register cache notification for item added, removed and updated.
        /// </summary>
        private static void RegisterCacheNotification()
        {
            _eventDescriptor = _cache.RegisterCacheNotification(new CacheDataNotificationCallback(CacheDataModified),
                                                   EventType.ItemAdded | EventType.ItemRemoved | EventType.ItemUpdated,
                                                   EventDataFilter.Metadata);

            Console.WriteLine("Cache Notification " + ((_eventDescriptor.IsRegistered) ? "Registered successfully" : "Not Registered"));
        }

        /// <summary>
        /// Unregister cache notification
        /// </summary>
        private static void UnRegisterCacheNotification()
        {
            _cache.UnRegisterCacheNotification(_eventDescriptor);
            Console.WriteLine("Cache Notification " + "Unregistered successfully");
        }

        /// <summary>
        /// This method adds the specified item in cache.
        /// </summary>
        /// <param name="key"> key against which item will be added. </param>
        /// <param name="product"> Item that will be added in the cache. </param>
        private static void AddItem(string key, Product product)
        {
            _cache.Add(key, product);
            Console.WriteLine("Object Added in Cache");
        }

        /// <summary>
        /// This method updates the specified item in the cache.
        /// </summary>
        /// <param name="key"> key against which item will be updated. </param>
        /// <param name="product"> Item that will be updated in the cache. </param>
        private static void UpdateItem(string key, Product product)
        {
            product.Name = "updatedProduct1";
            _cache.Insert(key, product);
            Console.WriteLine("Object Updated in Cache");
        }

        /// <summary>
        /// This method deletes the specified item in the cache.
        /// </summary>
        /// <param name="key"> key against which item will be deleted. </param>
        private static void DeleteItem(string key)
        {
            _cache.Delete(key);
            Console.WriteLine("Object Removed from Cache");
        }

        /// <summary>
        /// This method registers a custom event in NCache.
        /// </summary>
        private static void RegisterCustomEvent()
        {
            _cache.CustomEvent += new CustomEventCallback(CacheCustomEvent);
        }

        /// <summary>
        /// This method raises the custom event in NCache against the specified key.
        /// </summary>
        /// <param name="key"> key against which event will be reaised. </param>
        /// <param name="customEventProduct"> Item against which event will be raised. </param>
        private static void RaiseCustomEvent(string key, Product customEventProduct)
        {
            _cache.RaiseCustomEvent(key, customEventProduct);
        }

        /// <summary>
        /// This method will be used as a callback for cache data notification.
        /// </summary>
        /// <param name="key"> key against which callback was fired. </param>
        /// <param name="cacheEventArgs"> Event arguments. </param>
        public static void CacheDataModified(string key, CacheEventArg cacheEventArgs)
        {
            Console.WriteLine("Cache data modification notification for the the item of the key : {0}", key); //To change body of generated methods, choose Tools | Templates.
        }

        /// <summary>
        /// This method will be used as a callback for cache data notification.
        /// </summary>
        /// <param name="key"> key against which callback was fired. </param>
        /// <param name="cacheEventArgs"> Event arguments. </param>
        private static void KeyNotificationMethod(string key, CacheEventArg cacheEventArgs)
        {
            switch (cacheEventArgs.EventType)
            {
                case EventType.ItemAdded:
                    Console.WriteLine("Key: " + key + " is added to the cache");
                    break;
                case EventType.ItemRemoved:
                    Console.WriteLine("Key: " + key + " is removed from the cache");
                    break;
                case EventType.ItemUpdated:
                    Console.WriteLine("Key: " + key + " is updated in the cache");
                    break;
            }
        }

        /// <summary>
        /// This method will be used as a callback for custom events.
        /// </summary>
        /// <param name="notifId"> The key specified while raising the event will be received as notifId. </param>
        /// <param name="data"> The data specified while raising the event will be received sa data. </param>
        private static void CacheCustomEvent(object notifId, object data)
        {
            Console.WriteLine("The custom event has been raised");
        }

        /// <summary>
        /// This method generates a new instance of product class with some data.
        /// </summary>
        /// <param name="id"> This id will be set as productId</param>
        /// <returns> Returns a populated instance of a product. </returns>
        private static Product GenerateProduct(int id)
        {
            Product product = new Product();
            switch (id)
            {
                case 1:
                    product.Id = id;
                    product.Name = "product1";
                    product.ClassName = "class1";
                    product.Category = "category1";
                    break;
                case 2:
                    product.Id = id;
                    product.Name = "custom event product1";
                    product.ClassName = "custom class";
                    product.Category = "custom category";
                    break;
                default:
                    product.Id = id;
                    product.Name = "product" + id;
                    product.ClassName = "class" + id;
                    product.Category = "category" + id;
                    break;
            }
            return product;
        }
    }
}