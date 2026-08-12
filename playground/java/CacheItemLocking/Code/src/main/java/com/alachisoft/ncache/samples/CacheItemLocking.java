package com.alachisoft.ncache.samples;

import java.io.Serializable;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.client.LockHandle;
import com.alachisoft.ncache.runtime.caching.WriteThruOptions;
import com.alachisoft.ncache.runtime.util.TimeSpan;

// Locking an item prevents other users from accessing it while you update it.
// You can lock with cache.GetCacheItem() or cache.Lock() and unlock with cache.Insert() or cache.Unlock()
// NOTE: Locking API is for application level locking. NCache already has its own internal locking
// for concurrency and thread-safe operations.

public class CacheItemLocking {

    public static void main(String[] args) throws Exception {

        System.out.println("Sample showing item level lock and unlock in NCache.\n");
        String cacheName = args[0];

        System.out.println("Connecting to cache: " + cacheName);
        // Connect to the cache and return a cache handle
        Cache cache = CacheManager.getCache(cacheName);
        System.out.println("Connected to Cache successfully: " + cacheName + "\n");

        Product prod1 = new Product(11001, "Laptop", 1000, "Electronics");
        
        String prod1Key = String.valueOf(prod1.getProductID());
        System.out.println("Adding product 11001 to the cache...");

        // Add a product to the cache. If it already exists, the operation will fail
        cache.insert(prod1Key, prod1);
        System.out.println("Product added successfully:");
        printProductDetails(prod1);

        // Get product 11001 and lock it in the cache
        System.out.println("Get and lock product 11001 in the cache...");
        LockHandle lockHandle = new LockHandle();

        // Lock the item and set a lock timeout of 10 seconds
        CacheItem lockedCacheItem = cache.getCacheItem(prod1Key, true, TimeSpan.FromSeconds(10), lockHandle);
        if (lockedCacheItem != null) {
            System.out.println("Product 11001 retrieved and locked successfully:");
            Product retrievedProduct = (Product) lockedCacheItem.getValue(Product.class);
            printProductDetails(retrievedProduct);

            // Change the price of product 11001
            retrievedProduct.setPrice(2000);

            // Update the item and release the lock
            cache.insert(prod1Key, lockedCacheItem, new WriteThruOptions(), lockHandle, true);
            System.out.println("Updated price of product 11001 and released the lock simultaneously in the cache...");

            printProductDetails(retrievedProduct);
        }
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
    public double getPrice() { return price;}
    public void setPrice(int price) { this.price = price;}
    public String getCategory() { return category;}
	
}
