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
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Client.DataTypes;
using Alachisoft.NCache.Client.DataTypes.Collections;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the sample
    /// </summary>
    public class DistributedHashSet
    {
        private static ICache _cache;

        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run()
        {
            // Initialize cache
            InitializeCache();

            // Add values to a hashset
            AddValues();

            // Perform union of numbers using distributed hashset
            PerformUnion();

            // Perform intersection of numbers using distributed hashset
            PerformIntersection();

            // Perform difference of names using distributed hashset to 
            // calculate the complement set of names
            TakeComplement();

            Console.WriteLine("\nPerforming mixed operations now,");
            Console.WriteLine(new string('-', 32));

            // Performs other basic operations of collections on 
            // distributed hashset
            PerformMixedOperations();

            // Dispose the cache once done
            _cache.Dispose();
        }

        #region ---- [Main  Methods] ----

        /// <summary>
        /// This method duplicates how we can add duplicate values to distributed 
        /// hashset and only unique values remain after the operations are 
        /// performed.
        /// </summary>
        private static void AddValues()
        {
            // Create name of hashset to store it against
            const string hashSetName = "DistributedHashSet:UniqueValueHashSet";

            // Create data for hashset
            var daysOfWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            // Get or create a hashset
            var hashSet = GetOrCreate<string>(hashSetName);

            // Add multiple entries to the hashset
            hashSet.AddRange(daysOfWeek);

            // Adding a single entry to the hashset. Since this entry already exists, no change to the hashset will be made.
            hashSet.Add("Monday");

            Console.WriteLine("\nData added to distributed hashset and duplicate values were ignored");

            // Display all values of the hashset
            IterateOverDistributedHashSet(hashSet);
        }

        /// <summary>
        /// This method demonstrates how we can use distributed hashset to 
        /// take union of different distributed hashsets.
        /// </summary>
        private static void PerformUnion()
        {
            // Create names of hashsets to store them against
            const string hashSetNumbersDivisibleBy3Name = "DistributedHashSet:NumbersDivisibleBy3";
            const string hashSetNumbersDivisibleBy5Name = "DistributedHashSet:NumbersDivisibleBy5";
            const string hashSetNumbersDivisibleBy3Or5Name = "DistributedHashSet:NumbersDivisibleBy3Or5";

            // Create data for hashsets
            var numbersDivisibleBy3 = new int[] { 03, 06, 09, 12, 15 };
            var numbersDivisibleBy5 = new int[] { 05, 10, 15, 20, 25 };

            // Get or create hashsets
            var hashSetNumbersDivisibleBy3 = GetOrCreate<int>(hashSetNumbersDivisibleBy3Name);
            var hashSetNumbersDivisibleBy5 = GetOrCreate<int>(hashSetNumbersDivisibleBy5Name);
            var hashSetNumbersDivisibleBy3Or5 = GetOrCreate<int>(hashSetNumbersDivisibleBy3Or5Name);

            // Add data to hashsets
            hashSetNumbersDivisibleBy3.AddRange(numbersDivisibleBy3);
            hashSetNumbersDivisibleBy5.AddRange(numbersDivisibleBy5);

            Console.WriteLine("\nPerforming union of numbers divisible by 3 and numbers divisible by 5");

            // Performs union of two hashsets and returns the result
            IEnumerable<int> numbersDivisibleBy3Or5 = hashSetNumbersDivisibleBy3.Union(hashSetNumbersDivisibleBy5Name);

            // Performs union of two hashsets and stores the result in another distributed hashset
            hashSetNumbersDivisibleBy3.StoreUnion(hashSetNumbersDivisibleBy3Or5Name, hashSetNumbersDivisibleBy5Name);

            // Remove distributed hashsets not needed
            RemoveDistributedHashSet(hashSetNumbersDivisibleBy3Or5);
        }

        /// <summary>
        /// This method demonstrates how we can use distributed hashset to 
        /// take intersection of different distributed hashsets.
        /// </summary>
        private static void PerformIntersection()
        {
            // Create names of hashsets to store them against
            const string hashSetNumbersDivisibleBy3Name = "DistributedHashSet:NumbersDivisibleBy3";
            const string hashSetNumbersDivisibleBy5Name = "DistributedHashSet:NumbersDivisibleBy5";
            const string hashSetNumbersDivisibleBy3And5Name = "DistributedHashSet:NumbersDivisibleBy3And5";

            // Create data for hashsets
            var numbersDivisibleBy3 = new int[] { 03, 06, 09, 12, 15 };
            var numbersDivisibleBy5 = new int[] { 05, 10, 15, 20, 25 };

            // Get or create hashsets
            var hashSetNumbersDivisibleBy3 = GetOrCreate<int>(hashSetNumbersDivisibleBy3Name);
            var hashSetNumbersDivisibleBy5 = GetOrCreate<int>(hashSetNumbersDivisibleBy5Name);
            var hashSetNumbersDivisibleBy3And5 = GetOrCreate<int>(hashSetNumbersDivisibleBy3And5Name);

            // Add data to hashsets
            hashSetNumbersDivisibleBy3.AddRange(numbersDivisibleBy3);
            hashSetNumbersDivisibleBy5.AddRange(numbersDivisibleBy5);

            Console.WriteLine("\nPerforming intersection of numbers divisible by 3 and numbers divisible by 5");

            // Performs intersection of two hashsets and returns the result
            IEnumerable<int> numbersDivisibleBy3And5 = hashSetNumbersDivisibleBy3.Intersect(hashSetNumbersDivisibleBy5Name);

            // Performs intersection of two hashsets and stores the result in another hashset
            hashSetNumbersDivisibleBy3.StoreIntersection(hashSetNumbersDivisibleBy3And5Name, hashSetNumbersDivisibleBy5Name);

            // Remove distributed hashsets not needed
            RemoveDistributedHashSet(hashSetNumbersDivisibleBy3And5);
        }

        /// <summary>
        /// This method demonstrates how we can use distributed hashset to 
        /// take difference of different distributed hashsets.
        /// </summary>
        private static void TakeComplement()
        {
            // Create names of hashsets to store them against
            const string hashSetUniversalNamesName = "DistributedHashSet:UniversalNames";
            const string hashSetNamesComplementName = "DistributedHashSet:NamesComplement";
            const string hashSetNamesWithFourCharactersName = "DistributedHashSet:NamesWithFourCharacters";

            // Create data for hashsets
            var universalNames = new string[]
            {
                "Alejandro", "Alexander", "Aria", "Hanna", "Hari", "Jamie", "Jean", "John", "Jose", "Mario", "Mary"
            };
            var namesWithFourCharacters = new string[]
            {
                "Aria", "Hari", "Jean", "John", "Jose", "Mary"
            };

            // Get or create hashsets
            var hashSetUniversalNames = GetOrCreate<string>(hashSetUniversalNamesName);
            var hashSetNamesComplement = GetOrCreate<string>(hashSetNamesComplementName);
            var hashSetNamesWithFourCharacters = GetOrCreate<string>(hashSetNamesWithFourCharactersName);

            // Add data to hashsets
            hashSetUniversalNames.AddRange(universalNames);
            hashSetNamesWithFourCharacters.AddRange(namesWithFourCharacters);

            Console.WriteLine("\nPerforming difference of names to calculate complement");

            // Performs difference of names set from universal set and returns the result
            IEnumerable<string> namesComplement = hashSetUniversalNames.Difference(hashSetNamesWithFourCharactersName);

            // Performs difference of names set from universal set and stores the result in another hashset
            hashSetUniversalNames.StoreDifference(hashSetNamesComplementName, hashSetNamesWithFourCharactersName);
        }

        /// <summary>
        /// This method demonstrates how we can perform various other basic 
        /// collection operations on distributed hashset.
        /// </summary>
        private static void PerformMixedOperations()
        {
            Console.WriteLine();

            // Create a name for the distributed hashset on which mixed 
            // operations are to be performed
            const string mixedOperationHashSetName = "DistributedHashSet:MixedOperations";

            // Add all data
            MixedOperations("Add", mixedOperationHashSetName, "Lorem");
            MixedOperations("Add", mixedOperationHashSetName, "ipsum");
            MixedOperations("Add", mixedOperationHashSetName, "dolor");
            MixedOperations("Add", mixedOperationHashSetName, "sit");
            MixedOperations("Add", mixedOperationHashSetName, "amet");

            Console.WriteLine();

            // Check the added data
            MixedOperations("Contains", mixedOperationHashSetName, "Lorem");
            MixedOperations("Contains", mixedOperationHashSetName, "ipsum");

            Console.WriteLine();

            // Remove all the data
            MixedOperations("Remove", mixedOperationHashSetName, "Lorem");
            MixedOperations("Remove", mixedOperationHashSetName, "ipsum");
            MixedOperations("Remove", mixedOperationHashSetName, "dolor");
            MixedOperations("Remove", mixedOperationHashSetName, "sit");
            MixedOperations("Remove", mixedOperationHashSetName, "amet");

            Console.WriteLine();

            // Check the data again
            MixedOperations("Contains", mixedOperationHashSetName, "dolor");
            MixedOperations("Contains", mixedOperationHashSetName, "sit");

            // Remove the hashset when no longer needed
            RemoveDistributedHashSet<string>(mixedOperationHashSetName);

            Console.WriteLine();
        }

        /// <summary>
        /// This method demonstrates how we can perform other basic operations on a 
        /// distributed hashset like contains and remove etc.
        /// </summary>
        /// <param name="operationType">Determines what operation to perform.</param>
        /// <param name="hashSetName">Name of hashset on which operation is to be 
        /// performed.</param>
        /// <param name="item">Item to be used as data for operation.</param>
        private static void MixedOperations(string operationType, string hashSetName, string item)
        {
            // Nothing to do if an invalid operation type or item value is received
            if (string.IsNullOrEmpty(operationType) || string.IsNullOrEmpty(item))
                return;

            var hashSet = GetOrCreate<string>(hashSetName);

            switch (operationType.ToLowerInvariant())
            {
                case "add":
                    {
                        // Adding a duplicate item into hashset ignores the duplicate item
                        hashSet.Add(item);

                        // Print success output
                        Console.WriteLine(" - Item '{0,5}' successfully added into hashset '{1}'", item, hashSet.Key);
                    }
                    break;

                case "contains":
                    {
                        // Contains in a hashset is an operation of complexity O(1)
                        if (hashSet.Contains(item))
                        {
                            Console.WriteLine(" - Item '{0,5}' exists in hashset '{1}'", item, hashSet.Key);
                        }
                        else
                        {
                            Console.WriteLine(" - Item '{0,5}' does not exist in hashset '{1}'", item, hashSet.Key);
                        }
                    }
                    break;

                case "remove":
                    {
                        // Remove in a hashset is an operation of complexity O(1)
                        // 
                        // Also, remove returns a boolean value based on which, it 
                        // can be determined whether an item was removed from the 
                        // hashset or not
                        if (hashSet.Remove(item))
                        {
                            Console.WriteLine(" - Item '{0,5}' successfully removed from hashset '{1}'", item, hashSet.Key);
                        }
                        else
                        {
                            Console.WriteLine(" - Item '{0,5}' could not be removed from hashset '{1}'", item, hashSet.Key);
                        }
                    }
                    break;

                default:
                    // Nothing to do if an unknown operation type is received
                    break;
            }
        }

        #endregion

        #region --- [Helping Methods] ---

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache()
        {
            string cacheId = ConfigurationManager.AppSettings["CacheID"];

            if (string.IsNullOrEmpty(cacheId))
            {
                Console.WriteLine("Cache ID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations
            _cache = CacheManager.GetCache(cacheId);

            // Print output on console
            Console.WriteLine("\nCache '{0}' is initialized.", cacheId);
        }

        /// <summary>
        /// <para>
        /// Fetches (or creates if does not exist) a distributed hashset. The hashset created 
        /// through this call is created with an absolute expiration of 1 minute.
        /// </para>
        /// <para>
        /// NOTE : Distributed hashset of primitive data types and string data type ought to be 
        /// created only. Created or getting a distributed hashset of any other type does not throw 
        /// any exception but upon performing operations, exceptions will be thrown accordingly.
        /// </para>
        /// </summary>
        /// <typeparam name="T">Generic type argument for distributed hashset.</typeparam>
        /// <param name="hashSetName">Name against which the distributed hash 
        /// set needed is to be stored.</param>
        /// <returns>Instance of the distributed hashset required.</returns>
        private static IDistributedHashSet<T> GetOrCreate<T>(string hashSetName)
        {
            IDistributedHashSet<T> distributedHashSet = _cache.DataTypeManager.GetHashSet<T>(hashSetName);

            if (distributedHashSet == null)
            {
                // additional properties can be added to the hashset using the DataTypeAttributes class
                // using attributes, properties can be added to the whole hashset
                DataTypeAttributes attributes = new DataTypeAttributes
                {
                    // creating expiration of 1 minute and adding it to attributes
                    Expiration = new Expiration(ExpirationType.Absolute, new TimeSpan(0, 1, 0))
                };

                // Creating distributed hashSet with absolute expiration
                distributedHashSet = _cache.DataTypeManager.CreateHashSet<T>(hashSetName, attributes);
            }

            return distributedHashSet;
        }

        /// <summary>
        /// This method demonstrates how we can iterate over all the values in a distributed hashset.
        /// </summary>
        /// <typeparam name="T">Generic type argument for distributed hashset.</typeparam>
        /// <param name="distributedHashSet">Instance of the distributed hashset to iterate over.</param>
        private static void IterateOverDistributedHashSet<T>(IDistributedHashSet<T> distributedHashSet)
        {
            // If handle for distributed hashset is null, no need to iterate over anything
            if (distributedHashSet == null)
                return;

            // Print information of distributed hashset whose data is to be printed
            Console.WriteLine("\nIterating over '{0}',", distributedHashSet.Key);
            Console.WriteLine(new string('-', 18 + distributedHashSet.Key.Length));

            // Print each item contained in distributed hashset
            foreach (T item in distributedHashSet)
            {
                Console.WriteLine(" - {0}", item);
            }
        }

        /// <summary>
        /// This method removes the provided distributed hash from cache if it exists based on name 
        /// against which the distributed hashset is stored.
        /// </summary>
        /// <typeparam name="T">Generic type argument for distributed hashset.</typeparam>
        /// <param name="hashSetName">Name of instance of the distributed hashset required 
        /// to remove from cache.</param>
        private static void RemoveDistributedHashSet<T>(string hashSetName)
        {
            // No need to remove anything if an invalid hashset name is provided
            if (string.IsNullOrEmpty(hashSetName))
                return;

            // Distributed hashset removed if exists
            RemoveDistributedHashSet(GetOrCreate<T>(hashSetName));
        }

        /// <summary>
        /// This method removes the provided distributed hash from cache if it exists.
        /// </summary>
        /// <typeparam name="T">Generic type argument for distributed hashset.</typeparam>
        /// <param name="distributedHashSet">Instance of the distributed hashset required 
        /// to remove from cache.</param>
        private static void RemoveDistributedHashSet<T>(IDistributedHashSet<T> distributedHashSet)
        {
            // If handle for distributed hashset is null, no need to return anything
            if (distributedHashSet == null)
                return;

            // Distributed hashset removed if exists
            _cache.DataTypeManager.Remove(distributedHashSet.Key);
        }

        #endregion
    }
}
