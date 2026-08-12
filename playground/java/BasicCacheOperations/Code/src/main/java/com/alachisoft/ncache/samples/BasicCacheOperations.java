package com.alachisoft.ncache.samples;

import java.io.Serializable;
import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.runtime.caching.expiration.*;
import com.alachisoft.ncache.runtime.util.TimeSpan;

// Basic caching operations (CRUD) as an intro to NCache API

public class BasicCacheOperations {

    public static void main(String[] args) throws Exception {

        System.out.println("Sample showing basic caching operations (CRUD) in NCache, along with expiration.\n");

        String cacheName = args[0];
        System.out.println("Connecting to cache: " + cacheName);

        CacheConnectionOptions connectionOptions = new CacheConnectionOptions();
        connectionOptions.setIsolationLevel(IsolationLevel.OutProc);

        // Connect to the cache and return a cache handle
        Cache cache = CacheManager.getCache(cacheName, connectionOptions);
        System.out.println("Connected to Cache successfully: " + cacheName + "\n");

        // Add product 10001 to the cache with a 5-minutes expiration
        Product prod1 = new Product(10001, "Laptop", 1000, "Electronics");

        String prod1Key = String.valueOf(prod1.getProductID());
        CacheItem productCacheItem = new CacheItem(prod1);
        productCacheItem.setExpiration(new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(5)));

        // Add to the cache with 5min absolute expiration (TTL). Item expires automatically
        cache.add(prod1Key, productCacheItem);

        System.out.println("Product 10001 added successfully (5min expiration):");
        printProductDetails(prod1);

        // Get from the cache. If not found, a null is returned
        Product retrievedprod1 = cache.get(prod1Key, Product.class);
        if (retrievedprod1 != null) {
            System.out.println("Product 10001 retrieved successfully:");
            printProductDetails(retrievedprod1);
        }

        retrievedprod1.setPrice(3000);

        // Update product in the cache. If not found, it is added
        CacheItem updatedProductCacheItem = new CacheItem(retrievedprod1);
        updatedProductCacheItem.setExpiration(new Expiration(ExpirationType.Absolute, TimeSpan.FromMinutes(5)));
        cache.insert(prod1Key, updatedProductCacheItem);

        System.out.println("Price for Product 10001 updated successfully:");
        printProductDetails(retrievedprod1);

        // Remove the item from the cache
        cache.delete(prod1Key);
        System.out.println("Deleted the product " + prod1Key + " from the cache...\n");

        System.out.println("Sample completed successfully.");
        cache.close();
    }

    private static void printProductDetails(Product product) {
        System.out.println("ProductID, Name, Price, Category");
        System.out.println("- " + product.getProductID() + ", " + product.getName() + ", " + product.getPrice() + ", " + product.getCategory() + "\n");
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
    public double getPrice() { return price; }
    public void setPrice(int price) { this.price = price; }
    public String getCategory() { return category; }
}