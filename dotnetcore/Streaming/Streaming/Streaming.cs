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

using Alachisoft.NCache.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that demonstrate the functionality of the NCache Streaming API.
    /// </summary>
    public class Streaming
    {
        private static int _bufferSize = 1024;//--- ONE(1) KB

        /// <summary>
        /// Executing this method will perform the operations of the sample using streaming api.
        /// Streaming allows to read data from cache in chunks just like any buffered stream.
        /// </summary>
        public static void Run()
        {
            var bufferSize = ConfigurationManager.AppSettings["BufferSize"];
            if (string.IsNullOrEmpty(bufferSize) == false)
                _bufferSize = Convert.ToInt32(bufferSize);

            // Initialize Cache 
            using (var cache = InitializeCache())
            {
                var path = ConfigurationManager.AppSettings["DocsFolder"];
                // Get all .PDF files from the given path
                var files = Directory.GetFiles(path, "*.pdf");

                IList<string> keys = new List<string>();
                foreach (var file in files)
                {
                    // Read the content of the file to store it in cache
                    using (FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read))
                    {
                        // Use the FileName as cache key ...
                        var key = Path.GetFileName(fileStream.Name);

                        // Write the given fileStream in cache
                        StoreCacheStream(cache, key, fileStream);

                        keys.Add(key);
                    }
                }

                // Get the cached streams using NCache Streaming API
                foreach (var key in keys)
                {
                    ReadCacheStream(cache, key);
                }
            }
        }

        /// <summary>
        /// This method initializes the cache.
        /// </summary>
        private static ICache InitializeCache()
        {
            string cacheName = ConfigurationManager.AppSettings["CacheName"];
            if (String.IsNullOrEmpty(cacheName))
            {
                throw new Exception("The CacheName cannot be null or empty.");
            }

            // Initialize an instance of the cache to begin performing operations:
            var cache = CacheManager.GetCache(cacheName);
            cache.Clear();

            Console.WriteLine("Cache [" + cache.ToString() + "] initialized successfully.");
            return cache;
        }

        /// <summary>
        /// This method writes stream in cache using CacheStream.Write()
        /// </summary>
        /// <param name="cache">The cache instance in which stream will be cached.</param>
        /// <param name="key">The key against which stream will be written.</param>
        /// <param name="fileStream">FileStream that will be stored in the cache.</param>
        private static void StoreCacheStream(ICache cache, string key, FileStream fileStream)
        {
            var bytesCount = 0;
            var buffer = new byte[_bufferSize];

            if (fileStream.CanRead)
            {
                CacheStreamAttributes cacheStreamAttributes = new CacheStreamAttributes(StreamMode.Write);
                //cacheStreamAttributes.Group
                //cacheStreamAttributes.Expiration
                //cacheStreamAttributes.CacheItemPriority;

                using (CacheStream caceheStream = cache.GetCacheStream(key, cacheStreamAttributes))
                {
                    if (fileStream.CanRead)
                    {
                        while (bytesCount < fileStream.Length)
                        {
                            var bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                            //--- Now write/store these bytes in the cache ...
                            caceheStream.Write(buffer, 0, bytesRead);

                            bytesCount += bytesRead;
                        }
                    }
                }
            }

            Console.WriteLine(key + " file content stored as CacheStream. Total bytes: " + bytesCount);
        }

        /// <summary>
        /// This method fetches stream from the cache using CacheStream.Read().
        /// </summary>
        /// <param name="cache">The cache instance from which stream will be retrieve.</param>
        /// <param name="key">The key of the stream that needs to be fetched from the cache.</param>
        private static void ReadCacheStream(ICache cache, string key)
        {
            int readByteCount = 0;
            byte[] readBuffer = new byte[_bufferSize];

            // StramMode.Read allows only simultaneous reads but no writes!
            // StramMode.ReadWithoutLock allows simultaneous reads and also let the stream be writeable!
            CacheStreamAttributes cacheStreamAttributes = new CacheStreamAttributes(StreamMode.Read);

            using (var cacheStream = cache.GetCacheStream(key, cacheStreamAttributes))
            {
                // Now you have stream perform operations on it just like any regular stream.
                if (cacheStream.CanRead)
                {
                    while (readByteCount < cacheStream.Length)
                    {
                        var bytesRead = cacheStream.Read(readBuffer, 0, readBuffer.Length);

                        readByteCount += bytesRead;
                        //--- readBuffer is available for further processing/manipulation 
                        //--- valid readBuffer contents (readBuffer[0] to readBuffer[bytesRead])
                    }
                }
            }

            Console.WriteLine(key + " file content read as CacheStream. Total bytes: " + readByteCount);
        }
    }
}