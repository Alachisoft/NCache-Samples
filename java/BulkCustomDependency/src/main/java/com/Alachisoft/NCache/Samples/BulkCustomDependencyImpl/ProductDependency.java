package com.Alachisoft.NCache.Samples.BulkCustomDependencyImpl;

import com.alachisoft.ncache.runtime.dependencies.BulkExtensibleDependency;

import java.io.Serializable;
import java.sql.*;
import java.util.*;

/**
 * Implementation of BulkExtensibleDependency.
 */
public class ProductDependency extends BulkExtensibleDependency implements Serializable {

    private int productId;
    private int unitsInStock;
    private final String _connString;
    private final String _userId;
    private final String _password;

    private transient  Connection _conn;

    /**
     * Custom constructor initialize dependency
     * @param connString
     * @param id
     * @param units
     * @param userId
     * @param password
     */
    public ProductDependency(String connString, String userId, String password, int id, int units) {
        _connString = connString;
        _userId = userId;
        _password = password;
        unitsInStock = units;
        productId = id;
    }

    /**
     * This method is used to invalidate a cached items on clean interval
     * @param iterable
     * @throws Exception
     */
    @Override
    public void evaluateBulk(Iterable<BulkExtensibleDependency> iterable) throws Exception {
        List<BulkExtensibleDependency> dependencyList = new ArrayList<>();
        for (BulkExtensibleDependency dep : iterable) {
            dependencyList.add(dep);
        }

        List<Integer> productIds = getDependencyProductIds(dependencyList);

        Map<Integer, Integer> productInfo = unitsInStockStatus(productIds);

        for (BulkExtensibleDependency dep : dependencyList) {
            ProductDependency productDependency = (ProductDependency)dep;
            if(productInfo.containsKey(productDependency.productId))
            {
                if(productInfo.get(productDependency.productId) != productDependency.unitsInStock)
                {
                    dep.expire();
                }
            }
        }
    }

    /**
     * Get list of productID of Products with bulk dependency
     * @param dependencyList IEnumerable BulkExtensibleDependency collection
     * @return List of productId Products with bulk dependency
     */
    private List<Integer> getDependencyProductIds(List<BulkExtensibleDependency> dependencyList) {
        List<Integer> productIds = new ArrayList<>();

        for (BulkExtensibleDependency dependency:
             dependencyList) {
            ProductDependency productDependency = (ProductDependency)dependency;
            productIds.add(productDependency.productId);
        }
        return productIds;
    }

    /**
     *  Initialize custom dependency
     * @return
     */
    @Override
    public boolean initialize() {
        return true;
    }

    /**
     * Establishes the database connection
     * @param connString
     * @param user
     * @param pass
     */
    private void establishConnection(String connString, String user, String pass){
        try {
            if(connString == null)
                throw new IllegalArgumentException();
            if(user == null)
                throw new IllegalArgumentException();
            if(pass == null)
                throw new IllegalArgumentException();
            _conn = DriverManager.getConnection(connString, user, pass);
        } catch (SQLException throwable) {
            throwable.printStackTrace();
        }
    }

    /**
     * Get units in stock of each product
     * @param productIds
     * @return
     * @throws SQLException
     */
    private Map<Integer, Integer> unitsInStockStatus(List<Integer> productIds) throws SQLException {
        Map<Integer, Integer> productInfo = new HashMap<>();
        establishConnection(_connString, _userId, _password);
        String query = "Select PRODUCTID, UNITSINSTOCK from SCOTT.PRODUCTS WHERE PRODUCTID = ?";
        ResultSet rs = null;
        for(int id : productIds){
            PreparedStatement ps = _conn.prepareStatement(query);
            ps.setInt(1, id);
            rs = ps.executeQuery();
            while (rs.next()){
                productId = rs.getInt("PRODUCTID");
                unitsInStock = rs.getInt("UNITSINSTOCK");
                productInfo.put(productId, unitsInStock);
            }
            rs.close();
        }
        return productInfo;
    }

    /**
     * Dispose custom dependency
     */
    @Override
    public void dispose() {
        if(_conn != null) {
            try {
                _conn.close();
            } catch (SQLException throwables) {
                throwables.printStackTrace();
            }
        }
        super.dispose();
    }

    @Override
    public String toString() {
        return "ProductDependency{" +
                "productId=" + productId +
                ", unitsInStock=" + unitsInStock +
                '}';
    }
}
