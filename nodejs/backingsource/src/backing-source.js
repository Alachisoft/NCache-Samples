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
let readThruProviderName = "SqlReadThruProvider";
let writeThruProviderName = "SqlWriteThruProvider";

/// <summary>
/// This method configures oracle dependency
/// </summary>
async function run() {
    // initialize cache before any operations
    await initializeCache();

    // clearing cache to see readthru/writethru changes
    await cache.clear();

    //  customer ID
    console.log("-- Customer ID: 300 ");
    let customerID = "300";

    let customer = await cache.get(customerID, ncache.JsonDataType.Object, null, await new ncache.ReadThruOptions(ncache.ReadMode.ReadThru, readThruProviderName));

    if (customer != null) {

        // display details of entered customer ID
        console.log("\n" + "Fetched Customer Details [readthru]: ");

        displayCustomerData(customer);

        console.log("Update customer detail [writethru]");

        // update existing customer with new data
        storeNewCustomerData();

        // show newly fetched customer details
        console.log("\n" + "Fetched Customer Details [cache]: ");
        customer = cache.get(CustomerID, ncache.JsonDataType.Object);
        displayCustomerData(customer);
    }
    else {
        console.log("\nCustomer with provided ID does not exist.");
    }

    //Must dispose cache
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

/// <summary>
/// Insert customer data to cache
/// </summary>
async function storeNewCustomerData() {
    let customer = {
        CustomerID: "300",
        ContactName: "David Johnes",
        CompanyName: "Lonesome Pine Restaurant",
        ContactNo: "12345-6789",
        Address: "Silicon Valley, Santa Clara, California"
    };

    try {
        await cache.insert(customer.CustomerID, new ncache.CacheItem(customer, "json"), new ncache.WriteThruOptions(ncache.WriteMode.WriteThru, writeThruProviderName));

        console.log("Customer information updated successfuly");
    }
    catch (error) {
        console.log("\n" + "Error: " + error);
    }
}

/// <summary>
/// Displays customer data
/// </summary>
/// <param name="customer">object containing customer details</param>
async function displayCustomerData(customer) {
    console.log("Customer ID: " + customer.CustomerID);
    console.log("Contact Name: " + customer.ContactName);
    console.log("Contact Number: " + customer.ContactNo);
    console.log("Company: " + customer.CompanyName);
    console.log("Address: " + customer.Address);

}

module.exports = {
    run
}