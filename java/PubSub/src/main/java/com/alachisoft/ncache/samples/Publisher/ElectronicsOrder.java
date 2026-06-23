package com.alachisoft.ncache.samples.Publisher;
import com.alachisoft.ncache.samples.data.Order;

import java.io.Serializable;
import java.util.UUID;

/**
 * Electronics Order
 */
public class ElectronicsOrder extends Order implements Serializable {

    public ElectronicsOrder() {
        // Java equivalent of Guid → int
        setOrderID(UUID.randomUUID().hashCode());
    }
}

