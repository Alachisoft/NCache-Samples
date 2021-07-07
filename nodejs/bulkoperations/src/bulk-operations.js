const {CacheManager,CacheItem,JsonDataType} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
async function Run() {
    // Initialize cache 
    await InitializeCache();
    // Create a simple product object
    const products = CreateNewProducts();
    const keys = GetKeys(products);

    // Generate hashmap of the keys and products
    let productItems = GetCacheItemMap(keys, products);
    // Adding item
    await AddMultipleObjectsToCache(productItems);

    // Modify the object and update in cache
    await UpdateMultipleObjectsInCache(keys, productItems);

    // Get the objects from cache
    await GetMultipleObjectsFromCache(keys);

    // Delete the existing object
    await RemoveMultipleObjectsFromCache(keys);

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

async function AddMultipleObjectsToCache(items) {
 
    const result = await cache.addBulk(items);
    if (result.size === 0) {
        console.log("All items are successfully added to cache.");
    } else {
        console.log("One or more items could not be added to the cache. Iterate map for details.");
        for (let value of result.values()) {
            console.log(value)
          }
    }
}

async function GetMultipleObjectsFromCache(keys) {
    const items = await cache.getBulk(keys,JsonDataType.Object);
    if (items.size > 0)
    {
         // Print output on console
         console.log("Following items are fetched from cache.");
         for(let item of items.values()) {
            PrintProductDetails(item);
         }
    }
}
//change
async function UpdateMultipleObjectsInCache(keys, items) {
     // Update item
     const oldItemValue = items.get(keys[0]).getValue(JsonDataType.Object);
     oldItemValue.ClassName = "ClassC";
     items.get(keys[0]).setValue(oldItemValue,"product");

     const results = await cache.insertBulk(items);
     if (results.size === 0) {
        console.log("nAll items are successfully updated in cache.")
     } else {
        console.log("One or more items could not be added to the cache. Iterate hashmap for details.")
        for(let result of results.values()) {
            console.log(result);
         }
     }
}

async function RemoveMultipleObjectsFromCache(key) {
    // Remove the existing customer
    const items = await cache.removeBulk(key,JsonDataType.Object);
    if (items.size === 0) {
        console.log("nNo items removed from the cache against the provided keys.")
    } else {
        console.log("Following items have been removed from the cache:")
        for(let item of items.values()) {
            PrintProductDetails(item);
         }
    }
}

function GetKeys(products) {
    return products.map(product =>`Product:${product.Name}`);
}
function GetCacheItemMap(keys,products) {
    const items = new Map();
    for (i = 0 ; i < keys.length ; i++) {
        const cacheItem = new CacheItem(products[i],"product")
        items.set(keys[i],cacheItem);
    }
    return items;
}
function CreateNewProducts() {
    return([
        {Id : 1, Name : "Dairy Milk Cheese", ClassName : "ClassA", Category : "Edibles"},
        {Id : 2, Name : "American Butter", ClassName : "ClassA", Category : "Edibles"},
        {Id : 3, Name : "Walmart Delicious Cream", ClassName : "ClassA", Category : "Edibles"},
        {Id : 4, Name : "Nestle Yogurt", ClassName : "ClassA", Category : "Edibles"},
    ]);
}
function PrintProductDetails(product) {
    if (product == null) return;
    console.log("Id:       " + product.Id);
    console.log("Name:     " + product.Name);
    console.log("Class:    " + product.ClassName);
    console.log("Category: " + product.Category);
    console.log();

}
module.exports = {
    Run
}
