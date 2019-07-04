// ===============================================================================
// Alachisoft (R) NCache Sample Code
// NCache Hibernate sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package hibernator.BLL;

import java.util.Collection;

public class Customer
{

    public Customer(String _customerID, String _companyName, String _contactName, String _address, String _city, String _region, String _postakCode, String _country)
    {
        this._customerID = _customerID;
        this._companyName = _companyName;
        this._contactName = _contactName;
        this._address = _address;
        this._city = _city;
        this._region = _region;
        this._postalCode = _postakCode;
        this._country = _country;
    }

    public  String _customerID, _companyName, _contactName, _address, _city, _region, _postalCode,_country;
    public Collection _orders;

    public void setOrders(Collection _order)
    {
        this._orders = _order;
    }

    public Collection getOrders()
    {
        return _orders;
    }

    public void setAddress(String _address)
    {
        this._address = _address;
    }

    public void setCity(String _city)
    {
        this._city = _city;
    }

    public void setCompanyName(String _companyName)
    {
        this._companyName = _companyName;
    }

    public void setContactName(String _contactName)
    {
        this._contactName = _contactName;
    }

    public void setCountry(String _country)
    {
        this._country = _country;
    }

    public void setCustomerID(String _customerID)
    {
        this._customerID = _customerID;
    }

    public void setPostalCode(String _postalCode)
    {
        this._postalCode = _postalCode;
    }

    public void setRegion(String _region)
    {
        this._region = _region;
    }

    public String getAddress()
    {
        return _address;
    }

    public String getCity()
    {
        return _city;
    }

    public String getCompanyName()
    {
        return _companyName;
    }

    public String getContactName()
    {
        return _contactName;
    }

    public String getCountry()
    {
        return _country;
    }

    public String getCustomerID()
    {
        return _customerID;
    }

    public String getPostalCode()
    {
        return _postalCode;
    }

    public String getRegion()
    {
        return _region;
    }

    public Customer()
    {
    }



}
