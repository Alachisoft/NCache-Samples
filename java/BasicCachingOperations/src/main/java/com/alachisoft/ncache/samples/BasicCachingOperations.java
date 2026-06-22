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

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.runtime.caching.expiration.Expiration;
import com.alachisoft.ncache.runtime.caching.expiration.ExpirationType;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.*;
import java.util.Properties;

/*
 * ===============================================================================
 * NCache allows you to perform basic caching operations using synchronous APIs.
 * The following synchronous operations are demonstrated in this sample:
 *   1. Connecting to a running cache
 *   2. Adding an object to cache
 *   3. Getting an object from cache
 *   4. Updating an existing object in cache
 *   5. Deleting an object from cache
 * ===============================================================================
 */

public class BasicCachingOperations {

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

        // Creating a customer object and key that will be used in this sample
        Customer customer = createNewCustomer();
        String key = getKey(customer);

        // Adding customer to cache
        addToCache(key, customer);

        // Getting the customer from cache
        customer = getFromCache(key);

        // Modifying the customer and updating in cache
        updateInCache(key, customer);

        // Deleting the existing customer from cache
        removeFromCache(key);

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
     * Method to add object in the cache using synchronous API
     *
     * @param key String key to be added in cache
     * @param customer Instance of Customer that will be added to cache
     */
    private static void addToCache(String key, Customer customer) {
        // Creating time span for setting expiration interval
        TimeSpan expirationInterval = TimeSpan.FromSeconds(60); // 1 minute

        // Creating expiration interval with absolute type to assign to cache item
        Expiration expiration = new Expiration(ExpirationType.Absolute);
        expiration.setExpireAfter(expirationInterval);

        //Populating cache item and adding new expiration
        CacheItem item = new CacheItem(customer);
        item.setExpiration(expiration);

        try {
            // Adding cacheitem to cache with an absolute expiration of 1 minute
            _cache.add(key, item);

            // Print output on console
            System.out.println("\nObject is added to cache.");
        } catch (Exception ex) {
            System.out.println("Error adding item to cache: " + ex.getMessage());
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to get customer from the cache using synchronous api
     *
     * @param key Key of customer
     * @return Instance of Customer retrieved from cache
     */
    private static Customer getFromCache(String key) throws CacheException {
        // Fetching the customer from cache
        Customer cachedCustomer = _cache.get(key, Customer.class);

        // Printing output on console
        System.out.println("\nObject is fetched from cache");

        // Printing customer details on console
        printCustomerDetails(cachedCustomer);

        // Returning the cached customer
        return cachedCustomer;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to modify customer and update it in cache
     *
     * @param key Key of the item
     * @param customer Instance of Customer
     */
    private static void updateInCache(String key, Customer customer) throws CacheException {
        // Changing customer object
        customer.setCompanyName("Gourmet Lanchonettes");

        // Creating expiration time of 30 seconds
        TimeSpan expirationInterval = TimeSpan.FromSeconds(30);

        // Creating expiration interval with sliding type
        Expiration expiration = new Expiration(ExpirationType.Sliding);
        expiration.setExpireAfter(expirationInterval);

        // Creating cache item with required data and adding expiration
        CacheItem item = new CacheItem(customer);
        item.setExpiration(expiration);

        // Updating the item in cache
        _cache.insert(key, item);

        // Printing output on console
        System.out.println("\nObject is updated in cache.");
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to delete the customer from cache
     *
     * @param key Key of customer to be deleted from cache
     */
    private static void removeFromCache(String key) throws CacheException {
        // Removing the existing customer
        _cache.remove(key, Customer.class);

        // Printing output on console
        System.out.println("\nObject is removed from cache.");
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to generate instance of Customer to be used in this sample
     *
     * @return Instance of Customer
     */
    private static Customer createNewCustomer() {
        // Creating new customer object
        Customer customer = new Customer();

        // Populating customer object with sample data
        customer.setCustomerID("DAVJO");
        customer.setContactName("David Johnes");
        customer.setCompanyName("Lonesome Pine Restaurant");
        customer.setContactNo("12345-6789");
        customer.setAddress("Silicon Valley, Santa Clara, California");

        // Returning customer object
        return customer;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to generate a string key for specified customer
     *
     * @param customer Instance of Customer
     * @return Key for the specified customer
     */
    private static String getKey(Customer customer) {
        // Generating key using customer ID
        return String.format("Customer:%s", customer.getCustomerID());
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to print the details of customer object on the console
     *
     * @param customer Instance of Customer
     */
    private static void printCustomerDetails(Customer customer) {
        // Validating customer object
        if (customer == null) return;

        // Printing customer details on console
        System.out.println();
        System.out.println("Customer Details are as follows: ");
        System.out.println("ContactName: " + customer.getContactName());
        System.out.println("CompanyName: " + customer.getCompanyName());
        System.out.println("Contact No.: " + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to read configuration value from config.properties file
     */
    private static String getConfigValue(String property) {
        try {
            // Loading config.properties file from resources folder into stream
            InputStream stream = BasicCachingOperations.class
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