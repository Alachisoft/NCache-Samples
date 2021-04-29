// ===============================================================================
// Alachisoft (R) NCache Sample Code
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

package com.alachisoft.ncache.samples.data;

import java.io.Serializable;

/**
 Model class for orders
 */
public class Order implements Serializable
{
    /**
     Unique Id assigned to each order
     */
    private int orderID;
    public final int getOrderID()
    {
        return orderID;
    }
    public final void setOrderID(int value)
    {
        orderID = value;
    }

    /**
     The time when order was made
     */
    private java.util.Date orderDate = new java.util.Date(0);
    public final java.util.Date getOrderDate()
    {
        return orderDate;
    }
    public final void setOrderDate(java.util.Date value)
    {
        orderDate = value;
    }

    /**
     Name of the person whom order is to be delivered
     */
    private String shipName;
    public final String getShipName()
    {
        return shipName;
    }
    public final void setShipName(String value)
    {
        shipName = value;
    }

    /**
     The address where order is to be delivered
     */
    private String shipAddress;
    public final String getShipAddress()
    {
        return shipAddress;
    }
    public final void setShipAddress(String value)
    {
        shipAddress = value;
    }

    /**
     City where order is to be delivered
     */
    private String shipCity;
    public final String getShipCity()
    {
        return shipCity;
    }
    public final void setShipCity(String value)
    {
        shipCity = value;
    }

    /**
     Country where order is to be delivered
     */
    private String shipCountry;
    public final String getShipCountry()
    {
        return shipCountry;
    }
    public final void setShipCountry(String value)
    {
        shipCountry = value;
    }
}