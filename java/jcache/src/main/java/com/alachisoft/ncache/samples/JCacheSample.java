package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Customer;

import javax.cache.CacheManager;
import javax.cache.Caching;
import javax.cache.spi.CachingProvider;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.util.*;

public class JCacheSample<K,V>
{
    private static javax.cache.Cache jCache;
    public static void main(String[] args)
    {
        try
        {
            //--- Get cache provider ...
            CachingProvider cachingProvider = Caching.getCachingProvider();
            //--- Get cache manager ...
            CacheManager cacheManager = cachingProvider.getCacheManager();

            Properties properties=  getProperties();
            String cacheName= properties.getProperty("CacheID");

            //--- Get cache instance ...
            jCache = cacheManager.getCache(cacheName);

            //--- Perform basic cache operations ...
            cacheOperations();
        }
        catch(Exception ex)
        {
            System.out.println(ex.toString());
        }
    }

    private static void cacheOperations()
    {
        try
        {
            //--- Clear the cache
            clearCache();

            //--- Add customer object in cache ...
            addCustomer();

            //--- Get customer from the cache
            retrieveAndPrintCustomerData();

            //--- Update the customer age ...
            updateCustomerData();

            //--- Get customer from the cache
            retrieveAndPrintCustomerData();

            //--- Remove a customer from the cache
            removeCustomer();

            //--- Add bulk items in the cache
            addBulkItems();

            //--- Retrieve bulk items from the cache
            retrieveBulkDataFromCache();

            //--- Remove bulk items from the cache
            removeBulkData();

            //--- Disconnect with cache ...
            disconnectCache();
        }
        catch(Exception ex)
        {
            System.out.println(ex.toString());
        }
    }
    private static void clearCache(){
        jCache.clear();
    }
    private static void addCustomer(){
        Customer customer = new Customer();
        customer.setName("David Johnes");
        customer.setAge(23);
        customer.setContactNo("12345-6789");
        customer.setAddress("Silicon Valley, Santa Clara, California");

        //--- Add customer object in cache ...
        jCache.put("Customer:DavidJohnes", customer);
    }
    private static void printCustomerDetails(Customer customer) {
        System.out.println();
        System.out.println("Customer Details are as follows: ");
        System.out.println("Name: " + customer.getName());
        System.out.println("Age: " + customer.getAge());
        System.out.println("Contact No: "  + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
        System.out.println();
    }
    private static void retrieveAndPrintCustomerData(){
        Customer cachedCustomer = (Customer) jCache.get("Customer:DavidJohnes");
        printCustomerDetails(cachedCustomer);
    }
    private static void updateCustomerData(){
        Customer cachedCustomer = (Customer) jCache.get("Customer:DavidJohnes");
        //--- Update the customer age ...
        cachedCustomer.setAge(50);
        //--- Put the updated customer object in cache ...
        jCache.put("Customer:DavidJohnes", cachedCustomer);
    }
    private static void addBulkItems() throws CacheException {
        Customer customer1 = new Customer();
        customer1.setName("Jane Miller");
        customer1.setAddress("234 Maple Avenue, Los Angeles, CA");
        customer1.setAge(23);
        customer1.setContactNo("111-222-3333");
        String key1 = "Customer:1001";

        Customer customer2 = new Customer();
        customer2.setName("Sophia Anderson");
        customer2.setAddress("44 Elm Street, Phoenix, AZ");
        customer2.setAge(31);
        customer2.setContactNo("(777) 888-9999");
        String key2 = "Customer:1002";

        HashMap map = new HashMap();
        String[] keys = {key1, key2};

        Customer[] items = new Customer[2];
        items[0] = customer1;
        items[1] = customer2;

        for (int i = 0; i < 2; i++)
        {
            map.put(keys[i], items[i]);
        }

        jCache.putAll(map);
    }
    private static void retrieveBulkDataFromCache(){
        HashSet set = new HashSet();
        set.add("Customer:1001");
        set.add("Customer:1002");
        Map items = jCache.getAll(set);
        if (!items.isEmpty())
        {
            for (Iterator iter = items.values().iterator(); iter.hasNext();)
            {
                Customer cachedCustomer = (Customer) iter.next();
                //utilize product object accordingly
                printCustomerDetails(cachedCustomer);
            }
        }
    }
    private static void removeBulkData(){
        String[] keys = {"Customer:1001", "Customer:1002"};

        HashSet set = new HashSet();
        for (int i = 0; i < 2; i++)
        {
            set.add(keys[i]);
        }
        jCache.removeAll(set);
    }
    private static void removeCustomer() throws CacheException {
        String key = "Customer:DavidJohnes";

        // False is returned if key does not exist in cache
        boolean result = jCache.remove(key);
        if (result != true) {
            //Deleted.
        }
        else {
            //Failed.
        }
    }
    private static void disconnectCache(){
        jCache.close();
    }
    private static Properties getProperties() throws IOException {
        String path = "config.properties";
        InputStream inputStream = JCacheSample.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
