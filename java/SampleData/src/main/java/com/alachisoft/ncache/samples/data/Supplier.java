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
 * Model class for suppliers.
 */
public class Supplier implements Serializable {

    private static final long serialVersionUID = 1L;

    /**
     * Unique SupplierID assigned to each supplier.
     */
    private int supplierID;

    /**
     * Name of the supplier company.
     */
    private String companyName;

    /**
     * Contact name of the supplier.
     */
    private String contactName;

    /**
     * Address of the supplier.
     */
    private String address;

    // -------------------- Getters and Setters --------------------

    /**
     * Returns the unique SupplierID assigned to this supplier.
     *
     * @return the supplierID
     */
    public int getSupplierID() {
        return supplierID;
    }

    /**
     * Sets the unique SupplierID for this supplier.
     *
     * @param supplierID the supplierID to set
     */
    public void setSupplierID(int supplierID) {
        this.supplierID = supplierID;
    }

    /**
     * Returns the name of the supplier company.
     *
     * @return the companyName
     */
    public String getCompanyName() {
        return companyName;
    }

    /**
     * Sets the name of the supplier company.
     *
     * @param companyName the companyName to set
     */
    public void setCompanyName(String companyName) {
        this.companyName = companyName;
    }

    /**
     * Returns the contact name for the supplier.
     *
     * @return the contactName
     */
    public String getContactName() {
        return contactName;
    }

    /**
     * Sets the contact name for the supplier.
     *
     * @param contactName the contactName to set
     */
    public void setContactName(String contactName) {
        this.contactName = contactName;
    }

    /**
     * Returns the address of the supplier.
     *
     * @return the address
     */
    public String getAddress() {
        return address;
    }

    /**
     * Sets the address of the supplier.
     *
     * @param address the address to set
     */
    public void setAddress(String address) {
        this.address = address;
    }
}
