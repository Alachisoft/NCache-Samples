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

package com.alachisoft.ncache.sample.service;

import com.alachisoft.ncache.sample.repository.ProductRepository;
import com.alachisoft.ncache.samples.data.Product;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.annotation.CacheEvict;
import org.springframework.cache.annotation.CachePut;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.stereotype.Service;

/**
 * Product Service class to manage product repository
 *
 */
@Service
public class ProductService {

    // Declaring product repository
    @Autowired
    private ProductRepository productRepo;

    // ------------------------------------------------------------------------------
    /**
     * Method to retrieve product instance from repository
     *
     * @param id ID of product instance to be retrieved
     * @return Instance of product
     */
    @Cacheable(value="demoCache", key="#id")
    public Product getProduct(int id) {
        System.out.println("Fetching from repository...");
        return productRepo.getById(id);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to save product instance to repository
     *
     * @param product Product instance to be saved to repository
     * @return Instance of product
     */
    @CachePut(value="demoCache", key="#product.productID")
    public Product save(Product product) {
        return productRepo.save(product);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to remove product instance from repository
     *
     * @param id Product instance ID to be removed
     * @return Instance of Product
     */
    @CacheEvict(value = "demoCache", key = "#id")
    public Product remove(int id) {
        return productRepo.remove(id);
    }
}
