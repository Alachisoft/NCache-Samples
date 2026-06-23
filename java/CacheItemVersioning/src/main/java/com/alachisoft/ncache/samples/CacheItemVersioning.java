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

/*
 * ===============================================================================
 * Cache item versioning allows you to track changes to cache items over time by
 * associating a version number with each item. This version number is incremented
 * each time the item is updated in the cache. Some use cases of this include:
 *   1. Concurrency Control: Ensuring that updates to cache items are based on
 *      the most recent version, preventing lost updates.
 *   2. Change Tracking: Keeping track of changes made to cache items over time.
 *   3. Data Synchronization: Synchronizing cache items with external data sources
 *      by comparing version numbers.
 *   4. Conflict Resolution: Resolving conflicts when multiple clients attempt to
 *      update the same cache item simultaneously.
 *   5. Auditing: Maintaining a history of changes made to cache items for
 *      auditing purposes.
 * ===============================================================================
 */

public class CacheItemVersioning {

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

        // Creating instance of Customer and key to use in this sample
        Customer customer = createNewCustomer();
        String key = getKey(customer);

        // Adding item in cache and fetching the cache item version
        CacheItemVersion itemVersion = addToCache(key, customer);

        // Updating item in cache if cache item version matches and getting latest cache item version
        CacheItemVersion latestVersion = updateInCache(key, customer, itemVersion);

        // Fetching item from cache using old version.
        getItemWithVersion(key, itemVersion);

        // Creating a copy of the latest version
        CacheItemVersion latestVersionCopy = new CacheItemVersion(latestVersion.getVersion());

        // Fetching item from cache using latest version.
        getItemWithVersion(key, latestVersionCopy);

        // Removing item from cache using latest version
        removeWithVersion(key, latestVersion);

        // Closing the cache handle
        _cache.close();
    }

    // --------------------------------------------------------------------------
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

    // --------------------------------------------------------------------------
    /**
     * Method to generate instance of Customer to be used in this sample
     *
     * @return Instance of Customer
     */
    private static Customer createNewCustomer() {
        // Creating an instance of Customer
        Customer customer = new Customer();

        // Setting customer details
        customer.setCustomerID("SCOPR");
        customer.setContactName("Scott Prince");
        customer.setCompanyName("Lonesome Pine Restaurant");
        customer.setContactNo("25632-5646");
        customer.setAddress("95-A Barnes Road Wallingford, CT");

        // Returning customer instance
        return customer;
    }

    // --------------------------------------------------------------------------
    /**
     * Method to generate a string key for specified customer
     *
     * @param customer Instance of Customer to generate a key
     * @return Key for the specified customer
     */
    private static String getKey(Customer customer) {
        // Generating and returning key for specified customer
        return String.format("Customer:%s", customer.getCustomerID());
    }

    // --------------------------------------------------------------------------
    /**
     * Method to add object in the cache and return cache item version
     *
     * @param key Key to be added in cache
     * @param customer Instance of Customer that will be inserted in the cache
     * @return Cache item version
     */
    private static CacheItemVersion addToCache(String key, Customer customer) throws CacheException {
        // Adding item in cache
        CacheItemVersion cacheItemVersion = _cache.add(key, customer);

        // Printing output on console
        System.out.println("Item is added to cache.");

        // Returning cache item version
        return cacheItemVersion;
    }

    // --------------------------------------------------------------------------
    /**
     * Method to update object in the cache if cache item version matches
     *
     * @param key Key to be updated in cache
     * @param customer Instance of Customer that will be updated in the cache
     * @param currentVersion Instance of CacheItemVersion to verify item is updated in cache
     * @return Updated cache item version
     */
    private static CacheItemVersion updateInCache(String key, Customer customer, CacheItemVersion currentVersion) throws CacheException {
        // Changing the customer details
        customer.setCompanyName("Gourmet Lanchonettes");

        // Creating a CacheItem from customer instance and assigning current Item Version
        CacheItem cacheItem = new CacheItem(customer);
        cacheItem.setCacheItemVersion(currentVersion);

        // Updating item in cache if current version matches with the version in cache
        CacheItemVersion newVersion = _cache.insert(key, cacheItem);

        // Checking if item version has changed
        if (currentVersion.getVersion() != newVersion.getVersion()) {
            // Printing output on console
            System.out.println("Item has changed since last time it was fetched.");
        }

        // Returning updated cache item version
        return newVersion;
    }

    // --------------------------------------------------------------------------
    /**
     * Method to fetch and display object from cache if the provided cache item version matches latest in cache
     *
     * @param key Key of item
     * @param version Instance of CacheItemVersion
     */
    private static void getItemWithVersion(String key, final CacheItemVersion version) throws CacheException {
        // Fetching item from cache if cache item version is newer than provided version
        Customer customer = _cache.getIfNewer(key, version, Customer.class);

        // Checking if item is null
        if (customer == null) {
            // This means item version provided is the latest in cache
            System.out.println("Specified item version is latest.");
        } else {
            // This means item version provided is obsolete and item is fetched from cache
            System.out.println("Specified item version is obsolete. Fetched item from cache:");

            // Printing customer details on console
            printCustomerDetails(customer);
        }
    }

    // --------------------------------------------------------------------------
    /**
     * Method to delete an object in the cache using cache item version
     *
     * @param key String key to be removed from cache
     * @param cacheItemVersion Instance of CacheItemVersion to remove item from cache if item version is same in cache
     */
    private static void removeWithVersion(String key, CacheItemVersion cacheItemVersion) throws CacheException {
        // Removing object from cache if cache item version matches in cache
        _cache.remove(key, null, cacheItemVersion, null, Customer.class);

        // Printing output on console
        System.out.println("Customer is removed from cache.");
    }

    // --------------------------------------------------------------------------
    /**
     * Method to print details of customer type
     *
     * @param customer Customer instance whose attributes are to be printed
     */
    private static void printCustomerDetails(Customer customer) {
        // Printing customer details on console
        System.out.println("Customer Details are as follows: ");
        System.out.println("ContactName: " + customer.getContactName());
        System.out.println("CompanyName: " + customer.getCompanyName());
        System.out.println("Contact No: " + customer.getContactNo());
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
            InputStream stream = CacheItemVersioning.class
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
