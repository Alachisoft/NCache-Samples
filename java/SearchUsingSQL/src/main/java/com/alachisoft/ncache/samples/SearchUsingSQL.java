
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

import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.runtime.caching.NamedTagsDictionary;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Product;

import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

/**
 Class that provides the functionality of the sample
 */
public class SearchUsingSQL
{
    private static Cache _cache;

    /**
     Executing this method will perform the operations fo the sample using object query language.
     */
    public static void run() throws Exception {
        // Initialize Cache
        initializeCache();

        _cache.clear();
        // Querying an item in NCache requires either NamedTags or
        // Indexes defined for objects or both.

        // Generate a new instance of Product
        List<Product> products = generateProducts();

        // Insert Items in cache with Named Tags
        insertItemsWithNamedTags(products);

        // Querying a simple cache item via named Tags
        queryItemsUsingNamedTags();

        // Query Items with defined index
        queryItemsUsingDefinedIndex();

        // Query Items with both named tags and predefined index
        queryItemsUsingProjection();

        // Dispose the cache once done
        _cache.close();
    }

    /**
     This method initializes the cache
     */
    private static void initializeCache() throws Exception {
        Properties properties=  getProperties();
        String cacheName= properties.getProperty("CacheID");

        if (tangible.DotNetToJavaStringHelper.isNullOrEmpty(cacheName))
        {
            System.out.println("The Cache Name cannot be null or empty.");
            return;
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);
        System.out.println("Cache initialized successfully");
    }
    /**
     This method inserts items in cache with named tags.
     @param products list of products to be inserted in cache
     */
    private static void insertItemsWithNamedTags(List<Product> products) throws CacheException {
        for (Product product : products)
        {
            NamedTagsDictionary namedTagDictionary = GetNamedTagsDictionary(product);
            CacheItem cacheItem = new CacheItem(product);
            cacheItem.setNamedTags(namedTagDictionary);
            _cache.add("Product:"+product.getId(), cacheItem);
        }

        System.out.println("Items added in cache with NamedTag 'Supplier'.");
    }

    /**
     This method queries Items from NCache using named tags.
     */
    private static void queryItemsUsingNamedTags() throws CacheException, IOException, ClassNotFoundException {
        String query = "SELECT $Value$ FROM com.alachisoft.ncache.java.com.alachisoft.ncache.samples.data.Product WHERE Supplier = ?";
        QueryCommand queryCommand = new QueryCommand(query);
        // Defining Searching criteria
        queryCommand.getParameters().put("Supplier", "Tokyo Traders");
        CacheReader reader = _cache.getSearchService().executeReader(queryCommand);

        int counter = 0;
        if (reader.getFieldCount() > 0)
        {
            while (reader.read())
            {
                Product product = reader.getValue(1,Product.class);
                PrintProductDetails(product);
                counter++;
            }
        }

        System.out.println(counter+" cache items fetched from cache using query on named tags.\n");
    }

    /**
     This method queries items in the cache on which index was defined using NCache Manager.
     */
    private static void queryItemsUsingDefinedIndex() throws CacheException, IOException, ClassNotFoundException {
        //  Query can only be applied to C# Primitive data types:
        //  and for those non primitive data types whose indexes are defined in NCache manager

        String query = "SELECT $Value$ FROM com.alachisoft.ncache.java.com.alachisoft.ncache.samples.data.Product WHERE UnitPrice > ?";


        QueryCommand queryCommand = new QueryCommand(query);
        queryCommand.getParameters().put("UnitPrice", 100);
        CacheReader reader = _cache.getSearchService().executeReader(queryCommand);

        int counter = 0;
        if (reader.getFieldCount() > 0)
        {
            while (reader.read())
            {
                Product product = reader.getValue(1,Product.class);
                PrintProductDetails(product);
                counter++;
            }
        }

        System.out.println(counter+ " cache items fetched from cache using query on unit price.\n");
    }

    /**
     This method queries items from the cache using projection attributes.
     */
    private static void queryItemsUsingProjection() throws CacheException, IOException, ClassNotFoundException {
        String query = "SELECT Name, Supplier FROM com.alachisoft.ncache.java.com.alachisoft.ncache.samples.data.Product WHERE UnitPrice > ?";

        QueryCommand queryCommand = new QueryCommand(query);
        queryCommand.getParameters().put("UnitPrice", 100);
        CacheReader reader = _cache.getSearchService().executeReader(queryCommand);

        int counter = 0;
        if (reader.getFieldCount() > 0)
        {
            while (reader.read())
            {
                System.out.println("Name:      " + reader.getValue("Name",String.class));
                System.out.println("Supplier:  " + reader.getValue("Supplier",String.class));
                System.out.println();
                counter++;
            }
        }

        System.out.println(counter+ " cache items fetched from cache using projection query on unit price.\n");
    }

    /**
     This method generates a new instance of product with the specified key.

     @return  returns a new instance of Product.
     */
    private static List<Product> generateProducts()
    {
        ArrayList<Product> productList = new ArrayList<>();

        // product1
        Product product1 = new Product();
        product1.setId(1);
        product1.setName ("Chai");
        product1.setClassName ("Electronics");
        product1.setCategory ("Beverages");
        product1.setUnitPrice(357);
        productList.add(product1);

        // product2
        Product product2 = new Product();
        product2.setId (2);
        product2.setName ( "Chang");
        product2.setClassName ( "Electronics");
        product2.setCategory ( "Meat");
        product2.setUnitPrice ( 188);
        productList.add(product2);

        // product3
        Product product3 = new Product();
        product3.setId ( 3);
        product3.setName ( "Aniseed Syrup");
        product3.setClassName ("Electronics");
        product3.setCategory ( "Beverages");
        product3.setUnitPrice ( 258);
        productList.add(product3);

        // product4
        Product product4 = new Product();
        product4.setId ( 4);
        product4.setName ("IKura");
        product4.setClassName ( "Electronics");
        product4.setCategory ( "Produce");
        product4.setUnitPrice (50);
        productList.add(product4);

        // product5
        Product product5 = new Product();
        product5.setId ( 5);
        product5.setName ( "Tofu");
        product5.setClassName ( "Electronics");
        product5.setCategory ("Seafood");
        product5.setUnitPrice (78);
        productList.add(product5);

        return productList;
    }

    private static NamedTagsDictionary GetNamedTagsDictionary(Product product)
    {
        NamedTagsDictionary nameTagDict = new NamedTagsDictionary();

        String productName = product.getName();
        if (productName.equals("Chai") || productName.equals("Chang") || productName.equals("Aniseed Syrup"))
        {
            nameTagDict.add("Supplier", "Exotic Liquids");

        }
        else if (productName.equals("IKura") || productName.equals("Tofu"))
        {
            nameTagDict.add("Supplier", "Tokyo Traders");
        }

        return nameTagDict;
    }

    /**
     Method for printing details of product type.

     @param product Product whos details will be printed
     */
    public static void PrintProductDetails(Product product)
    {
        System.out.println("Id:        " + product.getId());
        System.out.println("Name:      " + product.getName());
        System.out.println("Class:     " + product.getClassName());
        System.out.println("Category:  " + product.getCategory());
        System.out.println("UnitPrice: " + product.getUnitPrice());
        System.out.println();
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = SearchUsingSQL.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }

}
