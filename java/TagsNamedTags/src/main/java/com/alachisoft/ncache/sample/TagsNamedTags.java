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

package com.alachisoft.ncache.sample;

import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.runtime.caching.NamedTagsDictionary;
import com.alachisoft.ncache.runtime.caching.Tag;
import com.alachisoft.ncache.samples.data.Product;

import java.io.InputStream;
import java.util.*;

public class TagsNamedTags {

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

        System.out.println("\n=== TAGS DEMONSTRATION ===\n");

        /*
         * ===============================================================================
         * Tags allow you to logically group different cached items together based on
         * something common among them. Tags provide a many-to-many relationship between
         * cached items and tags.
         * You can assign the same tag to multiple cached items and assign multiple tags
         * to a single cached item.
         * Some use cases include:
         *   1. Grouping related items (e.g., products by category, region, or supplier)
         *      for easier retrieval.
         *   2. Fast filtering and searching of cached items based on tag associations.
         *   3. Performing bulk operations such as invalidation or removal of all items
         *      under a specific tag.
         *   4. Associating cached items with multiple dimensions, such as "Brand:Apple"
         *      and "Type:Electronics" to support multidimensional querying.
         *   5. Implementing cache dependency or invalidation strategies — e.g. removing
         *      all items tagged with a supplier when that entity changes in the database.
         * Tags are a powerful feature that help you manage and organize your cached
         * data more effectively.
         * ===============================================================================
         */

        // Creating sample products for tags demonstration
        Product laptop = new Product();
        laptop.setProductID(12001);
        laptop.setProductName("Laptop");
        laptop.setCategory("Electronics");

        Product book = new Product();
        book.setProductID(12002);
        book.setProductName("Book");
        book.setCategory("Stationery");

        Product kindle = new Product();
        kindle.setProductID(12003);
        kindle.setProductName("Kindle");
        kindle.setCategory("Electronics");

        Product smartPhone = new Product();
        smartPhone.setProductID(12004);
        smartPhone.setProductName("Smart Phone");
        smartPhone.setCategory("Electronics");

        Product mobilePhone = new Product();
        mobilePhone.setProductID(12005);
        mobilePhone.setProductName("Mobile Phone");
        mobilePhone.setCategory("Electronics");

        // Creating CacheItems with tags
        CacheItem laptopItem = new CacheItem(laptop);
        laptopItem.setTags(Arrays.asList(new Tag("Technology"), new Tag("Gadgets")));

        CacheItem bookItem = new CacheItem(book);
        bookItem.setTags(Arrays.asList(new Tag("Education"), new Tag("Reading")));

        CacheItem kindleItem = new CacheItem(kindle);
        kindleItem.setTags(Arrays.asList(new Tag("Technology"), new Tag("Reading")));

        CacheItem smartPhoneItem = new CacheItem(smartPhone);
        smartPhoneItem.setTags(Arrays.asList(new Tag("Technology"), new Tag("Communication")));

        CacheItem mobilePhoneItem = new CacheItem(mobilePhone);
        mobilePhoneItem.setTags(Arrays.asList(new Tag("Technology"), new Tag("Communication")));

        // Creating the products Map and adding CacheItems to it
        Map<String, CacheItem> products = new HashMap<>();
        products.put("Product:" + laptop.getProductID(), laptopItem);
        products.put("Product:" + book.getProductID(), bookItem);
        products.put("Product:" + kindle.getProductID(), kindleItem);
        products.put("Product:" + smartPhone.getProductID(), smartPhoneItem);
        products.put("Product:" + mobilePhone.getProductID(), mobilePhoneItem);

        // Adding all the products to the cache in bulk.
        _cache.insertBulk(products);

        // Printing output on console
        System.out.println("Products added in the cache with Tags successfully.\n");

        // Fetching products by "Technology" tag
        System.out.println("Fetching items with Tag \"Technology\"");
        Map<String, Product> techItems = _cache.getSearchService()
                .getByTag("Technology");

        // Printing the fetched products' details
        printProductDetails(techItems);

        // Fetching keys by "Gadgets" tag
        System.out.println("\nFetching keys by tag \"Gadgets\"");
        Collection<String> gadgetKeys = _cache.getSearchService()
                .getKeysByTag("Gadgets");

        // Printing the fetched keys
        printKeys(gadgetKeys);

        // Creating an array of tags for multi-tag search
        ArrayList<Tag> tags = new ArrayList<>();
        tags.add(new Tag("Technology"));
        tags.add(new Tag("Reading"));
        System.out.println("\nFetch items by ANY tag: Technology OR Reading");

        // Fetching items from the cache that match any of the specified tags (e.g. Technology or Reading)
        Map<String, Product> anyTagItems = _cache.getSearchService()
                .getByTags(tags, TagSearchOptions.ByAnyTag);

        // Printing the fetched products' details
        printProductDetails(anyTagItems);

        // Multi-tag removal: ALL tags
        System.out.println("\nRemoving items by ALL tags: Technology AND Reading\n");

        // Removing items from the cache that match all the specified tags (e.g. Technology and Reading)
        _cache.getSearchService().removeByTags(tags, TagSearchOptions.ByAllTags);

        System.out.println("\n=== NAMED TAGS DEMONSTRATION ===\n");

        /*
         * ===============================================================================
         * Named Tags allow you to define custom attributes for the data that you are
         * caching. This is useful when your data is unstructured (not defined in a class)
         * or when you want to add additional attributes without modifying the class itself.
         * Some use cases include:
         *   1. Storing metadata about cached items, such as creation date, last accessed
         *      date, or expiration date.
         *   2. Enabling advanced search and filtering capabilities based on Named Tags.
         *   3. Implementing custom business logic based on Named Tag values.
         *   4. Categorizing cached items for easier grouping and bulk operations, such as
         *      invalidating all items of a certain type.
         *   5. Tagging items with user or session identifiers to support personalized
         *      caching and quick retrieval per user/session.
         * Named Tags provide a flexible way to enhance the functionality of your cache
         * data store.
         * ===============================================================================
         */

        // Creating sample image items for Named Tags demonstration
        CacheItem imgSunset = new CacheItem("001_sunset.jpg");
        CacheItem imgBeach = new CacheItem("002_beach.jpg");
        CacheItem imgMountain = new CacheItem("003_mountain.jpg");
        CacheItem imgNight = new CacheItem("004_night.jpg");
        CacheItem imgForest = new CacheItem("005_forest.jpg");

        // Adding NamedTags to sample image items
        NamedTagsDictionary sunsetNamedTags = new NamedTagsDictionary();
        sunsetNamedTags.add("Resolution", "1080p");
        sunsetNamedTags.add("SizeMB", 2.3);
        imgSunset.setNamedTags(sunsetNamedTags);

        NamedTagsDictionary beachNamedTags = new NamedTagsDictionary();
        beachNamedTags.add("Resolution", "4K");
        beachNamedTags.add("SizeMB", 5.1);
        imgBeach.setNamedTags(beachNamedTags);

        NamedTagsDictionary mountainNamedTags = new NamedTagsDictionary();
        mountainNamedTags.add("Resolution", "720p");
        mountainNamedTags.add("SizeMB", 1.8);
        imgMountain.setNamedTags(mountainNamedTags);

        NamedTagsDictionary nightNamedTags = new NamedTagsDictionary();
        nightNamedTags.add("Resolution", "4K");
        nightNamedTags.add("SizeMB", 4.7);
        imgNight.setNamedTags(nightNamedTags);

        NamedTagsDictionary forestNamedTags = new NamedTagsDictionary();
        forestNamedTags.add("Resolution", "1080p");
        forestNamedTags.add("SizeMB", 3.0);
        forestNamedTags.add("ColorProfile", "sRGB");
        imgForest.setNamedTags(forestNamedTags);

        // Creating the image items dictionary
        Map<String, CacheItem> images = new HashMap<>();
        images.put("Image:001", imgSunset);
        images.put("Image:002", imgBeach);
        images.put("Image:003", imgMountain);
        images.put("Image:004", imgNight);
        images.put("Image:005", imgForest);

        // Adding all the image items to the cache in bulk
        _cache.addBulk(images);
        System.out.println("Images added to cache with NamedTags.\n");

        // Fetching and printing images having at least 5MB size
        printImagesWithMinSize(5.0);

        // Update Named Tag
        forestNamedTags.remove("ColorProfile");
        imgForest.setNamedTags(forestNamedTags);

        // Updating the image item in the cache
        _cache.insert("Image:005", imgForest);

        // Cleaning up the cache by removing added products and images
        _cache.removeBulk(products.keySet(), Product.class);
        _cache.removeBulk(images.keySet(), String.class);

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


    // ------------------------------------------------------------------------------
    /**
     * Method to print the details of products on the console
     *
     * @param items Map of products
     */
    private static void printProductDetails(Map<String, Product> items) {
        // Validating product map
        if (items == null || items.isEmpty()) {
            System.out.println("No items found.\n");
            return;
        }

        // Iterating through each product and printing its details
        System.out.println("ProductID | Name | Category");

        // loop through map entries
        for (Map.Entry<String, Product> entry : items.entrySet()) {
            // get product from entry
            Product p = entry.getValue();

            // print product details
            System.out.printf("%d | %s | %s\n", p.getProductID(), p.getProductName(), p.getCategory());
        }
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to print the keys on the console
     *
     * @param keys Collection of keys
     */
    private static void printKeys(Collection<String> keys) {
        // Validating keys collection
        if (keys == null || keys.isEmpty()) {
            System.out.println("No keys found.\n");
            return;
        }

        // Printing output on console
        System.out.println("Keys:");

        // Iterating through each key
        for (String key : keys) {
            // Printing the key on console
            System.out.println(key);
        }
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to print images with minimum size on the console
     *
     * @param sizeMB Minimum size in MB
     */
    private static void printImagesWithMinSize(double sizeMB) throws Exception {
        // Printing output on console
        System.out.printf("Images with SizeMB >= %.1f\n", sizeMB);

        // Creating query to get cache items with NamedTag "SizeMB"
        String query = "SELECT $Value$ FROM $Text$ WHERE SizeMB >= ?";

        // Creating QueryCommand with the specified query
        QueryCommand cmd = new QueryCommand(query);

        // Setting "SizeMB" parameter in query
        cmd.getParameters().put("SizeMB", sizeMB);

        // Executing the query to get images with minimum size and getting the result set
        CacheReader result = _cache.getSearchService().executeReader(cmd);

        // Iterating through each record in result set
        while (result.read()) {
            // Getting image from the record
            String image = result.getValue(1, String.class);

            // Printing the image on console
            System.out.println(image);
        }
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to read configuration value from config.properties file
     */
    private static String getConfigValue(String property) {
        try {
            // Loading config.properties file from resources folder into stream
            InputStream stream = TagsNamedTags.class
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