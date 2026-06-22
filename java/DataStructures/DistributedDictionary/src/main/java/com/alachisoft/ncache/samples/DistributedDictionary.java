/*
 * ===============================================================================
 * Alachisoft (R) NCache Sample Code.
 * NCache Tags & NamedTags Sample (Java)
 * ===============================================================================
 * Copyright © Alachisoft. All rights reserved.
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
 * OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE.
 * ===============================================================================
 */

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.datastructures.DataStructureAttributes;
import com.alachisoft.ncache.client.datastructures.DistributedMap;
import com.alachisoft.ncache.runtime.caching.expiration.Expiration;
import com.alachisoft.ncache.runtime.caching.expiration.ExpirationType;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Product;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Properties;

/*
 * ===============================================================================
 * Distributed Dictionary is a NCache data structure to store key-value pairs
 * across distributed cache nodes. It behaves like a standard .NET Dictionary but
 * is scalable, synchronized and accessible by multiple applications in a cluster.
 * Some possible use cases of Distributed Dictionary include:
 *   1. Maintaining frequently accessed lookup/reference data (e.g. product
 *      catalog, configuration settings, or exchange rates)
 *   2. Managing user-specific data such as shopping carts, sessions, or
 *      preferences, configurations etc.
 *   3. Storing application-wide settings or feature flags for quick access
 *   4. Caching shared metadata (e.g., user-role mappings, localization data)
 *   5. Coordinating state between microservices by storing shared information
 * In this example, we will demonstrate how to create and use two distributed
 * dictionaries to manage collection of top selling and new arrival products in an
 * e-commerce application.
 * ===============================================================================
 */

public class DistributedDictionary {

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

        // Names of two dictionaries
        String topSellingName = "Products:TopSelling";
        String newArrivalsName = "Products:NewArrivals";

        // Creating or Getting Distributed Dictionary for Top Selling products
        DistributedMap<String, Product> topSelling = createOrGetDict(topSellingName);

        // Creating or Getting Distributed Dictionary for New Arrivals products
        DistributedMap<String, Product> newArrivals = createOrGetDict(newArrivalsName);

        // Adding products to the Top Selling dictionary
        addTopSellingProducts(topSelling);

        // Adding products to the New Arrivals dictionary
        addNewArrivalProducts(newArrivals);

        // Fetching and displaying products from Top Selling dictionary
        printProducts(topSellingName);

        // Fetching and displaying products from New Arrivals dictionary
        printProducts(newArrivalsName);

        // Updating a product in the Top Selling dictionary
        System.out.println("Updating Prod:1003 in Top Selling Products (Setting units in stock to 15)\n");

        // Fetching existing product from the dictionary
        Product existingProduct = topSelling.get("Prod:1003");
        if (existingProduct != null) {
            // Modifying the product's stock
            existingProduct.setUnitsInStock((short) 15);

            // Saving the updated product back to the dictionary
            topSelling.put("Prod:1003", existingProduct);
        }

        // Removing a product from the Top Selling dictionary
        topSelling.remove("Prod:1005");
        System.out.println("Removed Prod:1005 from " + topSellingName);

        // Displaying updated Top Selling products
        printProducts(topSellingName);

        // Deleting both distributed dictionaries from cache
        deleteDictionary(topSellingName);
        deleteDictionary(newArrivalsName);

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

    // --------------------------------------------------------------------------
    /**
     * Method to create a distributed Dictionary to which instances of Products are to be added
     *
     * @param DictionaryName The name of Dictionary as key
     * @return Instance of DistributedMap
     */
    private static DistributedMap<String, Product> createOrGetDict(String DictionaryName) throws CacheException {
        // Trying to get existing distributed Dictionary from cache
        DistributedMap<String, Product> dictionary = _cache.getDataStructuresManager().getMap(DictionaryName, Product.class);

        // Checking if dictionary already exists
        if (dictionary == null) {
            // Defining attributes for distributed Dictionary
            DataStructureAttributes attributes = new DataStructureAttributes();

            // Setting absolute expiration of 1 day for this dictionary
            attributes.setExpiration(new Expiration(ExpirationType.Absolute, TimeSpan.FromDays(1)));

            // Creating a new distributed Dictionary in cache
            dictionary = _cache.getDataStructuresManager().createMap(DictionaryName, attributes, null, Product.class);
        }

        // Returning the distributed Dictionary
        return dictionary;
    }

    // --------------------------------------------------------------------------
    /**
     * Adds a list of top-selling products into the given distributed dictionary
     *
     * @param dict Distributed Dictionary to add top-selling products to
     */
    private static void addTopSellingProducts(DistributedMap<String, Product> dict) {
        // Creating list of top-selling products
        List<Product> topSellingProducts = new ArrayList<>();

        // Creating sample products and adding them to the list
        Product p1 = new Product();
        p1.setProductID(1001);
        p1.setProductName("Macbook M3");
        p1.setCategory("Electronics");
        p1.setUnitPrice(1500);
        p1.setUnitsInStock((short) 20);
        topSellingProducts.add(p1);

        Product p2 = new Product();
        p2.setProductID(1002);
        p2.setProductName("Wireless Headphones");
        p2.setCategory("Accessories");
        p2.setUnitPrice(99);
        p2.setUnitsInStock((short) 200);
        topSellingProducts.add(p2);

        Product p3 = new Product();
        p3.setProductID(1003);
        p3.setProductName("Sneakers Classic");
        p3.setCategory("Footwear");
        p3.setUnitPrice(79);
        p3.setUnitsInStock((short) 80);
        topSellingProducts.add(p3);

        Product p4 = new Product();
        p4.setProductID(1004);
        p4.setProductName("4K LED TV");
        p4.setCategory("Electronics");
        p4.setUnitPrice(800);
        p4.setUnitsInStock((short) 50);
        topSellingProducts.add(p4);

        Product p5 = new Product();
        p5.setProductID(1005);
        p5.setProductName("Espresso Coffee Maker");
        p5.setCategory("Home Appliances");
        p5.setUnitPrice(120);
        p5.setUnitsInStock((short) 100);
        topSellingProducts.add(p5);

        // Looping through the list
        for (Product product : topSellingProducts) {
            // Adding each product to the distributed dictionary
            dict.put("Prod:" + product.getProductID(), product);
        }
    }

    // --------------------------------------------------------------------------
    /**
     * Adds a list of newly arrived products into the given distributed dictionary
     *
     * @param dict Dictionary to add new arrival products to
     */
    private static void addNewArrivalProducts(DistributedMap<String, Product> dict) {
        // Creating list of new arrival products
        List<Product> newArrivalProducts = new ArrayList<>();

        // Creating sample products and adding them to the list
        Product p1 = new Product();
        p1.setProductID(2001);
        p1.setProductName("Classic Denim Jacket");
        p1.setCategory("Clothing");
        p1.setUnitPrice(65);
        p1.setUnitsInStock((short) 150);
        newArrivalProducts.add(p1);

        Product p2 = new Product();
        p2.setProductID(2002);
        p2.setProductName("VR Gaming Set");
        p2.setCategory("Gaming");
        p2.setUnitPrice(450);
        p2.setUnitsInStock((short) 60);
        newArrivalProducts.add(p2);

        Product p3 = new Product();
        p3.setProductID(2003);
        p3.setProductName("Smart Home Speaker");
        p3.setCategory("Electronics");
        p3.setUnitPrice(120);
        p3.setUnitsInStock((short) 90);
        newArrivalProducts.add(p3);

        Product p4 = new Product();
        p4.setProductID(2004);
        p4.setProductName("Electric Scooter");
        p4.setCategory("Transportation");
        p4.setUnitPrice(300);
        p4.setUnitsInStock((short) 40);
        newArrivalProducts.add(p4);

        Product p5 = new Product();
        p5.setProductID(2005);
        p5.setProductName("Noise-Cancelling Earbuds");
        p5.setCategory("Accessories");
        p5.setUnitPrice(150);
        p5.setUnitsInStock((short) 110);
        newArrivalProducts.add(p5);

        // Looping through the list
        for (Product product : newArrivalProducts) {
            // Adding each product to the distributed dictionary
            dict.put("Prod:" + product.getProductID(), product);
        }
    }

    // --------------------------------------------------------------------------
    /**
     * Method to print details of product
     *
     * @param dictName Name of distributed Dictionary whose values are to be printed
     */
    private static void printProducts(String dictName) throws CacheException {
        // Getting distributed Dictionary from cache
        DistributedMap<String, Product> dict = _cache.getDataStructuresManager().getMap(dictName, Product.class);

        // Validating dictionary if it exists
        if (dict == null || dict.isEmpty()) {
            System.out.println("\nProduct List (" + dictName + ") is empty or removed.");
            return;
        }

        // Printing product details on console
        System.out.println("\nProduct List (" + dictName + ") contains " + dict.size() + " product(s):");

        // Looping through dictionary entries
        for (Map.Entry<String, Product> entry : dict.entrySet()) {
            // Getting product from entry
            Product p = entry.getValue();

            // Printing product details
            System.out.println(
                    "  - " + entry.getKey() +
                            " | Name: " + p.getProductName() +
                            ", Price: " + p.getUnitPrice() +
                            ", Stock: " + p.getUnitsInStock()
            );
        }
        System.out.println();
    }

    // --------------------------------------------------------------------------
    /**
     * Method to remove distributed Dictionary from cache
     *
     * @param dictionaryName The name of distributed Dictionary to be removed from cache
     */
    private static void deleteDictionary(String dictionaryName) throws CacheException, IOException {
        // Removing the distributed Dictionary from cache
        _cache.getDataStructuresManager().remove(dictionaryName);

        // Printing output on console.
        System.out.println("Distributed Dictionary ("+ dictionaryName + ") successfully removed from cache.\n");
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to read configuration value from config.properties file
     */
    private static String getConfigValue(String property) {
        try {
            // Loading config.properties file from resources folder into stream
            InputStream stream = DistributedDictionary.class
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
