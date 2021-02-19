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
    /// This method configures oracle dependency
    /// </summary>
     async function run() 
    {
       await initializeCache();
        await cache.clear();

        // Change notification previleges must be granted for OracleDBDependency to work
        // -- run this script
        // -- 'grant change notification to username' without quotes where username 
        // is to replaced with one being used by the db instance

        // Notification based DBDependency
        // JDBC connectionString     
        let connectionString = config.db.oracleConnectionString;
        let commandText = "SELECT ROWID, ProductID, ProductName, UnitPrice FROM Products WHERE ProductID = 1";

        // create CacheItem to with your desired object
        let cacheItem = new ncache.CacheItem("OracleDependentValue");

        let oracleDependency = new ncache.OracleCacheDependency(connectionString, commandText);
        cacheItem.setDependency(oracleDependency);

        // Inserting item into cache 
        await cache.add("key-1", cacheItem);

        console.log("\nItem 'key-1' added to cache with oracle dependency. ");

        // Any record that is modified in the database is invalidated in cache and thus the cache item is removed.
        // To Verify modify the record in the database
        // The code checks if the record is removed form the cache after modification.

        if (await cache.contains("key-1")) {
            console.log("Oracle dependency did not work as expected. " + "Check your connection string and sql command syntax.");
        }
        else {
            console.log("Item:1 removed due to oracle dependency.");
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