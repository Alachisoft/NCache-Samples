/*
 * ===============================================================================
 * Alachisoft (R) NCache Sample Code.
 * NCache Basic Operations sample
 * ===============================================================================
 * Copyright © Alachisoft.  All rights reserved.
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
 * OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE.
 * ===============================================================================
 */

package com.alachisoft.ncache.sample;

/*
 * ===============================================================================
 * NCache Events are used to get notifications about changes happening in the
 * cache. Events can be Cache-Level, which notify about changes happening to any
 * item in the cache, or Item-Level, which notify about changes happening to a
 * specific cached item identified by its key.
 * Some possible use cases of NCache events include:
 *   1. Keeping external systems in sync with cache changes.
 *   2. Implementing reactive programming models where actions are triggered
 *      based on cache events.
 *   3. Monitoring cache usage patterns and item lifecycle for analytics.
 *   4. Implementing custom cache eviction or refresh strategies based on item
 *      access patterns.
 *   5. Building real-time applications that respond to data changes instantly.
 * NOTE: Event notifications must be enabled in NCache Manager for events to work.
 * ===============================================================================
 */

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheDataModificationListener;
import com.alachisoft.ncache.client.CacheEventDescriptor;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.events.EventDataFilter;
import com.alachisoft.ncache.runtime.events.EventType;
import com.alachisoft.ncache.samples.data.Product;

import java.io.IOException;
import java.io.InputStream;
import java.util.EnumSet;
import java.util.Properties;

public class Events
{
    // Cache handle
    private static Cache _cache;

    public static void main( String[] args ) throws Exception {

        // Connect to a running cache and get cache handle for it
            _cache = getCache();

        if (_cache == null) {
            System.out.println("Cache could not be initialized.");
            return;
        }

        // Creating sample product and key for demonstration
        Product product = new Product();
        product.setProductID(1);
        product.setProductName("Laptop");
        product.setCategory("Electronics");
        product.setUnitPrice(500);
        product.setUnitsInStock((short) 10);
        product.setQuantityPerUnit(10+" pcs");
        String key= "Product:" + product.getProductID();

        /*
         * CACHE-LEVEL EVENTS:
         * Events that notify about changes happening to any item in the cache.
         * These events include {@code ItemAdded}, {@code ItemRemoved},
         * and {@code ItemUpdated}.
         */

        // Registering events for all cache item events: add, remove, and update
        CacheEventDescriptor eventDescriptor= RegisterCacheEvents();

        // Adding an item to the cache which will trigger a cache-level event notification for ItemAdded
        _cache.add(key,product);

        // Updating an item in the cache which will trigger a cache-level event notification for ItemUpdated
        product.setUnitsInStock((short) 15);
        _cache.insert(key,product);

        // Removing an item from cache which will trigger a cache-level event notification for ItemRemoved
        _cache.remove(key, Product.class);

        // Pausing for 5 seconds to allow time for cache events to be received
        Thread.sleep(5000);

        // Un-registering cache events to stop receiving further event updates
        UnRegisterCacheEvents(eventDescriptor);

        /*
         * ITEM-LEVEL EVENTS:
         * Events that notify about changes happening to a specific cached item
         * identified by its key. These events include {@code ItemUpdated} and
         * {@code ItemRemoved}.
         *
         * <p>The {@code ItemAdded} event is not applicable for item-level events since
         * the item does not exist in the cache prior to being added.</p>
         */

        // Adding an item to the cache which will trigger item-level event notifications for ItemUpdated and ItemRemoved
        _cache.add(key,product);

        // Registering item-level notifications for a specific key
        RegisterItemEvents(key);

        // Updating item to trigger ItemUpdated notification on this key
        product.setUnitPrice(700);
        _cache.insert(key,product);

        // Pausing for 5 seconds to allow time for notifications to be received
        Thread.sleep(5000);

        // Un-registering ItemUpdated notifications on this key
        UnRegisterItemEvents(key, EventType.ItemUpdated);

        // Removing item to trigger ItemRemoved notification on this key
        _cache.remove(key, Product.class);

        // Pausing for 5 seconds to allow time for notifications to be received
        Thread.sleep(5000);

        // Un-registering ItemRemoved notifications on this key
        UnRegisterItemEvents(key, EventType.ItemRemoved);

        // Closing the cache handle
        _cache.close();


    }

    // ------------------------------------------------------------------------------
    /**
     * Method to connect to a running cache and return cache handle for it
     *
     * @return Cache handle for the connected cache
     */
    private static Cache getCache() throws Exception {
        // Getting cache name from configuration file
        String cacheName = getConfigValue("CacheName");

        // Validating cache name
        if (cacheName == null || cacheName.isEmpty()) {
            System.out.println("The CacheName cannot be null or empty.");
            return null;
        }

        // Trying to connect to the cache
        try {
            // Connecting to a running cache and return a cache handle for it
            Cache cache = CacheManager.getCache(cacheName);

            // Printing output on console
            System.out.printf("Cache '%s' is connected.%n", cacheName);

            // Returning cache handle
            return cache;
        } catch(Exception e) {
            System.out.println("Unable to connect to cache: " + e.getMessage());
            return null;
        }
    }


    // ------------------------------------------------------------------------------
    /**
     * Method to register cache-level events: ItemAdded, ItemUpdated, and ItemRemoved.
     *
     * @return CacheEventDescriptor for the registered cache-level events.
     */
    private static CacheEventDescriptor RegisterCacheEvents() throws Exception {
        // Creating an instance of CacheDataModificationListener to handle event notifications
        CacheDataModificationListener dataModificationListener = new CacheDataModificationListenerImpl();

        // Registering cache-level events
        CacheEventDescriptor eventDescriptor = _cache.getMessagingService().addCacheNotificationListener(
                dataModificationListener,
                EnumSet.of(EventType.ItemAdded, EventType.ItemUpdated, EventType.ItemRemoved), EventDataFilter.None);

        System.out.println("Cache Notifications" + (eventDescriptor.getIsRegistered() ? " " : " not ") + "registered successfully.\n");

        return eventDescriptor;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to register item-level events: ItemUpdated and ItemRemoved.
     *
     * @param key The key of the item for which to register item-level events.
     */
    private static void RegisterItemEvents(String key) throws Exception {
        // Creating an instance of CacheDataModificationListener to handle event notifications
        CacheDataModificationListener dataModificationListener = new CacheDataModificationListenerImpl();

        // Registering item-level events
        _cache.getMessagingService().addCacheNotificationListener(
                key,
                dataModificationListener,
                EnumSet.of(EventType.ItemUpdated, EventType.ItemRemoved), EventDataFilter.DataWithMetadata);

        System.out.println("Event registered for key: " + key+ "\n");
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to un-register cache-level events: ItemAdded, ItemUpdated, and ItemRemoved.
     *
     * @param eventDescriptor The CacheEventDescriptor of the registered cache-level events.
     */
    private static void UnRegisterCacheEvents(CacheEventDescriptor eventDescriptor) throws Exception {
        // Unregistering cache-level events
        _cache.getMessagingService().removeCacheNotificationListener(eventDescriptor);
        System.out.println("Cache-level event unregistered successfully.\n");
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to un-register item-level events: ItemUpdated and ItemRemoved.
     *
     * @param key The key of the item for which to un-register item-level events.
     * @param eventType The type of event to un-register (ItemUpdated or ItemRemoved
     */
    private static void UnRegisterItemEvents(String key, EventType eventType) throws Exception {
        // Unregistering item-level events
        CacheDataModificationListener dataModificationListener = new CacheDataModificationListenerImpl();
        _cache.getMessagingService().removeCacheNotificationListener(key, dataModificationListener, EnumSet.of(eventType));
        System.out.println("Item-level notification for event type '" + eventType + "' unregistered successfully for key: " + key+ "\n");
    }

    // ------------------------------------------------------------------------------
    /**
     * Reads value from config.properties
     */
    private static String getConfigValue(String key) {
        try {
            // Getting input stream to read properties file
            InputStream stream = Events.class
                    .getClassLoader()
                    .getResourceAsStream("config.properties");

            if (stream == null) {
                System.out.println("config.properties not found.");
                return null;
            }

            // Loading properties from the input stream
            Properties props = new Properties();
            props.load(stream);

            // Returning value for the provided key
            return props.getProperty(key);

        } catch (IOException e) {
            return null;
        }
    }
}
