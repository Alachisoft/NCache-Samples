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

import com.alachisoft.ncache.runtime.events.EventDataFilter;
import com.alachisoft.ncache.runtime.events.EventType;
import com.alachisoft.ncache.samples.data.Customer;
import com.alachisoft.ncache.web.caching.*;
import com.alachisoft.ncache.web.events.query.CQEventArg;
import java.util.Collection;
import java.util.EnumSet;
import java.util.HashMap;
import java.util.Iterator;

public class ContinuousQuerySample
{

    public static void main(String[] args)
    {

        try
        {
            //Initialize new cache
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();

            //New customer product 
            Customer customer = new Customer("John Mathew",
                                        40,
                                        "25564-4546",
                                        "92-A Barnes Road, WA, Ct.",
                                        "Male");

            cache.add("Customer:JohnMathew", customer);

            String query = "SELECT com.alachisoft.ncache.samples.data.Customer WHERE this.gender = ?";

            HashMap values = new HashMap();
            values.put("gender", "Male");

            ContinuousQuery continuousQuery = new ContinuousQuery(query, values);

            //Add query notifications

            //Item added notification
            EnumSet<EventType> addEnumSet = EnumSet.of(EventType.ItemAdded);

            continuousQuery.addDataModificationListener(new QueryDataModificationListener() {

                @Override
                public void queryDataModified(String string, CQEventArg cqea) {
                    
                    System.out.println(string);                                    
                }

                @Override
                public void cacheCleared() {
                    
                    System.out.println("Cache has been cleared...");   
                    
                }
            },addEnumSet,EventDataFilter.None);

            //Item update notification
            
            EnumSet<EventType> updateEnumSet = EnumSet.of(EventType.ItemUpdated);
            
            continuousQuery.addDataModificationListener(new QueryDataModificationListener() {

                @Override
                public void queryDataModified(String string, CQEventArg cqea) {
                    
                    System.out.println(string);                                    
                }

                @Override
                public void cacheCleared() {
                    
                    System.out.println("Cache has been cleared...");   
                    
                }
            },updateEnumSet,EventDataFilter.None);

            //Item delete notification
            
            EnumSet<EventType> removeEnumSet = EnumSet.of(EventType.ItemRemoved);
            
            continuousQuery.addDataModificationListener(new QueryDataModificationListener() {

                @Override
                public void queryDataModified(String string, CQEventArg cqea) {
                    
                    System.out.println(string);                                    
                }

                @Override
                public void cacheCleared() {
                    
                    System.out.println("Cache has been cleared...");   
                    
                }
            },removeEnumSet,EventDataFilter.None);

            //Register continuous query on server
            cache.registerCQ(continuousQuery);

            //getting keys via query
            Collection keys = cache.searchCQ(continuousQuery);
            if (!keys.isEmpty())
            {
                Iterator itor = keys.iterator();
                while (itor.hasNext())
                {
                    String key = (String) itor.next();
                    Customer customerFound = (Customer) cache.get(key);
                    printCustomerDetails(customerFound);
                }
            }

            //getting keyvalue pair via query
            //Much faster way ... avoids round trip to cache
            HashMap result = cache.searchEntriesCQ(continuousQuery);
            if (!result.isEmpty())
            {
                Iterator iter = result.keySet().iterator();
                while (iter.hasNext())
                {
                    Customer customerFound = (Customer) result.get(iter.next());
                    printCustomerDetails(customerFound);
                }
            }

            customer.setAge(35);

            //This should invoke query RegisterUpdateNotification 
            cache.insert("Customer:JohnMathew", customer);

            System.out.println();

            cache.unregisterCQ(continuousQuery);

            //Must dispose cache
            cache.dispose();

            System.exit(0);

        }
        catch (Exception ex)
        {
            System.out.println(ex.getMessage());
            System.exit(0);
        }
    }
    
     public static void printCustomerDetails(Customer customer) {
        System.out.println();        
        System.out.println("Customer Details are as follows: ");
        System.out.println("Name:       " + customer.getName());
        System.out.println("Age:        " + customer.getAge());
        System.out.println("Gender:     " + customer.getGender());
        System.out.println("Contact No: "  + customer.getContactNo());
        System.out.println("Address:    " + customer.getAddress());
    }
}
