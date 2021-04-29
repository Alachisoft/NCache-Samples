// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Basic Operations sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.datastructures.DataStructureAttributes;
import com.alachisoft.ncache.client.datastructures.DistributedList;
import com.alachisoft.ncache.runtime.CacheItemPriority;
import com.alachisoft.ncache.runtime.caching.NamedTagsDictionary;
import com.alachisoft.ncache.runtime.caching.Tag;
import com.alachisoft.ncache.runtime.caching.datasource.ResyncOptions;
import com.alachisoft.ncache.runtime.caching.expiration.Expiration;
import com.alachisoft.ncache.runtime.caching.expiration.ExpirationType;
import com.alachisoft.ncache.runtime.events.DataStructureDataChangeListener;
import com.alachisoft.ncache.runtime.events.DataStructureEventArg;
import com.alachisoft.ncache.runtime.events.DataTypeEventDataFilter;
import com.alachisoft.ncache.runtime.events.EventType;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.*;
import java.util.Arrays;
import java.util.EnumSet;
import java.util.Properties;

/**
 Class that provides the functionality of the sample
 */
public class CacheAttributes
{
    private static Cache _cache;
    private static final String _datatype = "DistributedList:Customers";
    /**
     Executing this method will perform all the operations of the sample
     */
    public static void run() throws Exception {
        // Initialize cache
        initializeCache();

        // Create distributed list with Different Datatype Attributes
        createListWithDataTypeAttributes();

        // Create customers object
        Customer[] customers = getCustomers();

        // Adding items to Distributed Datatype
        addObjectsToDatatype(customers);

        //Register For Notification on datatype
        registerNotification();

        // Get a single object from distributed list by index
        Customer customer = getObjectFromList(1);

        // Modify the object and update in distributed list to trigger Notification Callback
        updateObjectsInList(0);

        // Remove the existing object from distributed list by index to Trigger Notification Callback
        removeObjectFromList(3);

        //Sleep for 5 seconds
        Thread.sleep(5000);

        //Register For Notifications
        unRegisterNotification();

        //Lock the list for 1 Minute
        lockDatatype();

        //UnLock the list
        unLockList();

        // Remove the distributed list from cache
        removeList();

        // Dispose the cache once done
        _cache.close();
    }

    /**
     This method initializes the cache
     */
    private static void initializeCache() throws Exception {
        Properties properties=  getProperties();
        String cacheName= properties.getProperty("CacheID");

        if (tangible.DotNetToJavaStringHelper.isNullOrEmpty(cacheName))
        {
            System.out.println("The CacheID cannot be null or empty.");
            return;
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);

        //Clear the Cache
        _cache.clear();
        // Print output on console.
        System.out.println(String.format("\nCache %1$s is initialized.", cacheName));
    }


    /**
     This method acquire lock on Distributed datatype
     */
    private static void lockDatatype() throws CacheException {
        // Get Distributed datatype
        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);

        // Acquire Lock
        boolean locked = list.lock(new TimeSpan(0, 1, 0));

        // Print output on console.
        System.out.println("\nDistributed Datatype:  " +_datatype+ ((locked) ? "Locked" : "Cannot be Locked"));

    }

    /**
     This method UnLock List Datastructure

     */
    private static void unLockList() throws CacheException {
        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);

        //Unlock the Distributed Datatype
        list.unlock();

        //Print output on console.
        System.out.println("\nDistributed Datatype:+ "+_datatype +" Unlocked");

    }


    /**
     This method unregister notification on Distributed Datatype


     */
    private static void unRegisterNotification() throws Exception {
        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);

        DataStructureDataChangeListener changeListener=new DataStructureDataChangeListener() {
            @Override
            public void onDataStructureChanged(String key, DataStructureEventArg dataStructureEventArg) {
                OnDataNotificationCallback(key,dataStructureEventArg);
            }
        };
        //unregister notifications for all Event types
        list.removeChangeListener(changeListener, EnumSet.of(EventType.ItemAdded, EventType.ItemRemoved , EventType.ItemUpdated));

        //Print output on console.
        System.out.println("\nNotifications unregistered on Distributed Datatype: "+ _datatype);
    }
    /**
     This method register notifications for Add,Update and Remove on List
     */
    private static void registerNotification() throws CacheException {

        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);
        DataStructureDataChangeListener changeListener=new DataStructureDataChangeListener() {
            @Override
            public void onDataStructureChanged(String key, DataStructureEventArg dataStructureEventArg) {
                OnDataNotificationCallback(key,dataStructureEventArg);
            }
        };

        //register callback for notification when item is added , removed or Updated in Distributed Datatype with DataFilter as None
        list.addChangeListener(changeListener, EnumSet.of(EventType.ItemAdded,EventType.ItemRemoved,EventType.ItemUpdated), DataTypeEventDataFilter.None);

        //Print output on console.
        System.out.println("\nNotifications are registered on Dictionary: "+ _datatype);
    }

    /**
     This is method is trigered when Add,Update or Remove operation is applied on list

     @param collectionName
     @param collectionEventArgs
     */
    public static void OnDataNotificationCallback(String collectionName, DataStructureEventArg collectionEventArgs)
    {
        synchronized (_datatype)
        {

            System.out.println("\nNotification Received");

            if (collectionEventArgs.getEventType() == EventType.ItemUpdated)
            {
                System.out.println("Item Updated in Datatype: "+ collectionName);
            }
            else if (collectionEventArgs.getEventType() == EventType.ItemAdded)
            {
                System.out.println("Item added in  Datatype: "+ collectionName);
            }
            else
            {
                System.out.println("Item Removed from datatype: "+ collectionName);
            }
        }
    }



    /**
     Creates a distributed list to which instances of customers are to be added

     be the key against which distributed list is to be stored.
     */
    private static void createListWithDataTypeAttributes() throws CacheException {

        //Attribute NamedTags Dictionary
        NamedTagsDictionary namedTags = new NamedTagsDictionary();
        namedTags.add("Customers", "Loyal");
        //Expiration
        Expiration expiration = new Expiration(ExpirationType.Absolute, new TimeSpan(0, 1, 0));
        //with Tag Frequent
        Tag tag = new Tag("Frequent");

        //Group for distributed datatype
        String group = "Potential";

        // Item priority Default
        CacheItemPriority cacheItemPriority = CacheItemPriority.Default;

        // Resync Operation is disabled
        ResyncOptions resyncOptions = new ResyncOptions(false);

        DataStructureAttributes attributes = new DataStructureAttributes();
        attributes.setExpiration(expiration);
        attributes.setNamedTags ( namedTags);
        attributes.setGroup ( group);
        attributes.setResyncOptions ( resyncOptions);
        attributes.setPriority (cacheItemPriority);
        attributes.setPriority (cacheItemPriority);

        // Creating distributed list with the defined attributes
        _cache.getDataStructuresManager().<Customer>createList(_datatype, attributes,null,Customer.class);
    }

    /**
     This method adds objects to distributed list

     @param customers Instances of Customer that will be added to
     distributed list
     */
    private static void addObjectsToDatatype(Customer[] customers) throws CacheException {
        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);
        //Add Bulk into Distributed List
        list.addRange(Arrays.asList(customers));

        // Print output on console.
        System.out.println("\nObjects are added to distributed list.");
    }

    /**
     This method gets an object from distributed list specified by index

     @return Returns instance of Customer retrieved from distributed list
     by index
     */
    private static Customer getObjectFromList(int index) throws CacheException {
        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);
        Customer cachedCustomer = list.get(index);

        // Print output on console.
        System.out.println("\nObject is fetched from distributed list");

        PrintCustomerDetails(cachedCustomer);

        return cachedCustomer;
    }


    /**
     This method updates object in distributed list

     @param index Index of object to be updated at in distributed list
     */
    private static void updateObjectsInList(int index) throws CacheException {

        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);
        // Modify company name of customer

        Customer customer = list.get(index);

        customer.setCompanyName("Gourmet Lanchonetes");

        // Update object via indexer
        list.add(index ,customer);

        // Print output on console.
        System.out.println("\nObject is updated in distributed list.");
    }

    /**
     Remove an object in distributed list by index

     @param index Index of object to be deleted from distributed list
     */
    private static void removeObjectFromList(int index) throws CacheException {
        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);
        // Remove the existing customer
        list.remove(index);

        // Print output on console.
        System.out.println("\nObject is removed from distributed list.");
    }

    /**
     Remove an object in distributed list by instance of object

     @param customer Instance of object to be deleted from distributed list
     */
    private static void RemoveObjectFromList(Customer customer) throws CacheException {
        DistributedList<Customer> list = _cache.getDataStructuresManager().<Customer>getList(_datatype,Customer.class);
        // Remove the existing customer
        // Expensive operation use carefully
        boolean removed = list.remove(customer);

        //Print output on console.
        System.out.println("\nObject is" + ((removed) ? " " : " not ") + "removed from distributed List.");

    }

    /**
     Removes distributed list from cache
     */
    private static void removeList() throws CacheException, IOException {
        // Remove list
        _cache.getDataStructuresManager().remove(_datatype);

        // Print output on console.
        System.out.println("\nDistributed list successfully removed from cache.");
    }

    /**
     Generates instances of Customer to be used in this sample

     @return  returns instances of Customer
     */
    private static Customer[] getCustomers()
    {
        Customer[] customers = new Customer[5];

        Customer tempVar = new Customer();
        tempVar.setContactName( "David Johnes");
        tempVar.setCompanyName("Lonesome Pine Restaurant");
        tempVar.setContactNo ( "(1) 354-9768");
        tempVar.setAddress ("Silicon Valley, Santa Clara, California");
        customers[0] = tempVar;
        Customer tempVar2 = new Customer();
        tempVar2.setContactName ("Carlos Gonzalez");
        tempVar2.setCompanyName ("LILA-Supermercado");
        tempVar2.setContactNo ("(9) 331-6954");
        tempVar2.setAddress ("Carrera 52 con Ave. Bolivar #65-98 Llano Largo");
        customers[1] = tempVar2;
        Customer tempVar3 = new Customer();
        tempVar3.setContactName ("Carlos Hernandez");
        tempVar3.setCompanyName ("HILARION-Abastos");
        tempVar3.setContactNo ("(5) 555-1340");
        tempVar3.setAddress ("Carrera 22 con Ave. Carlos Soublette #8-35");
        customers[2] = tempVar3;
        Customer tempVar4 = new Customer();
        tempVar4.setContactName ("Elizabeth Brown");
        tempVar4.setCompanyName ("Consolidated Holdings");
        tempVar4.setContactNo ("(171) 555-2282");
        tempVar4.setAddress ("Berkeley Gardens 12 Brewery");
        customers[3] = tempVar4;
        Customer tempVar5 = new Customer();
        tempVar5.setContactName ("Felipe Izquierdo");
        tempVar5.setCompanyName ("LINO-Delicateses");
        tempVar5.setContactNo ("(8) 34-56-12");
        tempVar5.setAddress ("Ave. 5 de Mayo Porlamar");
        customers[4] = tempVar5;

        return customers;
    }

    /**
     Generates a string key for specified customer

     @param customer Instance of Customer to generate a key
     @return  returns a key
     */
    private static String GetKey(Customer customer)
    {
        return String.format("Customer:%1$s", customer.getContactName());
    }

    /**
     This method prints details of customer type.

     @param customer
     */
    private static void PrintCustomerDetails(Customer customer)
    {
        if (customer == null)
        {
            return;
        }

        System.out.println();
        System.out.println("Customer Details are as follows: ");
        System.out.println("ContactName: " + customer.getContactName());
        System.out.println("CompanyName: " + customer.getCompanyName());
        System.out.println("Contact No: " + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
        System.out.println();
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = CacheAttributes.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }

}
