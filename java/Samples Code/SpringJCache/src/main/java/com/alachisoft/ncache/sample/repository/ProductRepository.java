/*
 * ===============================================================================
 * Alachisoft (R) NCache Sample Code.
 * NCache Basic Operations sample
 * ===============================================================================
 * Copyright © Alachisoft.  All rights reserved.
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
 * OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE.
 * ===============================================================================
 */

package com.alachisoft.ncache.sample.repository;

import com.alachisoft.ncache.samples.data.Product;
import org.springframework.stereotype.Repository;
import java.util.HashMap;
import java.util.Map;

/**
 * Product storage repository (using NCache as secondary storage)
 *
 */
@Repository
public class ProductRepository {

    // Initializing store
    private final Map<Integer, Product> store = new HashMap<>();

    // ------------------------------------------------------------------------------
    /**
     * Method to retrieve product instance from store
     *
     * @param id ID of product instance to be retrieved
     * @return Instance of product
     */
    public Product getById(int id) {
        return store.get(id);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to save product instance to store
     *
     * @param product Product instance to be inserted into store
     * @return Instance of Product
     */
    public Product save(Product product) {
        store.put(product.getProductID(), product);
        return product;
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to remove product from store
     *
     * @param id ID of product instance to be removed from store
     * @return Instance of Product
     */
    public Product remove(int id) {
        return store.remove(id);
    }
}
