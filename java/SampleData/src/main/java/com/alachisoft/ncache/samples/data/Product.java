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
package com.alachisoft.ncache.samples.data;

import java.io.Serializable;

/**
 * Model class for Products.
 */
public class Product implements Serializable {

    private static final long serialVersionUID = 1L;

    /**
     * Unique ProductID assigned to each product.
     */
    private int productID;

    /**
     * Price of one unit of this product.
     */
    private float unitPrice;

    /**
     * Name of the product.
     */
    private String productName;

    /**
     * Fully qualified class name.
     */
    private String className;

    /**
     * Quantity per unit of the product.
     */
    private String quantityPerUnit;

    /**
     * Units in stock of the product.
     */
    private short unitsInStock;

    /**
     * Units available of the product.
     */
    private int unitsAvailable;

    /**
     * Category of the product.
     */
    private String category;

    // -------------------- Getters and Setters --------------------

    /**
     * Returns the unique ProductID assigned to this product.
     *
     * @return the productID
     */
    public int getProductID() {
        return productID;
    }

    /**
     * Sets the unique ProductID for this product.
     *
     * @param productID the productID to set
     */
    public void setProductID(int productID) {
        this.productID = productID;
    }

    /**
     * Returns the unit price of this product.
     *
     * @return the unitPrice
     */
    public float getUnitPrice() {
        return unitPrice;
    }

    /**
     * Sets the unit price of this product.
     *
     * @param unitPrice the unitPrice to set
     */
    public void setUnitPrice(float unitPrice) {
        this.unitPrice = unitPrice;
    }

    /**
     * Returns the name of the product.
     *
     * @return the productName
     */
    public String getProductName() {
        return productName;
    }

    /**
     * Sets the name of the product.
     *
     * @param productName the productName to set
     */
    public void setProductName(String productName) {
        this.productName = productName;
    }

    /**
     * Returns the fully qualified class name for this product.
     *
     * @return the className
     */
    public String getClassName() {
        return className;
    }

    /**
     * Sets the fully qualified class name for this product.
     *
     * @param className the className to set
     */
    public void setClassName(String className) {
        this.className = className;
    }

    /**
     * Returns the quantity per unit description for this product.
     *
     * @return the quantityPerUnit
     */
    public String getQuantityPerUnit() {
        return quantityPerUnit;
    }

    /**
     * Sets the quantity per unit description for this product.
     *
     * @param quantityPerUnit the quantityPerUnit to set
     */
    public void setQuantityPerUnit(String quantityPerUnit) {
        this.quantityPerUnit = quantityPerUnit;
    }

    /**
     * Returns the number of units currently in stock.
     *
     * @return the unitsInStock
     */
    public short getUnitsInStock() {
        return unitsInStock;
    }

    /**
     * Sets the number of units currently in stock.
     *
     * @param unitsInStock the unitsInStock to set
     */
    public void setUnitsInStock(short unitsInStock) {
        this.unitsInStock = unitsInStock;
    }

    /**
     * Returns the number of units available for this product.
     *
     * @return the unitsAvailable
     */
    public int getUnitsAvailable() {
        return unitsAvailable;
    }

    /**
     * Sets the number of units available for this product.
     *
     * @param unitsAvailable the unitsAvailable to set
     */
    public void setUnitsAvailable(int unitsAvailable) {
        this.unitsAvailable = unitsAvailable;
    }

    /**
     * Returns the category of the product.
     *
     * @return the category
     */
    public String getCategory() {
        return category;
    }

    /**
     * Sets the category of the product.
     *
     * @param category the category to set
     */
    public void setCategory(String category) {
        this.category = category;
    }

    /**
     * Returns the information about this product in string format.
     *
     * @return the string containing product information.
     */
    @Override
    public String toString() {
        return "[" +
                "Name " + productName +
                ", Quantity/Unit " + quantityPerUnit +
                ", UnitPrice " + unitPrice +
                ", UnitsInStock " + unitsInStock +
                "]";
    }
}
