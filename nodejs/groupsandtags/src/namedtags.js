const {CacheManager,CacheItem,NamedTagsDictionary,QueryCommand,JsonDataType} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
async function Run() {
    // Initialize cache 
    await InitializeCache();
    // Creating named tag dictionary. 
    const namedTagDict = new NamedTagsDictionary();
    namedTagDict.add("Category", "Beverages");
    namedTagDict.add("ProductName", "Coke");

    // Add Items in cache with named tags
    await AddItems(namedTagDict);

    // Fetch Items from the cache
    await GetItems();

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
    await cache.clear();
    // Print output on console
    console.log(`Cache ${cacheName} is initialized.`);
            
}
async function AddItems(namedTagDict) {
    await Promise.all([
            AddNamedTagDataToCache(14, "Andrew Ng","USA",namedTagDict),
            AddNamedTagDataToCache(15, "Jhon Getsby", "France", namedTagDict)
    ]);
}
async function GetItems() {
    const query = "SELECT $Value$ FROM Alachisoft.NCache.Sample.Data.Order WHERE Category = ? AND ProductName = ?";
    const queryCommand = new QueryCommand(query);
    let parametersMap = new Map();
    parametersMap.set("Category", "Beverages");
    parametersMap.set("ProductName", "Coke");
    queryCommand.setParameters( parametersMap);
    const searchService = await cache.getSearchService();
    const cacheReader = await searchService.executeReader(queryCommand);
    let counter = 0 ;
    if (cacheReader.getFieldCount() > 0) {
        while (await cacheReader.read()) {
            const order = cacheReader.getValue(1,JsonDataType.Object);
            console.log(order);
            counter++;
        }
    }
}
async function AddNamedTagDataToCache(productId, shipName, shipCountry ,namedTagDict) {
    const order = { OrderId : productId, ShipName : shipName, ShipCountry : shipCountry};

    // create object key
    const key = "order:" + order.OrderId;

    // create CacheItem with your desired object
    const  item = new CacheItem(order,"Alachisoft.NCache.Sample.Data.Order");

    // assign group to CacheItem object
    item.setNamedTags(namedTagDict)

    // add CacheItem object to cache
    await cache.add(key, item);
    console.log("items added in cache")
}
module.exports = {
    Run
}