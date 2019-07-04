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

import com.alachisoft.ncache.runtime.caching.NamedTagsDictionary;
import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;
import java.util.HashMap;
import java.util.Iterator;

public class NamedTags {
    public static void runNamedTagsDemo()
    {
        try
        {
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();
            
            NamedTagsDictionary namedTagDict = new NamedTagsDictionary();
            
            namedTagDict.add("Department", "Marketing");
            namedTagDict.add("EmployeeCount", 40);
            
            cache.add("EmployeeID:1", "John Samuel", namedTagDict);
            cache.add("EmployeeID:2", "David Parker", namedTagDict);
            
            
            String query = "SELECT $Text$ WHERE this.Department = ? AND this.EmployeeCount > ?";
                                 
            HashMap values = new HashMap();
            values.put("Department", "Marketing");
            values.put("EmployeeCount", 35);
            
            HashMap resultItems = cache.searchEntries(query, values);
            
            if ( !resultItems.isEmpty() )
            {
                Iterator iter = resultItems.values().iterator();
                while( iter.hasNext() )
                {
                    System.out.println(iter.next().toString());                                                                     
                }                
            }         
            
            //Must dispose cache
            cache.dispose();

        }
        catch(Exception ex)
        {
            System.out.println(ex.getMessage());
        }
    }        
}
