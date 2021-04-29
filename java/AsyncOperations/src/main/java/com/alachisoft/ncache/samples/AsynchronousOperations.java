package com.alachisoft.ncache.samples;


import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Customer;
import com.alachisoft.ncache.samples.interfaces.OnAsyncOperationListener;

import java.io.*;
import java.util.Properties;

public class AsynchronousOperations {

    private static Cache cache;

    public static void run() {
        try{
            Properties properties = getProperties();
            //Initialize cache
            String cacheName = properties.getProperty("CacheID");

            initializeCache(cacheName);

            // Create a simple object
            Customer customer = createNewCustomer();
            String key = getKey(customer);

            // Adding item asynchronously
            addObjectToCacheAsynchronous(key, customer);

            // Modify the object and update in cache
            updateObjectInCacheAsynchronous(key, customer);

            // Remove the existing object asynchronously
            removeObjectFromCacheAsynchronous(key);

            // Dispose the cache once done
            cache.close();


        }catch (Exception e){
            System.out.println(e.getMessage());
        }
    }

    /**
     Delete an object in the cache using async api

     @param key String key to be removed from cache
     */
    private static void removeObjectFromCacheAsynchronous(String key) throws CacheException, InterruptedException {
        long count = cache.getCount();

        System.out.println("Cache count before item remove: " + count);

        AsyncController controller = new AsyncController();
        OnAsyncOperationListener listener = new Listeners(cache);
        controller.registerCacheCallback(listener);
        controller.doRemoveOperation(key, cache.removeAsync(key, Customer.class));

        count = cache.getCount();

        System.out.println("Cache count after item remove: " + count);
        Thread.sleep(3000);
    }

    /**
     This method updates object in the cache using async api

     @param key String key to be updated in cache
     @param customer Instance of Customer that will be updated in the cache
     */
    private static void updateObjectInCacheAsynchronous(String key, Customer customer) throws CacheException, InterruptedException {
        AsyncController controller = new AsyncController();
        OnAsyncOperationListener listener = new Listeners(cache);
        controller.registerCacheCallback(listener);
        controller.doUpdateOperation(key, cache.insertAsync(key, customer));
        Thread.sleep(3000);
    }

    /**
     This method adds object in the cache using async api

     @param key String key to be added in cache
     @param customer Instance of Customer that will be added to cache
     */
    private static void addObjectToCacheAsynchronous(String key, Customer customer) throws CacheException, InterruptedException {
        AsyncController controller = new AsyncController();
        OnAsyncOperationListener listener = new Listeners(cache);
        controller.registerCacheCallback(listener);
        controller.doAddOperation(key, cache.addAsync(key, customer));
        Thread.sleep(3000);
    }


    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = AsynchronousOperations.class.getClassLoader().getResourceAsStream(path);
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
        System.out.println("Cache initialized successfully.");
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

}
