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

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheItemVersion;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.InputStream;
import java.util.Properties;
import java.util.concurrent.Future;

/*
 * ===============================================================================
 * NCache allows you to perform asynchronous operations on the cache using async
 * APIs. Following are the asynchronous operations demonstrated in this sample:
 *   1. Connecting to a running cache
 *   2. Adding an object to cache asynchronously
 *   3. Updating an existing object in cache asynchronously
 *   4. Deleting an object from cache asynchronously
 * ===============================================================================
 */

public class AsynchronousOperations {

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

        // Creating a simple customer object and key for this sample
        Customer customer = createNewCustomer();
        String key = getKey(customer);

        // Adding customer in cache asynchronously
        addToCacheAsync(key, customer);

        // Modifying the customer and updating in cache
        updateInCacheAsync(key, customer);

        // Removing the existing customer asynchronously
        removeFromCacheAsync(key);

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
     * Method to add customer to cache using async API
     *
     * @param key Key of customer to be added in cache
     * @param customer Instance of Customer that will be added in cache
     */
    private static void addToCacheAsync(String key, Customer customer) {
        try {
            // Adding customer to cache asynchronously
            Future<CacheItemVersion> future = _cache.addAsync(key, new CacheItem(customer));

            // Blocking until addAsync operation completes
            CacheItemVersion version = future.get();

            // Invoking callback method on completion of add operation
            onItemAdded(key);
        } catch (Exception ex) {
            System.out.printf("Failed to add key '%s' in cache due to %s.%n", key, ex);
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to update customer in cache using async API
     *
     * @param key Key of customer to be updated in cache
     * @param customer Instance of Customer that will be updated in the cache
     */
    private static void updateInCacheAsync(String key, Customer customer) {
        try {
            // Changing the customer detail
            customer.setCompanyName("Gourmet Lanchonettes");

            // Updating customer in cache asynchronously
            Future<CacheItemVersion> future = _cache.insertAsync(key, new CacheItem(customer));

            // Blocking until insertAsync operation completes
            future.get();

            // Invoking callback method on completion of update operation
            onItemUpdate(key);
        } catch (Exception ex) {
            System.out.printf("Failed to update key '%s' in cache due to %s.%n", key, ex);
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to delete customer from the cache using async API
     *
     * @param key Key of customer to be removed from cache
     */
    private static void removeFromCacheAsync(String key) {
        try {
            // Removing customer from cache asynchronously
            Future<Customer> future = _cache.removeAsync(key, Customer.class);

            // Blocking until removeAsync operation completes
            future.get();

            // Invoking callback method on completion of remove operation
            onItemRemoved(key);
        } catch (Exception ex) {
            System.out.printf("Failed to remove key '%s' in cache due to %s.%n", key, ex);
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Callback method executed each time an AddAsync operation completes
     *
     * @param key Key of the item added
     */
    private static void onItemAdded(String key) throws CacheException {
        // Printing message on console
        System.out.println("\nObject is added to cache.");

        // Retrieving the added customer from cache
        Customer cachedCustomer = _cache.get(key, Customer.class);

        // Printing details of added customer
        printCustomerDetails(cachedCustomer);
    }

    // ------------------------------------------------------------------------------
    /**
     * Callback method executed each time InsertAsync operation completes
     *
     * @param key Key of the item added
     */
    private static void onItemUpdate(String key) throws CacheException {
        // Printing message on console
        System.out.println("\nObject is updated to cache.");

        // Retrieving the updated customer from cache
        Customer cachedCustomer = _cache.get(key, Customer.class);

        // Printing details of updated customer
        printCustomerDetails(cachedCustomer);
    }

    // ------------------------------------------------------------------------------
    /**
     * Callback method executed each time RemoveAsync operation completes
     *
     * @param key Key of the item added
     */
    private static void onItemRemoved(String key) {
        // Printing message on console
        System.out.println("\nObject with key  is removed from cache.");
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to generate an instance of Customer to be used in this sample
     *
     * @return Instance of Customer
     */
    private static Customer createNewCustomer() {
        // Creating a new customer object
        Customer customer = new Customer();

        // Setting sample values for customer attributes
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
     * @param customer  Instance of Customer to generate a key
     * @return Key for the customer object
     */
    private static String getKey(Customer customer) {
        // Generating key using CustomerID attribute
        return String.format("Customer:%s", customer.getCustomerID());
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to print details of customer object
     *
     * @param customer Customer instance whose attributes are to be printed
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
            InputStream stream = AsynchronousOperations.class
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
