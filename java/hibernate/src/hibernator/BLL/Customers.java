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
//package hibernator.BLL;
package hibernator.BLL;

import java.util.Collection;
import java.util.List;
import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.OneToMany;
import org.hibernate.annotations.CacheConcurrencyStrategy;

@Entity
@org.hibernate.annotations.Cache(usage = CacheConcurrencyStrategy.READ_WRITE, region = "CustomersCache")
public class Customers
{

    public Customers(String _customerID, String _companyName, String _contactName, String _address, String _city, String _region, String _postakCode, String _country)
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

    @Id
//    @GeneratedValue(strategy=GenerationType.AUTO)
    @Column(name = "CustomerID")
    public  String _customerID; 
    
    @Column(name = "CompanyName")
    public String _companyName;
    
    @Column(name = "ContactName")
    private String _contactName;
    
    @Column(name = "Address")
    private String _address;
    
    @Column(name = "City")
    private String _city;
    
    @Column(name = "Region")
    private String _region;
    
    @Column(name = "PostalCode")
    private String _postalCode;

    @Column(name = "Country")
    private String _country;
    
    @OneToMany(cascade = CascadeType.ALL)
    @JoinColumn(name="CustomerID")
    public List<Orders> _orders;
    
    public void setOrders(List<Orders> _order)
    {
        this._orders = _order;
    }

    public Collection<Orders> getOrders()
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

    public Customers()
    {
    }



}
