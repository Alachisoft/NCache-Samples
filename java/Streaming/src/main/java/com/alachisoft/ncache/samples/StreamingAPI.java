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


import com.alachisoft.ncache.client.*;

import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.Properties;

public class StreamingAPI
{

    public static void main(String[] args)
    {

        try
        {
            //Streaming allows to read data from cache in chunks just like any buffered stream
            //Initialize cache

            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();

            cache.add("StreamingObject:1", new Object[]
            {
                "StreamingObject:Value"
            });

            //StramMode.Read allows only simultaneous reads but no writes!

            CacheStream stream = cache.getCacheStream("StreamingObject:1", new CacheStreamAttributes(StreamMode.Read));

            //Now you have stream perform operations on it just like any regular stream.

            //Must dispose cache
            cache.close();

            System.exit(0);
        }
        catch (Exception ex)
        {
            System.out.println(ex.getMessage());
            System.exit(0);
        }

    }

    /**
     This method returns property file
     */
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
