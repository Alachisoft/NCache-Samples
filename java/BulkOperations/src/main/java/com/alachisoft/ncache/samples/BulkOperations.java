/*
 * ==============================================================================
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

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.InputStream;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

/*
 * ===============================================================================
 * NCache allows you to perform bulk operations on the cache using bulk APIs. The
 * following bulk operations are performed using bulk APIs in this sample:
 *   1. Adding multiple items to cache in bulk
 *   2. Updating multiple items in cache in bulk
 *   3. Getting multiple items from cache in bulk
 *   4. Removing multiple items from cache in bulk
 * ===============================================================================
 */

public class BulkOperations {

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

        // Generating multiple customers and their keys that will be used in this sample
        Customer[] customers = createNewCustomers();
        String[] custKeys = getKeys(customers);

        // Generating hashmap of the keys and products
        Map<String, CacheItem> customerItems = getCacheItemDictionary(custKeys, customers);

        // Adding multiple items in cache using bulk API
        addMultipleObjectsToCache(customerItems);

        // Updating multiple items in cache using bulk API
        updateMultipleObjectsInCache(custKeys, customerItems);

        // Getting multiple items from cache
        getMultipleObjectsFromCache(custKeys);

        // Removing the existing objects using bulk API
        removeMultipleObjectsFromCache(custKeys);

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
     * Method to create an array of customers to be used in this sample
     *
     * @return Array of Customers
     */
    private static Customer[] createNewCustomers() {
        // Creating array for customers
        Customer[] customers = new Customer[4];

        // Creating and populating four customer objects and adding them to customers array
        Customer c1 = new Customer();
        c1.setCustomerID("C001");
        c1.setContactName("John");
        c1.setCompanyName("MNO Solutions");
        customers[0] = c1;

        Customer c2 = new Customer();
        c2.setCustomerID("C002");
        c2.setContactName("Sam");
        c2.setCompanyName("IJK Digital");
        customers[1] = c2;

        Customer c3 = new Customer();
        c3.setCustomerID("C003");
        c3.setContactName("Pamela");
        c3.setCompanyName("XYZ Global");
        customers[2] = c3;

        Customer c4 = new Customer();
        c4.setCustomerID("C004");
        c4.setContactName("Steve");
        c4.setCompanyName("DEF Tech");
        customers[3] = c4;

        // Returning customers array
        return customers;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to generate list of keys for provided customer array
     *
     * @param customers Array of customers
     * @return Array of keys
     */
    private static String[] getKeys(Customer[] customers) {
        // Creating array for keys
        String[] keys = new String[customers.length];

        // looping through customers array to generate keys
        for (int i = 0; i < customers.length; i++) {
            // Generating key for each customer
            keys[i] = String.format("Customer:%s", customers[i].getCustomerID());
        }

        // Returning keys array
        return keys;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to generate hashmap of cache items and keys
     *
     * @param keys Array of keys to generate hashmap
     * @param customers Array of customers to generate hashmap
     * @return Map of Cache Items and Key
     */
    private static Map<String, CacheItem> getCacheItemDictionary(String[] keys, Customer[] customers) {
        // Creating hashmap for cache items
        Map<String, CacheItem> cacheItems = new HashMap<>();

        // Populating cache items hashmap
        for (int i = 0; i < customers.length; i++) {
            CacheItem item = new CacheItem(customers[i]);
            cacheItems.put(keys[i], item);
        }

        // Returning cache items hashmap
        return cacheItems;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to add multiple objects in the cache using bulk API
     *
     * @param items Map of keys and items to add
     */
    private static void addMultipleObjectsToCache(Map<String, CacheItem> items) throws CacheException {
        // Adding multiple items in cache using bulk API
        Map<String, Exception> result = _cache.addBulk(items);

        // Checking result for any failures
        if (result.isEmpty()) {
            System.out.println("All items are successfully added to cache.\n");
        } else {
            System.out.println("Some items failed to add:");
            result.forEach((k, v) -> System.out.println("Key: " + k + ", Error: " + v.getMessage()));
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to update multiple objects in the cache using bulk API
     *
     * @param keys Array of keys
     * @param items Map of keys and items to update
     */
    private static void updateMultipleObjectsInCache(String[] keys, Map<String, CacheItem> items) throws CacheException {
        // Modifying company names of two customers before updating
        ((Customer) items.get(keys[0]).getValue(Customer.class)).setCompanyName("ABC Corp");
        ((Customer) items.get(keys[1]).getValue(Customer.class)).setCompanyName("QRS Enterprises");

        // Updating multiple items in cache using bulk API
        Map<String, Exception> result = _cache.insertBulk(items);

        // Checking result for any failures
        if (result.isEmpty()) {
            System.out.println("All items are successfully updated in cache.\n");
        } else {
            System.out.println(result.size() + " item(s) failed to update:");
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to read multiple objects from the cache using bulk API
     *
     * @param keys Array of string keys to fetch data from cache
     */
    private static void getMultipleObjectsFromCache(String[] keys) throws CacheException {
        // Fetching multiple items from cache using bulk API
        Map<String, Customer> retrievedItems = _cache.getBulk(Arrays.asList(keys), Customer.class);

        // Printing output on console
        System.out.println("Fetched bulk items from cache.");

        // looping through retrieved items
        for (Map.Entry<String, Customer> entry : retrievedItems.entrySet()) {
            // Getting current customer
            Customer c = entry.getValue();

            // Printing customer details on console
            System.out.printf("Key: %s, ID: %s, Company Name: %s%n", entry.getKey(), c.getCustomerID(), c.getCompanyName());
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to remove multiple objects from the cache using bulk API
     *
     * @param keys Array of string keys to be removed from cache
     */
    private static void removeMultipleObjectsFromCache(String[] keys) throws CacheException {
        // Removing multiple items from cache using bulk API
        _cache.removeBulk(Arrays.asList(keys), Customer.class);

        // Printing output on console
        System.out.println("\nItems deleted from cache.");
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to read configuration value from config.properties file
     */
    private static String getConfigValue(String property) {
        try {
            // Loading config.properties file from resources folder into stream
            InputStream stream = BulkOperations.class
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
