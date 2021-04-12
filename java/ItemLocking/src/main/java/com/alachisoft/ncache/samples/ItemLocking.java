// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache ItemLocking sample
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
import com.alachisoft.ncache.client.LockHandle;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;

import java.io.*;
import java.util.Properties;

public class ItemLocking
{

    public static void run() throws Exception {
            //Initialize cache            
            Properties properties =  getProperties();
            String cacheName= properties.getProperty("CacheID");
            Cache cache = CacheManager.getCache(cacheName);
            cache.clear();

            //Locking prevents multiple clients from updating the same data simultaneously
            //and also provides the data consistency.

            //Adding an item the cache
            Customer customer = new Customer();
            customer.setContactName("Kirsten Goli");
            customer.setAddress("45-A West Boulevard, Cartago, Costa Rica");
            customer.setContactNo("52566-1779");
            
            cache.add("Customer:KirstenGoli", customer);

            //Get     
            TimeSpan timeSpan = new TimeSpan(0, 0, 20);
            LockHandle lockHandle = new LockHandle();
            Customer getCustomer = cache.get(
                    "Customer:KirstenGoli",
                    true,
                    timeSpan,
                    lockHandle, Customer.class);

            printCustomerDetails(getCustomer);

            System.out.println("Lock acquired on " + lockHandle.getLockId());

            //Lock item in cache
            boolean isLocked = cache.lock("Customer:KirstenGoli", timeSpan, lockHandle);

            if (!isLocked)
            {
                System.out.println("Lock acquired on " + lockHandle.getLockId());
            }

            //Unlock item in cache
            cache.unlock("Customer:KirstenGoli");

            //Unlock via lockhandle
            cache.unlock("Customer:KirstenGoli", lockHandle);

            //Must dispose cache
            cache.close();

            System.exit(0);

    }
    
    public static void printCustomerDetails(Customer customer) {
        System.out.println();        
        System.out.println("Customer Details are as follows: ");
        System.out.println("Name: " + customer.getContactName());
        System.out.println("Contact No: "  + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = ItemLocking.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }
}
