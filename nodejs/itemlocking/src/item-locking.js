const {CacheManager,CacheItem,LockHandle,TimeSpan, Expiration,JsonDataType, ExpirationType} = require('ncache-client');
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
    await AddObjectToCache(key, customer);

    const lockHandle = new LockHandle();
    const timeSpan = new TimeSpan(0,0,0,20);
    // Get the object from cache
    await GetObjectFromCache(key,timeSpan,lockHandle);

    // Lock Item in cache 
    await LockItemInCache(key, timeSpan,lockHandle);

    // Unlock item in cache using multiple ways
    await UnLockItemInCache(key, lockHandle);

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
    item.Expiration = expiration;

    // Adding cacheitem to cache with an absolute expiration of 1 minute
    await cache.add(key, item);

    // Print output on console
    console.log("\nObject is added to cache.");
}

async function GetObjectFromCache(key,timeSpan,lockHandle) {
    const cachedCustomer = await cache.get(key,JsonDataType.Object,null,null,true,timeSpan,lockHandle);
    PrintCustomerDetails(cachedCustomer);
}

async function RemoveObjectFromCache(key) {
    // Remove the existing customer
    await cache.remove(key,JsonDataType.Object);

    // Print output on console
    console.log("\nObject is removed from cache.");
}
async function LockItemInCache(key, timeSpan, lockHandle) {
    const isLocked = await cache.lock(key,timeSpan,lockHandle);
    if (!isLocked)
    {
        console.log("Lock acquired on " + lockHandle.getLockId());
    }
}
async function UnLockItemInCache(key, lockHandle) {
    await cache.unlock(key,lockHandle)
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