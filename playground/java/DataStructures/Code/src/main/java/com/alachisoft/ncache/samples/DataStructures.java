package com.alachisoft.ncache.samples;

import java.io.Serializable;
import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.Date;
import com.alachisoft.ncache.client.*;
import com.alachisoft.ncache.client.datastructures.DistributedList;
import com.alachisoft.ncache.client.datastructures.DistributedQueue;

// Use NCache distributed data structures (List and Queue in this sample). NCache provides
// Set, Dictionary, and Counter data structures as well. These data structures are distributed in the cluster
// for scalability and accessible to all clients.

public class DataStructures {

    public static void main(String[] args) throws Exception {

        System.out.println("Sample showing distributed data structures in NCache (List, Queue) for more powerful data management.\n");

        String cacheName = args[0];

        System.out.println("Connecting to cache: " + cacheName);
        // Connect to the cache and return a cache handle
        Cache cache = CacheManager.getCache(cacheName);
        System.out.println("Connected to Cache successfully: " + cacheName + "\n");
        System.out.println("Create a List data structure and add products 'Electronics' to it...");

        // Get the list if it already exists in the cache
        DistributedList<Product> productList = cache.getDataStructuresManager().getList("productList", Product.class);

        if (productList == null) {
            // Create a List data structure in the cache and then add products to it
            productList = cache.getDataStructuresManager().createList("productList", Product.class);
        }

        // These products are directly added to the cache inside the List data structure when you add them below
        productList.add(new Product(15001, "Laptop", BigDecimal.valueOf(2000), "Electronics"));
        productList.add(new Product(15002, "Kindle", BigDecimal.valueOf(1000), "Electronics"));
        productList.add(new Product(15004, "Smart Phone", BigDecimal.valueOf(1000), "Electronics"));
        productList.add(new Product(15005, "Mobile Phone", BigDecimal.valueOf(200), "Electronics"));

        printProductList(productList);

        System.out.println("Create a Queue data structure and add orders to it...");

        // Get the queue if it already exists in the cache
        DistributedQueue<Order> orderQueue = cache.getDataStructuresManager().getQueue("orderQueue", Order.class);

        if (orderQueue == null) {
            // Create a Queue data structure and then add orders to it by maintaining their sequence
            orderQueue = cache.getDataStructuresManager().createQueue("orderQueue", Order.class);
        }

        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd");
        // These orders are directly added to the cache inside the Queue data structure when you enqueue them below
        orderQueue.add(new Order(16248, "Vins et alcools Chevalier", dateFormat.parse("1997-07-04"), "Federal Shipping"));
        orderQueue.add(new Order(16249, "Toms Spezialitäten", dateFormat.parse("1997-07-05"), "Speedy Express"));
        orderQueue.add(new Order(16250, "Hanari Carnes", dateFormat.parse("1997-07-07"), "United Package"));
        orderQueue.add(new Order(16251, "Victuailles en stock", dateFormat.parse("1997-07-08"), "Speedy Express"));

        printOrderQueue(orderQueue);

        System.out.println("Fetch the list and print all items from the List data structure...");

        // Fetch the List data structure from the cache and then access all items in it
        DistributedList<Product> fetchedProductList = cache.getDataStructuresManager().getList("productList", Product.class);

        printProductList(fetchedProductList);

        System.out.println("Process all orders from the Queue one by one in a sequence\n");

        // Fetch the Queue data structure from the cache and then process all orders one by one in a sequence
        DistributedQueue<Order> fetchedOrderQueue = cache.getDataStructuresManager().getQueue("orderQueue", Order.class);
        while (fetchedOrderQueue.size() > 0) {
            Order order = orderQueue.poll();
            System.out.println("Process order : " + order.getOrderID());
            printOrder(order);
        }

        // Removing all items from the list
        productList.removeRange(0, productList.size());

        System.out.println("Removed products from the List data structure.\n");

        System.out.println("Sample completed successfully.");
        cache.close();
    }

    static void printProductList(DistributedList<Product> productList) {
        System.out.println("ProductID, Name, Price, Category, Tags");
        for (Product product : productList) {
            System.out.println("- " + product.getProductID() + ", " + product.getName() + ", $" + product.getPrice() + ", " + product.getCategory());
        }
        System.out.println();
    }

    static void printOrderQueue(DistributedQueue<Order> orderQueue) {
        System.out.println("OrderID, Customer, Order Date, Ship Via");

        for (Order order : orderQueue) {
            System.out.println("- " + order.getOrderID() + ", " + order.getCustomer() + ", " + order.getOrderDate().toString() + ", " + order.getShipVia());
        }
        System.out.println();
    }

    static void printOrder(Order order) {
        System.out.println("OrderID, Customer, Order Date, Ship Via");
        System.out.println("- " + order.getOrderID() + ", " + order.getCustomer() + ", " + order.getOrderDate().toString() + ", " + order.getShipVia() + "\n");
    }
}

// Sample class for Product
class Product implements Serializable {
    private int productID;
    private String name;
    private BigDecimal price;
    private String category;

    public Product(){ }

    public Product(int productID, String name, BigDecimal price, String category) {
        this.productID = productID;
        this.name = name;
        this.price = price;
        this.category = category;
    }

    public int getProductID() { return productID; }
    public String getName() { return name; }
    public BigDecimal getPrice() { return price; }
    public String getCategory() { return category; }
}

// Sample class for Order
class Order implements Serializable {
    private int orderID;
    private String customer;
    private Date orderDate;
    private String shipVia;

    public Order(){ }

    public Order(int orderID, String customer, Date orderDate, String shipVia) {
        this.orderID = orderID;
        this.customer = customer;
        this.orderDate = orderDate;
        this.shipVia = shipVia;
    }

    public int getOrderID() { return orderID; }
    public String getCustomer() { return customer; }
    public String getOrderDate() { return new SimpleDateFormat("yyyy-MM-dd").format(orderDate); }
    public String getShipVia() { return shipVia; }
}
