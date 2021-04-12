// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Basic Operations sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.*;
import java.util.Properties;

/**
 Class that provides the functionality of the sample
 */
public class BasicOperations
{
    private static Cache cache;

    public static void run() throws Exception {
            Properties properties=  getProperties();
            // Initialize cache
            String cacheName= properties.getProperty("CacheID");

            initializeCache(cacheName);

            // Create a simple customer object
            Customer customer = createNewCustomer();
            String key = getKey(customer);

            // Adding item synchronously
            addObjectToCache(key, customer);

            // Get the object from cache
            customer = getObjectFromCache(key);

            // Modify the object and update in cache
            updateObjectInCache(key, customer);

            //Count before Removal
            getCount();

            //Get and Remove the existing object from cache
            removeObjectFromCache(key);

            //Count after Removal
            getCount();

            // Dispose the cache once done
            disposeCache();
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = BasicOperations.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }

    /**
     This method initializes the cache
     */
    private static void initializeCache(String cacheName) throws Exception {

        if(cacheName == null){
            System.out.println("The CacheID cannot be null.");
            return;
        }

        if(cacheName.isEmpty()){
            System.out.println("The CacheID cannot be empty.");
            return;
        }

        // Initialize an instance of the cache to begin performing operations:
        cache = CacheManager.getCache(cacheName);

        // Print output on console
        System.out.println();
        System.out.println("Cache initialized succesfully.");
    }

    /**
     This method adds object in the cache using synchronous api

     @param key String key to be added in cache
     @param customer Instance of Customer that will be added to cache
     */
    private static void addObjectToCache(String key, Customer customer) throws CacheException {
        //Populating cache item
        CacheItem item = new CacheItem(customer);

        // Adding cache item to cache
        cache.add(key, item);

        // Print output on console
        System.out.println("\nObject is added to cache.");
    }

    /**
     This method gets an object from the cache using synchronous api

     @param key String key to get object from cache
     @return  returns instance of Customer retrieved from cache
     */
    private static Customer getObjectFromCache(String key) throws CacheException {
        Customer cachedCustomer = cache.get(key, Customer.class);

        // Print output on console
        System.out.println("\nObject is fetched from cache.");

        printCustomerDetails(cachedCustomer);
        return cachedCustomer;
    }

    /**
     This method updates object in the cache using synchronous api

     @param key String key to be updated in cache
     @param customer Instance of Customer that will be updated in the cache
     */
    private static void updateObjectInCache(String key, Customer customer) throws CacheException {
        // Update cache item present in cache
        customer.setCompanyName("Gourmet Lanchonetes");

        CacheItem item = new CacheItem(customer);

        cache.insert(key, item);

        // Print output on console
        System.out.println("Object is updated in cache.");
        System.out.println();
    }

    /**
     Remove an object in the cache using synchronous api

     @param key String key to be deleted from cache
     */
    private static void removeObjectFromCache(String key) throws CacheException {
        //Get and Remove the existing customer
        Customer cachedCustomer = cache.remove(key, Customer.class);

        // Print output on console
        System.out.println("\nObject is removed from cache.");
        System.out.println("The removed object is:");

        printCustomerDetails(cachedCustomer);
    }

    /**
     Generates instance of Customer to be used in this sample

     @return  returns instance of Customer
     */
    private static Customer createNewCustomer()
    {
        Customer tempVar = new Customer();
        tempVar.setCustomerID("DAVJO");
        tempVar.setContactName("David Johnes");
        tempVar.setCompanyName("Lonesome Pine Restaurant");
        tempVar.setContactNo("12345-6789");
        tempVar.setAddress("Silicon Valley, Santa Clara, California");
        return tempVar;
    }

    /**
     Generates a string key for specified customer

     @param customer Instance of Customer to generate a key
     @return  returns a key
     */
    private static String getKey(Customer customer)
    {
        return String.format("Customer:%1$s", customer.getCustomerID());
    }

    /**
     This method prints detials of customer type.
     @param customer instance of customer to print customer details
     */
    private static void printCustomerDetails(Customer customer)
    {
        if (customer == null)
        {
            return;
        }

        System.out.println();
        System.out.println("Customer Details are as follows: ");
        System.out.println("ContactName: " + customer.getContactName());
        System.out.println("CompanyName: " + customer.getCompanyName());
        System.out.println("Contact No: " + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
        System.out.println();
    }

    /**
     This method gets number of items Cache.
     */
    private static void getCount() throws CacheException {
        // Print output on console
        System.out.println("Number of items in Cache: "+ cache.getCount());
    }

    /**
     This method disposes Cache.
     */
    private static void disposeCache() throws Exception {

        cache.close();
        
        // Print output on console
        System.out.println();
        System.out.println("Cache Closed.");
    }
}

