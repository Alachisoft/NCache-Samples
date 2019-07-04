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

import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;
import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;
import com.alachisoft.ncache.web.caching.LockHandle;

public class ItemLocking
{

    public static void main(String[] args)
    {

        try
        {
            //Initialize cache            
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();

            //Locking prevents multiple clients from updating the same data simultaneously
            //and also provides the data consistency.

            //Adding an item the cache
            Customer customer = new Customer();
            customer.setName("Kirsten Goli");
            customer.setAge(40);
            customer.setAddress("45-A West Boulevard, Cartago, Costa Rica");
            customer.setGender("Female");
            customer.setContactNo("52566-1779");
            
            cache.add("Customer:KirstenGoli", customer);

            //Get     
            TimeSpan timeSpan = new TimeSpan();
            timeSpan.setSeconds(20);
            LockHandle lockHandle = new LockHandle();
            Customer getCustomer = (Customer) cache.get(
                    "Customer:KirstenGoli",
                    timeSpan,
                    lockHandle,
                    true);

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
        System.out.println("Name: " + customer.getName());
        System.out.println("Age: " + customer.getAge());
        System.out.println("Gender: " + customer.getGender());
        System.out.println("Contact No: "  + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
    }
}
