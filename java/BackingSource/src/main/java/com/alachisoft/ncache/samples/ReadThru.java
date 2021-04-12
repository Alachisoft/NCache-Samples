// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Read Through sample class.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.runtime.caching.expiration.Expiration;
import com.alachisoft.ncache.runtime.caching.expiration.ExpirationType;
import com.alachisoft.ncache.runtime.datasourceprovider.DistributedDataStructureType;
import com.alachisoft.ncache.runtime.datasourceprovider.ProviderCacheItem;
import com.alachisoft.ncache.runtime.datasourceprovider.ProviderDataStructureItem;
import com.alachisoft.ncache.runtime.datasourceprovider.ReadThruProvider;
import com.alachisoft.ncache.samples.data.DataSource;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

/**
 * Contains methods used to read an object by its key from the master data source.
 */
public class ReadThru implements ReadThruProvider
{

    DataSource _dataSource;

    /**
     * Perform tasks like allocating resources or acquiring connections
     * @param map Startup paramters defined in the configuration
     * @param s Define for which cache provider is configured
     * @throws Exception
     */
    @Override
    public void init(Map<String, String> map, String s) throws Exception {
        String connString = map.get("connString");
        String user = map.get("user");
        String pass = map.get("pass");
        _dataSource = new DataSource();
        _dataSource.Init(connString, user, pass);
    }

    /**
     Responsible for loading an object from the external data source.
     Key is passed as parameter.
     @param key item identifier; probably a primary
     @return data contained in ProviderCacheItem
    */
    @Override
    public ProviderCacheItem loadFromSource(String key) throws Exception {
        ProviderCacheItem cacheItem = new ProviderCacheItem(_dataSource.loadCustomer(key));
        cacheItem.getResyncOptions().setResyncOnExpiration(true);
        // Resync provider name will be picked from default provider.
        return cacheItem;
    }


    /**
     * Responsible for loading multiple objects from the external data source.
     * @param collection Collection of keys to be fetched from data source
     * @return Dictionary of keys with respective data contained in ProviderCacheItem
     * @throws Exception
     */
    @Override
    public Map<String, ProviderCacheItem> loadFromSource(Collection<String> collection) throws Exception {
        // initialize dictionary to return to cache
        Map<String, ProviderCacheItem> providerItems = new HashMap<>();
        // iterate through all the keys and fetch data
        for (String key : collection)
        {
            // get data from data source
            Object data = _dataSource.loadCustomer(key);
            // initialize ProviderCacheItem with the received data
            ProviderCacheItem cacheItem = new ProviderCacheItem(data);
            // you can change item properties before adding to cache. For example
            // adding expiration
            cacheItem.setExpiration(new Expiration(ExpirationType.DefaultAbsolute));
            // assigning group to key
            cacheItem.setGroup("customers");
            // add the provider item with the key to result
            providerItems.put(key, cacheItem);
        }
        // return result to cache
        return providerItems;
    }

    /**
     * Responsible for loading data structures from the external data source.
     * @param key key to fetch from data source
     * @param distributedDataStructureType type of data structure received
     * @return Data structure contained in ProviderCacheItem which can be enumerated
     * @throws Exception
     */
    @Override
    public ProviderDataStructureItem loadDataStructureFromSource(String key, DistributedDataStructureType distributedDataStructureType) throws Exception {

        switch (distributedDataStructureType){

            case List:
                return new ProviderDataStructureItem<>(_dataSource.loadCustomersFromCountry(key));
            case Queue:
                return new ProviderDataStructureItem<>(_dataSource.loadCustomersByOrder(key));
            case Set:
                return new ProviderDataStructureItem<>(_dataSource.LoadOrderIDsByCustomer(key));
            case Map:
                return new ProviderDataStructureItem<>(_dataSource.LoadCustomersByCity(key));
            case Counter:
                return new ProviderDataStructureItem<>(_dataSource.getCustomerCountByCompanyName(key));
            default:
                throw new IllegalArgumentException(distributedDataStructureType.name() + " Data type not supported...");
        }
    }

    /**
     * Perform tasks associated with freeing, releasing, or resetting resources.
     * @throws Exception
     */
    @Override
    public void close() throws Exception {
        _dataSource.closeConnection();
    }
}
