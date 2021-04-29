// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Bulk Operations sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.samples.data.Product;

import java.io.*;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

public class BulkOperations
{
    public static void run() throws Exception {
            Properties properties=  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();

            String[] keysToAdd = new String[]
            {
                "Product:Cheese", "Product:Butter",
                "Product:Cream", "Product:Youghart"
            };

            Product[] products = new Product[4];

            products[0] = new Product();
            products[0].setId(1);
            products[0].setName("Dairy Milk Cheese");
            products[0].setClassName("ClassA");
            products[0].setCategory("Edibles");

            products[1] = new Product();
            products[1].setId(2);
            products[1].setName("American Butter");
            products[1].setClassName("ClassA");
            products[1].setCategory("Edibles");

            products[2] = new Product();
            products[2].setId(3);
            products[2].setName("Walmart Delicious Cream");
            products[2].setClassName("ClassA");
            products[2].setCategory("Edibles");

            products[3] = new Product();
            products[3].setId(4);
            products[3].setName("Nestle Youghart");
            products[3].setClassName("ClassA");
            products[3].setCategory("Edibles");

            HashMap<String, CacheItem> prodMap = new HashMap<>();
            prodMap.put(keysToAdd[0], new CacheItem(products[0]));
            prodMap.put(keysToAdd[1], new CacheItem(products[1]));
            prodMap.put(keysToAdd[2], new CacheItem(products[2]));
            prodMap.put(keysToAdd[3], new CacheItem(products[3]));

            //Adding Bulk Items
            //Bulk operation returns 
            Map<String, Exception> result = cache.addBulk(prodMap);

            if (result.isEmpty())
            {
                System.out.println("All items are successfully added to cache.");
            }
            else
            {
                System.out.println("One or more items could not be added to the cache. Iterate hashmap for details.");
                //This is how to iterate the hashmap
                for (Exception ex : result.values()) {
                    System.out.println(ex.getMessage());
                }
            }

            //Getting bulk items
            Map<String, Object> items = cache.getBulk(Arrays.asList(keysToAdd), Product.class);
            if (!items.isEmpty())
            {
                for (Object o : items.values()) {
                    Product product = (Product) o;
                    printProductDetails(product);
                }
            }

            //Updating Bulk Items
            //Previous products have their product id, class and category changed.
            products[0].setClassName("ClassB");
            products[1].setClassName("ClassC");
            products[2].setClassName("ClassA");
            products[3].setClassName("ClassD");

            prodMap.clear();
            prodMap.put(keysToAdd[0], new CacheItem(products[0]));
            prodMap.put(keysToAdd[1], new CacheItem(products[1]));
            prodMap.put(keysToAdd[2], new CacheItem(products[2]));
            prodMap.put(keysToAdd[3], new CacheItem(products[3]));

            result = cache.insertBulk(prodMap);

            if (result.isEmpty())
            {
                System.out.println("All items successfully added/updated in cache.");
            }
            else
            {
                System.out.println("One or more items could not be added to the cache. Iterate hashmap for details.");
                //This is how to iterate the hashmap
                for (Exception ex : result.values()) {
                    System.out.println(ex.getMessage());
                }
            }

            items = cache.getBulk(Arrays.asList(keysToAdd), Product.class);
            if (!items.isEmpty())
            {
                for (Object o : items.values()) {
                    Product product = (Product) o;
                    printProductDetails(product);
                }
            }

            //Remove Bulk operation
            //Remove returns all the items removed from the cache in them form of Hashmap
            items = cache.removeBulk(Arrays.asList(keysToAdd), Product.class);

            if (items.isEmpty())
            {
                System.out.println("No items removed from the cache against the provided keys.");
            }
            else
            {
                System.out.println("Following products have been removed from the cache:");
                //This is how to iterate the hashmap
                for (Object o : items.values()) {
                    Product product = (Product) o;
                    printProductDetails(product);
                }
            }

            //Delete Bulk from Cache is same as Remove
            //Delete bulk does not return anything 

            cache.deleteBulk(Arrays.asList(keysToAdd));
            System.out.println("Cache contains " + cache.getCount() + " items.");

            //Must dispose cache 
            cache.close();

            System.exit(0);

    }
    
    static void printProductDetails(Product product)
    {        
        System.out.println("Id:       " + product.getId());            
        System.out.println("Name:     " + product.getName());            
        System.out.println("Class:    " + product.getClassName());            
        System.out.println("Category: " + product.getCategory());       
        System.out.println();
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = BulkOperations.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
