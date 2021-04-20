// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache NamedTags sample class
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
import com.alachisoft.ncache.samples.data.Product;

import java.io.*;
import java.util.Properties;

public class NamedTags {
    public static void runNamedTagsDemo()
    {
        try
        {
            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();
            
            NamedTagsDictionary namedTagDict = new NamedTagsDictionary();
            
            namedTagDict.add("Category", "Electronics");
            namedTagDict.add("Price", 400);

            Product product1 = new Product("Product:CellularPhoneHTC", "HTCPhone", "Electronics", "Mobiles");
            Product product2 = new Product("Product:CellularPhoneNokia", "NokiaPhone", "Electronics", "Mobiles");


            CacheItem cacheItem1 = new CacheItem(product1);
            cacheItem1.setNamedTags(namedTagDict);
            cache.add(product1.getId(), cacheItem1);

            CacheItem cacheItem2 = new CacheItem(product2);
            cacheItem2.setNamedTags(namedTagDict);
            cache.add(product2.getId(), cacheItem2);

            
            String query = "SELECT $Value$ FROM com.alachisoft.ncache.samples.data.Product WHERE Category = ? AND Price > ?";

            QueryCommand command = new QueryCommand(query);
            command.getParameters().put("Category", "Electronics");
            command.getParameters().put("Price", 350);

           CacheReader reader = cache.getSearchService().executeReader(command);
            
            if ( reader.getFieldCount() != 0 )
            {
                while( reader.read() )
                {
                    Product product = reader.getValue(1, Product.class);
                    System.out.println(product.toString());
                }                
            }         
            
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
        InputStream inputStream = NamedTags.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
