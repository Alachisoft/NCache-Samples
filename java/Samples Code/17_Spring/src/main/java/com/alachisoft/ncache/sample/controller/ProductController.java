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

package com.alachisoft.ncache.sample.controller;

import com.alachisoft.ncache.runtime.exceptions.CacheException;
import com.alachisoft.ncache.sample.service.ProductService;
import com.alachisoft.ncache.samples.data.Product;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

/**
 * Controller class for product management
 *
 */
@RestController
@RequestMapping("/products")
public class ProductController {

    // Instance of Product Service
    @Autowired
    private ProductService service;

    // ------------------------------------------------------------------------------
    /**
     * Method to retrieve product from repository
     *
     * @param id ID of product instance
     * @return Instance of product
     */
    @GetMapping("/{id}")
    public Product getProduct(@PathVariable int id) throws CacheException {
        return service.getProduct(id);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to add product instance to repository
     *
     * @param product Instance of product
     * @return Instance of product
     */
    @PostMapping
    public Product addProduct(@RequestBody Product product) throws CacheException {
        return service.save(product);
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to remove product from repository
     *
     * @param id ID of product instance
     * @return Instance of product
     */
    @DeleteMapping("/{id}")
    public Product removeProduct(@PathVariable int id) throws CacheException {
        return service.remove(id);
    }
}