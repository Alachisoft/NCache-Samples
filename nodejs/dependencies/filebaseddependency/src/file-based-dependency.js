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

        let filename = config.app.dependencyFilePath;

        // Generate a json object of customer
        let customer = {
            CustomerID: "Customer:76",
            ContactName: "David Johnes",
            CompanyName: "Lonesome Pine Restaurant",
            ContactNo: "12345-6789",
            Address: "Silicon Valley, Santa Clara, California"
        };

        // create CacheItem to with your desired object
        let cacheItem = new ncache.CacheItem(customer, "Json");

        // add file dependency to cacheItem
        let fileDependency = new ncache.FileDependency(filename);
        cacheItem.setDependency(fileDependency);

        // Adding item dependent on file 
        await cache.add(customer.CustomerID, cacheItem);

        console.log("\nItem '" + customer.CustomerID + "' added to cache with file dependency. ");

        //Change file \\dependencies\\filebaseddependency\\foobar.txt meanwhile program awaits...

        setTimeout(() => {
        }, 5000);

        //... and then check for its existence
        let item = await cache.get(customer.CustomerID, ncache.JsonDataType.Object);
        if (item === null) {
            console.log("Item has been removed due to file dependency.");
        }
        else {
            console.log("File based dependency did not work. Dependency file located at " + filename +
                " might be missing or file not changed within the given interval.");
        }

        //Multiple files names can be specified in FileDependency parameter

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