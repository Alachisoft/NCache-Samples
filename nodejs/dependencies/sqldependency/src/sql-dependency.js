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
/// This method adds file based dependency 
/// </summary>
async function run() {

    await initializeCache();
    await cache.clear();

    let connectionString = config.db.sqlconnectionString;
    let queryString = "SELECT ProductID, ProductName, QuantityPerUnit, UnitPrice FROM dbo.PRODUCTS WHERE PRODUCTID = 6";

    // Generate a json object of product
    let product = {
        ProductID: 6,
        ProductName: "Grandma's Boysenberry Spread",
        SupplierID: 3,
        CategoryID: 2,
        QuantityPerUnit: "12 - 8 oz jars",
        UnitPrice: 25,
        UnitsInStock: 120,
        UnitsOnOrder: 10,
        ReorderLevel: 25,
        Discontinued: false,
    };

    // create CacheItem to with your desired object
    let cacheItem = new ncache.CacheItem(product, "Json");

    // add sql dependency to cacheItem
    let sqlServerDependency = new ncache.SqlCacheDependency(connectionString, queryString)
    cacheItem.setDependency(sqlServerDependency);

    // Inserting product into cache 
    await cache.add("ProductId:6", cacheItem);

    console.log("\nItem 'ProductId:6' added to cache with sql dependency. ");

    // Any item that is modified in the database will be invalidated in the cache.
    // Thus the item will be removed from the cache.

    console.log("Update product with id 6 in database");


    setTimeout(() => {
    }, 5000);

    //... and then check for its existence
    let item = await cache.get("ProductId:6", ncache.JsonDataType.Object);

    if (item === null) {
        console.log("product object removed from cache due to sql dependency.");
    }
    else {
        console.log("SQL Server dependency did not work as expected. " + "Check your connection string and sql command syntax.");
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

module.exports = {
    run
}