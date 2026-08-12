package com.alachisoft.ncache.samples;

import java.math.BigDecimal;
import java.io.Serializable;
import java.util.*;
import java.util.stream.Collectors;

import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.runtime.caching.Tag;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.runtime.exceptions.OperationFailedException;
import com.alachisoft.ncache.client.QueryIndexable;

// Use SQL to query for cached data in NCache based on object attributes and Tags instead of just keys.
// NCache SQL query requires indexes to be created on all objects being searched. One way is
// thru cache configuration. Second is through code using "Custom Attributes" to annotate object fields
// and create index on them automatically when they're added to the cache.
// In this sample, we're using "Custom Attributes" to create index on Product object.

public class SqlQueryCacheLookup {

    public static void main(String[] args) throws Exception {

        System.out.println("Sample showing SQL queries to find data in NCache based on object attributes instead of keys.\n");

        String cacheName = args[0];

        System.out.println("Connecting to cache: " + cacheName);
        // Connect to the cache and return a cache handle
        Cache cache = CacheManager.getCache(cacheName);
        System.out.println("Connected to Cache successfully: " + cacheName + "\n");

        // Adds all the products to the cache. This automatically creates indexes on various
        // attributes of Product object by using "Custom Attributes".
        addSampleData(cache);

        System.out.println("Find products with Category IN (\"Electronics\", \"Stationery\") AND Price < $2000");

        // $VALUE$ keyword means the entire object instead of individual attributes that are also possible
        String sql = "SELECT $VALUE$ FROM com.alachisoft.ncache.samples.Product WHERE category IN (?, ?) AND price < ?";

        QueryCommand sqlCommand = new QueryCommand(sql);

        List<String> catParamList = new ArrayList<>(Arrays.asList(("Electronics"), ("Stationery")));
        sqlCommand.getParameters().put("category", catParamList);
        sqlCommand.getParameters().put("price", 2000);

        // ExecuteReader returns ICacheReader with the query resultset
        CacheReader resultSet = cache.getSearchService().executeReader(sqlCommand);
        List<Product> fetchedProducts = new ArrayList<>();
        if (resultSet.getFieldCount() > 0) {
            while (resultSet.read()) {
                // getValue() with $VALUE$ keyword returns the entire object instead of just one column
                fetchedProducts.add(resultSet.getValue("$VALUE$", Product.class));
            }
        }
        printProducts(fetchedProducts);

        System.out.println("Find all \"phone\" products (using LIKE operator) AND (Tag = \"Technology\" OR Tag = \"Gadgets\")");
        sql = "SELECT $VALUE$ FROM com.alachisoft.ncache.samples.Product WHERE name LIKE ? AND ($Tag$ = ? OR $Tag$ = ?)";
        
		List<String> tagList = new ArrayList<>(Arrays.asList(("Technology"), ("Gadgets")));	
		
        sqlCommand = new QueryCommand(sql);
        sqlCommand.getParameters().put("name", "*Phone*");
        sqlCommand.getParameters().put("$Tag$", tagList);

        resultSet = cache.getSearchService().executeReader(sqlCommand);
        fetchedProducts.clear();
        if (resultSet.getFieldCount() > 0) {
            while (resultSet.read()) {
                // getValue() with $VALUE$ keyword returns the entire object instead of just one column
                fetchedProducts.add(resultSet.getValue("$VALUE$", Product.class));
            }
        }
        printProducts(fetchedProducts);

        System.out.println("GROUP BY query: Get a count of product by category with Price < $3000");
        sql = "SELECT category, COUNT(*) FROM com.alachisoft.ncache.samples.Product WHERE price < ? GROUP BY category";

        sqlCommand = new QueryCommand(sql);

        sqlCommand.getParameters().put("price", 3000);

        CacheReader groupByResultSet = cache.getSearchService().executeReader(sqlCommand);
        System.out.println("Category, Count");
        if (groupByResultSet.getFieldCount() > 0) {
            while (groupByResultSet.read()) {
                String category = groupByResultSet.getValue(0, String.class);
                BigDecimal count = groupByResultSet.getValue(1, BigDecimal.class);
                System.out.println("- " + category + ", " + count);
            }
        }

        System.out.println("\nDelete Items by Tags \"Technology\" and \"Reading\"");
        sql = "DELETE FROM com.alachisoft.ncache.samples.Product WHERE $Tag$ IN (?, ?)";
        sqlCommand = new QueryCommand(sql);

        List<String> removeTagsList = new ArrayList<>(Arrays.asList(("Technology"), ("Reading")));
        sqlCommand.getParameters().put("$Tag$", removeTagsList);

        int itemsRemoved = cache.getSearchService().executeNonQuery(sqlCommand);
        System.out.println(itemsRemoved + " items removed from the cache.\n");

        System.out.println("Sample completed successfully.");
        cache.close();
    }

    static void addSampleData(Cache cache) throws CacheException {
        System.out.println("Adding products to the cache with tags...");

        Product laptop = new Product(13001, "Laptop", 2000, "Electronics");
        Product kindle = new Product(13002, "Kindle", 1000, "Electronics");
        Product book = new Product(13003, "Book", 100, "Stationery");
        Product smartPhone = new Product(13004, "Smart Phone", 1000, "Electronics");
        Product mobilePhone = new Product(13005, "Mobile Phone", 200, "Electronics");

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
        products.put("13001", laptopCacheItem);
        products.put("13002", kindleCacheItem);
        products.put("13003", bookCacheItem);
        products.put("13004", smartPhoneCacheItem);
        products.put("13005", mobilePhoneCacheItem);

        // Adds all the products to the cache. This automatically creates indexes on various
        // attributes of Product object by using "Custom Attributes".
        for (Map.Entry<String,CacheItem> product : products.entrySet()) {
            cache.insert(product.getKey(), product.getValue());
        }

        List<Product> productValues = products.values().stream()
                .map(cacheItem -> {
                    try {
                        return (Product) cacheItem.getValue(Product.class);
                    } catch (OperationFailedException e) {
                        throw new RuntimeException(e);
                    }
                })
                .collect(Collectors.toList());
        printProducts(productValues);
    }

    static void printProducts(List<Product> products) {
        System.out.println("ProductID, Name, Price, Category");
        for (Product product : products) {
            System.out.println("- " + product.getProductID() + ", " + product.getName() + ", $" + product.getPrice() + ", " + product.getCategory());
        }
        System.out.println();
    }
}

// Sample class for Product
@QueryIndexable
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