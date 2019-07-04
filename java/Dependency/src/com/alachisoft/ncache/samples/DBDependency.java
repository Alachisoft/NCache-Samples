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

import com.alachisoft.ncache.runtime.CacheItemPriority;
import com.alachisoft.ncache.runtime.dependencies.CacheDependency;
import com.alachisoft.ncache.runtime.dependencies.DBDependencyFactory;
import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;

public class DBDependency
{

    public static void runDBDependencyDemo()
    {
        try
        {
            //Initialize cache

            Cache cache;
            cache = NCache.initializeCache("mycache");
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
                CacheDependency dependency = DBDependencyFactory.CreateOleDbCacheDependency(
                        connectionString,
                        Integer.toString(i) + ":oldRow.Products");
                cache.insert(i + ":oldRow.Products",
                             i, dependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default);
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
            cache.dispose();
                        

        }
        catch (Exception ex)
        {
            System.out.print(ex.getMessage());
        }
    }
}
