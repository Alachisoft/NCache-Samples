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

import com.alachisoft.ncache.sample.service.ProductService;
import com.alachisoft.ncache.samples.data.Product;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

/**
 * Controller class for product management
 *
 */
@Controller
@RequestMapping("/products")
public class ProductController {

    private final ProductService productService;

    public ProductController(ProductService service) {
        this.productService = service;
    }

    // ------------------------------------------------------------------------------

    @GetMapping
    public String productsPage(Model model) {
        model.addAttribute("product", new Product());
        return "products";
    }


    /**
     * Method to retrieve product from repository
     *
     * @param id ID of product instance
     * @return Instance of product
     */
    @GetMapping("/get")
    public String getProduct(@RequestParam int id, Model model) {
        Product product = productService.getProduct(id);
        if (product == null) {
            model.addAttribute("error", true);
        }
        else {
            model.addAttribute("error", false);
        }

        // send the retrieved product to the UI
        model.addAttribute("result", product);

        // send the product object so the UI/thymeleaf knows how to render
        model.addAttribute("product", new Product());
        return "products";
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to add product instance to repository
     *
     * @param product Instance of product
     * @return Instance of product
     */
    @PostMapping("/add")
    public String addProduct(@ModelAttribute Product product) {
        productService.save(product);
        return "redirect:/products";
    }

    // ------------------------------------------------------------------------------
    /**
     * Method to remove product from repository
     *
     * @param id ID of product instance
     * @return Instance of product
     */
    @PostMapping("/delete")
    public String removeProduct(@RequestParam int id) {
        productService.remove(id);
        return "redirect:/products";
    }
}