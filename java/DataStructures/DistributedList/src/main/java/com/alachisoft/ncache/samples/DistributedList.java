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
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.IOException;
import java.io.InputStream;
import java.util.List;
import java.util.Properties;

/*
 * ===============================================================================
 * Distributed List is a distributed collection that allows storing objects in a
 * list format and provides various methods to manipulate the list such as adding,
 * removing, updating, and fetching objects. Distributed List maintains the order
 * of objects based on the index at which they are added. It also allows
 * enumerating over the objects in the list. Some possible use cases of
 * Distributed List include:
 *   1. Storing a list of recent activities or events performed by users
 *   2. Maintaining a list of top N trending or frequently accessed items
 *   3. Keeping track of messages or chat history in a messaging application
 *   4. Managing task queues or ordered job lists across distributed services
 *   5. Recording time-ordered logs or audit trails for real-time monitoring
 * ===============================================================================
 */
public class DistributedList {

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

        // Names of distributed lists to be used in sample
        String priorityCustomers = "DList:PriorityCustomers";
        String regularCustomers = "DList:RegularCustomers";

        // Creating or getting both distributed lists by name
        List<Customer> priorityList = getOrCreateList(priorityCustomers);
        List<Customer> regularList = getOrCreateList(regularCustomers);

        // Adding items to both distributed lists
        addPriorityCustomers(priorityList);
        addRegularCustomers(regularList);

        // Displaying customers from both distributed lists
        displayList(priorityList);
        displayList(regularList);

        // Fetching the first customer from distributed list by index
        Customer customer = getCustomerFromList(0, priorityList);

        // Modifying the customer and updating in distributed list
        updateObjectsInList(0, customer, priorityList);

        // Removing the existing customer from distributed list by index
        removeViaIndex(2, regularList);

        // Removing the existing customer from distributed list by instance
        removeViaInstance(customer, priorityList);

        // Removing both distributed lists from cache
        deleteList(priorityCustomers);
        deleteList(regularCustomers);

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

    // ------------------------------------------------------------------------------
    /**
     * Method to create a distributed list to which instances of customers are to be added
     *
     * @param listName Name of distributed list
     * @return Distributed List containing customers
     */
    private static List<Customer> getOrCreateList(String listName) throws CacheException {
        // Fetching distributed list from cache
        com.alachisoft.ncache.client.datastructures.DistributedList<Customer> distributedList = _cache.getDataStructuresManager().getList(listName, Customer.class);

        // Validating distributed list
        if (distributedList == null) {
            // Creating attributes for distributed list
            DataStructureAttributes attributes = new DataStructureAttributes();

            // Setting expiration for distributed list for 1 minute
            attributes.setExpiration(new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(1)));

            // Creating distributed list in cache
            distributedList = _cache.getDataStructuresManager().createList(listName, attributes, null,  Customer.class);
        }

        // Returning distributed list
        return distributedList;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to add customers to priority list
     *
     * @param distributedList Distributed list to which customers are to be added
     */
    private static void addPriorityCustomers(List<Customer> distributedList) {
        // Printing output on console
        System.out.println("Adding customers to distributed list.");

        // Creating and adding customers to distributed list
        Customer c1 = new Customer();
        c1.setContactName("David Johnes");
        c1.setCompanyName("Lonesome Pine Restaurant");
        c1.setContactNo("(1) 354-9768");
        c1.setAddress("Silicon Valley, Santa Clara, California");
        distributedList.add(c1);

        Customer c2 = new Customer();
        c2.setContactName("Carlos Gonzalez");
        c2.setCompanyName("LILA-Supermercado");
        c2.setContactNo("(9) 331-6954");
        c2.setAddress("Carrera 52 con Ave. Bolivar #65-98 Llano Largo");
        distributedList.add(c2);

        Customer c3 = new Customer();
        c3.setContactName("Carlos Hernandez");
        c3.setCompanyName("HILARION-Abastos");
        c3.setContactNo("(5) 555-1340");
        c3.setAddress("Carrera 22 con Ave. Carlos Soublette #8-35");
        distributedList.add(c3);

        Customer c4 = new Customer();
        c4.setContactName("Elizabeth Brown");
        c4.setCompanyName("Consolidated Holdings");
        c4.setContactNo("(171) 555-2282");
        c4.setAddress("Berkeley Gardens 12 Brewery");
        distributedList.add(c4);

        Customer c5 = new Customer();
        c5.setContactName("Felipe Izquierdo");
        c5.setCompanyName("LINO-Delicateses");
        c5.setContactNo("(8) 34-56-12");
        c5.setAddress("Ave. 5 de Mayo Porlamar");
        distributedList.add(c5);

        // Printing output on console
        System.out.println("Customers are added to distributed list.");
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to add customers to regular list
     *
     * @param distributedList Distributed list to which customers are to be added
     */
    private static void addRegularCustomers(List<Customer> distributedList) {
        // Printing output on console
        System.out.println("Adding customers to distributed list.");

        // Creating and adding customers to distributed list
        Customer c1 = new Customer();
        c1.setContactName("Maria Anders");
        c1.setCompanyName("Alfreds Futterkiste");
        c1.setContactNo("(5) 555-4729");
        c1.setAddress("Obere Str. 57, Berlin");
        distributedList.add(c1);

        Customer c2 = new Customer();
        c2.setContactName("Ana Trujillo");
        c2.setCompanyName("Ana Trujillo Emparedados y helados");
        c2.setContactNo("(5) 555-4729");
        c2.setAddress("Avda. de la Constitución 2222, México D.F.");
        distributedList.add(c2);

        Customer c3 = new Customer();
        c3.setContactName("Antonio Moreno");
        c3.setCompanyName("Antonio Moreno Taquería");
        c3.setContactNo("(5) 555-3932");
        c3.setAddress("Mataderos 2312, México D.F.");
        distributedList.add(c3);

        // Printing output on console
        System.out.println("Customers are added to distributed list.");
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to get number of objects in distributed list
     *
     * @param distributedList Distributed list from which objects are to be fetched
     */
    private static void displayList(List<Customer> distributedList) {
        // Printing output on console
        System.out.println("Fetching all customers from distributed list.");

        // Creating array to hold customers fetched from distributed list
        Customer[] cachedCustomers = new Customer[distributedList.size()];

        // Creating counter to keep track of number of customers fetched
        int counter = 0;

        // Iterating over distributed list to fetch customers
        for (Customer customer : distributedList) {
            // Adding customer to array
            cachedCustomers[counter++] = customer;
        }

        // Printing output on console
        System.out.println(counter + " Customers are fetched from distributed list");
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to get an object from distributed list specified by index
     *
     * @param index Index of object to be fetched from distributed list
     * @param distributedList Distributed list from which object is to be fetched
     * @return Customer retrieved from distributed list by index
     */
    private static Customer getCustomerFromList(int index, List<Customer> distributedList) {
        // Fetching customer from distributed list by index
        Customer cachedCustomer = distributedList.get(index);

        // Printing output on console
        System.out.println("Customer is fetched from distributed list");

        // Printing details of fetched customer
        printCustomerDetails(cachedCustomer);
        System.out.println();

        // Returning fetched customer
        return cachedCustomer;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to update objects in distributed list
     *
     * @param index Index of object to be updated at in distributed list
     * @param customer Instance of Customer that will be updated in the distributed list
     * @param distributedList Distributed list in which object is to be updated
     */
    private static void updateObjectsInList(int index, Customer customer, List<Customer> distributedList) {
        // Printing output on console
        System.out.println("Updating customer in distributed list.");

        // Modifying customer details
        customer.setCompanyName("Gourmet Lanchonetes");

        // Updating customer in distributed list at specified index
        distributedList.set(index, customer);

        // Printing output on console
        System.out.println("Customer is updated in distributed list.");
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to remove an object from distributed list by index
     *
     * @param index Index of object to be deleted from distributed list
     * @param distributedList Distributed list from which object is to be deleted
     */
    private static void removeViaIndex(int index, List<Customer> distributedList) {
        // Printing output on console
        System.out.println("Removing customer from distributed list via index.");

        // Removing customer from distributed list at specified index
        distributedList.remove(index);

        // Printing output on console
        System.out.println("Customer is removed from distributed list.");
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to remove an object in distributed list by instance of object
     *
     * @param customer Instance of object to be deleted from distributed list
     * @param distributedList Distributed list from which object is to be deleted
     */
    private static void removeViaInstance(Customer customer, List<Customer> distributedList) {
        // Printing output on console
        System.out.println("Removing customer from distributed list via instance.");

        // Removing customer from distributed list by instance and getting status
        boolean removed = distributedList.remove(customer);

        // Printing output on console if customer is removed or not
        System.out.println("Customer is" + (removed ? " " : " not ") + "removed from distributed List.");
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to remove distributed list from cache
     *
     * @param listName Name of distributed list to be removed from cache
     */
    private static void deleteList(String listName) throws CacheException, IOException {
        // Printing output on console
        System.out.println("Removing distributed list from cache.");

        // Removing distributed list from cache by name
        _cache.getDataStructuresManager().remove(listName);

        // Printing output on console
        System.out.println("Distributed list successfully removed from cache.");
        System.out.println();
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to print details of customer type
     *
     * @param customer Customer instance whose attributes are to be printed
     */
    private static void printCustomerDetails(Customer customer) {
        // Validating customer instance
        if (customer == null) return;

        // Printing customer details on console
        System.out.println();
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
            InputStream stream = DistributedList.class
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

