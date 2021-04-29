// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Events sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.events.EventDataFilter;
import com.alachisoft.ncache.runtime.events.EventType;

import java.io.*;
import java.util.EnumSet;
import java.util.Properties;

public class Events {

    public static void run() throws Exception {

        //Initialize cache
        Properties properties = getProperties();
        System.out.println("Initializing Cache");
        String cacheName = properties.getProperty("CacheID");
        Cache cache = CacheManager.getCache(cacheName);
        System.out.println("Connected to cache: " + cacheName);
        cache.clear();

        //Event notifications must be enabled in NCache manager->Options for events to work
        EnumSet<EventType> e = EnumSet.allOf(EventType.class);
        cache.getMessagingService().addCacheNotificationListener(new CacheDataModificationImpl(), e, EventDataFilter.Metadata);

        //See output

        System.out.println("Adding item in cache.");
        cache.add("Item:1", "Value:1");
        Thread.sleep(2000);
        //Key based notifications allows item notifications for particular
        //items in cache
        //Registering key based notifications involves
        //Implementing CacheItemRemovedCallback Interface
        //Implementing CacheItemUpdatedCallback Interface
        //Implementations are included in sample
        CacheDataModificationImpl cacheDataModification = new CacheDataModificationImpl();
        cache.getMessagingService().addCacheNotificationListener("Item:1", cacheDataModification, e, EventDataFilter.Metadata);
        //See output

        System.out.println("Updating item in cache.");
        cache.insert("Item:1", "Value:1-------Changed for notifications");
        Thread.sleep(2000);
        //See output

        System.out.println("deleting item in cache.");
        cache.delete("Item:1");
        Thread.sleep(2000);
        //Must dispose cache
        cache.close();

        Thread.sleep(10000);

        System.exit(0);

    }

    /**
     * This method returns property file
     */
    private static Properties getProperties() throws IOException {
        String path = "config.properties";
        InputStream inputStream = Events.class.getClassLoader().getResourceAsStream(path);
        Properties properties = new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
