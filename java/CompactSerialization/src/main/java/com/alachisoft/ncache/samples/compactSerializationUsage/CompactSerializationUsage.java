// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples.compactSerializationUsage;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.compactSerializationImpl.*;

import java.io.*;
import java.time.LocalDate;
import java.util.Properties;
import java.util.Random;
import java.util.concurrent.LinkedBlockingQueue;

/**
 * Here is what this program should do:
 *
 * 1. Create three classes "NormalObject", "BigObject", and "BiggerObject".
 * They are 4k, 10k, 50k in size respectively. Use C# class for each of the
 * above and within the class you can put a byte-array to take up the desired
 * size if you want but have at least two data members in the class (e.g.
 * String _id being the first data member).
 *
 * 2. Write a command line program that goes into a loop (100 times) and for
 * each iteration, it uses the iteration number as the key and then randomly
 * picks one of the objects to add to the cache if GET on this key returns a
 * NULL.
 *
 * 3. At the end of this loop, write another loop that goes through a part of
 * the first loop (e.g. 1/10 of the first loop) and does a GET on item (a
 * subset of the first loop). It should find all of these items in the Cache.
 *
 * 4. Print out to a log-file (or a standard-output) for each iteration in the
 * loop. Print the following information on ONE LINE:
 * - print key
 * - found or not found
 * - Time it took to do a GET
 * - Time it took to do an INSERT (if insert is done)
 * - a sample line should look like this:
 *
 * 1005  Found  GET 20ms
 * 1006  Not    GET 50ms  INSERT  200ms
 *
 * 5. Calculate the same statistics that you have done in "Program" (best,
 * average, worst, and also different bracket of worst) and print out these
 * statistics at the end.
 *
 * 6. Put a 10 second wait after doing Initialize on the cache so when we run
 * it on two machines, the cluster is formed before data is actually added to
 * the cache.
 */
public class CompactSerializationUsage {

    final static int runCount = 100;
    final static boolean logObjectSize = false;

    static int part = 3;

    private static final TimeStat _addStats = new TimeStat(60, 120);
    private static final TimeStat  _getStats = new TimeStat(1, 10);

    static int TestID = 1;
    private static Cache _cache;

    public static void init() {
        EventLog.create(new LinkedBlockingQueue<>(), "cs_logs", 1);
    }

    public static void Run() {
        try {
            // Initialize cache
            Properties properties = getProperties();
            // Initialize cache
            String cacheName = properties.getProperty("CacheID");

            initializeCache(cacheName);

            System.out.println("Small objects (512 bytes)");

            performTest(1);

            System.out.println("Small objects (4KB)");

            performTest(2);

            System.out.println("Big objects    (10 K)");

            performTest(3);

            System.out.println("Bigger objects (50 K)");

            performTest(4);

            System.out.println("Sample objects ( >5 K)");

            performTest(5);

            System.out.println("Mix all objects randomly");

            performTest(6);

            _cache.clear();
            _cache.close();
        }catch (Exception e)
        {
            EventLog.write(e, "In Run Method", "");
        }
    }

    private static void performTest(int testID) throws CacheException {
        CompactSerializationUsage.TestID = testID;

        EventLog.write("\r\n\n====================================", "Performing test ", String.valueOf(testID));
        EventLog.write(getTestName(testID), "", "");
        EventLog.write("====================================","", "");
        EventLog.write("\r\nExpected results are:","", "");
        EventLog.write("Operation    Best(ms)    Avg(ms)    Worst(ms)","", "");
        EventLog.write("---------    --------    -------    ---------","", "");
        EventLog.write(String.format("%5s  <= %5d  > %5d", "Add()", _addStats.getExpBestTime(), _addStats.getExpWorstTime()),"", "");
        EventLog.write(String.format("%5s  <= %5d  > %5d", "Get()", _addStats.getExpBestTime(), _addStats.getExpWorstTime()),"", "");
        EventLog.write("\r\nCompact Serialization Sample Started...\r\n","", "");

        runSimulation(testID);
        displayStatistics();
    }

    private static void displayStatistics() {
        EventLog.write("\r\nStatistics at: {0}",  LocalDate.now().toString(), "");
        EventLog.write("Operation    # Runs     Best(ms)    Avg(ms)    Worst(ms)","","");
        EventLog.write("---------    ------     --------    -------    ---------","","");
        EventLog.write(String.format("Add()        %6s     %8d    %8f   %8d", _addStats.getRunCount(),
                _addStats.getBestTime(),
                _addStats.getAvgTime(),
                _addStats.getWorstTime()),
                "",
                "");
        EventLog.write(String.format("Get()        %6s     %8d    %8f   %8d",
                _getStats.getRunCount(),
                _getStats.getBestTime(),
                _getStats.getAvgTime(),
                _getStats.getWorstTime()),
                "",
                "");


        EventLog.write("\r\n            Performance distribution","","");
        EventLog.write("            ------------------------","","");

        if(_addStats.getRunCount() > 0)
            EventLog.write(String.format("Add()       %6s Best %6f Avg. %6f Worst -> (%d, %d, %d)",
                    _addStats.pctBestCases(),
                    _addStats.pctAvgCases(),
                    _addStats.pctWorstCases(),
                    _addStats.bestCases(),
                    _addStats.avgCases(),
                    _addStats.worstCases()), "", "");
        else
            EventLog.write("Add()        -","","");

        if(_getStats.getRunCount() > 0)
            EventLog.write(String.format("Add()       %6s Best %6f Avg. %6f Worst -> (%d, %d, %d)",
                    _addStats.pctBestCases(),
                    _addStats.pctAvgCases(),
                    _addStats.pctWorstCases(),
                    _addStats.bestCases(),
                    _addStats.avgCases(),
                    _addStats.worstCases()), "", "");
        else
            EventLog.write("Get()        -","","");

        _addStats.reset();
        _getStats.reset();
    }

    private static void runSimulation(int testID) throws CacheException {
        long objectSize = getObjectSize(testID);
        if(logObjectSize)
            EventLog.write("ObjectSize before adding item into cache: ", String.valueOf(objectSize), "");
        EventLog.write("Adding", String.valueOf(runCount), "Items.....");
        for(int i = (((testID - 1) * 100)); i < (runCount + ((testID - 1) * 100)); i++)
        {
           objectSize =  performOperation(i);
           if(logObjectSize)
               EventLog.write("ObjectSize after adding item into cache: ", String.valueOf(objectSize), "");
        }
        EventLog.write("\r\n-------------------------------------------------------------------------\r\n", "", "");
        EventLog.write("\r\nPerforming operations in parts...\r\n", "", "");

        for(int i = (CompactSerializationUsage.runCount / 10) * part ; i < (CompactSerializationUsage.runCount / 10) * (part + 1) ; i++)
        {
            objectSize =  performOperation(i);
            if(logObjectSize)
                EventLog.write("ObjectSize after adding item into cache: ", String.valueOf(objectSize), "");
        }
    }

    private static long performOperation(int i) throws CacheException {
        long objectSize = 0;
        EventLog.write(String.valueOf(i), "=>Iteration", "");
        Object obj = getObjectFromCache(String.valueOf(i));
        if(obj == null)
        {
            EventLog.write(" not found. Get operation took: ", String.valueOf(_addStats.getCurrent()), " ms\t");
            EventLog.write("Inserting " + i, " in cache", "");
            objectSize = add(String.valueOf(i));
            EventLog.write(", operation took:", String.valueOf(_addStats.getCurrent()), " ms\t");
        }
        else
        {
            EventLog.write(" found. Get operation took:", String.valueOf(_addStats.getCurrent()), " ms\t");
        }

        if (objectSize == 512)
            EventLog.write("Object size: 512 bytes", "", "");
        else
            EventLog.write("Object size:", String.valueOf((objectSize / 1000)), " KB");
        return objectSize;
    }

    private static long add(String key) throws CacheException {
        long objSize = 0;
        if (!_cache.contains(key))
        {
            try
            {
                int operation = TestID;
                if (CompactSerializationUsage.TestID == 6)
                {
                    Random rand = new Random();
                    operation = rand.nextInt(5);
                }

                Object Obj;
                switch(operation)
                {
                    case 1:
                        Obj = new SampleClass();
                        objSize = 512;
                        break;
                    case 2:
                        Obj = new NormalObject();
                        objSize = 4000;
                        break;
                    case 3:
                        Obj = new BigObject();
                        objSize = 10000;
                        break;
                    case 4:
                        Obj = new BiggerObject();
                        objSize = 50000;
                        break;
                    default:
                        Obj = new SampleObject();
                        objSize = 5000;
                        break;
                }

                _addStats.beginSample();
                _cache.add(
                        key,
                        Obj
                );
                _addStats.endSample();
            }
            catch(Exception ex)
            {
                EventLog.write(ex.getMessage(), "In Add Method", "");
            }
        }
        return objSize;
    }

    private static Object getObjectFromCache(String key) throws CacheException {
        _getStats.beginSample();
        Object obj = _cache.get(key, Object.class);
        _getStats.endSample();
        return obj;
    }

    private static long getObjectSize(int testID) {
        long objSize;
        switch (testID)
        {
            case 1:
                objSize = 512;
                break;
            case 2:
                objSize = 4000;
                break;
            case 3:
                objSize = 10000;
                break;
            case 4:
                objSize = 50000;
                break;
            default:
                objSize = 5000;
                break;
        }
        return objSize;
    }

    private static String getTestName(int testID) {
        String testName;

        switch (testID)
        {
            case 1:
                testName = "Small objects  (512 bytes)";
                break;
            case 2:
                testName = "Normal objects (4 KB)";
                break;
            case 3:
                testName = "Big objects    (10 K)";
                break;
            case 4:
                testName = "Bigger objects (50 K)";
                break;
            case 5:
                testName = "Sample objects ( >5 K)";
                break;
            default:
                testName = "Mix all objects randomly";
                break;
        }

        return testName;
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = CompactSerializationUsage.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }

    /**
     This method initializes the cache
     */
    private static void initializeCache(String cacheName) throws Exception {

        if(cacheName == null){
            System.out.println("The CacheID cannot be null.");
            return;
        }

        if(cacheName.isEmpty()){
            System.out.println("The CacheID cannot be empty.");
            return;
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);

        // Print output on console
        System.out.println();
        System.out.println("Cache initialized succesfully.");
    }
}
