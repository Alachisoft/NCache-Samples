// ===============================================================================
// Alachisoft (R) NCache Sample Code.
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
import com.alachisoft.ncache.runtime.JSON.JsonObject;
import com.alachisoft.ncache.runtime.JSON.JsonValue;
import com.alachisoft.ncache.runtime.caching.expiration.Expiration;
import com.alachisoft.ncache.runtime.caching.expiration.ExpirationType;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.exceptions.OperationFailedException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.*;
import java.util.Properties;

/**
 Class that provides the functionality of the sample
 */
public class BasicOperationsWithJSON
{
    private static Cache _cache;

    /**
     Executing this method will perform all the operations of the sample
     */
    public static void run() throws Exception {
        // Initialize cache
        initializeCache();

        // Create a simple customer object
        Customer customer = createNewCustomer();
        JsonObject jsonObject = populateJSONObjectFromCustomer(customer);

        String key = getKey(customer);

        // Adding item synchronously
        addJsonObjectToCache(key, jsonObject);

        // Get the object from cache
        jsonObject = getJsonObjectFromCache(key);

        // Modify the object and update in cache
        updateJsonObjectInCache(key, jsonObject);

        // Remove the existing object from cache
        deleteJsonObjectFromCache(key);

        // Dispose the cache once done
        _cache.close();
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = BasicOperationsWithJSON.class.getClassLoader().getResourceAsStream(path);
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
    private static void initializeCache() throws Exception {
        Properties properties=  getProperties();
        String cacheName= properties.getProperty("CacheID");

        if (tangible.DotNetToJavaStringHelper.isNullOrEmpty(cacheName))
        {
            System.out.println("The CacheID cannot be null or empty.");
            return;
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);

        // Print output on console
        System.out.println(String.format("\nCache '%1$s' is initialized.", cacheName));
    }

    /**
     This method adds json object in the cache using synchronous api

     @param key String key to be added in cache
     @param jsonObject Instance of JsonObject that will be added to cache
     */
    private static void addJsonObjectToCache(String key, JsonObject jsonObject) throws CacheException {
        TimeSpan expirationInterval = new TimeSpan(0, 1, 0);

        Expiration expiration = new Expiration(ExpirationType.Absolute);
        expiration.setExpireAfter(expirationInterval);

        //Populating cache item
        CacheItem item = new CacheItem(jsonObject);
        item.setExpiration(expiration);

        // Adding cacheitem to cache with an absolute expiration of 1 minute
        _cache.add(key, item);

        // Print output on console
        System.out.println("\nJSON Object is added to cache.");
    }

    /**
     This method gets a json object from the cache using synchronous api

     @param key String key to get Json object from cache
     @return  returns instance of JsonObject retrieved from cache
     */
    private static JsonObject getJsonObjectFromCache(String key) throws CacheException {
        JsonObject cachedJsonObject = _cache.get(key,JsonObject.class);

        // Print output on console
        System.out.println("\nJSON Object is fetched from cache");

        PrintJsonObjectDetails(cachedJsonObject);

        return cachedJsonObject;
    }

    /**
     This method updates json object in the cache using synchronous api

     @param key String key to be updated in cache
     @param jsonObject Instance of JsonObject that will be updated in the cache
     */
    private static void updateJsonObjectInCache(String key, JsonObject jsonObject) throws CacheException {
        // Update item with a sliding expiration of 30 seconds
        jsonObject.setItem("CompanyName",  new JsonValue("Gourmet Lanchonetes"));

        TimeSpan expirationInterval = new TimeSpan(0, 0, 30);

        Expiration expiration = new Expiration(ExpirationType.Sliding);
        expiration.setExpireAfter( expirationInterval);

        CacheItem item = new CacheItem(jsonObject);
        item.setExpiration(expiration);

        _cache.insert(key, jsonObject);

        // Print output on console
        System.out.println("\nJSON Object is updated in cache.");
    }

    /**
     Remove a json object in the cache using synchronous api

     @param key String key to be deleted from cache
     */
    private static void deleteJsonObjectFromCache(String key) throws CacheException {
        // Remove the existing json object
        _cache.delete(key);

        // Print output on console
        System.out.println("\nJSON Object is deleted from cache.");
    }

    /**
     Generates instance of Customer to be used in this sample

     @return  returns instance of Customer
     */
    private static Customer createNewCustomer()
    {
        Customer tempVar = new Customer();
        tempVar.setCustomerID ("DAVJO");
        tempVar.setContactName ("David Johnes");
        tempVar.setCompanyName ("Lonesome Pine Restaurant");
        tempVar.setContactNo ( "12345-6789");
        tempVar.setAddress ("Silicon Valley, Santa Clara, California");
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

     @param jsonObject Json Object from cache
     */
    private static void PrintJsonObjectDetails(JsonObject jsonObject)
    {
        if (jsonObject == null)
        {
            return;
        }

        JsonValue ContactName = (JsonValue)((jsonObject.getAttributeValue("ContactName") instanceof JsonValue) ? jsonObject.getAttributeValue("ContactName") : null);
        JsonValue CompanyName = (JsonValue)((jsonObject.getAttributeValue("CompanyName") instanceof JsonValue) ? jsonObject.getAttributeValue("CompanyName") : null);
        JsonValue ContactNo = (JsonValue)((jsonObject.getAttributeValue("ContactNo") instanceof JsonValue) ? jsonObject.getAttributeValue("ContactNo") : null);
        JsonValue Address = (JsonValue)((jsonObject.getAttributeValue("Address") instanceof JsonValue) ? jsonObject.getAttributeValue("Address") : null);


        System.out.println();
        System.out.println("Customer Details are as follows: ");
        System.out.println("ContactName: " + ContactName);
        System.out.println("CompanyName: " + CompanyName);
        System.out.println("Contact No: " + ContactNo);
        System.out.println("Address: " + Address);
        System.out.println();
    }

    private static JsonObject populateJSONObjectFromCustomer(Customer customer) throws OperationFailedException {
        JsonObject jsonObject = new JsonObject();

        jsonObject.setItem( "CustomerID", new JsonValue(customer.getCustomerID()));
        jsonObject.setItem( "ContactName", new JsonValue (customer.getContactName()));
        jsonObject.setItem( "CompanyName", new JsonValue (customer.getCompanyName()));
        jsonObject.setItem( "ContactNo", new JsonValue (customer.getContactNo()));
        jsonObject.setItem( "Address", new JsonValue (customer.getAddress()));

        return  jsonObject;
    }
}