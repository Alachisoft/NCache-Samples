package com.Alachisoft.NCache.Samples;

import com.Alachisoft.NCache.Samples.BulkCustomDependencyImpl.ProductDependency;
import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.dependencies.BulkExtensibleDependency;
import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.samples.data.Product;

import java.io.*;
import java.sql.*;
import java.util.*;

/**
 * Class that provides the functionality of the sample
 */
public class BulkCustomDependencyUsage {
    private static Cache _cache;
    private static String _connString;
    private static String _userId;
    private static String _password;
    private static Properties _properties;

    private static Statement _st;
    private static Connection _conn;

    /**
     * Executing this method will perform all the operations of the sample
     */
    public static void run() throws Exception {
        // Initialize connection
        _properties = getProperties();
        // Initialize cache
        String cacheName= _properties.getProperty("CacheID");
        //  initialize cache before any operations
        initializeCache(cacheName);

        // Initialize database connection
        initializeDatabaseConnection();

        // Add data to cache with custom dependency
        String[] keys = addDataIntoCache();

        // Update unit price of some products and wait for clean interval
        updateProducts();

        // Get objects from cache
        getObjectsFromCache(keys);

        // Undo the update
        undoUpdateOnProducts();

        // Dispose the cache once done
        _cache.close();
    }

    /**
     * Undo the update on products
     */
    private static void undoUpdateOnProducts() throws SQLException, InterruptedException {
        String query = "update SCOTT.PRODUCTS set UNITSINSTOCK = 30 where PRODUCTID = ?";
        List<Integer> productIds = new ArrayList<>();
        productIds.add(14);
        productIds.add(15);
        productIds.add(16);
        productIds.add(17);
        productIds.add(18);
        productIds.add(19);
        productIds.add(20);

        for(int id : productIds) {
            PreparedStatement ps = _conn.prepareStatement(query);
            ps.setInt(1, id);
            ps.executeUpdate();
        }

        System.out.println("Items updated to database.");
        Thread.sleep(20000);
    }

    /**
     * This method gets objects from the cache
     * @param keys String keys to get objects from cache
     */
    private static void getObjectsFromCache(String[] keys) throws CacheException {
        for (String key : keys) {
            Product cachedProduct = _cache.get(key, Product.class);
            if (cachedProduct == null) {
                System.out.println("Removed item with id : " + key);
            }
        }
    }

    /**
     *  Update unit available in stock of some products
     */
    private static void updateProducts() throws SQLException, InterruptedException {
        String query = "update SCOTT.PRODUCTS set UNITSINSTOCK = 0 where PRODUCTID = ?";
        List<Integer> productIds = new ArrayList<>();
        productIds.add(14);
        productIds.add(15);
        productIds.add(16);
        productIds.add(17);
        productIds.add(18);
        productIds.add(19);
        productIds.add(20);

        for(int id : productIds) {
            PreparedStatement ps = _conn.prepareStatement(query);
            ps.setInt(1, id);
            ps.executeUpdate();
        }

        System.out.println("Items updated to database.");
        Thread.sleep(20000);
    }

    /**
     * This method Adds objects to cache
     * @return List of keys added to cache
     * @throws CacheException
     * @throws SQLException
     */
    private static String[] addDataIntoCache() throws CacheException, SQLException {
        List<Product> products = loadProductsFromDatabase();
        CacheItem[] items = new CacheItem[products.size()];
        String[] keys = new String[products.size()];

        for(int i=0; i<products.size(); i++)
        {
            keys[i] = String.valueOf(products.get(i).getId());
            CacheItem item = new CacheItem(products.get(i));
            BulkExtensibleDependency bulkExtensibleDependency = new ProductDependency(
                    _connString, _userId, _password, products.get(i).getId(), products.get(i).getUnitsAvailable()
            );

            item.setDependency(bulkExtensibleDependency);
            items[i] = item;
        }

        Map<String, CacheItem> cacheItems = getCacheItemMap(keys, items);
        _cache.insertBulk(cacheItems);

        System.out.println("Items are added to cache");

        return keys;
    }

    /**
     * Generates hashmap of Cache Items and keys
     * @param keys  List of keys to generate hashmap
     * @param items List of cache items to generate hashmap
     * @return returns Dictionary of Cache Items and keys
     */
    private static Map<String, CacheItem> getCacheItemMap(String[] keys, CacheItem[] items) {
        Map<String, CacheItem> cacheItems = new HashMap<>();

        for(int i=0; i<items.length; i++)
        {
            cacheItems.put(keys[i], items[i]);
        }
        return cacheItems;
    }

    /**
     * Loads objects from the datasource.
     * @return List of orders
     * @throws SQLException
     */
    private static List<Product> loadProductsFromDatabase() throws SQLException {
        String query = "select PRODUCTID, PRODUCTNAME, UNITSINSTOCK from SCOTT.PRODUCTS";
        ResultSet rs = _st.executeQuery(query);

        List<Product> products = new ArrayList<>();

        while (rs.next()){
            Product product = new Product();

            product.setId(rs.getInt("PRODUCTID"));
            product.setName(rs.getString("PRODUCTNAME"));
            product.setUnitsAvailable(rs.getInt("UNITSINSTOCK"));

            products.add(product);
        }
        rs.close();
        return products;
    }

    /**
     * This method initializes the database connection
     */
    private static void initializeDatabaseConnection() {
        try {
            _userId = _properties.getProperty("user");
            _password = _properties.getProperty("pass");
            _connString = _properties.getProperty("connString");
            _conn = DriverManager.getConnection(_connString, _userId, _password);
            _st = _conn.createStatement();
        } catch (SQLException throwable) {
            throwable.printStackTrace();
        }
    }

    /**
     * Class that provides the functionality of the sample
     */
    private static void initializeCache(String cacheName) throws Exception {
        if(cacheName == null){
            System.out.println("The CacheID cannot be null.");
            return;
        }

        if(cacheName.isEmpty()){
            System.out.println("The CacheID cannot be empty.");
            return;
        }

        // Initialize an instance of the cache to begin performing operations:
        _cache = CacheManager.getCache(cacheName);

        // Print output on console
        System.out.println();
        System.out.println("Cache initialized succesfully.");
    }

    /**
     This method returns property file
     */
    private static Properties getProperties() throws IOException
    {
        String path = "config.properties";
        InputStream inputStream = BulkCustomDependencyUsage.class.getClassLoader().getResourceAsStream(path);
        Properties properties=new Properties();
        if (inputStream != null) {
            properties.load(inputStream);
        } else {
            throw new FileNotFoundException("property file '" + path + "' not found in the classpath");
        }
        return properties;
    }

}
