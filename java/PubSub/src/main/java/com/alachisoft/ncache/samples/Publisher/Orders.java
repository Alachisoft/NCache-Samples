package com.alachisoft.ncache.samples.Publisher;

import java.io.Serializable;
import java.lang.reflect.InvocationTargetException;
import java.time.LocalDate;
import java.util.UUID;

public class Orders implements Serializable {
    private String OrderId;
    private LocalDate orderDate;
    private LocalDate shippedDate;

    protected void setOrderDate(LocalDate orderDate) {
        this.orderDate = orderDate;
    }

    public void setShippedDate(LocalDate shippedDate) {
        this.shippedDate = shippedDate;
    }

    public void setOrderId(String orderId) {
        OrderId = orderId;
    }

    public String getOrderId() {
        return OrderId;
    }

    public LocalDate getOrderDate() {
        return orderDate;
    }

    public LocalDate getShippedDate() {
        return shippedDate;
    }


    public static  <T extends Orders> T generateOrder(Class<T> t) throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        T order = t.getDeclaredConstructor().newInstance();
        order.setOrderDate(LocalDate.now());
        return order;
    }
}

class ElectronicOrder extends Orders implements Serializable {
    public ElectronicOrder()
    {
        UUID uuid = UUID.randomUUID();
        setOrderId("ElectronicsOrder"+ uuid.toString());
    }
}

class GarmentsOrder extends Orders implements Serializable {
    public GarmentsOrder()
    {
        UUID uuid = UUID.randomUUID();
        setOrderId("GarmentsOrder"+ uuid.toString());
    }
}
