// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Tags sample class.
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
import com.alachisoft.ncache.client.TagSearchOptions;
import com.alachisoft.ncache.runtime.caching.Tag;
import com.alachisoft.ncache.samples.data.Product;

import java.io.*;
import java.util.*;


public class Tags {
    
    public static void runTagsDemo()
    {
        try
        {
            
            System.out.println();

            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();
            
            //Adding items with named Tags       
            //These tags are used to identify products who are ISOCertified, QCPassed and fall in ClassA
            Tag[] tagList = new Tag[3];
            tagList[0] = new Tag("ISOCertified");
            tagList[1] = new Tag("QCPassed");
            tagList[2] = new Tag("ClassA");

            //4 Items are added to the cache
            Product product1 = new Product();
            product1.setId("Product:MobilePhone1");
            product1.setClassName("XYZ");
            CacheItem cacheItem1 = new CacheItem(product1);
            cacheItem1.setTags(Arrays.asList(tagList));
            cache.add("Product:MobilePhone1", cacheItem1);

            Product product2 = new Product();
            product1.setId("Product:MobilePhone2");
            product1.setClassName("ABC");
            CacheItem cacheItem2 = new CacheItem(product2);
            cacheItem2.setTags(Arrays.asList(tagList));
            cache.add("Product:MobilePhone2", cacheItem2);

            Product product3 = new Product();
            product1.setId("Product:MobilePhone3");
            product1.setClassName("PQR");
            CacheItem cacheItem3 = new CacheItem(product3);
            cache.add("Product:MobilePhone3", cacheItem3);

            Product product4 = new Product();
            product1.setId("Product:MobilePhone4");
            product1.setClassName("DEF");
            CacheItem cacheItem4 = new CacheItem(product4);
            cache.add("Product:MobilePhone4", cacheItem4);

            
            //Retrieve items who are QCPassed
            Tag itemTag = new Tag("QCPassed");
            
            Map<String, Object> items = cache.getSearchService().getByTag(itemTag);
            if ( !items.isEmpty() )
            {
                for (Object o : items.values()) {
                   Product product = (Product) o;
                    System.out.println(product.toString());
                }
            }
            
            //Get keys by tags
            //Here keys can be retrived from the cache via following three methods
            
            //Retrives keys by specified tag
            Collection<String> keysByTag = cache.getSearchService().getKeysByTag(tagList[0]);
            
            //Retrieves those keys only where complete tagList matches
            Collection<String> keysByEntireTagList = cache.getSearchService().getKeysByTags(Arrays.asList(tagList), TagSearchOptions.ByAllTags);
            
            //Retrieves keys where any item in tagList matches
            Collection<String> keysByAnyTagInList = cache.getSearchService().getKeysByTags(Arrays.asList(tagList), TagSearchOptions.ByAnyTag);
                        
            //Get Data by tags
            //Here values can be retrived from the cache via following three methods
                        
            //Retrives values by specified tag
            Map<String, Object> valuesByTag = cache.getSearchService().getByTag(tagList[0]);
            
            //Retrieves those values only where complete tagList matches
            Map<String, Object> valuesByEntireTagList = cache.getSearchService().getByTags(Arrays.asList(tagList), TagSearchOptions.ByAllTags);
            
            //Retrivies values where any item in tagList matches
            Map<String, Object> valuesByAnyTagInList = cache.getSearchService().getByTags(Arrays.asList(tagList), TagSearchOptions.ByAnyTag);
            
            //Remove items from Cache by tags
            
            //Removes values by specified tag
            cache.getSearchService().removeByTag(tagList[0]);
            
            //Removes those values only where complete tagList matches
            cache.getSearchService().removeByTags(Arrays.asList(tagList), TagSearchOptions.ByAllTags);

            //Removes values where any item in tagList matches
            cache.getSearchService().removeByTags(Arrays.asList(tagList), TagSearchOptions.ByAnyTag);
                                    
            System.out.println();
            
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
        InputStream inputStream = Tags.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
    
}
