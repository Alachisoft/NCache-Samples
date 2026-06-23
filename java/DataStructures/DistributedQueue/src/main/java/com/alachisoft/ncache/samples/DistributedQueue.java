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
import com.alachisoft.ncache.runtime.caching.expiration.Expiration;
import com.alachisoft.ncache.runtime.caching.expiration.ExpirationType;
import com.alachisoft.ncache.runtime.events.DataStructureDataChangeListener;
import com.alachisoft.ncache.runtime.events.EventType;
import com.alachisoft.ncache.runtime.events.DataStructureEventArg;
import com.alachisoft.ncache.runtime.events.DataTypeEventDataFilter;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;
import java.util.Properties;

/*
 *  ===============================================================================
 * Distributed Queue is a powerful distributed data structure which allows
 * multiple clients to enqueue and dequeue items in a thread-safe manner. Some
 * possible use cases of distributed queue include:
 *   1. Task Scheduling: Managing and distributing tasks among multiple nodes.
 *   2. Load Balancing: Distributing jobs evenly across multiple servers/services.
 *   3. Message Queuing: Enabling asynchronous message processing between entities
 *      (e.g., microservices, applications, users, etc.).
 *   4. Event Processing: Handling real-time events in received order.
 *   5. Data Streaming: Buffering and managing continuous data streams between
 *      systems across distributed environments.
 * Distributed Queues are designed to handle high concurrency and provide fault
 * tolerance, making them suitable for large-scale distributed applications.
 * ===============================================================================
 */

public class DistributedQueue {

    // Cache handle for the connected cache
    private static Cache _cache;

    // Distributed queue to be used in sample
    private static com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> queue;

    public static void main(String[] args) throws Exception {
        // Connect to a running cache and get cache handle for it
        _cache = getCache();

        // Validating cache handle
        if (_cache == null) {
            System.out.println("Cache could not be initialized.");
            return;
        }

        // Name of the distributed queue to be used in sample
        String queueName = "DQueue:Customers";

        // Creating or getting a distributed queue by name
        queue = getOrCreateQueue(queueName);

        // Registering events for enqueue and dequeue operations on distributed queue
        registerEvents(queue);

        // Enqueue items in the distributed queue
        enqueue(queue);

        // Dequeue from distributed queue
        dequeue(queue);

        // Displaying customers from distributed queue
        displayQueue(queue);

        // Clearing the distributed queue
        clearQueue(queue);

        // Unregistering events for enqueue and dequeue operations on distributed queue
        unregisterEvents(queue);

        // Removing the distributed queue from cache
        removeQueueFromCache(queueName);

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
     * Method to create a distributed queue from queue name if it does not exist,
     * otherwise get the existing distributed queue
     *
     * @param queueName Name of the distributed queue
     * @return Distributed queue containing customers
     */
    private static com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> getOrCreateQueue(String queueName) throws CacheException {
        // Trying to get existing distributed queue
        com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> distributedQueue = _cache.getDataStructuresManager().getQueue(queueName, Customer.class);

        // Checking if distributed queue does not exist
        if (distributedQueue == null) {
            // Defining data structure attributes for distributed queue
            DataStructureAttributes attributes = new DataStructureAttributes();

            // Setting absolute expiration of 1 minute for distributed queue
            attributes.setExpiration(new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(1)));

            // Creating distributed queue with absolute expiration of 1 minute
            distributedQueue = _cache.getDataStructuresManager().createQueue(queueName, attributes, null,  Customer.class);
        }

        // Returning the distributed queue
        return distributedQueue;
    }

    // --------------------------------------------------------------------------
    /**
     * Method to register event notifications for enqueue and dequeue operations on Distributed Queue
     *
     * @param distributedQueue Distributed queue on which operation would be performed
     */
    private static void registerEvents(com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> distributedQueue) throws CacheException {
        // Defining event types for which notifications would be registered
        EnumSet<EventType> enumSet = EnumSet.of(EventType.ItemAdded, EventType.ItemRemoved);

        // Defining data change listener for distributed queue
        DataStructureDataChangeListener dataChangeListener = dataStructureListener;

        // Registering data change listener for defined event types on distributed queue
        distributedQueue.addChangeListener(dataChangeListener, enumSet, DataTypeEventDataFilter.Data);

        // Printing output on console
        System.out.println("Distributed queue events registered successfully.");
    }

    // --------------------------------------------------------------------------
    /**
     * Method to generate sample customers and enqueue them in the specified distributed queue
     *
     * @param queue Distributed queue on which operation would be performed
     */
    private static void enqueue(com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> queue) throws InterruptedException {
        // Printing output on console
        System.out.println("Enqueue Customers in Distributed Queue");

        // Creating sample customers list
        List<Customer> customers = new ArrayList<>();

        // Creating sample customers and adding them to the list
        Customer c1 = new Customer();
        c1.setContactName("Maria Anders");
        c1.setCompanyName("Alfreds Futterkiste");
        c1.setContactNo("(030) 0074321");
        c1.setAddress("Obere Str. 57, Berlin");
        customers.add(c1);

        Customer c2 = new Customer();
        c2.setContactName("Ana Trujillo");
        c2.setCompanyName("Ana Trujillo Emparedados y helados");
        c2.setContactNo("(5) 555-4729");
        c2.setAddress("Avda. de la Constitución 2222, México D.F.");
        customers.add(c2);

        Customer c3 = new Customer();
        c3.setContactName("Antonio Moreno");
        c3.setCompanyName("Antonio Moreno Taquería");
        c3.setContactNo("(5) 555-3932");
        c3.setAddress("Mataderos 2312, México D.F.");
        customers.add(c3);

        Customer c4 = new Customer();
        c4.setContactName("Thomas Hardy");
        c4.setCompanyName("Around the Horn");
        c4.setContactNo("(171) 555-7788");
        c4.setAddress("120 Hanover Sq., London");
        customers.add(c4);

        Customer c5 = new Customer();
        c5.setContactName("Hanna Moos");
        c5.setCompanyName("Blauer See Delikatessen");
        c5.setContactNo("(0521) 555-9933");
        c5.setAddress("Forsterstr. 57, Mannheim");
        customers.add(c5);

        // Iterating through customers list
        for (Customer customer : customers) {
            // Enqueue customer in distributed queue
            queue.add(customer);

            // Adding a delay for ItemAdded event processing
            Thread.sleep(500);
        }

        // Printing output on console
        System.out.println("Customers are enqueued in the distributed queue");
        System.out.println();
    }

    // --------------------------------------------------------------------------
    /**
     * Method to see the first customer from Distributed Queue without removing it
     *
     * @param distributedQueue Distributed queue on which operation would be performed
     */
    private static void peekFromQueue(com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> distributedQueue) {
        // Printing output on console
        System.out.println("Peek Customer From Distributed Queue");

        // Peeking customer from distributed queue
        Customer customer = distributedQueue.peek();

        // Printing customer details on console
        printCustomerDetails(customer);
    }

    // --------------------------------------------------------------------------
    /**
     * Method to dequeue customer from Distributed Queue
     *
     * @param distributedQueue Distributed queue on which operation would be performed
     */
    private static void dequeue(com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> distributedQueue) {
        // Dequeue customer from distributed queue
        Customer customer = distributedQueue.remove();

        // Printing customer details on console
        System.out.println("Dequeue customer from Distributed Queue:");
        printCustomerDetails(customer);

        // Peeking customer from distributed queue after dequeue operation
        peekFromQueue(distributedQueue);
    }

    // --------------------------------------------------------------------------
    /**
     * Method to display customers enqueued in the specified Distributed Queue
     *
     * @param distributedQueue Distributed queue on which operation would be performed
     */
    private static void displayQueue(com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> distributedQueue) {
        // Validating if distributed queue is not empty
        if (!distributedQueue.isEmpty()) {
            // Printing output on console
            System.out.println(distributedQueue.size() + " Customers are enqueued:");

            // Iterating through distributed queue
            for (Customer customer : distributedQueue) {
                // Printing customer details on console
                printCustomerDetails(customer);
            }
        } else {
            System.out.println("Distributed Queue is empty.");
        }
    }

    // --------------------------------------------------------------------------
    /**
     * Method to clear all the customers in the Distributed Queue
     *
     * @param distributedQueue Distributed queue on which operation would be performed
     */
    private static void clearQueue(com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> distributedQueue) {
        // Clearing the distributed queue
        distributedQueue.clear();

        // Printing output on console
        System.out.println("Distributed queue has been cleared.");
    }

    // --------------------------------------------------------------------------
    /**
     * Method to unregister event notifications for enqueue and dequeue operations on Distributed Queue
     *
     * @param distributedQueue Distributed queue on which operation would be performed
     */
    private static void unregisterEvents(com.alachisoft.ncache.client.datastructures.DistributedQueue<Customer> distributedQueue) throws Exception {
        // Defining event types for which notifications would be unregistered
        EnumSet<EventType> enumSet = EnumSet.of(EventType.ItemAdded, EventType.ItemRemoved);

        // Unregistering data change listener for defined event types on distributed queue
        distributedQueue.removeChangeListener(dataStructureListener, enumSet);

        // Printing output on console
        System.out.println("Distributed queue events unregistered successfully.");
    }

    // --------------------------------------------------------------------------
    /**
     * Method to remove distributed queue from cache
     *
     * @param queueName Name of the distributed queue
     */
    private static void removeQueueFromCache(String queueName) throws CacheException, IOException {
        // Removing the distributed queue from cache
        _cache.getDataStructuresManager().remove(queueName);

        // Printing output on console
        System.out.println("Distributed queue successfully removed from cache.");
    }

    // --------------------------------------------------------------------------
    /**
     * Event callback method to handle events for Distributed Queue
     */
    private static DataStructureDataChangeListener dataStructureListener = new DataStructureDataChangeListener() {
        @Override
        public void onDataStructureChanged(String collectionName, DataStructureEventArg collectionEventArgs) {
            // Handling events based on event type
            switch (collectionEventArgs.getEventType()) {
                case ItemAdded:
                    // Printing output on console
                    System.out.println("Event: An item was added to the Distributed Queue.");
                    // Peeking item from distributed queue after ItemAdded event
                    peekFromQueue(queue);
                    break;
                case ItemRemoved:
                    // Printing output on console
                    System.out.println("Event: An item was removed from the Distributed Queue.");
                    break;
            }
        }
    };

    // --------------------------------------------------------------------------
    /**
     * Method to print the details of customer on console
     *
     * @param customer Customer instance whose attributes are to be printed
     */
    private static void printCustomerDetails(Customer customer) {
        // Validating customer instance
        if (customer == null) return;

        // Printing customer details on console
        System.out.println("Customer details are as follows: ");
        System.out.println("ContactName: " + customer.getContactName());
        System.out.println("CompanyName: " + customer.getCompanyName());
        System.out.println("Contact No: " + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to read configuration value from config.properties file
     */
    private static String getConfigValue(String property) {
        try {
            // Loading config.properties file from resources folder into stream
            InputStream stream = DistributedQueue.class
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