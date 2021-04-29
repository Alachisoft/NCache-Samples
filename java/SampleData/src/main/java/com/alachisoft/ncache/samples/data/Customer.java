// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Customer Class used by java.com.alachisoft.ncache.samples
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples.data;

import java.io.Serializable;

public class Customer implements Serializable
{
    private String customerID;
    private String contactName;
    private String companyName;
    private String contactNo;
    private String ContactTitle;
    private String address;
    private String city;
    private String country;
    private String region;
    private String phone;

    public String getContactTitle() {
        return ContactTitle;
    }

    public void setContactTitle(String contactTitle) {
        ContactTitle = contactTitle;
    }

    public String getRegion() {
        return region;
    }

    public void setRegion(String region) {
        this.region = region;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    private String postalCode;
    private String fax;

    public Customer()
    {
    }

    /**
     Unique Id of the customer
     */
    public String getCustomerID()
    {
        return customerID;
    }
    public void setCustomerID(String value)
    {
        customerID = value;
    }

    /**
     Contact name of the customer
     */
    public String getContactName()
    {
        return contactName;
    }
    public void setContactName(String value)
    {
        contactName = value;
    }

    /**
     Company the customer works for
     */
    public String getCompanyName()
    {
        return companyName;
    }
    public void setCompanyName(String value)
    {
        companyName = value;
    }

    /**
     Contact number of the customer
     */
    public String getContactNo()
    {
        return contactNo;
    }
    public void setContactNo(String value)
    {
        contactNo = value;
    }

    /**
     Residential address of the customer
     */
    public String getAddress()
    {
        return address;
    }
    public void setAddress(String value)
    {
        address = value;
    }

    /**
     Residence city of the customer
     */
    public String getCity()
    {
        return city;
    }
    public void setCity(String value)
    {
        city = value;
    }

    /**
     Nationality of the customer
     */
    public String getCountry()
    {
        return country;
    }
    public void setCountry(String value)
    {
        country = value;
    }

    /**
     Postal code of the customer
     */
    public String getPostalCode()
    {
        return postalCode;
    }
    public void setPostalCode(String value)
    {
        postalCode = value;
    }

    /**
     Fax number of the customer
     */
    public String getFax()
    {
        return fax;
    }
    public void setFax(String value)
    {
        fax = value;
    }

    @Override
    public String toString() {
        return "Customer{" +
                "customerID='" + customerID + '\'' +
                ", contactName='" + contactName + '\'' +
                ", companyName='" + companyName + '\'' +
                ", contactNo='" + contactNo + '\'' +
                ", ContactTitle='" + ContactTitle + '\'' +
                ", address='" + address + '\'' +
                ", city='" + city + '\'' +
                ", country='" + country + '\'' +
                ", region='" + region + '\'' +
                ", phone='" + phone + '\'' +
                ", postalCode='" + postalCode + '\'' +
                ", fax='" + fax + '\'' +
                '}';
    }
}