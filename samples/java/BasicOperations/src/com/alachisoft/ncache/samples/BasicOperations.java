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

import com.alachisoft.ncache.runtime.CacheItemPriority;
import com.alachisoft.ncache.runtime.util.TimeSpan;
import com.alachisoft.ncache.samples.data.Customer;
import com.alachisoft.ncache.web.caching.Cache;
import com.alachisoft.ncache.web.caching.NCache;
import java.util.Calendar;

public class BasicOperations
{

    public static void main(String[] args)
    {

        try
        {
            //Initialize cache via 'initializeCache' using 'Cache Name'
            //to be initialized. 
            Cache cache =  NCache.initializeCache("mycache");
			System.out.println();   
			System.out.println("Cache initialized succesfully");   
            cache.clear();
			System.out.println();   
			System.out.println("Cache cleared succesfully");

            //Another method to add item(s) to cache is via CacheItem  object
            Customer customer = new Customer();
            customer.setName("David Johnes");
            customer.setAge(23);
            customer.setGender("Male");
            customer.setContactNo("12345-6789");
            customer.setAddress("Silicon Valley, Santa Clara, California");                        

            Calendar calendar =  Calendar.getInstance();
            calendar.add(Calendar.MINUTE, 1);
            
            //Adding item with an absolute expiration of 1 minute
            cache.add("Customer:DavidJohnes", customer, null, calendar.getTime(), Cache.NoSlidingExpiration, CacheItemPriority.Normal);
			System.out.println();
            System.out.println("Item added succesfully.");
			
            Customer cachedCustomer = (Customer) cache.get("Customer:DavidJohnes");
            printCustomerDetails(cachedCustomer);
            
            //updating item with a sliding expiration of 30 seconds
            customer.setAge(50);
            cache.insert("Customer:DavidJohnes", customer, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 30), CacheItemPriority.Normal);
            System.out.println("Item updated succesfully.");
            
            //get customer from the cache
            cachedCustomer = (Customer) cache.get("Customer:DavidJohnes");
            printCustomerDetails(cachedCustomer);
            
            //Total item count in the cache
            long count = cache.getCount();
            
            //remove the existing customer
            cache.remove("Customer:DavidJohnes");
            
            //try to get the customer again, getting non-existing items returns null
            cachedCustomer = (Customer) cache.get("Customer:DavidJohnes");
            if(cachedCustomer == null)
				System.out.println("Item removed succesfully.");
			else
				System.out.println("Item is not removed.");
            
            //get count again, item count should be 0
            count = cache.getCount();
            
            
            //Dispose the cache once done
            cache.dispose();

            System.exit(0);

        }
        catch (Exception e)
        {
            System.out.println(e.getMessage());
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
        System.out.println();
    }
    
}
