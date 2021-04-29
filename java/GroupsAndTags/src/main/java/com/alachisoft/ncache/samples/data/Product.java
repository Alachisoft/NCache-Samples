// ===============================================================================
// Alachisoft (R) NCache Sample Code
// NCache Product Class used by java.com.alachisoft.ncache.samples
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples.data;

import java.io.Serializable;

public class Product implements Serializable 
{
    private String productId;
    private String name;
    private String productClass;
    private String category;

    public Product(){
        this.productId = "Product:Mobile";
        this.name = "Test Product";
        this.productClass = "Electronics";
        this.category = "Mobile";
    }

    public Product(String productId, String name, String productClass, String category) {
        this.productId = productId;
        this.name = name;
        this.productClass = productClass;
        this.category = category;
    }

    public void setId(String productId) {
        this.productId = productId;        
    }
    
    public String getId() {
        return this.productId;
    }
            
    public void setName(String name) {
        this.name = name;
    }
    
    public String getName() {
        return this.name;
    }
    
    public void setClassName (String productClass) {
        this.productClass = productClass;
    }
    
    public String getClassName () {
        return this.productClass;
    }
    
    public void setCategory(String category) {
        this.category = category;        
    }
    
    public String getCategory() {
        return this.category;
    }

    @Override
    public String toString() {
        return "ID: " + productId + "\n"
                + "Name: " + name + "\n"
                + "Class: " + productClass + "\n"
                + "Category: " + category + "\n";
    }
}
