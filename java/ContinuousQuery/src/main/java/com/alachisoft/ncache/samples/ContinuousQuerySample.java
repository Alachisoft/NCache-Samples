// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Continuous Query sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.runtime.events.EventDataFilter;
import com.alachisoft.ncache.runtime.events.EventType;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.*;
import java.util.EnumSet;
import java.util.Properties;

public class ContinuousQuerySample {

    public static void run() throws Exception {
            //Initialize new cache
            //Initialize cache
            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();

            //New customer product 
            Customer customer = new Customer();
            customer.setContactName("John Mathew");
            customer.setPhone("25564-4546");
            customer.setAddress("92-A Barnes Road, WA, Ct.");

            cache.add("Customer:JohnMathew", customer);

            String query = "SELECT com.alachisoft.ncache.java.com.alachisoft.ncache.samples.data.Customer WHERE this.phone = ?";

            QueryCommand command = new QueryCommand(query);
            command.getParameters().put("gender", "25564-4546");

            ContinuousQuery continuousQuery = new ContinuousQuery(command);

            //Add query notifications

            //Item added notification
            EnumSet<EventType> addEnumSet = EnumSet.of(EventType.ItemAdded);

            continuousQuery.addDataModificationListener(new QueryDataModificationListener() {
                                                            @Override
                                                            public void onQueryDataModified(String s, CQEventArg cqEventArg) {
                                                                System.out.println(s);
                                                            }
                                                        },
                    addEnumSet, EventDataFilter.None);

            //Item update notification

            EnumSet<EventType> updateEnumSet = EnumSet.of(EventType.ItemUpdated);

            continuousQuery.addDataModificationListener(new QueryDataModificationListener() {
                                                            @Override
                                                            public void onQueryDataModified(String s, CQEventArg cqEventArg) {
                                                                System.out.println(s);
                                                            }
                                                        },
                    updateEnumSet, EventDataFilter.None);

            //Item delete notification

            EnumSet<EventType> removeEnumSet = EnumSet.of(EventType.ItemRemoved);

            continuousQuery.addDataModificationListener(new QueryDataModificationListener() {
                                                            @Override
                                                            public void onQueryDataModified(String s, CQEventArg cqEventArg) {
                                                                System.out.println(s);
                                                            }
                                                        },
                    removeEnumSet, EventDataFilter.None);

            //Register continuous query on server
            cache.getMessagingService().registerCQ(continuousQuery);

            //getting keys via query
            CacheReader reader = cache.getSearchService().executeReader(command);
            if (reader.getFieldCount() > 0) {
                while (reader.read()) {
                    Customer customerFound = reader.getValue(1, Customer.class);
                    printCustomerDetails(customerFound);
                }
            }

            customer.setPhone("12345678");

            //This should invoke query RegisterUpdateNotification 
            cache.insert("Customer:JohnMathew", customer);

            System.out.println();

            // Unregister notifications for ItemAdded events only
            continuousQuery.removeDataModificationListener(new QueryDataModificationListener() {
                @Override
                public void onQueryDataModified(String s, CQEventArg cqEventArg) {
                    System.out.println(s);
                }
            }, addEnumSet);

            cache.getMessagingService().unRegisterCQ(continuousQuery);

            //Must dispose cache
            cache.clear();

            System.exit(0);

    }

    public static void printCustomerDetails(Customer customer) {
        System.out.println();
        System.out.println("Customer Details are as follows: ");
        System.out.println("Name:       " + customer.getContactName());
        System.out.println("Contact No: " + customer.getContactNo());
        System.out.println("Address:    " + customer.getAddress());
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = ContinuousQuerySample.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
