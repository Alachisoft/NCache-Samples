// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Client;
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;
using Alachisoft.NCache.Client.DataTypes.Counter;
using Alachisoft.NCache.Licensing.DOM;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;

// ===============================================================================
// Distributed HashSet is a high-performance data structure provided by NCache. It
// stores unique values across multiple cache servers and can be shared safely
// among many applications and clients in a distributed environment. It is
// particularly useful when multiple servers need to maintain a consistent,
// duplicate-free collection of values with low latency. Some possible use cases
// of Distributed HashSet include:
//   1. Tracking unique visitors or active users in real time
//   2. Maintaining sets of processed items to prevent duplicates (e.g. IDs)
//   3. Implementing leaderboards or ranking systems based on unique identifiers
//   4. Managing unique tags, categories, or keywords in collaborative systems
//   5. Coordinating distributed tasks and events to ensure each is only processed
//      once in a distributed environment.
// NOTE: Please only use primitive data types and string data types when creating
// a Distributed Hashset. Using any other types does not throw an exception when
// you create the set. But when you try to perform operations on the set,
// exceptions will be thrown accordingly.
// ===============================================================================

namespace DistributedHashset
{
    internal class Program
    {
        // Cache handle for the connected cache
        static ICache cache;

        static void Main(string[] args)
        {
            // Getting cache handle
            cache = GetCache();

            // Validating cache handle
            if (cache == null)
            {
                Console.WriteLine("Cache connection failed. Exiting the sample.");
                return;
            }

            // Adding values to a hashset
            PerformBasicOps();

            // Performing union of numbers using distributed hashset
            PerformUnion();

            // Performing intersection of numbers using distributed hashset
            PerformIntersection();

            // Performing difference of names using distributed hashset to calculate the complement set of names
            TakeComplement();

            // Disposing the cache
            cache.Dispose();
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to connect to a running cache and return cache handle for it 
        /// </summary>
        /// <returns>Cache handle for the connected cache</returns>
        private static ICache GetCache()
        {
            string cacheName = ConfigurationManager.AppSettings["CacheName"];

            if (String.IsNullOrEmpty(cacheName))
            {
                Console.WriteLine("The CacheName cannot be null or empty.");
                return null;
            }

            // Trying to connect to cache
            try
            {
                // Connecting to a running cache and return a cache handle for it
                ICache cache = CacheManager.GetCache(cacheName);

                // Printing output on console
                Console.WriteLine(string.Format("Cache '{0}' is connected.", cacheName));

                // Returning the connected cache handle
                return cache;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to cache: " + ex.Message);
                return null;
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to perform basic operations of a distributed hashset like add, remove, etc
        /// </summary>
        private static void PerformBasicOps()
        {
            Console.WriteLine("Basic Operations on a distributed hashset");

            // Creating name of hashset to store it against
            const string hashSetName = "HashSet:Workingdays";

            // Creating data for hashset containing working days of the week
            var days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            // Getting or creating a hashset
            var hashSet = GetOrCreate<string>(hashSetName);

            // Adding multiple entries to the hashset
            hashSet.AddRange(days);
            Console.WriteLine("Added multiple entries to distributed hashset");

            // Adding a single entry that already exists has no effect.
            hashSet.Add("Monday");
            Console.WriteLine("Adding duplicate value to distributed hashset was ignored");

            // Adding a single entry that does not exist adds it to the Set
            hashSet.Add("Sunday");
            Console.WriteLine("Added a new value to the distributed hashset");

            // Checking if an entry exists in the hashset
            if (hashSet.Contains("Sunday"))
            {
                // Removing an entry from the hashset
                hashSet.Remove("Sunday");
                Console.WriteLine("Removed an existing value from distributed hashset");
            }

            Console.WriteLine("Data added to distributed hashset; duplicate values were ignored");
            Console.WriteLine();

            // Displaying all values of the hashset
            DisplayHashSet(hashSet);
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate union of different distributed hashsets
        /// </summary>
        private static void PerformUnion()
        {
            // Creating names of hashsets to store them against
            const string numsDivBy3 = "HashSet:NumsDivBy3";
            const string numsDivBy5 = "HashSet:NumsDivBy5";
            const string numsDivBy3Or5 = "HashSet:NumsDivBy3Or5";

            // Creating data for hashsets
            var numDivBy3 = new int[] { 3, 6, 9, 12, 15 };
            var numDivBy5 = new int[] { 5, 10, 15, 20, 25 };

            // Getting or creating hashsets
            var hashSetDivBy3 = GetOrCreate<int>(numsDivBy3);
            var hashSetDivBy5 = GetOrCreate<int>(numsDivBy5);
            var hashSetDivBy3Or5 = GetOrCreate<int>(numsDivBy3Or5);

            // Adding data to hashsets
            hashSetDivBy3.AddRange(numDivBy3);
            hashSetDivBy3.AddRange(numDivBy5);

            Console.WriteLine("Performing union of numbers divisible by 3 and numbers divisible by 5");

            // Performing union of two hashsets and returning the result
            IEnumerable<int> union = hashSetDivBy3.Union(numsDivBy5);

            // Performing union of two hashsets and storing the result in another distributed hashset
            hashSetDivBy3.StoreUnion(numsDivBy3Or5, numsDivBy5);

            // Removing distributed hashsets that are not needed
            RemoveHashSet(hashSetDivBy3Or5);

            Console.WriteLine("Union operation completed.");
            Console.WriteLine();
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate intersection of different distributed hashsets
        /// </summary>
        private static void PerformIntersection()
        {
            // Creating names of hashsets to store them against
            const string numsDivBy3 = "HashSet:NumsDivBy3";
            const string numsDivBy5 = "HashSet:NumsDivBy5";
            const string numsDivBy3Or5 = "HashSet:NumsDivBy3And5";

            // Creating data for hashsets
            var numDivBy3 = new int[] { 3, 6, 9, 12, 15 };
            var numDivBy5 = new int[] { 5, 10, 15, 20, 25 };

            // Getting or creating hashsets
            var hashSetDivBy3 = GetOrCreate<int>(numsDivBy3);
            var hashSetDivBy5 = GetOrCreate<int>(numsDivBy5);
            var hashSetDivBy3Or5 = GetOrCreate<int>(numsDivBy3Or5);

            // Adding data to hashsets
            hashSetDivBy3.AddRange(numDivBy3);
            hashSetDivBy5.AddRange(numDivBy5);

            Console.WriteLine("Performing intersection of numbers divisible by 3 and numbers divisible by 5");

            // Performing intersection of two hashsets and returning the result
            IEnumerable<int> intersection = hashSetDivBy3.Intersect(numsDivBy5);

            // Performing intersection of two hashsets and storing the result in another hashset
            hashSetDivBy3.StoreIntersection(numsDivBy3Or5, numsDivBy5);

            // Removing distributed hashsets not needed
            RemoveHashSet(hashSetDivBy3Or5);

            Console.WriteLine("Intersection operation completed.");
            Console.WriteLine();
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate difference of different distributed hashsets
        /// </summary>
        private static void TakeComplement()
        {
            // Creating names of hashsets to store them against
            const string allNames = "HashSet:AllNames";
            const string complementNames = "HashSet:ComplementNames";
            const string len4Names = "HashSet:Len4Names";

            // Creating data for hashsets
            var allNamesArr = new string[]
                {
            "Alejandro", "Alexander", "Aria", "Hanna", "Hari", "Jamie", "Jean", "John", "Jose", "Mario", "Mary"
                };
            var len4NamesArr = new string[]
                {
            "Aria", "Hari", "Jean", "John", "Jose", "Mary"
                };

            // Getting or creating hashsets
            var hsNamesComplement = GetOrCreate<string>(complementNames);
            var hsNamesOfLen4 = GetOrCreate<string>(len4Names);
            var hsAllNames = GetOrCreate<string>(allNames);

            // Adding data to hashsets
            hsAllNames.AddRange(allNamesArr);
            hsNamesOfLen4.AddRange(len4NamesArr);

            Console.WriteLine("Performing difference of names to calculate complement");

            // Performing difference of names set from universal set and returning the result
            IEnumerable<string> namesDifference = hsAllNames.Difference(len4Names);

            // The result can also be stored in another distributed hashset
            // Performing difference of names set from universal set and storing the result in another hashset
            hsAllNames.StoreDifference(complementNames, len4Names);

            Console.WriteLine("Difference operation completed.");
            Console.WriteLine();
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch (or create if does not exist) a distributed hashset
        /// </summary>
        /// <typeparam name="T">Generic type argument for distributed hashset</typeparam>
        /// <param name="hashSetName">Name against which the distributed hash 
        /// set needed is to be stored</param>
        /// <returns>Instance of the distributed hashset required</returns>
        private static IDistributedHashSet<T> GetOrCreate<T>(string hashSetName)
        {
            // Fetching distributed hashset from cache based on name
            IDistributedHashSet<T> distributedHashSet = cache.DataTypeManager.GetHashSet<T>(hashSetName);

            // Checking if distributed hashset exists in cache
            if (distributedHashSet == null)
            {
                // Additional properties can be added to the hashset using the DataTypeAttributes class
                // Using attributes, properties can be added to the whole hashset
                DataTypeAttributes attributes = new DataTypeAttributes
                {
                    // Creating expiration of 1 minute and adding it to attributes
                    Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(1))
                };

                // Creating distributed hashSet with absolute expiration
                distributedHashSet = cache.DataTypeManager.CreateHashSet<T>(hashSetName, attributes);
            }

            // Returning the distributed hashset
            return distributedHashSet;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to demonstrate how we can iterate over all the values in a distributed hashset
        /// </summary>
        /// <typeparam name="T">Generic type argument for distributed hashset</typeparam>
        /// <param name="distributedHashSet">Instance of the distributed hashset to iterate over</param>
        private static void DisplayHashSet<T>(IDistributedHashSet<T> distributedHashSet)
        {
            // Checking if distributed hashset is null
            if (distributedHashSet == null)
            {
                // Printing message and returning if distributed hashset is null
                Console.WriteLine("Distributed hashset is null; cannot iterate over it.");
                return;
            }

            // Checking if distributed hashset has any data
            if (distributedHashSet.Count == 0)
            {
                // Printing message and returning if distributed hashset has no data
                Console.WriteLine("Distributed hashset '{0}' has no data to iterate over.", distributedHashSet.Key);
                return;
            }
            else
            {
                // Printing information of distributed hashset whose data is to be printed
                Console.WriteLine("Iterating over '{0}',", distributedHashSet.Key);
                Console.WriteLine(new string('-', 18 + distributedHashSet.Key.Length));

                // Printing each item contained in distributed hashset
                foreach (T item in distributedHashSet)
                {
                    Console.WriteLine(" - {0}", item);
                }
            }
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// Method to remove the provided distributed hash from cache if it exists
        /// </summary>
        /// <typeparam name="T">Generic type argument for distributed hashset</typeparam>
        /// <param name="distributedHashSet">Instance of the distributed hashset required 
        /// to remove from cache</param>
        private static void RemoveHashSet<T>(IDistributedHashSet<T> distributedHashSet)
        {
            // Checking if distributed hashset is not null
            if (distributedHashSet != null)
            {
                // Removing distributed hashset if exists in cache
                cache.DataTypeManager.Remove(distributedHashSet.Key);
            }
        }
    }
}
