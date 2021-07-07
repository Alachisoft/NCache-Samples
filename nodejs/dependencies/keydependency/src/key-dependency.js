// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

const ncache = require("ncache-client");
const config = require('./app.config.json');

let cache = null;

/// <summary>
/// This method demonstrate a single key dependency 
/// </summary>
async function run() {

    await initializeCache();
    await cache.clear();

    // Key dependency makes an item to get removed when
    // it's accosiated item is removed or has it's value changed.

    // Generate a json objectof  customer
    let customer = {
        CustomerID: "Customer:76",
        ContactName: "David Johnes",
        CompanyName: "Lonesome Pine Restaurant",
        ContactNo: "12345-6789",
        Address: "Silicon Valley, Santa Clara, California"
    };

    // create CacheItem to with your desired object
    let cacheItem = new ncache.CacheItem(customer, "Json");

    // Add customer in cache

    await cache.add(customer.CustomerID, cacheItem);

    console.log("\nItem '" + customer.CustomerID + "' is added to cache.");

    // Generate a json object of order
    let order = {
        OrderID: "10248",
        OrderDate: "1996-08-16 00:00:00.000",
        ShipAddress: "Carrera 22 con Ave. Carlos Soublette #8-35"
    };

    // Generate an instance of key dependency.
    let dependency = new ncache.KeyDependency(customer.CustomerID);

    // Note that an item can be dependent on multiple items in cache this can be done like this
    // let multipleDependency = await new ncache.KeyDependency([customer.CustomerID, 'Customer:52' ]);

    // create CacheItem to with your desired object
    cacheItem = new ncache.CacheItem(order, "Json");

    // add key dependency
    cacheItem.setDependency(dependency);

    // Add order in cache with dependency on the customer added before
    await cache.add(order.OrderID, cacheItem);

    console.log("Item '" + order.OrderID + "' dependent upon '" + customer.CustomerID + "' is added to cache.");

    //To Verify that key dependency is working uncomment the following code.
    //Any modification in the dependent item will cause invalidation of the dependee item.
    //Thus the item will be removed from cache.

    // Delete customer from cache
    await cache.remove(customer.CustomerID, ncache.JsonDataType.Object);

    // Print output on console
    console.log("Item '" + customer.CustomerID + "' is deleted from cache.");

    let value = await cache.get(order.OrderID, ncache.JsonDataType.Object);

    // value should be null
    if (value === null) {
        console.log("Dependent item '" + order.OrderID + "' is successfully removed from cache.");
    }
    else {
        console.log("Error while removing the dependent items from cache.");
    }

    // Dispose the cache once done
    cache.close();
}

async function initializeCache() {
    let cacheName = config.cacheProperties.cacheName;
    if (cacheName === null) {
        console.log("The CacheID cannot be null or empty.");
        return;
    }

    // Initialize an instance of the cache to begin performing operations:
    cache = await ncache.CacheManager.getCache(cacheName);

    // Print output on console
    console.log("Cache has been initialized successfully: ", await cache.toString());
}


module.exports = {
    run
}