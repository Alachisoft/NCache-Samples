const {CacheManager,CacheItem,JsonDataType} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
async function Run() {
    // Initialize cache 
    await InitializeCache();
    await cache.clear();
    // Create a simple customer object
    let customer = CreateNewCustomer();
    const key = GetKey(customer);

    // Adding item
    const itemVersion = await AddObjectToCache(key, customer);

    // Modify the object and update in cache
    const latestItemVersion = await UpdateObjectInCache(key, customer,itemVersion);

    // Get an item with cache item version
    await GetObjectWithItemVersion(key,itemVersion,latestItemVersion);

    // Remove an item if cache item 
    await RemoveObjectFromCache(key,latestItemVersion);

    //close cache
    await cache.close();

}

async function InitializeCache() {
    // Get Cache Name
    const cacheName = config.cacheProperties.cacheName
    if (!cacheName) {
        console.log("The CacheID cannot be null or empty.");
        return;
    }
    // Initialize an instance of the cache to begin performing operations:
    cache = await CacheManager.getCache(cacheName);

    // Print output on console
    console.log(`Cache ${cacheName} is initialized.`);
            
}

async function AddObjectToCache(key, customer) {
    //Populating cache item
    const item = new CacheItem(customer,"Customer");

    let cacheItemVersion = await cache.add(key, item);

    // Print output on console
    console.log("\nObject is added to cache.");
    return  cacheItemVersion;
}

async function GetObjectWithItemVersion(key,oldCacheItemVersion,latestitemVersion) {
    // Retrives item from cache based on CacheItemVersion
    // Get item only is version superior to old version
    let cachedCustomer = await cache.getIfNewer(key,JsonDataType.Object,oldCacheItemVersion);
    if (cachedCustomer === null) {
        // Print output on console
        console.log("Specified item version is latest.");
    } else {
      // Print output on console
      console.log("\nObject is fetched from cache");
      PrintCustomerDetails(cachedCustomer);   
    }

    // Get an item from cache by latest item version
    cachedCustomer = await cache.getIfNewer(key,JsonDataType.Object,latestitemVersion);
    if (cachedCustomer === null) {
        // Print output on console
        console.log("Specified item version is latest.");
    } else {
      // Print output on console
      console.log("\nObject is fetched from cache");
      PrintCustomerDetails(cachedCustomer);   
    }
}

async function UpdateObjectInCache(key, customer,oldItemVersion) {
     // Updaing the customer object in cache;
     customer.CompanyName = "Gourmet Lanchonetes";
     // Create a CacheItem instance and assign Cache Item Version
     let item = new CacheItem(customer,"Customer");
     item.setCacheItemVersion(oldItemVersion);

     // Update item if version match
     const updatedItemVersion = await cache.insert(key, item);
     if (oldItemVersion.getVersion() != updatedItemVersion.getVersion()) {
        console.log("\nItem has changed since last time it was fetched.");
     }
     return updatedItemVersion;
}

async function RemoveObjectFromCache(key) {
   // Remove object from cache if cache item version matched in cache
    await cache.remove(key,JsonDataType.Object);

    // Print output on console
    console.log("\nObject is removed from cache.");
}

function GetKey(customer) {
    return `Customer:${customer.CustomerID}`;
}
function CreateNewCustomer() {
    return({
        CustomerID : "DAVJO",
        ContactName : "David Johnes",
        CompanyName : "Lonesome Pine Restaurant",
        ContactNo : "12345-6789",
        Address : "Silicon Valley, Santa Clara, California",
    });
}
function PrintCustomerDetails(customer) {
    if (customer == null) return;

        console.log();
        console.log("Customer Details are as follows: ");
        console.log("ContactName: " + customer.ContactName);
        console.log("CompanyName: " + customer.CompanyName);
        console.log("Contact No: " + customer.ContactNo);
        console.log("Address: " + customer.Address);
        console.log();

}
module.exports = {
    Run
}