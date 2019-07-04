// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Cache Item Versioning sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.samples.data.Customer;
import com.alachisoft.ncache.web.caching.*;

public class CacheItemVersioning
{

    public static void main(String[] args)
    {

        try
        {
            //Initialize cache
            Cache cache;
            cache = NCache.initializeCache("mycache");
            cache.clear();

            //Cache item versioning makes possible the concurrency checks on cacheitems.
            //Scenario:
            //User X and Y has fetched data from cache
            //User Y makes changes to some cache items and write back the changes to cache
            //Now the version of cache items with User X are obsolete and it can determine 
            //so by using cache item versioning.

            Customer customer = new Customer();
            customer.setName("Scott Prince");
            customer.setAddress("95-A Barnes Road Wallingford, CT");
            customer.setGender("Male");
            customer.setContactNo("25632-5646");
            customer.setAge(23);

            CacheItemVersion version1 = cache.add("Customer:ScottPrince", customer);

            //updaing the customer object in cache;
            customer.setAge(33);
            CacheItemVersion version2 = cache.insert("Customer:ScottPrince", customer);

            if (version1 != version2)
            {
                System.out.println("Item has changed since last time it was fetched.");
                System.out.println();
            }

            //GetItNewer
            //Retrives item from cache based on CacheItemVersion
            //Get item only is version superior to version2
            Customer customer2 = (Customer) cache.getIfNewer("Customer:ScottPrince",
                                                             version2 /*Version to be compared from cache */);

            if (customer2 == null)
            {
                System.out.println("Latest version of item is already available");
            }
            else
            {
                System.out.println("Current Version of Customer:ScottPrince is: " + version2.getVersion());
                printCustomerDetails(customer2);
            }

            //Remove
            customer.setName("Mr. Scott n Price");
            CacheItemVersion version3 = cache.insert("Customer:ScottPrince", customer);

            Customer customerRemoved = (Customer) cache.remove("Customer:ScottPrince", version3);

            if (customerRemoved == null)
            {
                System.out.println("Remove failed. The newer version of item exists in cache.");
            }
            else
            {
                System.out.println("Following Customer is removed from cache:");
                printCustomerDetails(customerRemoved);
            }

            //Always manipulate the latest item as follows.

            CacheItemVersion version4 = cache.insert("Customer:ScottPrince", customer);
            customer2 = (Customer) cache.getIfNewer("Customer:ScottPrince", version4);

            if (customer2 == null)
            {
                System.out.println("Item version available is latest!");
            }
            else
            {
                System.out.println("Latest available Item version is fetched from cache");
            }

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
