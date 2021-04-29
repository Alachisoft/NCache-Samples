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

import java.util.Date;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import org.hibernate.annotations.CacheConcurrencyStrategy;

@Entity
@org.hibernate.annotations.Cache(usage = CacheConcurrencyStrategy.READ_WRITE, region = "OrdersCache")
public class Orders
{

    @Id
    @Column(name = "OrderID")    
    private int _orderID;
        
    @Column(name = "CustomerID")    
    private String _customerID;

    @Column(name = "ShipName")    
    private String _shippedName;
    
    @Column(name = "ShipAddress")    
    private String _shipAddress;
    
    @Column(name = "ShipCity")    
    private String _shipCity;
        
    @Column(name = "ShipRegion")    
    private String _shipRegion;
    
    @Column(name = "ShipPostalCode")    
    private String _shipPostalCode;
                
//    @Column(name = "OrderDate")
//    private Date _orderDate;
//
//    @Column(name = "ShippedDate")
//    private Date _shippedDate;
    
    public Orders(int _orderID, String _shippedName, String _shipAddress, String _shipCity, String _shipRegion, String _shipPostalCode, String _customerID, Date _orderDate, Date _shippedDate)
    {
        this._orderID = _orderID;
        this._shippedName = _shippedName;
        this._shipAddress = _shipAddress;
        this._shipCity = _shipCity;
        this._shipRegion = _shipRegion;
        this._shipPostalCode = _shipPostalCode;
        this._customerID = _customerID;
//        this._orderDate = _orderDate;
//        this._shippedDate = _shippedDate;
    }

    public Orders()
    {
    }


    public void setCustomerID(String _customerID)
    {
        this._customerID = _customerID;
    }

//    public void setOrderDate(Date _orderDate)
//    {
//        this._orderDate = _orderDate;
//    }

    public void setOrderID(int _orderID)
    {
        this._orderID = _orderID;
    }

    public void setShipAddress(String _shipAddress)
    {
        this._shipAddress = _shipAddress;
    }

    public void setShipCity(String _shipCity)
    {
        this._shipCity = _shipCity;
    }

    public void setShipPostalCode(String _shipPostalCode)
    {
        this._shipPostalCode = _shipPostalCode;
    }

    public void setShipRegion(String _shipRegion)
    {
        this._shipRegion = _shipRegion;
    }

//    public void setShippedDate(Date _shippedDate)
//    {
//        this._shippedDate = _shippedDate;
//    }

    public void setShippedName(String _shippedName)
    {
        this._shippedName = _shippedName;
    }

    public String getCustomerID()
    {
        return _customerID;
    }

//    public Date getOrderDate()
//    {
//        return _orderDate;
//    }

    public int getOrderID()
    {
        return _orderID;
    }

    public String getShipAddress()
    {
        return _shipAddress;
    }

    public String getShipCity()
    {
        return _shipCity;
    }

    public String getShipPostalCode()
    {
        return _shipPostalCode;
    }

    public String getShipRegion()
    {
        return _shipRegion;
    }

//    public Date getShippedDate()
//    {
//        return _shippedDate;
//    }

    public String getShippedName()
    {
        return _shippedName;
    }

}
