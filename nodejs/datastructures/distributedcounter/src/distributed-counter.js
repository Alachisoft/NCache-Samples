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
let counter = null;

/// <summary>
/// Executing this method will perform all the operations of the sample
/// </summary>
async function run() {

    await initializeCache();
    await cache.clear();

    await getOrCreateCounter();


    //counter Operations 
    console.log("\n--- Set Counter value ---");
    // set counter value
    let counterValue = await counter.setValue(10);

    // output counter value
    console.log("Counter value set to {0}", counterValue);
    console.log();

    console.log("\n--- Get Counter value ---");
    // get counter value
    counterValue = await counter.getValue();

    // output counter value
    console.log("Counter Value: {0}", counterValue);
    console.log();

    console.log("\n--- Increment Counter ---");
    // increment counter value
    counterValue = await counter.increment();

    // output counter value
    console.log("Counter value incremented by 1");
    console.log("New Counter value is {0}", counterValue);
    console.log();

    console.log("\n--- Increment Counter by value ---");
    // increment counter by 5
    counterValue = await counter.incrementBy(5);

    // output counter value
    console.log("Counter value incremented by {0}", 5);
    console.log("New Counter value is {0}", counterValue);
    console.log();

    console.log("\n--- Decrement Counter ---");
    // decrement counter
    counterValue = await counter.decrement();

    // output counter value
    console.log("Counter value decremented by 1");
    console.log("New Counter value is {0}", counterValue);
    console.log();

    // decrement counter by value
    console.log("\n--- Decrement Counter by value ---");
    // store decremented counter value in local variable
    counterValue = await counter.decrementBy(2);

    // output counter value
    console.log("Counter value decremented by {0}", 2);
    console.log("New Counter value is {0}", counterValue);
    console.log();

    // output counter value
    console.log("\n--- Display Counter Value ---");
    console.log("Counter Value: {0}", await counter.getValue());
    console.log();

    // Remove the distributed counter from cache
    console.log("\n--- Remove Counter from Cache --- ");
    await deleteCounterFromCache();


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

async function getOrCreateCounter() {

    let _typeManager = await cache.getDataStructuresManager();
    // Creates a distributed counter 
    counter = await _typeManager.getCounter("DistributedCounter");

    if (counter === null) {
        let attributes = new ncache.DataStructureAttributes();
        let timeSpan = new ncache.TimeSpan(0, 0, 0, 1, 0, 0);
        let expiration = new ncache.Expiration(ncache.ExpirationType.Absolute, timeSpan);
        attributes.setExpiration(expiration);

        // Creating distributed counter with absolute expiration
        // to modify cache properties of the counter, provide an instance of DataTypeAttributes in the second parameter

        counter = await _typeManager.createCounter("DistributedCounter", 0, attributes);
    }

}

/// <summary>
/// Removes distributed counter from cache
/// </summary>
async function deleteCounterFromCache() {
    // Remove counter from cache
    let typeManager = await cache.getDataStructuresManager();
    await typeManager.remove("DistributedCounter");

    console.log("Distributed counter successfully removed from cache.");
}

module.exports = {
    run
}