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
let queue = null;

/// <summary>
/// Executing this method will perform all the operations of the sample
/// </summary>
async function run() {

    await initializeCache();
    await cache.clear();

    let queue = await getOrCreateQueue();

    // Adding items to distributed queue
    queue = await addObjectsToQueue(queue);

    // Get the first object from distributed queue
    console.log("\n--- Peek Customer From Queue ---");
    await peekFromQueue();
    console.log();

    // Dequeue from distributed queue
    console.log("\n--- Dequeue customer from Distributed Queue ---");
    await pollFromQueue();
    console.log();

    // Display customers from distributed queue
    console.log("\n--- Display Customers From Queue ---");
    await displayFromQueue(true);
    console.log();


    // Remove the distributed queue from cache
    console.log("\n--- Remove Distributed Queue from Cache --- ");
    await deleteQueueFromCache();
    console.log();

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

async function getOrCreateQueue() {
    let _typeManager = await cache.getDataStructuresManager();
    // Creates a distributed queue to which instances of customers are to be added
    queue = await _typeManager.getQueue("DistributedQueue", ncache.JsonDataType.Object);

    if (queue === null) {
        let attributes = new ncache.DataStructureAttributes();
        let timeSpan = new ncache.TimeSpan(0, 0, 0, 1, 0, 0);
        let expiration = new ncache.Expiration(ncache.ExpirationType.Absolute, timeSpan);
        attributes.setExpiration(expiration);

        // Creating distributed queue with absolute expiration
        // to modify cache properties of the queue, provide an instance of DataTypeAttributes in the second parameter

        queue = await _typeManager.createQueue("DistributedQueue", ncache.JsonDataType.Object, attributes);
    }

    return queue;
}

/// <summary>
/// This method adds objects to distributed queue
/// </summary>
async function addObjectsToQueue(queue) {
    let customer =
    {
        ContactName: "David Johnes",
        CompanyName: "Lonesome Pine Restaurant",
        ContactNo: "(1) 354-9768",
        Address: "Silicon Valley, Santa Clara, California",
    };
    await queue.add(customer);
    await queue.add(customer =
        {
            ContactName: "Carlos Gonzalez",
            CompanyName: "LILA-Supermercado",
            ContactNo: "(9) 331-6954",
            Address: "Carrera 52 con Ave. Bolivar #65-98 Llano Largo",
        });
    await queue.add(customer =
        {
            ContactName: "Carlos Hernandez",
            CompanyName: "HILARION-Abastos",
            ContactNo: "(5) 555-1340",
            Address: "Carrera 22 con Ave. Carlos Soublette #8-35",
        });
    await queue.add(customer =
        {
            ContactName: "Elizabeth Brown",
            CompanyName: "Consolidated Holdings",
            ContactNo: "(171) 555-2282",
            Address: "Berkeley Gardens 12 Brewery",
        });
    await queue.add(customer =
        {
            ContactName: "Felipe Izquierdo",
            CompanyName: "LINO-Delicateses",
            ContactNo: "(8) 34-56-12",
            Address: "Ave. 5 de Mayo Porlamar",
        });

    // Print output on console.
    console.log("\nObjects are added to distributed queue.");
    return queue;
}


async function peekFromQueue() {
    // store next customer for displaying
    let nextCustomer = await queue.peek();

    // print customer details on output
    await printCustomerDetails(nextCustomer);
}

async function pollFromQueue() {
    // store Dequeued customer in a variable
    let customer = await queue.poll();

    // print customer details on output
    await printCustomerDetails(customer);

    // print next customer details on output
    console.log("Next object to Dequeue: ");
    await printCustomerDetails(await queue.peek());
}




/// <summary>
/// Removes distributed queue from cache
/// </summary>
async function deleteQueueFromCache() {
    // Remove queue from cache
    let typeManager = await cache.getDataStructuresManager();
    await typeManager.remove("DistributedQueue");

    console.log("Distributed queue successfully removed from cache.");
}

async function printCustomerDetails(customer) {
    if (customer == null) return;

    console.log();
    console.log("Customer Details are as follows: ");
    console.log("ContactName: " + await customer.ContactName);
    console.log("CompanyName: " + await customer.CompanyName);
    console.log("Contact No: " + await customer.ContactNo);
    console.log("Address: " + await customer.Address);
    console.log();
}


/// Display customers from Distributed Queue
/// </summary>
/// <param name="showResults">whether elements of queue should be printed</param>
async function displayFromQueue() {
    // iterate over each member in queue
    let itv = await queue.iterator();
    while (await itv.hasNext()) {
        await printCustomerDetails(await itv.next());
    }
}

module.exports = {
    run
}