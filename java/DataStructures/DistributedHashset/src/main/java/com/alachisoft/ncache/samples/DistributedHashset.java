/*
 * ===============================================================================
 * Alachisoft (R) NCache Sample Code.
 * NCache Tags & NamedTags Sample (Java)
 * ===============================================================================
 * Copyright © Alachisoft. All rights reserved.
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
 * OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE.
 * ===============================================================================
 */

package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.datastructures.DataStructureAttributes;
import com.alachisoft.ncache.client.datastructures.DistributedHashSet;
import com.alachisoft.ncache.runtime.caching.expiration.Expiration;
import com.alachisoft.ncache.runtime.caching.expiration.ExpirationType;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;

import java.io.IOException;
import java.io.InputStream;
import java.util.Arrays;
import java.util.List;
import java.util.Properties;

/*
 * ===============================================================================
 * Distributed HashSet is a high-performance data structure provided by NCache. It
 * stores unique values across multiple cache servers and can be shared safely
 * among many applications and clients in a distributed environment. It is
 * particularly useful when multiple servers need to maintain a consistent,
 * duplicate-free collection of values with low latency. Some possible use cases
 * of Distributed HashSet include:
 *   1. Tracking unique visitors or active users in real time
 *   2. Maintaining sets of processed items to prevent duplicates (e.g. IDs)
 *   3. Implementing leaderboards or ranking systems based on unique identifiers
 *   4. Managing unique tags, categories, or keywords in collaborative systems
 *   5. Coordinating distributed tasks and events to ensure each is only processed
 *      once in a distributed environment.
 * NOTE: Please only use primitive data types and string data types when creating
 * a Distributed Hashset. Using any other types does not throw an exception when
 * you create the set. But when you try to perform operations on the set,
 * exceptions will be thrown accordingly.
 * ===============================================================================
 */

public class DistributedHashset {

    // Cache handle for the connected cache
    private static Cache _cache;

    public static void main(String[] args) throws Exception {
        // Connect to a running cache and get cache handle for it
        _cache = getCache();

        // Validating cache handle
        if (_cache == null) {
            System.out.println("Cache could not be initialized.");
            return;
        }

        // Adding values to a hashset
        performBasicOps();

        // Performing union of numbers using distributed hashset
        performUnion();

        // Performing intersection of numbers using distributed hashset
        performIntersection();

        // Performing difference of names using distributed hashset to calculate the complement set of names
        takeComplement();

        // Closing the cache handle
        _cache.close();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to connect to a running cache and return cache handle for it
     *
     * @return Cache handle for the connected cache
     */
    private static Cache getCache() throws Exception {
        // Getting cache name from configuration file
        String cacheName = getConfigValue("CacheName");

        // Validating cache name
        if (cacheName == null || cacheName.isEmpty()) {
            System.out.println("The CacheName cannot be null or empty.");
            return null;
        }

        // Trying to connect to the cache
        try {
            // Connecting to a running cache and return a cache handle for it
            Cache cache = CacheManager.getCache(cacheName);

            // Printing output on console
            System.out.printf("Cache '%s' is connected.%n", cacheName);

            // Returning cache handle
            return cache;
        } catch(Exception e) {
            System.out.println("Unable to connect to cache: " + e.getMessage());
            return null;
        }
    }

    // --------------------------------------------------------------------------
    /**
     * Method to perform basic operations of a distributed hashset like add, remove, etc
     */
    private static void performBasicOps() throws CacheException {
        // Printing heading on console
        System.out.println("Basic Operations on a distributed hashset");

        // Creating name of hashset to store it against
        final String hashSetName = "HashSet:Workingdays";

        // Creating data for hashset containing working days of the week
        List<String> days = Arrays.asList("Monday", "Tuesday", "Wednesday", "Thursday", "Friday");

        // Getting or creating a hashset
        DistributedHashSet<String> hashSet = getOrCreate(hashSetName, String.class);

        // Adding multiple entries to the hashset
        hashSet.addRange(days);
        System.out.println("Added multiple entries to distributed hashset");

        // Adding a single entry that already exists has no effect.
        hashSet.add("Monday");
        System.out.println("Adding duplicate value to distributed hashset was ignored");

        // Adding a single entry that does not exist adds it to the Set
        hashSet.add("Sunday");
        System.out.println("Added a new value to the distributed hashset");

        // Checking if an entry exists in the hashset
        if (hashSet.contains("Sunday")) {
            // Removing an entry from the hashset
            hashSet.remove("Sunday");
            System.out.println("Removed an existing value from distributed hashset");
        }

        // Printing status on console
        System.out.println("Data added to distributed hashset; duplicate values were ignored");
        System.out.println();

        // Displaying all values of the hashset
        displayHashSet(hashSet);
    }

    // --------------------------------------------------------------------------
    /**
     * Method to demonstrate union of different distributed hashsets
     */
    private static void performUnion() throws CacheException, IOException {
        // Creating names of hashsets to store them against
        final String numsDivBy3 = "HashSet:NumsDivBy3";
        final String numsDivBy5 = "HashSet:NumsDivBy5";
        final String numsDivBy3Or5 = "HashSet:NumsDivBy3Or5";

        // Creating data for hashsets
        List<Integer> numDivBy3 = Arrays.asList(3, 6, 9, 12, 15);
        List<Integer> numDivBy5 = Arrays.asList(5, 10, 15, 20, 25);

        // Getting or creating hashsets
        DistributedHashSet<Integer> hashSetDivBy3 = getOrCreate(numsDivBy3, Integer.class);
        DistributedHashSet<Integer> hashSetDivBy5 = getOrCreate(numsDivBy5, Integer.class);
        DistributedHashSet<Integer> hashSetDivBy3Or5 = getOrCreate(numsDivBy3Or5, Integer.class);

        // Adding data to hashsets
        hashSetDivBy3.addRange(numDivBy3);
        hashSetDivBy3.addRange(numDivBy5);

        // Printing status on console
        System.out.println("Performing union of numbers divisible by 3 and numbers divisible by 5");

        // Performing union of two hashsets and returning the result
        Iterable<Integer> union = hashSetDivBy3.union(numsDivBy5);

        // Performing union of two hashsets and storing the result in another distributed hashset
        hashSetDivBy3.storeUnion(numsDivBy3Or5, numsDivBy5);

        // Removing distributed hashsets that are not needed
        removeHashSet(hashSetDivBy3Or5);

        // Printing status on console
        System.out.println("Union operation completed.");
        System.out.println();
    }

    // --------------------------------------------------------------------------
    /**
     * Method to demonstrate intersection of different distributed hashsets
     */
    private static void performIntersection() throws CacheException, IOException {
        // Creating names of hashsets to store them against
        final String numsDivBy3 = "HashSet:NumsDivBy3";
        final String numsDivBy5 = "HashSet:NumsDivBy5";
        final String numsDivBy3Or5 = "HashSet:NumsDivBy3And5";

        // Creating data for hashsets
        List<Integer> numDivBy3 = Arrays.asList(3, 6, 9, 12, 15);
        List<Integer> numDivBy5 = Arrays.asList(5, 10, 15, 20, 25);

        // Getting or creating hashsets
        DistributedHashSet<Integer> hashSetDivBy3 = getOrCreate(numsDivBy3, Integer.class);
        DistributedHashSet<Integer> hashSetDivBy5 = getOrCreate(numsDivBy5, Integer.class);
        DistributedHashSet<Integer> hashSetDivBy3Or5 = getOrCreate(numsDivBy3Or5, Integer.class);

        // Adding data to hashsets
        hashSetDivBy3.addRange(numDivBy3);
        hashSetDivBy5.addRange(numDivBy5);

        // Printing output on console
        System.out.println("Performing intersection of numbers divisible by 3 and numbers divisible by 5");

        // Performing intersection of two hashsets and returning the result
        Iterable<Integer> intersection = hashSetDivBy3.intersect(numsDivBy5);

        // Performing intersection of two hashsets and storing the result in another hashset
        hashSetDivBy3.storeIntersection(numsDivBy3Or5, numsDivBy5);

        // Removing distributed hashsets not needed
        removeHashSet(hashSetDivBy3Or5);

        // Printing status on console
        System.out.println("Intersection operation completed.");
        System.out.println();
    }

    // --------------------------------------------------------------------------
    /**
     * Method to demonstrate difference of different distributed hashsets
     */
    private static void takeComplement() throws CacheException {
        // Creating names of hashsets to store them against
        final String allNames = "HashSet:AllNames";
        final String complementNames = "HashSet:ComplementNames";
        final String len4Names = "HashSet:Len4Names";

        // Creating data for hashsets
        List<String> allNamesArr = Arrays.asList("Alejandro", "Alexander", "Aria", "Hanna", "Hari",
                "Jamie", "Jean", "John", "Jose", "Mario", "Mary");
        List<String> len4NamesArr = Arrays.asList("Aria", "Hari", "Jean", "John", "Jose", "Mary");

        // Getting or creating hashsets
        DistributedHashSet<String> hsNamesComplement = getOrCreate(complementNames, String.class);
        DistributedHashSet<String> hsNamesOfLen4 = getOrCreate(len4Names, String.class);
        DistributedHashSet<String> hsAllNames = getOrCreate(allNames, String.class);

        // Adding data to hashsets
        hsAllNames.addRange(allNamesArr);
        hsNamesOfLen4.addRange(len4NamesArr);

        // Printing output on console
        System.out.println("Performing difference of names to calculate complement");

        // Performing difference of names set from universal set and returning the result
        Iterable<String> namesDifference = hsAllNames.difference(len4Names);

        // The result can also be stored in another distributed hashset
        // Performing difference of names set from universal set and storing the result in another hashset
        hsAllNames.storeDifference(complementNames, len4Names);

        // Printing status on console
        System.out.println("Difference operation completed.");
        System.out.println();
    }

    // --------------------------------------------------------------------------
    /**
     * Method to fetch (or create if does not exist) a distributed hashset
     *
     * @param <T> Generic type argument for distributed hashset
     * @param hashSetName Name against which the distributed hash
     *                    set needed is to be stored
     * @return Instance of the distributed hashset required
     */
    private static <T> DistributedHashSet<T> getOrCreate(String hashSetName, Class<T> typeParameter) throws CacheException {
        // Retrieving the distributed hashset from cache
        DistributedHashSet<T> distributedHashSet = _cache.getDataStructuresManager().getHashSet(hashSetName, typeParameter);

        // Checking if distributed hashset exists or not in cache
        if (distributedHashSet == null) {
            // Creating distributed hashset attributes
            DataStructureAttributes attributes = new DataStructureAttributes();

            // Setting expiration for 1 minute
            attributes.setExpiration(new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(1)));

            // Creating the distributed hashset in cache with specified attributes
            distributedHashSet = _cache.getDataStructuresManager().createHashSet(hashSetName, attributes, null, typeParameter);
        }

        // Returning distributed hashset instance
        return distributedHashSet;
    }

    // --------------------------------------------------------------------------
    /**
     * Method to demonstrate how we can iterate over all the values in a distributed hashset
     *
     * @param <T> Generic type argument for distributed hashset
     * @param distributedHashSet Instance of the distributed hashset to iterate over
     */
    private static <T> void displayHashSet(DistributedHashSet<T> distributedHashSet) throws CacheException {
        // Validating distributed hashset
        if (distributedHashSet == null) {
            System.out.println("Distributed hashset is null; cannot iterate over it.");
            return;
        }

        // Checking if distributed hashset has any data
        if (distributedHashSet.isEmpty()) {
            System.out.println("Distributed hashset '" + distributedHashSet.getKey() + "' has no data to iterate over.");
            return;
        } else {
            System.out.println("Iterating over '" + distributedHashSet.getKey() + "',");
            System.out.println("-------------------------------------");

            // Looping through all items in distributed hashset
            for (T item : distributedHashSet) {
                // Printing each item on console
                System.out.println(" - " + item);
            }
        }
    }

    // --------------------------------------------------------------------------
    /**
     * Method to remove the provided distributed hash from cache if it exists
     *
     * @param <T> Generic type argument for distributed hashset
     * @param distributedHashSet Instance of the distributed hashset required
     *                           to remove from cache
     */
    private static <T> void removeHashSet(DistributedHashSet<T> distributedHashSet) throws CacheException, IOException {
        // Validating distributed hashset
        if (distributedHashSet != null) {
            // Removing distributed hashset from cache
            _cache.getDataStructuresManager().remove(distributedHashSet.getKey());
        }
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to read configuration value from config.properties file
     */
    private static String getConfigValue(String property) {
        try {
            // Loading config.properties file from resources folder into stream
            InputStream stream = DistributedHashset.class
                    .getClassLoader()
                    .getResourceAsStream("config.properties");

            // Validating stream
            if (stream == null) {
                System.out.println("config.properties not found.");
                return null;
            }

            // Loading properties from stream
            Properties props = new Properties();
            props.load(stream);

            // Returning value for specified property
            return props.getProperty(property);
        } catch (Exception e) {
            return null;
        }
    }
}

