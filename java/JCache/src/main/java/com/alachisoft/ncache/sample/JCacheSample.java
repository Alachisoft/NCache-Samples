/**
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

import com.alachisoft.ncache.client.CacheDataModificationListener;
import com.alachisoft.ncache.client.CacheEventArg;
import com.alachisoft.ncache.runtime.events.EventDataFilter;
import com.alachisoft.ncache.runtime.events.EventType;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Customer;

import javax.cache.Cache;
import javax.cache.CacheManager;
import javax.cache.Caching;
import javax.cache.configuration.Factory;
import javax.cache.configuration.FactoryBuilder;
import javax.cache.configuration.MutableCacheEntryListenerConfiguration;
import javax.cache.configuration.MutableConfiguration;
import javax.cache.event.*;
import javax.cache.expiry.Duration;
import javax.cache.spi.CachingProvider;
import java.io.IOException;
import java.io.InputStream;
import java.util.*;

/**
 * ===============================================================================
 * NCache allows you to plug NCache into a JCache application and use the same
 * JCache API for distributed solutions. The following operations are demonstrated
 * in this sample:
 *   1. Connecting to a running cache
 *   2. Setting expiration for items in cache
 *   3. Registering cache level events
 *   4. Registering item level events
 *   5. Adding an object to cache
 *   6. Getting an object from cache
 *   7. Updating an existing object in cache
 *   8. Deleting an object from cache
 *   9. Unregistering cache level events
 *   10. Unregistering item level events
 *   11. Adding bulk objects to cache
 *   12. Getting bulk objects from cache
 *   13. Deleting bulk objects from cache
 *   14. Disposing of cache
 * ===============================================================================
 */

public class JCacheSample
{
    private static Cache<String, Customer> _jCache;
    public static void main( String[] args ) throws Exception {
        // Connect to a running cache and get cache handle for it
        _jCache = getJcache();

        if (_jCache == null) {
            System.out.println("JCache could not be initialized.");
            return;
        }

        // Creating a customer object and key that will be used in this sample
        Customer customer = createNewCustomer();
        String key = getKey(customer);

        // Adding customer to cache synchronously
        addToCache(key, customer);

        // Registering cache level events
        registerCacheLevelEvents();

        // Registering item level events for key
        registerItemLevelEvents(key);

        // Getting the customer from cache
        getFromCache(key);

        // Modifying the customer and updating in cache
        updateInCache(key, customer);

        // Deleting the existing customer from cache
        removeFromCache(key);

        // Small delay to receive remaining events
        Thread.sleep(1000);

        // Unregistering cache level events
        unregisterCacheLevelEvents();

        // Unregistering item level events for key
        unregisterItemLevelEvents(key);

        // Creating bulk customers to be added to cache
        Customer[] customers = createBulkCustomers();
        String[] keys = new String[customers.length];
        for (int i = 0; i < customers.length; i++) {
            keys[i] = getKey(customers[i]);
        }

        // Adding bulk customers to cache
        addBulkData(keys, customers);

        // Retrieving bulk customers from cache
        retrieveBulkData(keys);

        // Removing bulk customers from cache
        removeBulkData(keys);

        // Dispose the cache once done
        _jCache.close();
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to connect to a running cache and return a cache handle for it
     *
     * @return javax.cache.Cache handle for the connected cache
     */
    private static Cache<String, Customer> getJcache() {
        // Getting cache name from configuration file
        String cacheName = getConfigValue("CacheName");

        // Validating cache name
        if (cacheName == null || cacheName.isEmpty()) {
            System.out.println("The CacheName cannot be null or empty.");
            return null;
        }

        // Initializing cache manager
        CachingProvider cachingProvider = Caching.getCachingProvider();
        CacheManager cacheManager = cachingProvider.getCacheManager();

        // Setting expiration of all Customers to 5 minutes
        MutableConfiguration<String, Customer> config = new MutableConfiguration<>();
        // javax.cache.expiry.AccessedExpiryPolicy : Items expire based on last access time
        // javax.cache.expiry.CreatedExpiryPolicy : Items expire based on creation time
        // javax.cache.expiry.ModifiedExpiryPolicy : Items expire based on last update time
        // javax.cache.expiry.EternalExpiryPolicy : Items never expire
        // javax.cache.expiry.TouchedExpiryPolicy : Items expire based on creation, last update or access
        config.setExpiryPolicyFactory(FactoryBuilder.factoryOf(new javax.cache.expiry.CreatedExpiryPolicy(Duration.FIVE_MINUTES)));

        try{
            // Getting cache handle
            Cache<String, Customer> cache = cacheManager.createCache(cacheName, config);

            System.out.printf("Cache '%s' is connected.%n", cacheName);

            return cache;
        }catch (Exception e){
            System.out.println("Unable to connect to cache: " + e.getMessage());
            return null;
        }
    }

    /// ------------------------------------------------------------------------------
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

    /// ------------------------------------------------------------------------------
    /**
     * Method to generate a string key for specified customer
     *
     * @param customer Instance of Customer
     * @return Key for the specified customer
     */
    private static String getKey(Customer customer) {
        return String.format("Customer:%s", customer.getCustomerID());
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to add object in the cache
     *
     * @param key String key to be added in cache
     * @param customer Instance of Customer that will be added to cache
     */
    private static void addToCache(String key, Customer customer) {
        // Adding customer to cache
        try {
            _jCache.put(key, customer);
        } catch (Exception ex) {
            System.out.println("Error adding item to cache: " + ex.getMessage());
        }

        // Printing output on console
        System.out.println("\nObject is added to cache.");
    }

    /// ------------------------------------------------------------------------------
    /**
     * Listener to receive item level events
     *
     */
    public static class CacheDataModificationListenerImpl implements
            CacheDataModificationListener {
        @Override
        public void onCacheDataModified(String key, CacheEventArg eventArgs) {
            switch (eventArgs.getEventType())
            {
                case ItemUpdated:
                    System.out.println("An item level event has been received (ItemUpdated).");
                    break;

                case ItemRemoved:
                    System.out.println("An item level event has been received (ItemRemoved).");
                    break;
            }
        }
        @Override
        public void onCacheCleared(String s) {
            System.out.println("Cache cleared: " + s);
        }
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to get customer from the cache
     *
     * @param key Key of customer
     */
    private static void getFromCache(String key) {
        // Fetching the customer from cache
        Customer cachedCustomer = (Customer) _jCache.get(key);
        System.out.println("\nObject is fetched from cache: ");

        // Printing output on console
        printCustomerDetails(cachedCustomer);
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to modify customer and update it in cache
     *
     * @param key Key of the item
     * @param customer Instance of Customer
     */
    private static void updateInCache(String key, Customer customer) {
        // Changing customer object
        customer.setCompanyName("Gourmet Lanchonettes");

        // Updating the item in cache
        _jCache.put(key, customer);

        // Printing output on console
        System.out.println("Object is updated in cache.");
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to delete the customer from cache
     *
     * @param key Key of customer to be deleted from cache
     */
    private static void removeFromCache(String key) {
        // Removing the existing customer
        _jCache.remove(key);

        // Printing output on console
        System.out.println("\nObject is removed from cache.");
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to print the details of customer object on the console
     *
     * @param customer Instance of Customer
     */
    private static void printCustomerDetails(Customer customer) {
        // Validating customer object
        if (customer == null) return;

        // Printing customer details on console
        System.out.println("ContactName: " + customer.getContactName());
        System.out.println("CompanyName: " + customer.getCompanyName());
        System.out.println("Contact No.: " + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
        System.out.println();
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to add multiple customers to cache
     *
     * @param keys Keys of customers to be added to cache
     * @param customers Customer instances to be added to cache
     */
    private static void addBulkData(String[] keys, Customer[] customers) {
        // Initializing and populating hashmap with customers and keys
        HashMap<String, Customer> map = new HashMap<>();
        for (int i = 0; i < customers.length; i++) {
            map.put(keys[i], customers[i]);
        }

        // Bulk adding customers to cache
        System.out.println("\nBulk Data is added to cache.");
        _jCache.putAll(map);
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to create Customer instances in bulk
     *
     * @return Array of customer instances
     */
    private static Customer[] createBulkCustomers() {
        // Customer 1 to be added to cache
        Customer customer1 = new Customer();
        customer1.setCustomerID("C001");
        customer1.setContactName("John");
        customer1.setCompanyName("MNO Solutions");
        customer1.setContactNo("12327-2341");
        customer1.setAddress("123, 125 Grove Ln, Birmingham B17 0QT, United Kingdom");

        // Customer 2 to be added to cache
        Customer customer2 = new Customer();
        customer2.setCustomerID("C002");
        customer2.setContactName("Sam");
        customer2.setCompanyName("IJK Digital");
        customer2.setContactNo("90564-2356");
        customer2.setAddress("127 Pershore Rd, Birmingham B5 7NX, United Kingdom");

        // Adding customers to array
        Customer[] items = new Customer[2];
        items[0] = customer1;
        items[1] = customer2;

        // Returning array
        return items;
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to retrieve bulk data from cache
     *
     * @param keys Array of keys to fetch from cache
     */
    private static void retrieveBulkData(String[] keys) {
        // Initializing and populating hashset
        HashSet<String> set = new HashSet<>(Arrays.asList(keys));

        // Retrieving and printing Customers in cache
        Map<String, Customer> items = _jCache.getAll(set);
        if (!items.isEmpty())
        {
            System.out.println("\nFetched bulk data from cache:");
            for (Customer customer : items.values()) {
                printCustomerDetails(customer);
            }
        }
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to remove bulk data from cache
     *
     * @param keys Array of keys linked to data to be removed
     */
    private static void removeBulkData(String[] keys) {
        // Initializing and populating hashset
        HashSet<String> set = new HashSet<>(Arrays.asList(keys));

        // Removing items from cache
        _jCache.removeAll(set);
        System.out.println("Bulk Data is removed from cache.");
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to register item level events on specified key
     *
     * @param key Key on which to enable item level events
     */
    private static void registerItemLevelEvents(String key) throws CacheException {
        // Casting jCache to NCache
        com.alachisoft.ncache.client.Cache ncache = _jCache.unwrap(com.alachisoft.ncache.client.Cache.class);

        // Initializing event listener
        CacheDataModificationListenerImpl eventListener = new CacheDataModificationListenerImpl();

        // Adding cache notification listener
        ncache.getMessagingService().addCacheNotificationListener(key, eventListener,
                EnumSet.of(EventType.ItemUpdated, EventType.ItemRemoved), EventDataFilter.DataWithMetadata);
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to unregister item level events on specified key
     *
     * @param key Key on which to disable item level events
     */
    private static void unregisterItemLevelEvents(String key) throws CacheException {
        // Casting jCache to NCache
        com.alachisoft.ncache.client.Cache ncache = _jCache.unwrap(com.alachisoft.ncache.client.Cache.class);

        // Initializing event listener
        CacheDataModificationListenerImpl eventListener = new CacheDataModificationListenerImpl();

        // Removing cache notification listener
        ncache.getMessagingService().removeCacheNotificationListener(key, eventListener,
                EnumSet.of(EventType.ItemUpdated, EventType.ItemRemoved));
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to register cache level events
     * (requires enabling cache level events in Management Centre)
     *
     */
    private static void registerCacheLevelEvents(){
        // Setting for value requirement and synchronicity
        boolean isOldValueRequired = false;
        boolean isSynchronous = false;

        // Initializing event listener
        CacheEntryListenerFactoryImpl eventListener = new CacheEntryListenerFactoryImpl();

        // Making listener config
        MutableCacheEntryListenerConfiguration listenerConfig = new MutableCacheEntryListenerConfiguration(
                eventListener, new CacheEntryEventFilterImpl(), isOldValueRequired, isSynchronous);

        // Registering events
        _jCache.registerCacheEntryListener(listenerConfig);
    }

    /// ------------------------------------------------------------------------------
    /**
     * Method to unregister cache level events
     *
     */
    private static void unregisterCacheLevelEvents(){
        // Setting for value requirement and synchronicity
        boolean isOldValueRequired = false;
        boolean isSynchronous = false;

        // Initializing event listener
        CacheEntryListenerFactoryImpl eventListener = new CacheEntryListenerFactoryImpl();

        // Making listener config
        MutableCacheEntryListenerConfiguration listenerConfig = new MutableCacheEntryListenerConfiguration(
                eventListener, new CacheEntryEventFilterImpl(), isOldValueRequired, isSynchronous);

        // Unregistering events
        _jCache.deregisterCacheEntryListener(listenerConfig);
    }

    /// ------------------------------------------------------------------------------
    /**
     * Listener for cache level events
     *
     */
    public static class CacheEntryListenerFactoryImpl implements CacheEntryCreatedListener,
            CacheEntryUpdatedListener, CacheEntryRemovedListener, Factory<CacheEntryListener> {
        @Override
        public void onCreated(Iterable iterable) throws CacheEntryListenerException {
            System.out.println("A cache level event is received (ItemCreated).");
        }

        @Override
        public void onRemoved(Iterable iterable) throws CacheEntryListenerException {
            System.out.println("A cache level event is received (ItemRemoved).");
        }

        @Override
        public void onUpdated(Iterable iterable) throws CacheEntryListenerException {
            System.out.println("A cache level event is received (ItemUpdated).");
        }

        @Override
        public CacheEntryListener create() {
            return new CacheEntryListenerFactoryImpl();
        }
    }

    /// ------------------------------------------------------------------------------
    /**
     * Event filter for registering cache level events
     *
     */
    public static class CacheEntryEventFilterImpl implements CacheEntryEventFilter,
            Factory<CacheEntryEventFilter> {
        @Override
        public boolean evaluate(CacheEntryEvent event) throws CacheEntryListenerException {
            return true;
        }
        @Override
        public CacheEntryEventFilter create() {
            return new CacheEntryEventFilterImpl();
        }
    }

    /// ------------------------------------------------------------------------------
    /**
     * Reads value from config.properties
     * 
     */
    private static String getConfigValue(String key) {
        try {
            InputStream stream = JCacheSample.class
                    .getClassLoader()
                    .getResourceAsStream("config.properties");

            if (stream == null) {
                System.out.println("config.properties not found.");
                return null;
            }

            Properties props = new Properties();
            props.load(stream);

            return props.getProperty(key);

        } catch (IOException e) {
            return null;
        }
    }
}