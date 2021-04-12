// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Oracle Dependency Sample.
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
import com.alachisoft.ncache.runtime.dependencies.CacheDependency;
import com.alachisoft.ncache.runtime.dependencies.OracleCacheDependency;


public class OracleDependency {
    
    public static void runOracleDependencyDemo()            
    {
        try
        {
            //Initialize cache

            Cache cache;
            cache = CacheManager.getCache("mycache");
            cache.clear();
            
            //change notification previleges must be granted for OracleDBDependency to work
            //-- run this script
            //-- 'grant change notification to username' without quotes where username 
            //is to replaced with one being used by the db instance
            
            //Notification based DBDependency
            //JDBC connectionString                         
            
            String connectionString = "jdbc:oracle:thin:SCOTT/diyatech@//20.200.20.54/orcl.diyatech.org.pk";
            String commandText = "SELECT PRODUCTID, rowId, PRODUCTNAME FROM PRODUCTS WHERE PRODUCTID = 1";
            
            CacheDependency oracleDependency = new OracleCacheDependency(connectionString, commandText);                        
            
            CacheItem cacheItem = new CacheItem("OracleDependentValue");
            cacheItem.setDependency(oracleDependency);
            
            cache.add("Item:1", cacheItem);            
                       
            Thread.sleep(20000);
            
            if ( cache.contains("Item:1") )
                System.out.println("Oracle dependency did not work as expected. "
                        + "Check your connection string and sql command syntax.");
            else
                System.out.println("Item:1 removed due to oracle dependency.");
            
            //Must dispose cache
            cache.close();

                                    
        }
        catch(Exception ex)
        {
            System.out.print(ex.getMessage());            
        }        
    }
    
}
