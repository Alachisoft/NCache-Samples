//@ts-check
const {CacheManager,CacheItem,QueryCommand,ContinuousQuery,EventType,EventDataFilter,JsonDataType} = require('ncache-client');
const QueryDataModificationListener = require('ncache-client/src/client/QueryDataModificationListener');
const config = require('./app.config.json');
const {delay} = require("./helper/util");
let cache = null;
async function Run() { 
    // Initialize cache 
    await InitializeCache();

    //clear cache
    await cache.clear();
    // Create a simple products list
    const product = CreateNewProduct();
    // Adding item to cache
    await AddObjectToCache(product);
    // Build Query Command
    const queryCommand = BuildQueryCommand();

    // Register continuous query on cache
    const continuousQuery = await RegisterQuery(queryCommand);

    // Query keys in cache as per criteria
    await QueryKeysInCache(queryCommand);

    // Query data in cache as per criteria
    await QueryDataInCache(queryCommand);

    // Update an item in cache that is within query criteria to raise data modification event
    await UpdateObjectInCache(product);

    // Delete the existing object
    await DeleteObjectFromCache(product);

    //wait to recieve events
    await delay(1000);

    // Unregister query 
    await UnRegisterQuery(continuousQuery);
    
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
async function AddObjectToCache(product) {
    const cacheItem = new CacheItem(product,"Alachisoft.NCache.Sample.Data.Product");
    await cache.add(GetKey(product),cacheItem);
}
function BuildQueryCommand() {
    const query = "SELECT $Value$ FROM Alachisoft.NCache.Sample.Data.Product WHERE Category = ?";
    const queryCommand = new QueryCommand(query);
    let parametersMap = new Map();
    parametersMap.set("Category", "Edibles");
    queryCommand.setParameters(parametersMap);
    return queryCommand;
}
async function RegisterQuery(queryCommand) {
    const continuousQuery = new ContinuousQuery(queryCommand);
    // Add query notifications
    const listener = new QueryDataModificationListener(QueryDataModified);
    continuousQuery.addDataModificationListener(listener,[EventType.ItemRemoved], EventDataFilter.None);
    continuousQuery.addDataModificationListener(listener,[EventType.ItemUpdated], EventDataFilter.DataWithMetadata)

    // Register continuous query on server
    const messagingService = await cache.getMessagingService();
    await messagingService.registerCQ(continuousQuery);

    // Print output on console
    console.log("Continuous Query is registered.");

    return continuousQuery;
}
async function QueryKeysInCache(queryCommand) {
    const searchService = await cache.getSearchService();
    const cacheReader = await searchService.executeReader(queryCommand);
    if (cacheReader.getFieldCount() > 0) {
        while (await cacheReader.read()) {
            let key = cacheReader.getString(0);
            console.log("key: ",key);

            // A second call to fetch product details
            const product = await cache.get(key,JsonDataType.Object);
            PrintProductDetails(product);
        }
    }
}
async function QueryDataInCache(queryCommand) {
    const searchService = await cache.getSearchService();
    const cacheReader = await searchService.executeReader(queryCommand);
    if (cacheReader.getFieldCount() > 0) {
        while (await cacheReader.read()) {
            const product = cacheReader.getValue(1,JsonDataType.Object);
            PrintProductDetails(product);
        }
    }
}
async function UpdateObjectInCache(product) {
    // Modifiying and re-insert the item
    product.ClassName = "Class D";
    const cacheItem = new CacheItem(product,"Alachisoft.NCache.Sample.Data.Product");
    // This should invoke query RegisterUpdateNotification 
    await cache.insert(GetKey(product),  cacheItem);

    // Print output on console
    console.log("Item is updated in cache.");
}
async function DeleteObjectFromCache(product) {
    await cache.remove(GetKey(product), JsonDataType.Object);
    console.log('item is remove from cache');
}
async function  UnRegisterQuery(continuousQuery) {
    // UnRegister the continuous query on server
    const messagingService = await cache.getMessagingService();
    await messagingService.unRegisterCQ(continuousQuery);

    // Print output on console
    console.log("Continuous Query is unregistered.");
}
function QueryDataModified(_string, cQEventArg) {
     // Print output on console
     console.log("Continuous Query data modification event is received from cache.");
     switch (cQEventArg.getEventType())
     {
        case EventType.ItemAdded:
            console.log(`${_string} is added to cache`);
            break;
        case EventType.ItemRemoved:
            console.log(`${_string} is removed from cache`);
            break;
        case EventType.ItemUpdated:
            console.log(`${_string} is updated in cache`);
            break;
        default:
            break;
     }
}
function CreateNewProduct() {
    return({Id : 1, Name : "Dairy Milk Cheese", ClassName : "ClassA", Category : "Edibles",UnitPrice:12});
}
function PrintProductDetails(product) {
    if (product == null) return;
    console.log("\nId:           " + product.Id);
    console.log("Name:         " + product.Name);
    console.log("Class:        " + product.ClassName);
    console.log("Category:     " + product.Category);
    console.log("Unit price:   " + product.UnitPrice)
    console.log();

}
function GetKey(product) {
    return `Product:${product.Id}`
}
module.exports = {
    Run
}