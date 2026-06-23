/*
 * ===============================================================================
 * Alachisoft (R) NCache Sample Code.
 * NCache Tags & NamedTags Sample (Java)
 * ===============================================================================
 * Copyright © Alachisoft. All rights reserved.
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
 * OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE.
 * ===============================================================================
 */

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.datastructures.Counter;
import com.alachisoft.ncache.client.datastructures.DataStructureAttributes;
import com.alachisoft.ncache.runtime.caching.expiration.Expiration;
import com.alachisoft.ncache.runtime.caching.expiration.ExpirationType;
import com.alachisoft.ncache.runtime.util.TimeSpan;

import java.io.InputStream;
import java.util.Properties;

/*
 * ===============================================================================
 * Distributed Counter is a powerful data structure provided by NCache for
 * high-concurrency scenarios where multiple clients or application servers need
 * to increment, decrement, or read a shared numeric value atomically and with low
 * latency. Some possible use cases of distributed counters include:
 *   1. Counting page views or clicks on a website
 *   2. Tracking inventory levels in an e-commerce application
 *   3. Implementing rate limiting for APIs or services
 *   4. Managing distributed locks or semaphores
 *   5. Real-time analytics and monitoring
 * Distributed counters are designed to be highly available and fault-tolerant.
 * ===============================================================================
 */

public class DistributedCounter {

    // Cache handle for the connected cache
    private static Cache _cache;

    public static void main(String[] args) throws Exception {
        // Connect to a running cache and get cache handle for it
        _cache = getCache();

        // Validating cache handle
        if (_cache == null) {
            System.out.println("Cache could not be initialized.");
            return;
        }

        // Name of the distributed counter to be used in sample
        String counterName = "DistributedCounter:11001";

        // Creating or getting counter
        Counter counter = getOrCreateCounter(counterName);

        // Setting counter value
        long counterValue = counter.setValue(10);
        System.out.println("Counter value set to " + counterValue);

        // Incrementing counter value
        counterValue = counter.increment();
        System.out.println("Counter value incremented by 1");
        System.out.println("New Counter value is " + counterValue);

        // Decrementing counter value
        counterValue = counter.decrement();
        System.out.println("Counter value decremented by 1");
        System.out.println("New Counter value is " + counterValue);

        // Incrementing counter value by 5
        counterValue = counter.incrementBy(5);
        System.out.println("Counter value incremented by 5");
        System.out.println("New Counter value is " + counterValue);

        // Decrementing counter by value
        counterValue = counter.decrementBy(2);
        System.out.println("Counter value decremented by 2");
        System.out.println("New Counter value is " + counterValue);

        // Removing the distributed counter from cache
        _cache.getDataStructuresManager().remove(counterName);

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
     * Method to retrieve the distributed counter from Cache,
     * if the counter does not exist, it is created and returned
     *
     * @param counterName Name of counter to be used to make counter
     * @return Instance of ICounter
     */
    private static Counter getOrCreateCounter(String counterName) throws Exception {
        // Retrieving the counter from cache
        Counter counter = _cache.getDataStructuresManager().getCounter(counterName);

        // Checking if counter exists or not in cache
        if (counter == null) {
            // Creating counter attributes with expiration for 1 minute
            DataStructureAttributes attributes = new DataStructureAttributes();
            attributes.setExpiration(new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(1)));

            // Setting initial value for the counter
            long initialValue = 0;

            // Creating the counter in cache with specified attributes and initial value
            counter = _cache.getDataStructuresManager().createCounter(counterName, attributes, initialValue, null);
        }

        // Returning counter instance
        return counter;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to read configuration value from config.properties file
     */
    private static String getConfigValue(String property) {
        try {
            // Loading config.properties file from resources folder into stream
            InputStream stream = DistributedCounter.class
                    .getClassLoader()
                    .getResourceAsStream("config.properties");

            // Validating stream
            if (stream == null) {
                System.out.println("config.properties not found.");
                return null;
            }

            // Loading properties from stream
            Properties props = new Properties();
            props.load(stream);

            // Returning value for specified property
            return props.getProperty(property);
        } catch (Exception e) {
            return null;
        }
    }

}

