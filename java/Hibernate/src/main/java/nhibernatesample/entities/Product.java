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

package nhibernatesample.entities;

import javax.persistence.*;
import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;

import java.io.Serializable;

/**
 * Product class definition
 */
@Entity
@Table(name = "Products")
@Cacheable
@Cache(usage = CacheConcurrencyStrategy.NONSTRICT_READ_WRITE, region = "default")
public class Product implements Serializable {

    // Mapping primary key
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ProductID")
    private Long productID;

    // Mapping properties
    @Column(name = "ProductName")
    private String productName;

    @Column(name = "SupplierID")
    private Long supplierID;

    @Column(name = "CategoryID")
    private Long categoryID;

    @Column(name = "QuantityPerUnit")
    private String quantityPerUnit;

    @Column(name = "UnitPrice")
    private Double unitPrice;

    @Column(name = "UnitsInStock")
    private Short unitsInStock;

    @Column(name = "UnitsOnOrder")
    private Short unitsOnOrder;

    @Column(name = "ReorderLevel")
    private Short reorderLevel;

    @Column(name = "Discontinued")
    private Boolean discontinued;

    // Getters/setters
    public Long getProductID() { return productID; }
    public void setProductID(Long productID) { this.productID = productID; }

    public String getProductName() { return productName; }
    public void setProductName(String productName) { this.productName = productName; }

    public Long getSupplierID() { return supplierID; }
    public void setSupplierID(Long supplierID) { this.supplierID = supplierID; }

    public Long getCategoryID() { return categoryID; }
    public void setCategoryID(Long categoryID) { this.categoryID = categoryID; }

    public String getQuantityPerUnit() { return quantityPerUnit; }
    public void setQuantityPerUnit(String quantityPerUnit) { this.quantityPerUnit = quantityPerUnit; }

    public Double getUnitPrice() { return unitPrice; }
    public void setUnitPrice(Double unitPrice) { this.unitPrice = unitPrice; }

    public Short getUnitsInStock() { return unitsInStock; }
    public void setUnitsInStock(Short unitsInStock) { this.unitsInStock = unitsInStock; }

    public Short getUnitsOnOrder() { return unitsOnOrder; }
    public void setUnitsOnOrder(Short unitsOnOrder) { this.unitsOnOrder = unitsOnOrder; }

    public Short getReorderLevel() { return reorderLevel; }
    public void setReorderLevel(Short reorderLevel) { this.reorderLevel = reorderLevel; }

    public Boolean getDiscontinued() { return discontinued; }
    public void setDiscontinued(Boolean discontinued) { this.discontinued = discontinued; }
}

