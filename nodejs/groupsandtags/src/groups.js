const {CacheManager,CacheItem} = require('ncache-client');
let cache = null;
const config = require('./app.config.json');
async function Run() {
    // Initialize cache 
    await InitializeCache();
    //Adding item in same group        
    await AddItems();

    // Getting group data
    // Will return nine items
    await GetItemsByGroup();

    // getGroupKeys is yet another function to retrive group data.
    // It however requires multiple iterations to retrive actual data
    // 1) To get List of Keys and 2) TO get items for the return List of Keys

    // Updating items in groups
    // Item is updated at the specified group
    await UpdateItem();

    // Remove group data
    await RemoveGroupData();

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

    //clear cache data
    await cache.clear();

    // Print output on console
    console.log(`Cache ${cacheName} is initialized.`);
            
}

async function AddItems() {
    await Promise.all([
        //Mobile items
        AddGroupedDatatoCache(1, "PhoneSamsung", "Mobiles"),
        AddGroupedDatatoCache(2, "HTCPhone", "Mobiles"),
        AddGroupedDatatoCache(3, "NokiaPhone", "Mobiles"),

            // Laptop Items

        AddGroupedDatatoCache(4, "AcerLaptop", "Laptops"),
        AddGroupedDatatoCache(5, "HPLaptop", "Laptops"),
        AddGroupedDatatoCache(6, "DellLaptop", "Laptops"),

        // small electronics items

        AddGroupedDatatoCache(7, "HairDryer", "SmallElectronics"),
        AddGroupedDatatoCache(8, "VaccumCleaner", "SmallElectronics"),
        AddGroupedDatatoCache(9, "Iron", "SmallElectronics")
    ]);

    console.log("items added in cache");
}
async function AddGroupedDatatoCache(productId, productName, group, className = "Elec", category = "Elec")
{
    const product = { Id : productId, Name : productName, ClassName : className, Category : category };

    // create object key
    const key = "product:" + product.Id;

    // create CacheItem with your desired object
    const  item = new CacheItem(product,"Product");

    // assign group to CacheItem object
    item.setGroup(group);

    // add CacheItem object to cache
    await cache.add(key, item);
}
async function GetItemsByGroup() {
    const searchService = await cache.getSearchService();
    const items = await searchService.getGroupData("Mobiles");
    if (items.size > 0) {
        console.log("Item count: " + items.size);
        console.log("Following Products are found in group 'Mobiles'");
        for (const item of items.values()) {
            console.log(item.toStringSync());
        }
    }
}

async function UpdateItem() {
    const product = { Id : 8, Name : "PanaSonicIron"};
    const panasonicIron = new CacheItem(product,"Product");
    panasonicIron.setGroup("SmallElectronics");
    const key = "product:" + product.Id;
    await cache.insert(key, panasonicIron);
}

async function RemoveGroupData() {
    let count = await cache.getCount()
    console.log("Item count: " + count); 

    const searchService = await cache.getSearchService();
    await searchService.removeGroupData("Laptops");

    count = await cache.getCount()
    console.log("Item count: " + count); 
}
module.exports = {
    Run
}