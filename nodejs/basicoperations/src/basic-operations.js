const {CacheManager,CacheItem,TimeSpan, Expiration,JsonDataType, ExpirationType} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
async function Run() {
    // Initialize cache 
    await InitializeCache();
    // Create a simple customer object
    let customer = CreateNewCustomer();
    const key = GetKey(customer);

    // Adding item
    await AddObjectToCache(key, customer);

    // Get the object from cache
    customer = await GetObjectFromCache(key);

    // Modify the object and update in cache
    await UpdateObjectInCache(key, customer);

    // Delete the existing object
    await RemoveObjectFromCache(key);

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
    // creating time span for setting expiration interval
    const expirationInterval = new TimeSpan(0, 1, 0);

    // creating expiration interval with absolute type to assign to cache item
    const expiration =  new Expiration(ExpirationType.Absolute);
    expiration.setExpireAfter(expirationInterval)

    //Populating cache item and adding new expiration
    const item = new CacheItem(customer,"Customer");
    item.setExpiration(expiration);

    // Adding cacheitem to cache with an absolute expiration of 1 minute
    await cache.add(key, item);

    // Print output on console
    console.log("\nObject is added to cache.");
}

async function GetObjectFromCache(key) {
    const cachedCustomer = await cache.get(key,JsonDataType.Object);

    // Print output on console
    console.log("\nObject is fetched from cache");

    PrintCustomerDetails(cachedCustomer);
    return cachedCustomer;
}

async function UpdateObjectInCache(key, customer) {
     // Update item
     customer.CompanyName = "Gourmet Lanchonetes";

     // create expiration time of 30 seconds
     const expirationInterval = new TimeSpan(0, 0, 30);

     // create expiration interval with sliding type
     let expiration = new Expiration(ExpirationType.Sliding);
     expiration.ExpireAfter = expirationInterval;

     // create cache item with required data and add expiration
     let item = new CacheItem(customer,"Customer");
     item.setExpiration(expiration);

     // insert to cache
     await cache.insert(key, item);

     // Print output on console
     console.log("\nObject is updated in cache.");

}

async function RemoveObjectFromCache(key) {
    // Remove the existing customer
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