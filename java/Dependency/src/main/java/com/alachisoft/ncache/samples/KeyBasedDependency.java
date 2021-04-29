// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Key Based Dependency Sample.
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
import com.alachisoft.ncache.runtime.CacheItemPriority;
import com.alachisoft.ncache.runtime.dependencies.KeyDependency;

import java.io.*;
import java.util.Arrays;
import java.util.Properties;

public class KeyBasedDependency {

    public static void runKeyBasedDependencyDemo() {
        try {
            //Initialize cache
            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();

            //Key dependency makes an item to get removed when
            //it's associated item is removed or has it's value changed.

            cache.add("KeyItem1", "This is item 1");

            CacheItem cacheItem1 = new CacheItem();
            cacheItem1.setCacheItemPriority(CacheItemPriority.Normal);
            cacheItem1.setDependency(new KeyDependency("KeyItem1"));
            cacheItem1.setValue("This is item 2");

            cache.add("KeyItem2",
                    cacheItem1);

            cache.insert("KeyItem1", "This is item 1 updated");

            Object value = cache.get("KeyItem2", Object.class);

            if (value == null) {
                System.out.println("Dependent item is successfully removed from cache.");
            } else {
                System.out.println("Error while removing the dependent items from cache.");
            }

            //An item can be dependent on multiple keys!
            //All the keys specified for dependecy must exist in cache

            cache.add("KeyItem2", "This is item 2");
            CacheItem cacheItem3 = new CacheItem();
            cacheItem3.setCacheItemPriority(CacheItemPriority.Normal);
            cacheItem3.setDependency(new KeyDependency(Arrays.asList("KeyItem1", "KeyItem2")));
            cacheItem3.setValue("This is item 3");


            cache.add("KeyItem3", cacheItem3);

            cache.insert("KeyItem2", "This is item 2 updated");

            value = cache.get("KeyItem3", Object.class);

            if (value == null) {
                System.out.println("Dependent item is successfully removed from cache.");
            } else {
                System.out.println("Error while removing the dependent items from cache.");
            }

            //Key dependency is cascaded
            //If A is dependent of B and B is dependent on C
            //Altering C will remove B and A from the cache.

            //Must dispose cache
            cache.close();


        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = KeyBasedDependency.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
