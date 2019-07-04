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

public class OrderDetails
{
    private int _productID;
    private int _orderID;

    public OrderDetails()
    {
    }

    public OrderDetails(int productID, int orderID)
    {
        _productID = productID;
        _orderID = orderID;
    }

    public int getProductID()
    {
        return _productID;
    }

    public int getOrderID()
    {
        return _orderID;
    }

    public void setProductID(int productID)
    {
        _productID = productID;
    }

    public void setOrderID(int orderID)
    {
        _orderID = orderID;
    }

}
