// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache File Based Dependency sample
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
import com.alachisoft.ncache.runtime.dependencies.FileDependency;

import java.io.*;
import java.util.Properties;

public class FileBasedDependency {
    
    public static void runFileBasedDependencyDemo()
    {
        try
        {
            //Initialize cache
            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();
                        
            String filename = System.getenv("NCHOME") + "java.com.alachisoft.ncache.samples\\dependency\\dist\\foobar.txt";

            CacheItem cacheItem = new CacheItem("cache Item 1");
            cacheItem.setCacheItemPriority(CacheItemPriority.Normal);
            cacheItem.setDependency(new FileDependency(filename));

            //adding item dependent on file 
            cache.add("Key-1", cacheItem);
            
            //Change file java.com.alachisoft.ncache.samples\\dependency\\dist\\foobar.txt meanwhile program awaits...
            
            Thread.sleep(5000);            
                        
            //... and then check for its existence
            Object item = cache.get("Key-1", Object.class);
            if ( item == null )
            {
                System.out.println("Item has been removed due to file dependency.");
            }
            else
            {
                System.out.println("File based dependency did not work. Dependency file located at " + filename + 
                        " might be missing or file not changed within the given interval.");
            }
            
            //Multiple files names can be specified in FileDependency parameter
            
            //Must dispose cache
            cache.close();
                        
        }
        catch(Exception ex)
        {
            System.out.println(ex.getMessage());
        }            
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = FileBasedDependency.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
    
}
