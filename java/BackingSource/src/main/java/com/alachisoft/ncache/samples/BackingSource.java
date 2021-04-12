// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Read Thru / Write Thru Data Providers sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.datastructures.*;
import com.alachisoft.ncache.runtime.caching.ReadMode;
import com.alachisoft.ncache.runtime.caching.ReadThruOptions;
import com.alachisoft.ncache.runtime.caching.WriteMode;
import com.alachisoft.ncache.runtime.caching.WriteThruOptions;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.exceptions.OperationFailedException;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.*;
import java.util.HashMap;
import java.util.Objects;
import java.util.Properties;
import java.util.Scanner;

public class BackingSource
{

    /**
     * instance of cache on which operations are performed
     */
    private static Cache _cache;
    private static Customer _customer;
    private static String _readThruProviderName;
    private static String _writeThruProviderName;

    private static String _customerId;
    private static String _companyName;
    private static String _city;
    private static String _country;

    private static final Scanner scanner = new Scanner(System.in);

    public static void DisplayCustomerData(Customer customer) {
        System.out.println(("Customer ID: " + customer.getCustomerID()));
        System.out.println(("Contact Name: " + customer.getContactName()));
        System.out.println(("Contact Number: " + customer.getContactNo()));
        System.out.println(("Fax: " + customer.getFax()));
        System.out.println(("Company: " + customer.getCompanyName()));
        System.out.println(("Address: " + customer.getAddress()));
        System.out.println(("City: " + customer.getCity()));
        System.out.println(("Postal Code: " + customer.getPostalCode()));
        System.out.println(("Country: " + customer.getCountry()));
    }

    /**
     * displays list objects
     * @param distributedList List to display
     */
    private static void DisplayListData(DistributedList<Object> distributedList) {
        System.out.println(("Result count: " + distributedList.size()));
        for (Object listCustomer : distributedList) {
            System.out.println("--");
            DisplayCustomerData((Customer) listCustomer);
        }

    }

    /**
     * displays dictionary objects
     * @param distributedDictionary Dictionary to display
     */
    public static void DisplayDictionaryData(DistributedMap<String, Object> distributedDictionary) {
        System.out.println(("Result count: " + distributedDictionary.size()));

        distributedDictionary.forEach(
                (k, v) -> {
                    System.out.println("--");
                    DisplayCustomerData((Customer) v);
                }
        );

    }

    /**
     * displays queued objects
     * @param distributedQueue Queue to display
     */
    private static void DisplayQueueData(DistributedQueue<Object> distributedQueue) {
        System.out.println(("Result count: " + distributedQueue.size()));
        for (Object queueCustomer : distributedQueue) {
            System.out.println("--");
            DisplayCustomerData((Customer) queueCustomer);
        }
    }

    /**
     * displays hashset objects
     * @param distributedHashSet hashset to display
     * @throws CacheException
     */
    private static void DisplayHashSetData(DistributedHashSet<Integer> distributedHashSet) throws CacheException {
        distributedHashSet.getWriteThruOptions().setMode(WriteMode.WriteThru);
        distributedHashSet.getWriteThruOptions().setProviderName(_writeThruProviderName);
        System.out.println(("Result count: " + distributedHashSet.size()));
        System.out.println("--");
        for (int num : distributedHashSet) {
            System.out.println((num + "\t"));
        }

        System.out.println("\n--");
    }

    public static void ModifyCounterValue(Counter counter) throws CacheException {
        counter.getWriteThruOptions().setMode(WriteMode.WriteThru);
        counter.getWriteThruOptions().setProviderName(_writeThruProviderName);
        System.out.println("Enter New Counter Value: ");
        long value = scanner.nextLong();
        counter.setValue(value);
        System.out.println();
        System.out.println(("New Counter Value is: " + counter.getValue()));
    }

    /**
     * Modifies first entry of list
     * @param distributedList
     * @throws CacheException
     */
    private static void ModifyListValue(DistributedList<Object> distributedList) throws CacheException {
        distributedList.getWriteThruOptions().setMode(WriteMode.WriteThru);
        distributedList.getWriteThruOptions().setProviderName(_writeThruProviderName);
        InputDataFromUser();
        distributedList.set(0, _customer);
    }

    /**
     * modify dictionary entry by CustomerID
     * @param distributedDictionary
     * @throws CacheException
     */
    public static void ModifyDictionaryValue(DistributedMap<String, Object> distributedDictionary) throws CacheException {
        distributedDictionary.getWriteThruOptions().setMode(WriteMode.WriteThru);
        distributedDictionary.getWriteThruOptions().setProviderName(_writeThruProviderName);
        InputDataFromUser();
        distributedDictionary.put(_customer.getCustomerID(), _customer);
    }

    /**
     * remove dictionary entry by CustomerID
     * @param distributedDictionary
     * @throws CacheException
     */
    public static void RemoveDictionaryValue(DistributedMap<String, Object> distributedDictionary) throws CacheException {
        distributedDictionary.getWriteThruOptions().setMode(WriteMode.WriteThru);
        distributedDictionary.getWriteThruOptions().setProviderName(_writeThruProviderName);
        distributedDictionary.remove(_customer.getCustomerID());
    }

    /**
     * remove queue entry by Customer
     * @param distributedQueue
     * @throws CacheException
     */
    public static void RemoveQueueValue(DistributedQueue<Object> distributedQueue) throws CacheException {
        distributedQueue.getWriteThruOptions().setMode(WriteMode.WriteThru);
        distributedQueue.getWriteThruOptions().setProviderName(_writeThruProviderName);
        distributedQueue.poll();
    }

    /**
     * remove list entry by Customer
     * @param distributedList
     * @throws CacheException
     */
    public static void RemoveListValue(DistributedList<Object> distributedList) throws CacheException {
        distributedList.getWriteThruOptions().setMode(WriteMode.WriteThru);
        distributedList.getWriteThruOptions().setProviderName(_writeThruProviderName);
        distributedList.remove(_customer);
    }

    /**
     * Enqueue data to Queue. Similar methods for other data structures can be written in the same way
     * @param distributedQueue
     * @throws CacheException
     */
    private static void AddDataToQueue(DistributedQueue<Object> distributedQueue) throws CacheException {
        distributedQueue.getWriteThruOptions().setMode(WriteMode.WriteThru);
        distributedQueue.getWriteThruOptions().setProviderName(_writeThruProviderName);
        InputDataFromUser();
        distributedQueue.add(_customer);
    }

    public static MainMenu InputMainMenu() {
        ConsolePause(true);
        System.out.println("Main Menu");
        System.out.println();
        System.out.println("1 - Get Customer Detail");
        System.out.println("2 - Get Customer Count by Company Name");
        System.out.println("3 - Get Customers by City Name");
        System.out.println("4 - Get Customers by Country Name");
        System.out.println("5 - Get Customers by a Specific Order");
        System.out.println("6 - Get Order IDs of a customer");
        System.out.println();
        System.out.print("Enter Option: ");
        try {
            //  convert input to integer
            int choice = scanner.nextInt();
            ConsolePause(true);
            //  validate input; ensure the input is between required numbers
            if (((choice >= 1) && (choice < 7))) {
                switch (choice){
                    case 1:
                        return MainMenu.GetCacheItem;
                    case 2:
                        return MainMenu.GetDistributedCounter;
                    case 3:
                        return MainMenu.GetDistributedDictionary;
                    case 4:
                        return MainMenu.GetDistributedList;
                    case 5:
                        return MainMenu.GetDistributedQueue;
                    case 6:
                        return MainMenu.GetDistributedHashSet;
                }
            }
            else {
                return MainMenu.GetCacheItem;
            }

        }
        catch (Exception ex) {
            return MainMenu.GetCacheItem;
        }
        return null;
    }

    /**
     * take customer data input from user
     */
    public static void InputDataFromUser() {
        System.out.println("Enter the following information");
        _customer = new Customer();
        System.out.println("Customer ID: ");
        _customer.setCustomerID(scanner.nextLine());

        System.out.println("Contact Name: ");
        _customer.setContactName(scanner.nextLine());

        System.out.println("Company Name: ");
        _customer.setCompanyName(scanner.nextLine());

        System.out.println("Address: ");
        _customer.setAddress(scanner.nextLine());

        System.out.println("City: ");
        _customer.setCity(scanner.nextLine());

        System.out.println("Country: ");
        _customer.setCountry(scanner.nextLine());

        System.out.println("Postal Code: ");
        _customer.setPostalCode(scanner.nextLine());

        System.out.println("Phone Number: ");
        _customer.setPhone(scanner.nextLine());

        System.out.println("Fax Number: ");
        _customer.setFax(scanner.nextLine());
    }

    public static void StoreNewCustomerData() {

        try {
            _cache.insert(_customer.getCustomerID().toUpperCase(), new CacheItem(_customer), new WriteThruOptions(WriteMode.WriteThru, _writeThruProviderName));
            System.out.println("Customer information updated successfuly");
        }
        catch (Exception e) {
            System.out.println(("\n" + ("Error: " + e.getMessage())));
        }

    }

    /**
     * method for functioning
     * @throws Exception
     */
    public static void run() throws Exception {
        Properties properties = getProperties();
        // Initialize cache
        String cacheName= properties.getProperty("CacheID");
        //  initialize cache before any operations
        initializeCache(cacheName);
        _readThruProviderName = "SqlReadThruProvider";
        _writeThruProviderName = "SqlWriteThruProvider";
        //  initialize customer object
        Customer customer = null;
        Counter counter = null;
        DistributedMap<String, Object> distributedDictionary;
        DistributedList<Object> distributedList;
        DistributedQueue<Object> distributedQueue;
        DistributedHashSet<Integer> distributedHashSet;
        boolean isRunning = true;
        while (isRunning) {
            //  clearing cache to see readthru/writethru changes
            _cache.clear();
            //  take input according to main menu
            switch (Objects.requireNonNull(InputMainMenu())) {
                case GetCacheItem:
                    //  take customer ID from user
                    System.out.println("Enter Customer ID: ");
                    _customerId = scanner.nextLine();
                    customer = _cache.get(_customerId, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), Customer.class);
                    if ((customer != null)) {
                        //  display details of entered customer ID
                        System.out.println(("\n" + "Fetched Customer Details [readthru]: "));
                        DisplayCustomerData(customer);
                        ConsolePause(true);
                        System.out.println("Enter any detail to update (press enter on any value to skip) [writethru]");
                        //  take new customer data from user
                        InputDataFromUser();
                        //  update existing customer with new data
                        StoreNewCustomerData();
                        //  show newly fetched customer details
                        System.out.println(("\n" + "Fetched Customer Details [cache]: "));
                        customer = _cache.get(_customerId, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), Customer.class);
                        DisplayCustomerData(customer);
                    }
                    else {
                        System.out.println("\nCustomer with provided ID does not exist.");
                        isRunning = false;
                    }

                    break;
                case GetDistributedCounter:
                    //  input company name from user
                    System.out.println("Enter Company Name: ");
                    _companyName = scanner.nextLine();
                    //  fetch customers with same company name
                    counter = _cache.getDataStructuresManager().getCounter(_companyName, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName));
                    if ((counter.getValue() != 0)) {
                        System.out.println(("Customers working in company are: " + counter.getValue()));
                        int out = ConsolePause(true);
                        if (out == 27) {
                            break;
                        }

                        //  modify counters
                        ModifyCounterValue(counter);
                    }
                    else {
                        System.out.println("\nCustomer with provided CompanyName does not exist.");
                        isRunning = false;
                    }

                    break;
                case GetDistributedDictionary:
                    //  take city name input from user
                    System.out.println("Enter City Name: ");
                    _city = scanner.nextLine();
                    //  fetch customers with same city name
                    distributedDictionary = _cache.getDataStructuresManager().getMap(_city, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), HashMap.class);
                    if ((distributedDictionary != null)) {
                        DisplayDictionaryData(distributedDictionary);
                        if (ConsolePause(false) != 27) {
                            //  modify any dictionary record
                            System.out.println(("\n" + "Modify Any Record [writethru]"));
                            ModifyDictionaryValue(distributedDictionary);
                            System.out.println(("\n" + "Data Modified"));
                            //  view changed data
                            System.out.println(("\n" + "Fetched Customer Details [cache]: "));
                            distributedDictionary = _cache.getDataStructuresManager().getMap(_city, HashMap.class);
                            DisplayDictionaryData(distributedDictionary);
                        }

                        if (ConsolePause(false) != 27) {
                            //  remove entry from dictionary
                            System.out.println(("\n" + "Removing Dictionary Entry"));
                            //  take customer ID input from user
                            System.out.println("Enter Customer ID: ");
                            _customerId = scanner.nextLine();
                            RemoveDictionaryValue(distributedDictionary);
                            System.out.println(("\n" + "Dictionary Entry Removed"));
                            //  view changed data
                            System.out.println(("\n" + "Fetched Customer Details [readthru]: "));
                            distributedDictionary = _cache.getDataStructuresManager().getMap(_city, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), HashMap.class);
                            DisplayDictionaryData(distributedDictionary);
                        }

                    }
                    else {
                        System.out.println("\nCustomer with provided City does not exist.");
                        isRunning = false;
                    }

                    break;
                case GetDistributedList:
                    //  take country name input from user
                    System.out.println("Enter Country Name: ");
                    _country = scanner.nextLine();
                    //  fetch customers with same country name
                    distributedList = _cache.getDataStructuresManager().getList(_country, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), DistributedList.class);
                    if ((distributedList != null)) {
                        DisplayListData(distributedList);
                        if (ConsolePause(false) != 27) {
                            //  modify first entry of list
                            System.out.println("\nModify First Record [writethru]: ");
                            ModifyListValue(distributedList);
                            //  view changed data
                            System.out.println(("\n" + "Fetched Customer Details [cache]: "));
                            distributedList = _cache.getDataStructuresManager().getList(_country, DistributedList.class);
                            DisplayListData(distributedList);
                        }

                        if (ConsolePause(false) != 27) {
                            //  remove entry from dictionary
                            System.out.println(("\n" + "Removing List Entry"));
                            //  take customer ID input from user
                            System.out.println("Enter Customer ID: ");
                            _customerId = scanner.nextLine();
                            RemoveListValue(distributedList);
                            System.out.println(("\n" + "List Entry Removed"));
                            //  view changed data
                            System.out.println(("\n" + "Fetched Customer Details [readthru]: "));
                            distributedDictionary = _cache.getDataStructuresManager().getMap(_city, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), DistributedMap.class);
                            DisplayDictionaryData(distributedDictionary);
                        }

                    }
                    else {
                        System.out.println("\nCustomer with provided Country does not exist.");
                        isRunning = false;
                    }

                    break;
                case GetDistributedQueue:
                    //  take sorting field input from user
                    System.out.println("Enter Sorting Field Name: ");
                    String sort = scanner.nextLine();
                    distributedQueue = null;
                    try {
                        //  fetch customers sorted by sorting field
                        distributedQueue = _cache.getDataStructuresManager().getQueue(sort, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), DistributedQueue.class);
                    }
                    catch (OperationFailedException e) {
                        System.out.println(e.getMessage());
                    }

                    if ((distributedQueue != null)) {
                        DisplayQueueData(distributedQueue);
                        if (ConsolePause(false) != 27) {
                            //  Add a new customer
                            System.out.println("\nAdd new customer [writethru]: ");
                            AddDataToQueue(distributedQueue);
                            //  view changed data
                            System.out.println(("\n" + "Fetched Customer Details [cache]: "));
                            distributedQueue = _cache.getDataStructuresManager().getQueue(sort, DistributedQueue.class);
                            DisplayQueueData(distributedQueue);
                        }

                        if (ConsolePause(false) != 27) {
                            //  remove entry from dictionary
                            System.out.println(("\n" + "Dequeuing Queue Entry"));
                            //  take customer ID input from user
                            System.out.println("Enter Customer ID: ");
                            _customerId = scanner.nextLine();
                            RemoveQueueValue(distributedQueue);
                            System.out.println(("\n" + "Queue Entry Dequeued"));
                            //  view changed data
                            System.out.println(("\n" + "Fetched Customer Details [readthru]: "));
                            distributedDictionary = _cache.getDataStructuresManager().getMap(_city, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), DistributedMap.class);
                            DisplayDictionaryData(distributedDictionary);
                        }
                        else{
                            System.out.println("\nCustomer with provided Country does not exist.");
                            isRunning = false;
                        }

                    }

                    break;
                case GetDistributedHashSet:
                    //  take customer ID input from user
                    System.out.println("Enter Customer ID: ");
                    _customerId = scanner.nextLine();
                    //  fetch order IDs of a customer
                    distributedHashSet = _cache.getDataStructuresManager().getHashSet(_customerId, new ReadThruOptions(ReadMode.ReadThru, _readThruProviderName), DistributedHashSet.class);
                    if ((distributedHashSet != null)) {
                        DisplayHashSetData(distributedHashSet);
                    }
                    else {
                        isRunning = false;
                        System.out.println("\nCustomer with provided ID does not exist.");
                    }

                    break;
                default:
                    throw new IllegalStateException("Unexpected value: " + Objects.requireNonNull(InputMainMenu()));
            }
        }

    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = BackingSource.class.getClassLoader().getResourceAsStream(path);
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

    public static int ConsolePause(boolean clearScreen) {
        System.out.println();
        System.out.println("Press any key to continue (or escape to skip additional functioning)...");

        String key = scanner.next();

        if (clearScreen) {
            System.out.flush();
        }

        return key.charAt(0);
    }

    public enum MainMenu {

        GetCacheItem,

        GetDistributedCounter,

        GetDistributedDictionary,

        GetDistributedList,

        GetDistributedQueue,

        GetDistributedHashSet,
    }
}
