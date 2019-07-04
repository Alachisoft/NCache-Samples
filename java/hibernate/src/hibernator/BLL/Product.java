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

public class Product
{
    
    private int _orderID, _productID;
    private String _productName;
    private double _unitPrice;

    public Product(int _orderID, int _productID, String _productName, double _unitPrice)
    {
        this._orderID = _orderID;
        this._productID = _productID;
        this._productName = _productName;
        this._unitPrice = _unitPrice;
    }

    public Product()
    {
    }

    public void setOrderID(int _orderID)
    {
        this._orderID = _orderID;
    }

    public void setProductID(int _productID)
    {
        this._productID = _productID;
    }

    public void setProductName(String _productName)
    {
        this._productName = _productName;
    }

    public void setUnitPrice(double _unitPrice)
    {
        this._unitPrice = _unitPrice;
    }

    public int getOrderID()
    {
        return _orderID;
    }

    public int getProductID()
    {
        return _productID;
    }

    public String getProductName()
    {
        return _productName;
    }

    public double getUnitPrice()
    {
        return _unitPrice;
    }


}
