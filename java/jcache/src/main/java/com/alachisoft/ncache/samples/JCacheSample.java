package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.samples.data.Customer;
import javax.cache.Cache;
import javax.cache.CacheManager;
import javax.cache.Caching;
import javax.cache.configuration.MutableConfiguration;
import javax.cache.expiry.Duration;
import javax.cache.expiry.TouchedExpiryPolicy;
import javax.cache.spi.CachingProvider;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class JCacheSample<K,V>
{
    public static void main(String[] args)
    {
        try
        {
            //--- Get cache provider ...
            CachingProvider cachingProvider = Caching.getCachingProvider();
            //--- Get cache manager ...
            CacheManager cacheManager = cachingProvider.getCacheManager();
            //--- Create cache configurations ...
            MutableConfiguration<String, Customer> config = new MutableConfiguration<String, Customer>();
            config.setTypes(String.class, Customer.class).setExpiryPolicyFactory(TouchedExpiryPolicy.factoryOf(Duration.ONE_MINUTE));


            Properties properties=  getProperties();
            String cacheName= properties.getProperty("CacheID");

            //--- Get cache instance ...
            Cache<String, Customer> cache = cacheManager.createCache(cacheName, config);
            //--- Perform basic cache operations ...
            cacheOperations(cache);            
        }
        catch(Exception ex)
        {
            System.out.println(ex.toString());
        }
    }
    
    private static void cacheOperations(Cache cache)
    {
        try
        {
            //--- Clear the cache 
            cache.clear();
            
            Customer customer = new Customer();
            customer.setName("David Johnes");
            customer.setAge(23);
            customer.setGender("Male");
            customer.setContactNo("12345-6789");
            customer.setAddress("Silicon Valley, Santa Clara, California");

            //--- Add customer object in cache ...
            cache.put("Customer:DavidJohnes", customer);

            //--- Get customer from the cache
            Customer cachedCustomer = (Customer) cache.get("Customer:DavidJohnes");

            printCustomerDetails(cachedCustomer);

            //--- Update the customer age ...
            customer.setAge(50);
            //--- Put the updated customer object in cache ...
            cache.put("Customer:DavidJohnes", customer);
            //--- Get customer from the cache
            cachedCustomer = (Customer) cache.get("Customer:DavidJohnes");

            printCustomerDetails(cachedCustomer);

            //--- Disconnect with cache ...
            cache.close();
        }
        catch(Exception ex)
        {
            System.out.println(ex.toString());
        }
    }
    
    public static void printCustomerDetails(Customer customer)
    {
        System.out.println();        
        System.out.println("Customer Details are as follows: ");
        System.out.println("Name: " + customer.getName());
        System.out.println("Age: " + customer.getAge());
        System.out.println("Gender: " + customer.getGender());
        System.out.println("Contact No: "  + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
        System.out.println();
    }

    private static Properties getProperties() throws IOException
    {
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
