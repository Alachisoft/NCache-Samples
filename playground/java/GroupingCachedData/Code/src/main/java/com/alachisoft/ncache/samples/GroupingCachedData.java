package com.alachisoft.ncache.samples;

import java.io.Serializable;
import java.util.*;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.TagSearchOptions;
import com.alachisoft.ncache.runtime.caching.Tag;

// Use the Tags feature of NCache to group items. First, use Tags while adding to the cache. Later, fetch by Tags

public class GroupingCachedData {

    public static void main(String[] args) throws Exception {

        System.out.println("Sample showing the Tags feature of NCache to logically group related data to easily fetch later.\n");

        // Connect to the cache
        String cacheName = args[0];

        System.out.println("Connecting to cache: " + cacheName);
        // Connect to the cache and return a cache handle
        Cache cache = CacheManager.getCache(cacheName);
        System.out.println("Connected to Cache successfully: " + cacheName + "\n");

        Product laptop = new Product(12001, "Laptop", 2000, "Electronics");
        Product kindle = new Product(12002, "Kindle", 1000, "Electronics");
        Product book = new Product(12003, "Book", 100, "Stationery");
        Product smartPhone = new Product(12004, "Smart Phone", 1000, "Electronics");
        Product mobilePhone = new Product(12005, "Mobile Phone", 200, "Electronics");

        CacheItem laptopCacheItem = new CacheItem(laptop);
        List<Tag> laptopTags = new ArrayList<>(Arrays.asList(new Tag("Technology"), new Tag("Gadgets")));
        laptopCacheItem.setTags(laptopTags);

        CacheItem kindleCacheItem = new CacheItem(kindle);
        List<Tag> kindleTags = new ArrayList<>(Arrays.asList(new Tag("Technology"), new Tag("Reading")));
        kindleCacheItem.setTags(kindleTags);

        CacheItem bookCacheItem = new CacheItem(book);
        List<Tag> bookTags = new ArrayList<>(Arrays.asList(new Tag("Education"), new Tag("Reading")));
        bookCacheItem.setTags(bookTags);

        CacheItem smartPhoneCacheItem = new CacheItem(smartPhone);
        List<Tag> smartPhoneTags = new ArrayList<>(Arrays.asList(new Tag("Technology"), new Tag("Communication")));
        smartPhoneCacheItem.setTags(smartPhoneTags);
        
        CacheItem mobilePhoneCacheItem = new CacheItem(mobilePhone);
        List<Tag> mobilePhoneTags = new ArrayList<>(Arrays.asList(new Tag("Technology"), new Tag("Communication")));
        mobilePhoneCacheItem.setTags(mobilePhoneTags);
        
        Map<String, CacheItem> products = new HashMap<>();
        products.put("12001", laptopCacheItem);
        products.put("12002", kindleCacheItem);
        products.put("12003", bookCacheItem);
        products.put("12004", smartPhoneCacheItem);
        products.put("12005", mobilePhoneCacheItem);

        // Add all the products to the cache.
        for (Map.Entry<String,CacheItem> product : products.entrySet()) {
            cache.insert(product.getKey(), product.getValue());
        }

        Map<String, Product> technologyItems = cache.getSearchService().getByTag(new Tag("Technology"));
        System.out.println("Got Items from the cache by Tag \"Technology\"");
        printProductDetails(technologyItems.values());

        // Get all items from the cache with this Tag. readingItems is a list of key-value pairs
        Map<String, Product> readingItems = cache.getSearchService().getByTag(new Tag("Reading"));
        System.out.println("Got the Items from the cache by Tag \"Reading\"");
        printProductDetails(readingItems.values());

        List<Tag> tags = new ArrayList<>(Arrays.asList(new Tag("Technology"), new Tag("Reading")));

        // Remove all items from the cache with any of the tags
        cache.getSearchService().removeByTags(tags, TagSearchOptions.ByAnyTag);
        System.out.println("Removed Items by Tags \"Technology\" and \"Reading\"\n");

        System.out.println("Sample completed successfully.");
        cache.close();
    }

    private static void printProductDetails(Collection<Product> items) {
        if (items != null && !items.isEmpty()) {
            System.out.println("ProductID, Name, Price, Category");
            for (Product product : items) {
                System.out.println("- " + product.getProductID() + ", " + product.getName() + ", $" + product.getPrice() + ", " + product.getCategory());
            }
        } else {
            System.out.println("No items found...");
        }
        System.out.println();
    }
}

// Sample class for Product
class Product implements Serializable {
    private int productID;
    private String name;
    private int price;
    private String category;

    public Product(int productID, String name, int price, String category) {
        this.productID = productID;
        this.name = name;
        this.price = price;
        this.category = category;
    }

    public int getProductID() { return productID; }
    public String getName() { return name; }
    public int getPrice() { return price; }
    public String getCategory() { return category; }
}
