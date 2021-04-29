// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Cache Item Versioning sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItemVersion;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.LockHandle;
import com.alachisoft.ncache.runtime.caching.WriteThruOptions;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.*;
import java.util.Properties;

public class CacheItemVersioning {

    public static void run() throws Exception {
        //Initialize cache
        Properties properties = getProperties();
        String cacheName = properties.getProperty("CacheID");
        Cache cache = CacheManager.getCache(cacheName);
        cache.clear();

        //Cache item versioning makes possible the concurrency checks on cacheitems.
        //Scenario:
        //User X and Y has fetched data from cache
        //User Y makes changes to some cache items and write back the changes to cache
        //Now the version of cache items with User X are obsolete and it can determine
        //so by using cache item versioning.

        Customer customer = new Customer();
        customer.setContactName("Scott Prince");
        customer.setAddress("95-A Barnes Road Wallingford, CT");
        customer.setContactNo("25632-5646");

        CacheItemVersion version1 = cache.add("Customer:ScottPrince", customer);

        //updaing the customer object in cache;
        customer.setCity("Calafornia");
        CacheItemVersion version2 = cache.insert("Customer:ScottPrince", customer);

        if (version1 != version2) {
            System.out.println("Item has changed since last time it was fetched.");
            System.out.println();
        }

        //GetItNewer
        //Retrives item from cache based on CacheItemVersion
        //Get item only is version superior to version2
        Customer customer2 = cache.getIfNewer("Customer:ScottPrince",
                version2, Customer.class /*Version to be compared from cache */);

        if (customer2 == null) {
            System.out.println("Latest version of item is already available");
        } else {
            System.out.println("Current Version of Customer:ScottPrince is: " + version2.getVersion());
            printCustomerDetails(customer2);
        }

        //Remove
        customer.setContactName("Mr. Scott n Price");
        CacheItemVersion version3 = cache.insert("Customer:ScottPrince", customer);

        Customer customerRemoved = cache.remove("Customer:ScottPrince", new LockHandle(), version3,
                new WriteThruOptions(), Customer.class);

        if (customerRemoved == null) {
            System.out.println("Remove failed. The newer version of item exists in cache.");
        } else {
            System.out.println("Following Customer is removed from cache:");
            printCustomerDetails(customerRemoved);
        }

        //Always manipulate the latest item as follows.

        CacheItemVersion version4 = cache.insert("Customer:ScottPrince", customer);
        customer2 = cache.getIfNewer("Customer:ScottPrince", version4, Customer.class);

        if (customer2 == null) {
            System.out.println("Item version available is latest!");
        } else {
            System.out.println("Latest available Item version is fetched from cache");
        }

        //Must dispose cache
        cache.clear();

        System.exit(0);
    }

    public static void printCustomerDetails(Customer customer) {
        System.out.println();
        System.out.println("Customer Details are as follows: ");
        System.out.println("Name: " + customer.getContactName());
        System.out.println("Contact No: " + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
    }

    /**
     * This method returns property file
     */
    private static Properties getProperties() throws IOException {
        String path = "config.properties";
        InputStream inputStream = CacheItemVersioning.class.getClassLoader().getResourceAsStream(path);
        Properties properties = new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
