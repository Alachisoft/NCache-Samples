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
 * Model class representing a Customer entity.
 */
public class Customer implements Serializable {

    private static final long serialVersionUID = 1L;

    /**
     * Unique CustomerID of the customer.
     */
    private String customerID;

    /**
     * Contact name of the customer.
     */
    private String contactName;

    /**
     * Company the customer works for.
     */
    private String companyName;

    /**
     * Contact number of the customer.
     */
    private String contactNo;

    /**
     * Residential address of the customer.
     */
    private String address;

    /**
     * Residence city of the customer.
     */
    private String city;

    /**
     * Nationality of the customer.
     */
    private String country;

    /**
     * Postal code of the customer.
     */
    private String postalCode;

    /**
     * Fax number of the customer.
     */
    private String fax;

    // -------------------- Getters and Setters --------------------

    /**
     * Returns the unique identifier for this customer.
     *
     * @return the customerID
     */
    public String getCustomerID() {
        return customerID;
    }

    /**
     * Sets the unique identifier for this customer.
     *
     * @param customerID the customerID to set
     */
    public void setCustomerID(String customerID) {
        this.customerID = customerID;
    }

    /**
     * Returns the contact name of the customer.
     *
     * @return the contactName
     */
    public String getContactName() {
        return contactName;
    }

    /**
     * Sets the contact name of the customer.
     *
     * @param contactName the contactName to set
     */
    public void setContactName(String contactName) {
        this.contactName = contactName;
    }

    /**
     * Returns the company name associated with the customer.
     *
     * @return the companyName
     */
    public String getCompanyName() {
        return companyName;
    }

    /**
     * Sets the company name associated with the customer.
     *
     * @param companyName the companyName to set
     */
    public void setCompanyName(String companyName) {
        this.companyName = companyName;
    }

    /**
     * Returns the contact number of the customer.
     *
     * @return the contactNo
     */
    public String getContactNo() {
        return contactNo;
    }

    /**
     * Sets the contact number of the customer.
     *
     * @param contactNo the contactNo to set
     */
    public void setContactNo(String contactNo) {
        this.contactNo = contactNo;
    }

    /**
     * Returns the residential address of the customer.
     *
     * @return the address
     */
    public String getAddress() {
        return address;
    }

    /**
     * Sets the residential address of the customer.
     *
     * @param address the address to set
     */
    public void setAddress(String address) {
        this.address = address;
    }

    /**
     * Returns the city where the customer resides.
     *
     * @return the city
     */
    public String getCity() {
        return city;
    }

    /**
     * Sets the city where the customer resides.
     *
     * @param city the city to set
     */
    public void setCity(String city) {
        this.city = city;
    }

    /**
     * Returns the country (nationality) of the customer.
     *
     * @return the country
     */
    public String getCountry() {
        return country;
    }

    /**
     * Sets the country (nationality) of the customer.
     *
     * @param country the country to set
     */
    public void setCountry(String country) {
        this.country = country;
    }

    /**
     * Returns the postal code of the customer.
     *
     * @return the postalCode
     */
    public String getPostalCode() {
        return postalCode;
    }

    /**
     * Sets the postal code of the customer.
     *
     * @param postalCode the postalCode to set
     */
    public void setPostalCode(String postalCode) {
        this.postalCode = postalCode;
    }

    /**
     * Returns the fax number of the customer.
     *
     * @return the fax
     */
    public String getFax() {
        return fax;
    }

    /**
     * Sets the fax number of the customer.
     *
     * @param fax the fax to set
     */
    public void setFax(String fax) {
        this.fax = fax;
    }
}
