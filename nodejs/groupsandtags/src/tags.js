const {CacheManager,CacheItem,Tag,TagSearchOptions} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
async function Run() {
    // Adding items with named Tags       
    // These tags are used to identify products who are ISOCertified, QCPassed and fall in ClassA
    const tagList = [];
    tagList.push(new Tag("ISOCertified"));
    tagList.push(new Tag("QCPassed"));
    tagList.push(new Tag("ClassA"));

    // Initialize cache 
    await InitializeCache();
    // Add Items in cache
    await AddItems(tagList);

    // Get Items using tags
    await GetItemsWithTag();


    // Get keys by tags
    // Here keys can be retrived from the cache via following three methods
    // Get keys by a specific tag
    await GetKeysByTag(tagList[0].getTagName());

    // Get keys where all the tags match
    await GetKeysByMultipleTags(tagList);

    // Get keys where any item in tagList matches
    await GetKeysByAnyTag(tagList);


    // Get Data by tags
    // Here values can be retrived from the cache via following three methods
    // Get values by specified tag
    await GetValuesByTag(tagList[0].getTagName());

    // Get those values only where complete tagList matches
    await GetValuesByMultipleTags(tagList);

    // Retrivies values where any item in tagList matches
    await GetValuesByAnyTag(tagList);

    // Remove items from Cache by tags
    // Removes values by specified tag
    await RemoveItemsByTag(tagList[0]);

    // Removes those values only where complete tagList matches
    await RemoveItemsByMultipleTags(tagList);

    // Removes values where any item in tagList matches
    await RemoveItemsByAnyTag(tagList);

    //close cache;
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

    await cache.clear();
    // Print output on console
    console.log(`Cache ${cacheName} is initialized.`);
            
}
async function AddItems(tagList) {
    // 4 items added to cache
    await Promise.all([
        AddTagDataToCache(10, "ProductID: XYZ", "Z", "Z" , tagList),
        AddTagDataToCache(11, "ProductID: ABC", "Z", "Z", tagList),
        AddTagDataToCache(12, "ProductID: 123", "Z", "Z", tagList),
        AddTagDataToCache(13, "ProductID: 456", "Z", "Z", tagList),
    ]);
    console.log("Items added in cache");
}
async function AddTagDataToCache(productId, productName, className , category, tagList) {
    const product = { Id : productId, Name : productName, ClassName : className, Category : category };

    // create object key
    const key = "product:" + product.Id;

    // create CacheItem with your desired object
    const  item = new CacheItem(product,"Product");

    // assign group to CacheItem object
    item.setTags(tagList);

    // add CacheItem object to cache
    await cache.add(key, item);
}
async function GetItemsWithTag() {
    //Retrieve items who are QCPassed
    const searchService = await cache.getSearchService();
    const data = await searchService.getByTag("QCPassed");
    PrintData(data);
}
function PrintData(data) {
    if (data.size > 0) {
        for (const value of data.values()) {
            console.log(value.toStringSync());
        }
    }
}
function PrintKeys(keys) {
    if (keys.length > 0) {
        keys.forEach(key => {
            console.log(key);
        });
    }
}
async function GetKeysByTag(tag) {
    const searchService = await cache.getSearchService();
    const data = await searchService.getKeysByTag(tag);
    console.log("Following keys by Tag");
    PrintKeys(data);
}
async function GetKeysByMultipleTags(tagList) {
    const searchService = await cache.getSearchService();
    const data = await searchService.getKeysByTags(tagList,TagSearchOptions.ByAllTags);
    console.log("Following keys by Multiple Tags");
    PrintKeys(data);
}
async function GetKeysByAnyTag(tagList) {
    const searchService = await cache.getSearchService();
    const data = await searchService.getKeysByTags(tagList,TagSearchOptions.ByAnyTag);
    console.log("Following keys by Any Tag");
    PrintKeys(data);
}
async function GetValuesByTag(tag) {
    const searchService = await cache.getSearchService();
    const data = await searchService.getByTag(tag);
    PrintData(data);
}
async function GetValuesByMultipleTags(tagList) {
    const searchService = await cache.getSearchService();
    const data = await searchService.getByTags(tagList,TagSearchOptions.ByAllTags);
    PrintData(data);
}
async function GetValuesByAnyTag(tagList) {
    const searchService = await cache.getSearchService();
    const data = await searchService.getByTags(tagList,TagSearchOptions.ByAnyTag);
    PrintData(data);
}
async function  RemoveItemsByTag(tag) {
    const searchService = await cache.getSearchService();
    await searchService.removeByTag(tag);
}
async function RemoveItemsByMultipleTags(tagList) {
    const searchService = await cache.getSearchService();
    await searchService.removeByTags(tagList,TagSearchOptions.ByAnyTag);
}
async function RemoveItemsByAnyTag(tagList) {
    const searchService = await cache.getSearchService();
    await searchService.removeByTags(tagList,TagSearchOptions.ByAnyTag);
}module.exports = {
    Run
}