const {CacheManager,CacheItem,NamedTagsDictionary,QueryCommand,JsonDataType} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
async function Run() { 
    // Initialize cache 
    await InitializeCache();

    //clear cache
    await cache.clear();
    // Create a simple products list
    const products = CreateNewProducts();

    // Insert Items in cache with Named Tags
    await InsertItemsWithNamedTags(products);

    // Querying a simple cache item via named Tags
    await QueryItemsUsingNamedTags();

    // Query Items with defined index
    await QueryItemsUsingDefinedIndex();

    // Query Items with both named tags and predefined index
    await QueryItemsUsingProjection();
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
async function InsertItemsWithNamedTags(products) {
    for (let index = 0; index < products.length; index++) {
        const cacheItem = new CacheItem(products[index],"Alachisoft.NCache.Sample.Data.Product");
        const namedTags = await GetNamedTagsDictionary(products[index]);
        cacheItem.setNamedTags(namedTags);
        await cache.add(`Product:${products[index].Id}`,cacheItem);
    }
}
async function GetNamedTagsDictionary(product) {
    let namedTagsDictionary = new NamedTagsDictionary();
    switch(product.Name) {
        case "Dairy Milk Cheese":
            await namedTagsDictionary.add("Supplier","Fresh Farms");
            break;
        case "Nestle Yogurt":
            await namedTagsDictionary.add("Supplier","Walmart");
            break;
        case "American Butter":
            await namedTagsDictionary.add("Supplier","Fresh Farms");
            break;
        case "Walmart Delicious Cream":
            await namedTagsDictionary.add("Supplier","Walmart");
            break;
    }
    return namedTagsDictionary;
}
async function QueryItemsUsingNamedTags() {
    const query = "SELECT $Value$ FROM Alachisoft.NCache.Sample.Data.Product WHERE Supplier = ?";
    const queryCommand = new QueryCommand(query);
    let parametersMap = new Map();
    parametersMap.set("Supplier","Walmart");
    queryCommand.setParameters( parametersMap);
    const searchService = await cache.getSearchService();
    const cacheReader = await searchService.executeReader(queryCommand);
    let counter = 0 ;
    if (cacheReader.getFieldCount() > 0) {
        while (await cacheReader.read()) {
            const product = cacheReader.getValue(1,JsonDataType.Object);
            PrintProductDetails(product);
            counter++;
        }
    }
}
async function QueryItemsUsingDefinedIndex() {
    const query = "SELECT $Value$ FROM Alachisoft.NCache.Sample.Data.Product WHERE Category = ?";
    const queryCommand = new QueryCommand(query);
    let parametersMap = new Map();
    parametersMap.set("Category","Edibles");
    queryCommand.setParameters( parametersMap);
    const searchService = await cache.getSearchService();
    const cacheReader = await searchService.executeReader(queryCommand);
    let counter = 0 ;
    if (cacheReader.getFieldCount() > 0) {
        while (await cacheReader.read()) {
            const product = cacheReader.getValue(1,JsonDataType.Object);
            PrintProductDetails(product);
            counter++;
        }
    }
}
async function QueryItemsUsingProjection() {
    const query = "SELECT Name, Supplier FROM Alachisoft.NCache.Sample.Data.Product WHERE Category = ?";
    const queryCommand = new QueryCommand(query);
    let parametersMap = new Map();
    parametersMap.set("Category","Edibles");
    queryCommand.setParameters( parametersMap);
    const searchService = await cache.getSearchService();
    const cacheReader = await searchService.executeReader(queryCommand);
    let counter = 0 ;
    if (cacheReader.getFieldCount() > 0) {
        while (await cacheReader.read()) {
            const name = cacheReader.getString(1);
            const supplier = cacheReader.getString(2);
            console.log("Name:     " + name);
            console.log("Supplier:     " + supplier);
            counter++;
        }
    }
}
function CreateNewProducts() {
    return([
        {Id : 1, Name : "Dairy Milk Cheese", ClassName : "ClassA", Category : "Edibles",UnitPrice:12},
        {Id : 2, Name : "American Butter", ClassName : "ClassA", Category : "Edibles",UnitPrice:8},
        {Id : 3, Name : "Walmart Delicious Cream", ClassName : "ClassA", Category : "Edibles",UnitPrice:3},
        {Id : 4, Name : "Nestle Yogurt", ClassName : "ClassA", Category : "Edibles",UnitPrice:1},
    ]);
}
function PrintProductDetails(product) {
    if (product == null) return;
    console.log("Id:       " + product.Id);
    console.log("Name:     " + product.Name);
    console.log("Class:    " + product.ClassName);
    console.log("Category: " + product.Category);
    console.log("Unit price",product.UnitPrice)
    console.log();

}
module.exports = {
    Run
}