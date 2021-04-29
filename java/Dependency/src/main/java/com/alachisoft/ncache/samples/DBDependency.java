// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Database Dependency sample
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
import com.alachisoft.ncache.runtime.dependencies.CacheDependency;
import com.alachisoft.ncache.runtime.dependencies.DBDependencyFactory;

import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.Properties;

public class DBDependency
{

    public static void runDBDependencyDemo()
    {
        try
        {
            //Initialize cache

            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();

            //Scripts are provided to enable 
            //DBNotifications to NCache

            //Notification based DBDependency
            //JDBC connectionString                         

            String connectionString = "jdbc:oracle:thin:SCOTT/diyatech@//20.200.20.54/orcl.diyatech.org.pk";
            //String commandText = "SELECT PRODUCTID, rowId, PRODUCTNAME FROM PRODUCTS WHERE PRODUCTID = 1";

            for (int i = 0; i < 10; i++)
            {
                //Cache key for product id 10 will be "10:dbo.Products"
                CacheDependency dependency = DBDependencyFactory.createOleDbCacheDependency(
                        connectionString,
                        i + ":oldRow.Products");

                CacheItem cacheItem = new CacheItem("Product 1");
                cacheItem.setCacheItemPriority(CacheItemPriority.Default);
                cacheItem.setDependency(dependency);


                cache.insert(i + ":oldRow.Products", cacheItem);
            }

            System.out.println("Update product with id 6 in database and press enter...");
            System.in.read();

            //Demo is specified for cache_key 6:oldRow.Products 
            if (cache.contains("6:oldRow.Products"))
            {
                System.out.println("Oracle dependency did not work as expected. "
                        + "Check your connection string and sql command syntax.");
            }
            else
            {
                System.out.println("6:oldRow.Products removed due to oracle dependency.");
            }

            //Must dispose cache
            cache.close();
                        

        }
        catch (Exception ex)
        {
            System.out.print(ex.getMessage());
        }
    }

    private static Properties getProperties() throws IOException
    {
        String path = System.getProperty("user.dir")
                + File.separator
                + "src"
                + File.separator
                + "main"
                + File.separator
                + "resources"
                + File.separator
                + "config.properties";
        FileReader reader=new FileReader(path);
        Properties properties=new Properties();
        properties.load(reader);
        return properties;
    }
}
