package com.alachisoft.ncache.samples.cacheLoaderImpl;

import com.alachisoft.ncache.client.Cache;
import com.alachisoft.ncache.client.CacheItem;
import com.alachisoft.ncache.client.CacheManager;
import com.alachisoft.ncache.runtime.cacheloader.CacheLoader;
import com.alachisoft.ncache.runtime.cacheloader.RefreshPreference;
import com.alachisoft.ncache.samples.data.Product;
import com.alachisoft.ncache.samples.data.Suppliers;

import java.io.InvalidObjectException;
import java.sql.*;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Refresher implements CacheLoader {
    private static Cache _cache;
    private static String _connectionString;
    private static String _user;
    private static String _password;
    @Override
    public void init(Map<String, String> parameters, String cacheName) throws Exception {
        if(parameters != null && !parameters.isEmpty())
        {
            _connectionString = parameters.getOrDefault("connString", "");
            _user = parameters.getOrDefault("user", "");
            _password = parameters.getOrDefault("password", "");
        }
        if(cacheName == null || cacheName.isEmpty())
            throw new IllegalArgumentException("Cache name cannot be null or empty.");

        _cache = CacheManager.getCache(cacheName);
    }

    @Override
    public Object loadDatasetOnStartup(String dataset) throws Exception {
        List<Object> loadDatasetAtStartup;
        if(dataset==null||dataset.isEmpty())
            throw new InvalidObjectException("Invalid dataset");

        switch (dataset.toLowerCase())
        {
            case "products":
                loadDatasetAtStartup = FetchProductsFromDatabase();
                break;
            case "suppliers":
                loadDatasetAtStartup = FetchSuppliersFromDatabase();
                break;
            default:
                throw new IllegalArgumentException("Invalid dataset");
        }
        String[] keys = GetKeys(loadDatasetAtStartup);
        Map<String, CacheItem> cacheData = GetCacheItemMap(keys, loadDatasetAtStartup);
        _cache.insertBulk(cacheData);
        return LocalDate.now();
    }

    @Override
    public Object refreshDataset(String dataset, Object userContext) throws Exception {
        if(dataset==null||dataset.isEmpty())throw new IllegalArgumentException("Invalid Dataset.");

        LocalDate lastRefreshTime;
        switch (dataset.toLowerCase())
        {
            case "products":
                lastRefreshTime = (LocalDate) userContext;
                List<Object> productsNeedToRefresh = FetchUpdatedProducts(lastRefreshTime);
                for (Object prod :
                        productsNeedToRefresh) {
                    Product product = (Product) prod;
                    String key = "ProductID:"+product.getId();
                    CacheItem cacheItem = new CacheItem(product);
                    _cache.insert(key, cacheItem);
                }
                break;
            case "suppliers":
                lastRefreshTime = (LocalDate) userContext;
                List<Object> suppliersNeedToRefresh = FetchUpdatedSuppliers(lastRefreshTime);
                for (Object sup :
                        suppliersNeedToRefresh) {
                    Suppliers supplier = (Suppliers)sup;
                    String key = "SupplierID:" + supplier.getId();
                    CacheItem cacheItem = new CacheItem(supplier);
                    _cache.insert(key, cacheItem);
                }
                break;
            default:
                throw new IllegalArgumentException("Invalid Dataset.");
        }
        return LocalDate.now();
    }

    @Override
    public Map<String, RefreshPreference> getDatasetsToRefresh(Map<String, Object> map) throws SQLException {

        Map<String, RefreshPreference> datasetNeedToRefresh = new HashMap<>();
        LocalDate lastRefreshTime;
        boolean datasetHasUpdated;
        for (String key :
                map.keySet()) {
            switch (key.toLowerCase())
            {
                case "products":
                    lastRefreshTime = (LocalDate) map.get(key);
                    datasetHasUpdated = hasProductDatasetUpdated(key, lastRefreshTime);
                    if(datasetHasUpdated){
                        datasetNeedToRefresh.put(key, RefreshPreference.RefreshNow);
                    }
                    break;
                case "suppliers":
                    lastRefreshTime = (LocalDate) map.get(key);
                    datasetHasUpdated = hasSupplierDatasetUpdated(key, lastRefreshTime);
                    if(datasetHasUpdated){
                        datasetNeedToRefresh.put(key, RefreshPreference.RefreshOnNextTimeOfDay);
                    }
                    break;
                default:
                    throw new IllegalArgumentException("Invalid Dataset.");
            }
        }
        return datasetNeedToRefresh;
    }

    private Map<String, CacheItem> GetCacheItemMap(String[] keys, List<Object> values) {
        Map<String, CacheItem> items = new HashMap<>();
        CacheItem cacheItem;

        for(int i=0; i<values.size(); i++)
        {
            cacheItem = new CacheItem(values.get(i));
            items.put(keys[i], cacheItem);
        }
        return items;
    }

    private String[] GetKeys(List<Object> objects) {
        String[] keys = new String[objects.size()];
        for(int i=0; i<keys.length;i++)
        {
            if(objects.get(i) instanceof Product)
            {
                Product product = (Product)objects.get(i);
                keys[i] = "ProductID:" + product.getId();
            }
            else if (objects.get(i) instanceof Suppliers)
            {
                Suppliers suppliers = (Suppliers)objects.get(i);
                keys[i] = "SupplierID:" + suppliers.getId();
            }else keys[i] = "";
        }
        return keys;
    }

    private List<Object> FetchSuppliersFromDatabase() throws SQLException {
        String query = "SELECT * FROM SUPPLIERS";
        return executeQuery(query, "suppliers");
    }

    private List<Object> FetchProductsFromDatabase() throws SQLException {
        String query = "SELECT * FROM PRODUCTS WHERE UNITSINSTOCK > 0";
        return executeQuery(query, "products");
    }

    private List<Object> FetchUpdatedSuppliers(LocalDate lastRefreshTime) throws SQLException {
        String query = "SELECT * FROM PRODUCTS WHERE CREATIONTIME > "
                + lastRefreshTime
                + " LASTMODIFY > "
                + lastRefreshTime;
        return executeQuery(query, "suppliers");
    }

    private List<Object> FetchUpdatedProducts(LocalDate lastRefreshTime) throws SQLException {
        String query = "SELECT * FROM PRODUCTS WHERE CREATIONTIME > "
                + lastRefreshTime
                + " LASTMODIFY > "
                + lastRefreshTime;
        return executeQuery(query, "products");
    }

    private boolean hasSupplierDatasetUpdated(String key, LocalDate lastRefreshTime) throws SQLException {
        String query = "SELECT COUNT(*) FROM SUPPLIERS WHERE CREATIONTIME > "
                + lastRefreshTime
                + " LASTMODIFY > "
                + lastRefreshTime;
        return executeAggregateQuery(query) > 0;
    }

    private boolean hasProductDatasetUpdated(String key, LocalDate lastRefreshTime) throws SQLException {
        String query = "SELECT COUNT(*) FROM PRODUCTS WHERE CREATIONTIME > "
                + lastRefreshTime
                + " LASTMODIFY > "
                + lastRefreshTime;
        return executeAggregateQuery(query) > 0;
    }

    private int executeAggregateQuery(String query) throws SQLException {
        int result = 0;
        Connection _conn;
        Statement _st;
        _conn = DriverManager.getConnection(_connectionString, _user, _password);
        _st = _conn.createStatement();
        ResultSet rs = _st.executeQuery(query);
        while (rs.next())
        {
            result = rs.getInt(1);
        }
        _st.close();
        _conn.close();
        return result;
    }

    private List<Object> executeQuery(String query, String dataset) throws SQLException {
        Connection _conn;
        Statement _st;
        List<Object> data;
        _conn = DriverManager.getConnection(_connectionString, _user, _password);
        _st = _conn.createStatement();

        ResultSet rs = _st.executeQuery(query);
        data = getData(rs, dataset);

        _st.close();
        _conn.close();

        return data;
    }

    private List<Object> getData(ResultSet set, String dataSet) throws SQLException {
        List<Object> dataList = new ArrayList<>();
        while (set.next())
        {
            if(dataSet.equals("suppliers"))
            {
                Suppliers suppliers = new Suppliers();
                suppliers.setId(set.getInt(1));
                suppliers.setContactName(set.getString(2));
                suppliers.setCompanyName(set.getString(3));
                suppliers.setAddress(set.getString(4));
                dataList.add(suppliers);
            }else if(dataSet.equals("products"))
            {
                Product product = new Product();
                product.setId(set.getInt(1));
                product.setName(set.getString(2));
                product.setUnitsAvailable(set.getInt(3));
                product.setUnitPrice(set.getInt(4));
                dataList.add(product);
            }
        }
        return dataList;
    }

    @Override
    public void close() throws Exception {

    }
}
