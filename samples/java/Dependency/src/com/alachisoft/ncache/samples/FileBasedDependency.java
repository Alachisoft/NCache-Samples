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

import com.alachisoft.ncache.runtime.CacheItemPriority;
import com.alachisoft.ncache.runtime.dependencies.FileDependency;
import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;

public class FileBasedDependency {
    
    public static void runFileBasedDependencyDemo()
    {
        try
        {
            //Initialize cache
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();
                        
            String filename = System.getenv("NCHOME") + "samples\\dependency\\dist\\foobar.txt";
            
            //adding item dependent on file 
            cache.add("Key-1", 
                    "Item dependent on file...",
                    new FileDependency(filename),
                    Cache.NoAbsoluteExpiration,
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal);                        
            
            //Change file samples\\dependency\\dist\\foobar.txt meanwhile program awaits...
            
            Thread.sleep(5000);            
                        
            //... and then check for its existence
            Object item = cache.get("Key-1");
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
            cache.dispose();
                        
        }
        catch(Exception ex)
        {
            System.out.println(ex.getMessage());
        }            
    }
    
}
