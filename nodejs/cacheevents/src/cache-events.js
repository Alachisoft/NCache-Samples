const {CacheManager,CacheItem,CacheDataModificationListener,EventType,EventDataFilter,JsonDataType} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
const {delay} = require('./helper/util');
async function Run() {
    // Initialize cache 
    await InitializeCache();
    // Generate a new instance of customer
    customer = CreateNewCustomer();

    const key = GetKey(customer);

    // Add item in cache
    await AddItem(key, customer);

    // Register Notification for given key
    await AddNotificationOnKey(key);

    // Update item to trigger key based notification.
    await UpdateItem(key, customer);

    // Delete item to trigger key based notification.
    await DeleteItem(key);

    //wait to recieve events
    await delay(1000);

    await UnRegisterCacheNotification(key);
        //close cache
    await cache.close();
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
async function InitializeCache() {
    // Get Cache Name
    const cacheName = config.cacheProperties.cacheName
    if (!cacheName) {
        console.log("The CacheID cannot be null or empty.");
        return;
    }
    // Initialize an instance of the cache to begin performing operations:
    cache = await CacheManager.getCache(cacheName);

    cache.clear();
    // Print output on console
    console.log(`Cache ${cacheName} is initialized.`);
            
}

async function AddItem(key, customer) {

    const item = new CacheItem(customer,"Customer");
    await cache.add(key, item);

    // Print output on console
    console.log("Object is added to cache.");
}

async function AddNotificationOnKey(key) {
    const messagingService = await cache.getMessagingService();

    await messagingService.addCacheNotificationListener(
        key,
        new CacheDataModificationListener(onCacheDataModified),
        [EventType.ItemUpdated],
        EventDataFilter.Metadata);
    await messagingService.addCacheNotificationListener(
        key,
        new CacheDataModificationListener(onCacheDataModified),
        [EventType.ItemRemoved],
        EventDataFilter.Metadata);
    console.log("Cache Notification registered for key " + key);    
}
async function UnRegisterCacheNotification(key) {
    const messagingService = await cache.getMessagingService();
    messagingService.removeCacheNotificationListener([key],
        new CacheDataModificationListener(onCacheDataModified),
        [EventType.ItemRemoved,EventType.ItemUpdated])
    console.log("Cache Notification unregistered for key " + key);
}

function onCacheDataModified(key, cacheEventArgs) {
    const eventType  = cacheEventArgs.getEventType()
    switch (eventType)
    {
        case EventType.ItemAdded:
            console.log("Notification -> Key: " + key + " is added to the cache");
            break;
        case EventType.ItemUpdated:
            console.log("Notification -> Key: " + key + " is updated in the cache");
            break;
        case EventType.ItemRemoved:
            console.log("Notification -> Key: " + key + " is removed from the cache");
            break;
        
    }
}
async function  UpdateItem(key,customer) {
    // Update item
    customer.CompanyName = "Gourmet Lanchonetes";
    // create cache item with required data and add expiration
    let item = new CacheItem(customer,"Customer");
    // insert to cache
    await cache.insert(key, item);

    // Print output on console
    console.log("\nObject is updated in cache.");
}

async function  DeleteItem(key) {
    // Remove the existing customer
    const result = await cache.remove(key,JsonDataType.Object);
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
module.exports = {
    Run
}