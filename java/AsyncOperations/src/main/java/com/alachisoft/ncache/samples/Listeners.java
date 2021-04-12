package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItemVersion;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Customer;
import com.alachisoft.ncache.samples.interfaces.OnAsyncOperationListener;

import java.util.concurrent.Future;

public class Listeners implements OnAsyncOperationListener {
    final Cache cache;

    public Listeners(Cache cache) {
        this.cache = cache;
    }

    @Override
    public void onItemAdded(String key, Future<CacheItemVersion> itemVersionFuture) throws CacheException {
        System.out.println("onItemAdded");
        if(itemVersionFuture.isDone()){
            System.out.println("Object is added to cache.");
            Customer cachedCustomer = cache.get(key, Customer.class);
            printCustomerDetails(cachedCustomer);
        }
        if(itemVersionFuture.isCancelled()){
            System.out.println("Failed to add key " + key + " in cache.");
        }
    }

    @Override
    public void onItemRemoved(String key, Future<CacheItemVersion> itemVersionFuture) throws CacheException {
        System.out.println("onItemRemoved");
        if(itemVersionFuture.isDone()){
            System.out.println("Object is removed from cache.");
            Customer cachedCustomer = cache.get(key, Customer.class);
            printCustomerDetails(cachedCustomer);
        }
        if(itemVersionFuture.isCancelled()){
            System.out.println("Failed to remove key " + key + " from cache.");
        }
    }

    @Override
    public void onItemUpdated(String key, Future<CacheItemVersion> itemVersionFuture) throws CacheException {
        System.out.println("onItemUpdated");
        if(itemVersionFuture.isDone()){
            System.out.println("Object is updated in cache.");
            Customer cachedCustomer = cache.get(key, Customer.class);
            printCustomerDetails(cachedCustomer);
        }
        if(itemVersionFuture.isCancelled()){
            System.out.println("Failed to update key " + key + " in cache.");
        }
    }

    /**
     This method prints detials of customer type.
     */
    public void printCustomerDetails(Customer customer)
    {
        System.out.println();
        System.out.println("Customer Details are as follows: ");
        System.out.println("ContactName: " + customer.getContactName());
        System.out.println("CompanyName: " + customer.getCompanyName());
        System.out.println("Contact No: " + customer.getContactNo());
        System.out.println("Address: " + customer.getAddress());
        System.out.println();
    }
}
