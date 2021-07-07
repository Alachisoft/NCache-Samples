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
let list = null;


/// <summary>
/// Executing this method will perform all the operations of the sample
/// </summary>
async function run() {

    await initializeCache();

    await cache.clear();

    await getOrCreateList();

    // Adding items to distributed list
    list = await addObjectsToList(list);

    // Get a single object from distributed list by index
    let cachedCustomer = await list.get(1);
    // Print output on console
    console.log("\nObject is fetched from distributed list");
    console.log(cachedCustomer);

    // Modify the object and update in distributed list
    cachedCustomer.CompanyName = "Gourmet Lanchonetes";
    await list.add(1, cachedCustomer);

    await list.removeIndex(1);
    let customer =
    {
        ContactName: "Carlos Gonzalez",
        CompanyName: "LILA-Supermercado",
        ContactNo: "(9) 331-6954",
        Address: "Carrera 52 con Ave. Bolivar #65-98 Llano Largo",
    }

    let removed = await list.remove(customer);
    console.log("\nObject is" + ((removed) ? " " : " not ") + "removed from distributed List.");


    // Remove the distributed list from cache
    console.log("\n--- Remove lsit from Cache --- ");
    await deleteListFromCache();

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

async function getOrCreateList() {
    let _typeManager = await cache.getDataStructuresManager();
    // Creates a distributed list to which instances of customers are to be added
    list = await _typeManager.getList("DistributedList", ncache.JsonDataType.Object);

    if (list === null) {
        let attributes = new ncache.DataStructureAttributes();
        let timeSpan = new ncache.TimeSpan(0, 0, 0, 1, 0, 0);
        let expiration = new ncache.Expiration(ncache.ExpirationType.Absolute, timeSpan);
        attributes.setExpiration(expiration);

        // Creating distributed list with absolute expiration
        // to modify cache properties of the list, provide an instance of DataTypeAttributes in the second parameter

        list = await _typeManager.createList("Distributedlist", ncache.JsonDataType.Object, attributes);
    }
}

/// <summary>
/// This method adds objects to distributed list
/// </summary>
async function addObjectsToList(list) {
    let customer =
    {
        ContactName: "David Johnes",
        CompanyName: "Lonesome Pine Restaurant",
        ContactNo: "(1) 354-9768",
        Address: "Silicon Valley, Santa Clara, California",
    };
    await list.add(customer);
    await list.add(customer =
        {
            ContactName: "Carlos Gonzalez",
            CompanyName: "LILA-Supermercado",
            ContactNo: "(9) 331-6954",
            Address: "Carrera 52 con Ave. Bolivar #65-98 Llano Largo",
        });
    await list.add(customer =
        {
            ContactName: "Carlos Hernandez",
            CompanyName: "HILARION-Abastos",
            ContactNo: "(5) 555-1340",
            Address: "Carrera 22 con Ave. Carlos Soublette #8-35",
        });
    await list.add(customer =
        {
            ContactName: "Elizabeth Brown",
            CompanyName: "Consolidated Holdings",
            ContactNo: "(171) 555-2282",
            Address: "Berkeley Gardens 12 Brewery",
        });
    await list.add(customer =
        {
            ContactName: "Felipe Izquierdo",
            CompanyName: "LINO-Delicateses",
            ContactNo: "(8) 34-56-12",
            Address: "Ave. 5 de Mayo Porlamar",
        });

    // Print output on console.
    console.log("\nObjects are added to distributed list.");
    return list;
}

/// <summary>
/// Removes distributed list from cache
/// </summary>
async function deleteListFromCache() {
    // Remove list from cache
    let typeManager = await cache.getDataStructuresManager();
    await typeManager.remove("DistributedList")

    console.log("Distributed list successfully removed from cache.");
}

module.exports = {
    run
}