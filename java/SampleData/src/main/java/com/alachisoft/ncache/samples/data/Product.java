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
    private int productId;
    private String name;
    private String productClass;
    private String category;
    private int unitPrice ;
    private String quantityPerUnit;
    private int unitsAvailable;
    private int supplierId;
    private int catId;

    public void setId(int productId) {
        this.productId = productId;
    }
    public int getId() {
        return this.productId;
    }

    public void setProductId(int productId) {
        this.productId = productId;
    }
    public int getProductId() {
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

    public final int getUnitPrice()
    {
        return unitPrice;
    }
    public final void setUnitPrice(int value)
    {
        unitPrice = value;
    }

    public final String getQuantityPerUnit()
    {
        return quantityPerUnit;
    }
    public final void setQuantityPerUnit(String value)
    {
        quantityPerUnit = value;
    }

    public final int getUnitsAvailable()
    {
        return unitsAvailable;
    }
    public final void setUnitsAvailable(int value)
    {
        unitsAvailable = value;
    }

    public int getSupplierId() {
        return supplierId;
    }
    public void setSupplierId(int supplierId) {
        this.supplierId = supplierId;
    }

    public int getCatId() {
        return catId;
    }
    public void setCatId(int catId) {
        this.catId = catId;
    }
}
