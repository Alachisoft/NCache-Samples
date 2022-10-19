// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Streaming API Sample
// ===============================================================================
// Copyright © Alachisoft. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.*;

import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

/*
 * Class that demonstrate the functionality of the NCache Streaming API.
 * NCache Streaming API allows to read & write data from cache in chunks just like any buffered stream
 */
public class StreamingAPI
{
    private static int _bufferSize = 1024;//--- ONE(1) KB
    private static Properties properties;

    public static void main(String[] args)
    {
        try
        {
            //--- Get the application settings from .properties file
            properties = getProperties("config.properties");

            // Initialize the cache.
            try (Cache cache = initializeCache())
            {
                _bufferSize = Integer.parseInt(properties.getProperty("BufferSize"));

                var path = properties.getProperty("DocsFolder");
                if (path == null || path.isEmpty())
                    throw new IllegalArgumentException("'DocsFolder' is not specified or its value is null or empty!");

                List<String> keys = new ArrayList<String>();
                // Get all .PDF files from the given path
                var files = (new File(path)).listFiles(getFileNameFilter(".pdf"));
                for (File file : files)
                {
                    // Read the content of the file to store the file in cache ...
                    try (FileInputStream fileStream = new FileInputStream(file))
                    {
                        var key = file.getName();

                        // Write the given fileStream in cache
                        storeCacheStream(cache, key, fileStream);

                        keys.add(key);
                    }
                }

                // Read the data inserted using streaming api.
                for (String key : keys)
                {
                    readCacheStream(cache, key);
                }
            }
        }
        catch (Exception ex)
        {
            System.out.println(ex.getMessage());
        }
    }

    /**
     * This method initializes the cache.
     */
    private static Cache initializeCache() throws Exception
    {
        String cacheName = properties.getProperty("CacheName");
        if (cacheName == null || cacheName.isEmpty())
        {
            throw new IllegalArgumentException("The CacheName cannot be null or empty.");
        }

        // Initialize an instance of the cache to begin performing operations:
        Cache cache = CacheManager.getCache(cacheName);
        cache.clear();

        System.out.println("Cache [" + cache.toString() + "] initialized successfully!");
        return cache;
    }

    /**
     * This method writes stream in cache using CacheStream.Write()
     */
    private static void storeCacheStream(Cache cache, String key, FileInputStream fileStream) throws Exception
    {
        var bytesCount = 0;
        var buffer = new byte[_bufferSize];

        CacheStreamAttributes cacheStreamAttributes = new CacheStreamAttributes(StreamMode.Write);
        //cacheStreamAttributes.setGroup();
        //cacheStreamAttributes.setExpiration();
        //cacheStreamAttributes.setCacheItemPriority();

        try (CacheStream caceheStream = cache.getCacheStream(key, cacheStreamAttributes))
        {
            while (fileStream.available() > 0)
            {
                var bytesRead = fileStream.read(buffer, 0, buffer.length);

                //--- Now write/store these bytes in the cache ...
                caceheStream.write(buffer, 0, bytesRead);

                bytesCount += bytesRead;
            }
        }

        System.out.println(key + " file content stored as CacheStream. Total bytes: " + bytesCount);
    }

    /**
     * This method fetches stream from the cache using CacheStream.Read().
     */
    private static void readCacheStream(Cache cache, String key) throws Exception
    {
        int readByteCount = 0;
        byte[] readBuffer = new byte[_bufferSize];

        // StramMode.Read allows only simultaneous reads but no writes!
        // StramMode.ReadWithoutLock allows simultaneous reads and also let the stream be writeable!
        CacheStreamAttributes cacheStreamAttributes = new CacheStreamAttributes(StreamMode.Read);

        try (CacheStream cacheStream = cache.getCacheStream(key, cacheStreamAttributes))
        {
            //int bytesRead = -1;
            while (readByteCount < cacheStream.length())
            {
                var bytesRead = cacheStream.read(readBuffer, 0, readBuffer.length);

                readByteCount += bytesRead;
                //--- readBuffer is available for further processing/manipulation
                //--- valid readBuffer contents (readBuffer[0] to readBuffer[bytesRead])
            }
        }

        System.out.println(key + " file content read as CacheStream. Total bytes: " + readByteCount);
    }

    /**
     * This method returns property file
     */
    private static Properties getProperties(String path) throws IOException
    {
        InputStream inputStream = StreamingAPI.class.getClassLoader().getResourceAsStream(path);
        Properties properties = new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }

    private static FilenameFilter getFileNameFilter(String filterString)
    {
        FilenameFilter filter = new FilenameFilter() {
            @Override
            public boolean accept(File file, String fileName) {
                return fileName.endsWith(filterString);
            }
        };

        return  filter;
    }
}
