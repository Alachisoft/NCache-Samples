/**
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

package com.alachisoft.ncache.sample;

import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.runtime.caching.NamedTagsDictionary;
import com.alachisoft.ncache.runtime.caching.Tag;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Product;

import java.io.InputStream;
import java.util.*;

/**
 * ===============================================================================
 * This sample demonstrates how to perform SQL-like queries on cached data in
 * NCache. You can use queries to filter, search, or project specific data
 * efficiently. Some possible use cases of SearchUsingSQL in NCache include:
 *   1. Filtering items based on attributes like price, category, or warranty.
 *   2. Retrieving only required fields instead of fetching entire objects to
 *      optimize performance.
 *   3. Running tag-based or named-tag-based searches for analytics, reporting,
 *      or dashboards.
 *   4. Implementing dynamic search features in applications (e.g., user-driven
 *      queries).
 *   5. Enhancing data retrieval performance using indexed queries on cached data.
 *  ==============================================================================
 */

public class SearchUsingSQL {

    // Cache handle for the connected cache
    private static Cache _cache;

    public static void main(String[] args) throws Exception {
        // Getting cache handle
        _cache = getCache();

        // Generating and inserting sample products into cache for demonstration
        addSampleData();

        // Querying using Namedtags
        queryByNamedTags(true);

        // Performing queries using defined indexes
        queryByPriceIndex(100);

        // Performing queries using projection
        querySelectedFields(100);

        // Dispose the cache once done
        _cache.close();
    }

    /// --------------------------------------------------------------------------
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

    /// --------------------------------------------------------------------------
    /**
     * Method to generate sample products and add them to the cache with associated tags and named tags
     */
    private static void addSampleData() throws CacheException {
        // Creating sample products for demonstration
        Product laptop = new Product();
        laptop.setProductID(13001);
        laptop.setProductName("Laptop");
        laptop.setUnitPrice(2000);
        laptop.setCategory("Electronics");

        Product kindle = new Product();
        kindle.setProductID(13002);
        kindle.setProductName("Kindle");
        kindle.setUnitPrice(1000);
        kindle.setCategory("Electronics");

        Product book = new Product();
        book.setProductID(13003);
        book.setProductName("Book");
        book.setUnitPrice(100);
        book.setCategory("Stationery");

        Product smartPhone = new Product();
        smartPhone.setProductID(13004);
        smartPhone.setProductName("Smart Phone");
        smartPhone.setUnitPrice(1000);
        smartPhone.setCategory("Electronics");

        Product mobilePhone = new Product();
        mobilePhone.setProductID(13005);
        mobilePhone.setProductName("Mobile Phone");
        mobilePhone.setUnitPrice(200);
        mobilePhone.setCategory("Electronics");

        // Creating CacheItems with Tags and Named Tags from sample products
        CacheItem laptopCacheItem = new CacheItem(laptop);
        laptopCacheItem.setTags(Arrays.asList(new Tag("Technology"), new Tag("Gadgets")));
        laptopCacheItem.setNamedTags(new NamedTagsDictionary());

        CacheItem kindleCacheItem = new CacheItem(kindle);
        kindleCacheItem.setTags(Arrays.asList(new Tag("Technology"), new Tag("Reading")));
        kindleCacheItem.setNamedTags(new NamedTagsDictionary());

        CacheItem bookCacheItem = new CacheItem(book);
        bookCacheItem.setTags(Arrays.asList(new Tag("Education"), new Tag("Reading")));
        bookCacheItem.setNamedTags(new NamedTagsDictionary());

        CacheItem smartPhoneCacheItem = new CacheItem(smartPhone);
        smartPhoneCacheItem.setTags(Arrays.asList(new Tag("Technology"), new Tag("Communication")));
        smartPhoneCacheItem.setNamedTags(new NamedTagsDictionary());

        CacheItem mobilePhoneCacheItem = new CacheItem(mobilePhone);
        mobilePhoneCacheItem.setTags(Arrays.asList(new Tag("Technology"), new Tag("Communication")));
        mobilePhoneCacheItem.setNamedTags(new NamedTagsDictionary());

        // Adding NamedTags to CacheItems
        laptopCacheItem.getNamedTags().add("Discount", 20);
        laptopCacheItem.getNamedTags().add("Warranty", true);
        bookCacheItem.getNamedTags().add("Discount", 10);
        bookCacheItem.getNamedTags().add("Warranty", false);
        kindleCacheItem.getNamedTags().add("Discount", 15);
        kindleCacheItem.getNamedTags().add("Warranty", false);
        smartPhoneCacheItem.getNamedTags().add("Discount", 25);
        smartPhoneCacheItem.getNamedTags().add("Warranty", true);
        mobilePhoneCacheItem.getNamedTags().add("Discount", 5);
        mobilePhoneCacheItem.getNamedTags().add("Warranty", true);

        // Creating a map of products to be added to cache
        Map<String, CacheItem> products = new HashMap<>();
        products.put("Product:" + laptop.getProductID(), laptopCacheItem);
        products.put("Product:" + kindle.getProductID(), kindleCacheItem);
        products.put("Product:" + book.getProductID(), bookCacheItem);
        products.put("Product:" + smartPhone.getProductID(), smartPhoneCacheItem);
        products.put("Product:" + mobilePhone.getProductID(), mobilePhoneCacheItem);

        // Adding all the products to the cache
        System.out.println("Adding products to the cache with tags...");
        _cache.insertBulk(products);

        // Fetching and printing added products on console
        List<Product> productValues = new ArrayList<>();
        for (CacheItem ci : products.values()) {
            productValues.add(ci.getValue(Product.class));
        }
        printProducts(productValues);
    }

    /// --------------------------------------------------------------------------
    /**
     * Method to print products to the console
     *
     * @param products Products whose details are to be printed
     */
    private static void printProducts(List<Product> products) {
        System.out.println("ProductID, Name, Price, Category");
        for (Product product : products) {
            System.out.println("- " + product.getProductID() + ", " + product.getProductName() + ", $" +
                    product.getUnitPrice() + ", " + product.getCategory());
        }
        System.out.println();
    }

    /// --------------------------------------------------------------------------
    /**
     * Method to demonstrate how to query items from NCache using Named Tags and print them
     *
     * @param warranty Warranty value to filter products
     */
    private static void queryByNamedTags(boolean warranty) throws Exception {
        String query = "SELECT $Value$ FROM com.alachisoft.ncache.samples.data.Product WHERE Warranty = ?";
        QueryCommand queryCommand = new QueryCommand(query);
        HashMap<String, Object> parameters = new HashMap<>();
        parameters.put("Warranty", warranty);
        queryCommand.getParameters().putAll(parameters);

        try (CacheReader reader = _cache.getSearchService().executeReader(queryCommand)) {
            int count = 0;
            while (reader.read()) {
                Product product = reader.getValue(1, Product.class);
                printProductDetails(product);
                count++;
            }
            System.out.println(count + " cache items fetched using query on Named Tags.\n");
        }
    }

    /// --------------------------------------------------------------------------
    /**
     * Method to demonstrate how to query cached items based on indexed attributes and print them
     *
     * @param unitPrice Unit price value to filter products
     */
    private static void queryByPriceIndex(double unitPrice) throws Exception {
        String query = "SELECT $Value$ FROM com.alachisoft.ncache.samples.data.Product WHERE unitPrice > ?";
        QueryCommand queryCommand = new QueryCommand(query);
        HashMap<String, Object> parameters = new HashMap<>();
        parameters.put("unitPrice", unitPrice);
        queryCommand.getParameters().putAll(parameters);

        try (CacheReader reader = _cache.getSearchService().executeReader(queryCommand)) {
            int count = 0;
            while (reader.read()) {
                Product product = reader.getValue(1, Product.class);
                printProductDetails(product);
                count++;
            }
            System.out.println(count + " cache items fetched using query on indexed attribute (UnitPrice).\n");
        }
    }

    /// --------------------------------------------------------------------------
    /**
     * Method to demonstrate querying using Projection, fetching only selected attributes and printing the items
     *
     * @param unitPrice Unit price value to filter products
     */
    private static void querySelectedFields(double unitPrice) throws Exception {
        String query = "SELECT productName, category FROM com.alachisoft.ncache.samples.data.Product WHERE unitPrice > ?";
        QueryCommand queryCommand = new QueryCommand(query);
        queryCommand.getParameters().put("unitPrice", unitPrice);

        try (CacheReader reader = _cache.getSearchService().executeReader(queryCommand)) {
            int count = 0;
            while (reader.read()) {
                System.out.println("Name:     " + reader.getValue("productName", String.class));
                System.out.println("Category: " + reader.getValue("category", String.class));
                System.out.println();
                count++;
            }
            System.out.println(count + " cache items fetched using projection query.\n");
        }
    }

    /// --------------------------------------------------------------------------
    /**
     * Method to print product details to the console
     *
     * @param product Product object whose details are to be printed
     */
    private static void printProductDetails(Product product) {
        System.out.println("Id:        " + product.getProductID());
        System.out.println("Name:      " + product.getProductName());
        System.out.println("Class:     " + product.getClassName());
        System.out.println("Category:  " + product.getCategory());
        System.out.println("UnitPrice: " + product.getUnitPrice());
        System.out.println();
    }

    /// --------------------------------------------------------------------------
    /**
     * Reads value from config.properties
     */
    private static String getConfigValue(String key) throws Exception {
        try (InputStream stream = SearchUsingSQL.class.getClassLoader().getResourceAsStream("config.properties")) {
            if (stream == null) {
                System.out.println("config.properties not found.");
                return null;
            }
            Properties props = new Properties();
            props.load(stream);
            return props.getProperty(key);
        }
    }
}

