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
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.Date;
import java.util.UUID;


/**
 * Model class for Orders.
 */
public class Order implements Serializable {

    private static final long serialVersionUID = 1L;

    /**
     * Unique OrderID assigned to each order.
     */
    private int orderID;

    /**
     * The time when the order was made.
     */
    private Date orderDate;

    /**
     * Name of the person to whom the order is to be delivered.
     */
    private String shipName;

    /**
     * Address where the order is to be delivered.
     */
    private String shipAddress;

    /**
     * City where the order is to be delivered.
     */
    private String shipCity;

    /**
     * Country where the order is to be delivered.
     */
    private String shipCountry;

    // -------------------- Getters and Setters --------------------


    /**
     * Returns the unique OrderID assigned to this order.
     *
     * @return the orderID
     */
    public int getOrderID() {
        return orderID;
    }

    /**
     * Sets the unique OrderID for this order.
     *
     * @param orderID the orderID to set
     */
    public void setOrderID(int orderID) {
        this.orderID = orderID;
    }

    /**
     * Returns the date and time when the order was placed.
     *
     * @return the orderDate
     */
    public Date getOrderDate() {
        return orderDate;
    }

    /**
     * Sets the date and time when the order was placed.
     *
     * @param orderDate the orderDate to set
     */
    public void setOrderDate(Date orderDate) {
        this.orderDate = orderDate;
    }

    /**
     * Returns the name of the person to whom the order is to be delivered.
     *
     * @return the shipName
     */
    public String getShipName() {
        return shipName;
    }

    /**
     * Sets the name of the person to whom the order is to be delivered.
     *
     * @param shipName the shipName to set
     */
    public void setShipName(String shipName) {
        this.shipName = shipName;
    }

    /**
     * Returns the address where the order is to be delivered.
     *
     * @return the shipAddress
     */
    public String getShipAddress() {
        return shipAddress;
    }

    /**
     * Sets the address where the order is to be delivered.
     *
     * @param shipAddress the shipAddress to set
     */
    public void setShipAddress(String shipAddress) {
        this.shipAddress = shipAddress;
    }

    /**
     * Returns the city where the order is to be delivered.
     *
     * @return the shipCity
     */
    public String getShipCity() {
        return shipCity;
    }

    /**
     * Sets the city where the order is to be delivered.
     *
     * @param shipCity the shipCity to set
     */
    public void setShipCity(String shipCity) {
        this.shipCity = shipCity;
    }

    /**
     * Returns the country where the order is to be delivered.
     *
     * @return the shipCountry
     */
    public String getShipCountry() {
        return shipCountry;
    }

    /**
     * Sets the country where the order is to be delivered.
     *
     * @param shipCountry the shipCountry to set
     */
    public void setShipCountry(String shipCountry) {
        this.shipCountry = shipCountry;
    }

    /**
     * Generates an order of the specified type with the current date.
     *
     * @param clazz The class type extending Order.
     * @param <T>   A specific Order type.
     * @return A new instance of the specified Order type.
     */
    public static <T extends Order> T generateOrder(Class<T> clazz) {
        try {
            T order = clazz.getDeclaredConstructor().newInstance();
            order.setOrderDate(new Date());
            return order;
        } catch (Exception e) {
            throw new RuntimeException("Unable to generate order instance.", e);
        }
    }
}

