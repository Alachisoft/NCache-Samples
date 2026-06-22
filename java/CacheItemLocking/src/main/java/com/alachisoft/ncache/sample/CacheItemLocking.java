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
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.LockHandle;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/*
 * ===============================================================================================================
 * Cache Item Locking is a mechanism that prevents multiple clients from modifying
 * the same cache item simultaneously. When an item is locked by a client, other
 * clients are prevented from modifying that item until the lock is released.
 * This ensures data consistency and integrity in a distributed caching environment.
 * Some possible use cases of Cache Item Locking include:
 *   1. Preventing race conditions when multiple clients attempt concurrent updates on the same data.
 *   2. Ensuring data integrity during critical operations.
 *   3. Coordinating access to shared resources in a distributed system.
 *   4. Implementing optimistic concurrency control to avoid lost update.
 *   5. Managing transactions in a distributed cache environment.
 * ===============================================================================================================
 */

public class CacheItemLocking
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

        // Creating a customer object and key that will be used in this sample
        Customer customer = createNewCustomer();
        String key= "Customer:" + customer.getCustomerID();

        // Creating new lock handle and one second timespan for locking cache item
        TimeSpan timeSpan = new TimeSpan(0, 0, 1);
        LockHandle lockHandle = new LockHandle();

        // Adding customer to cache synchronously
        AddInCache(key, customer);

        //Fetching customer from cache and locking it via GET operation
        customer= _cache.get(
                key,
                true,
                timeSpan,
                lockHandle, Customer.class);
        System.out.println("Customer with key"+ " '"+ key + "' fetched from cache and locked via GET operation, lock id:"+ lockHandle.getLockId()+"\n");

        //Changing customer contact no
        customer.setContactNo("98765-4321");
        System.out.println("Customer contact changed");

        // Updating customer in cache and releasing lock via UPDATE operation
        boolean releaseLock = true;
        _cache.insert(key, new CacheItem(customer), null, lockHandle,releaseLock);
        System.out.println("Customer with key"+ " '"+ key + "' updated in cache and lock released via INSERT operation\n");

        //Explicitly locking item in cache via LOCK operation
        boolean isLocked = _cache.lock(key, timeSpan, lockHandle);
        System.out.println("Attempting to acquire lock again on customer with key"+ " '"+ key + "' via explicit LOCK operation\n");
        if (!isLocked) {
            System.out.println("Lock acquired again on customer with key"+ " '"+ key + "' via explicit LOCK operation, lock id:"+ lockHandle.getLockId()+"\n");
        }
        else{
            System.out.println("Unable to acquire lock on customer with key"+ " '"+ key + "' via explicit LOCK operation\n");
        }

        // Releasing the lock on item via UNLOCK operation
        _cache.unlock(key, lockHandle);
        System.out.println("Lock released on customer with key"+ " '"+ key + "' via explicit UNLOCK operation\n");

        // Removing customer from cache for cleanup
        _cache.remove(key, Customer.class);

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
            System.out.println("Unable to connect to cache.");
            return null;
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to generate instance of Customer to be used in this sample
     *
     * @return Instance of Customer
     */
    private static Customer createNewCustomer() {
        Customer customer = new Customer();
        customer.setCustomerID("DAVJO");
        customer.setContactName("David Johnes");
        customer.setCompanyName("Lonesome Pine Restaurant");
        customer.setContactNo("12345-6789");
        customer.setAddress("Silicon Valley, Santa Clara, California");
        return customer;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to add object in the cache using synchronous API
     *
     * @param key String key to be added in cache
     * @param customer Instance of Customer that will be added to cache
     */
    private static void AddInCache(String key, Customer customer) {
        // Adding customer to cache synchronously
        try {
            _cache.add(key, customer);
        } catch (Exception e) {
            System.out.printf("Error adding customer object with key '%s' to cache. %s%n", key, e.getMessage());
            return;
        }

        // Printing output on console
        System.out.printf("Customer object with key '%s' added to cache.%n", key);
    }

    // ------------------------------------------------------------------------------
    /**
     * Reads value from config.properties
     */
    private static String getConfigValue(String key) {
        try {
            // Getting input stream to read properties file
            InputStream stream = CacheItemLocking.class
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
