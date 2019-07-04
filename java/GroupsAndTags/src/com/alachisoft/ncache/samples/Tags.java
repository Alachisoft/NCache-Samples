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

import com.alachisoft.ncache.runtime.caching.Tag;
import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;


public class Tags {
    
    public static void runTagsDemo()
    {
        try
        {
            
            System.out.println();
            
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();
            
            //Adding items with named Tags       
            //These tags are used to identify products who are ISOCertified, QCPassed and fall in ClassA
            Tag[] tagList = new Tag[3];
            tagList[0] = new Tag("ISOCertified");
            tagList[1] = new Tag("QCPassed");
            tagList[2] = new Tag("ClassA");
            
            //4 Items are added to the cache
            cache.add("Product:MobilePhone1", "ProductID: XYZ", tagList);
            cache.add("Product:MobilePhone2", "ProductID: ABC", tagList);
            cache.add("Product:MobilePhone3", "ProductID: 123");
            cache.add("Product:MobilePhone4", "ProductID: 456");
            
            //Retrieve items who are QCPassed
            Tag itemTag = new Tag("QCPassed");
            
            HashMap items = cache.getByTag(itemTag);
            if ( !items.isEmpty() )
            {
                Iterator iter = items.values().iterator();
                while(iter.hasNext())
                {
                    System.out.println(iter.next().toString());
                }
            }
            
            //Get keys by tags
            //Here keys can be retrived from the cache via following three methods
            
            //Retrives keys by specified tag
            Collection keysByTag = cache.getKeysByTag(tagList[0]); 
            
            //Retrieves those keys only where complete tagList matches
            Collection keysByEntireTagList = cache.getKeysByAllTags(tagList); 
            
            //Retrieves keys where any item in tagList matches
            Collection keysByAnyTagInList = cache.getKeysByAnyTag(tagList);
                        
            //Get Data by tags
            //Here values can be retrived from the cache via following three methods
                        
            //Retrives values by specified tag
            HashMap valuesByTag = cache.getByTag(tagList[0]);
            
            //Retrieves those values only where complete tagList matches
            HashMap valuesByEntireTagList = cache.getByAllTags(tagList);
            
            //Retrivies values where any item in tagList matches
            HashMap valuesByAnyTagInList = cache.getByAnyTag(tagList);
            
            //Remove items from Cache by tags
            
            //Removes values by specified tag
            cache.removeByTag(tagList[0]);
            
            //Removes those values only where complete tagList matches
            cache.removeByAllTags(tagList);
            
            //Removes values where any item in tagList matches
            cache.removeByAnyTag(tagList);
                                    
            System.out.println();
            
            //Must dispose cache
            cache.dispose();

        }
        catch(Exception ex)
        {
            
        }
    }
    
}
