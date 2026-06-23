package com.alachisoft.ncache.samples.Publisher;
import com.alachisoft.ncache.samples.data.Order;
import java.io.Serializable;
import java.util.UUID;

/**
 * Garments Order
 */
public class GarmentsOrder extends Order implements Serializable {

    public GarmentsOrder() {
        // Java equivalent of Guid → int
        setOrderID(UUID.randomUUID().hashCode());
    }
}