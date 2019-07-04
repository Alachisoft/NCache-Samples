// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Configuration;
using Alachisoft.NCache.Client;
using System.Collections.Generic;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Client.DataTypes.Counter;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the sample
    /// </summary>
    public class DistributedCounter
    {
        private static ICache _cache;
        private static ICounter _counter;
        private static readonly string _counterName = "DistributedCounter";

        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run()
        {
            // Initialize cache
            InitializeCache();

            // Create or get counter
            _counter = GetOrCreateCounter();

            Console.WriteLine("\n--- Set Counter value ---");
            // set counter value
            long counterValue = _counter.SetValue(10);

            // output counter value
            Console.WriteLine("Counter value set to {0}", counterValue);
            Console.WriteLine();

            Console.WriteLine("\n--- Get Counter value ---");
            // get counter value
            counterValue = _counter.Value;

            // output counter value
            Console.WriteLine("Counter Value: {0}", counterValue);
            Console.WriteLine();

            Console.WriteLine("\n--- Increment Counter ---");
            // increment counter value
            counterValue = _counter.Increment();

            // output counter value
            Console.WriteLine("Counter value incremented by 1");
            Console.WriteLine("New Counter value is {0}", counterValue);
            Console.WriteLine();

            Console.WriteLine("\n--- Increment Counter by value ---");
            // increment counter by 5
            counterValue = _counter.IncrementBy(5);
            
            // output counter value
            Console.WriteLine("Counter value incremented by {0}", 5);
            Console.WriteLine("New Counter value is {0}", counterValue);
            Console.WriteLine();

            Console.WriteLine("\n--- Decrement Counter ---");
            // decrement counter
            counterValue = _counter.Decrement();

            // output counter value
            Console.WriteLine("Counter value decremented by 1");
            Console.WriteLine("New Counter value is {0}", counterValue);
            Console.WriteLine();

            // decrement counter by value
            Console.WriteLine("\n--- Decrement Counter by value ---");
            // store decremented counter value in local variable
            counterValue = _counter.DecrementBy(2);

            // output counter value
            Console.WriteLine("Counter value decremented by {0}", 2);
            Console.WriteLine("New Counter value is {0}", counterValue);
            Console.WriteLine();

            // output counter value
            Console.WriteLine("\n--- Display Counter Value ---");
            Console.WriteLine("Counter Value: {0}", _counter.Value);
            Console.WriteLine();

            // Remove the distributed counter from cache
            Console.WriteLine("\n--- Remove Counter from Cache --- ");
            _cache.DataTypeManager.Remove(_counterName);
            Console.WriteLine();

            // Dispose the cache once done
            _cache.Dispose();
        }

        #region Counter functions

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Client.CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }

        /// <summary>
        /// Creates a distributed counter to which instances of customers are to be added
        /// </summary>
        private static ICounter GetOrCreateCounter()
        {
            ICounter counter = _cache.DataTypeManager.GetCounter(_counterName);

            if(counter == null)
            {
                DataTypeAttributes attributes = new DataTypeAttributes
                {
                    Expiration = new Expiration(ExpirationType.Absolute, new TimeSpan(0, 1, 0))
                };

                // Creating distributed counter with absolute expiration
                // to modify cache properties of the counter, provide an instance of DataTypeAttributes in the second parameter
                counter = _cache.DataTypeManager.CreateCounter(_counterName, attributes);
            }

            return counter;
        }

        #endregion
    }
}
