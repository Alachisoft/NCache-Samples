// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache StreamingAPI sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.CacheStream;
import com.alachisoft.ncache.web.caching.NCache;
import com.alachisoft.ncache.web.caching.StreamMode;

public class StreamingAPI
{

    public static void main(String[] args)
    {

        try
        {
            //Streaming allows to read data from cache in chunks just like any buffered stream
            //Initialize cache

            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();

            cache.add("StreamingObject:1", new Object[]
            {
                "StreamingObject:Value"
            });

            //StramMode.Read allows only simultaneous reads but no writes!

            CacheStream stream = cache.getCacheStream("StreamingObject:1", StreamMode.Read);

            //Now you have stream perform operations on it just like any regular stream.

            //Must dispose cache
            cache.dispose();

            System.exit(0);
        }
        catch (Exception ex)
        {
            System.out.println(ex.getMessage());
            System.exit(0);
        }

    }
}
