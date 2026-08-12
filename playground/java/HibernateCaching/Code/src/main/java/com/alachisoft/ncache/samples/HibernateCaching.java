package com.alachisoft.ncache.samples;


import jakarta.persistence.Column;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.annotations.Cache;
import org.hibernate.annotations.CacheConcurrencyStrategy;
import org.hibernate.boot.registry.StandardServiceRegistryBuilder;
import org.hibernate.cfg.Configuration;
import org.hibernate.service.ServiceRegistry;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;

// Use NCache integration with Hibernate to seamlessly cache query results in an L2 cache.
// Next time, Hibernate fetches these objects from the cache and saves an expensive database trip.
// We're using an H2 in-memory database that goes away when the process ends.

public class HibernateCaching {

    public static void main(String[] args) {

        System.out.println("Sample showing Hibernate caching with NCache to save expensive database trips.\n");
        Logger.getLogger("org.hibernate").setLevel(Level.OFF);

        // Configure Hibernate properties programmatically
        Properties hibernateProperties = new Properties();
        hibernateProperties.put("hibernate.connection.driver_class", "org.h2.Driver");
        hibernateProperties.put("hibernate.connection.url", "jdbc:h2:mem:testdb");
        hibernateProperties.put("hibernate.show_sql", "false");
        hibernateProperties.put("hibernate.hbm2ddl.auto", "create-drop");
        hibernateProperties.put("hibernate.cache.use_query_cache", "true");
        hibernateProperties.put("hibernate.cache.use_second_level_cache", "true");
        hibernateProperties.put("hibernate.cache.region.factory_class", "com.alachisoft.ncache.NCacheRegionFactory");
        hibernateProperties.put("cache-name", args[0]);
        hibernateProperties.put("ncache.application_id", "myapp");

        // Set other Hibernate properties as needed
        Configuration configuration = new Configuration()
                .setProperties(hibernateProperties).addAnnotatedClass(Product.class);

        // Build the ServiceRegistry
        ServiceRegistry serviceRegistry = new StandardServiceRegistryBuilder()
                .applySettings(configuration.getProperties()).build();

        // Build the SessionFactory
        SessionFactory factory = configuration.buildSessionFactory(serviceRegistry);

        // Create a List of Product objects
        ArrayList<Product> products = (ArrayList<Product>) getProducts();

        // Open a new Hibernate session to save products to the database. This also caches it
        try (Session session = factory.openSession()) {
            Transaction transaction = session.beginTransaction();
            // save() method saves products to the database and caches it too
            System.out.println("ProductID, Name, Price, Category");
            for (Product product : products) {
                System.out.println("- " + product.getProductID() + ", " + product.getName() + ", " + product.getPrice() + ", " + product.getCategory());
                session.save(product);
            }
            transaction.commit();
            System.out.println();
        }

        // Now open a new session to fetch products from the DB.
        // But, these products are actually fetched from the cache
        try (Session session = factory.openSession()) {
            List<Product> productList = (List<Product>) session.createQuery("from Product").list();
            if (productList != null) {
                System.out.println("Found and retrieved all product from the cache...");
                printProductDetails(productList);
            }
        }
        System.out.println("Sample completed successfully.");
        System.exit(0);
    }

    // Helper method to print product details.
    private static void printProductDetails(Collection<Product> items) {
        System.out.println("ProductID, Name, Price, Category");
        if (items != null && !items.isEmpty()) {
            for (Product product : items) {
                System.out.println("- " + product.getProductID() + ", " + product.getName() + ", " + product.getPrice() + ", " + product.getCategory());
            }
        } else {
            System.out.println("No items found...");
        }
        System.out.println();
    }

    // Helper method to create a list of Product objects.
    private  static List<Product> getProducts()
    {
        ArrayList<Product> products = new ArrayList<>();
        products.add(new Product(14001, "Laptop", 2000, "Electronics"));
        products.add(new Product(14002, "Kindle", 1000, "Electronics"));
        products.add(new Product(14003, "Book", 100, "Stationery"));
        products.add(new Product(14004, "Smart Phone", 1000, "Electronics"));
        products.add(new Product(14005, "Mobile Phone", 200, "Electronics"));
        return products;
    }
}

// Sample class for Product
@jakarta.persistence.Entity
@jakarta.persistence.Cacheable
@Cache(usage = CacheConcurrencyStrategy.READ_WRITE, region = "CustomersCache")
class Product implements Serializable {
    @jakarta.persistence.Id
    private int productID;
    @jakarta.persistence.Column
    private String name;
    @jakarta.persistence.Column
    private double price;
    @Column
    private String category;

    public Product() {
    }

    public Product(int productID, String name, double price, String category) {
        this.productID = productID;
        this.name = name;
        this.price = price;
        this.category = category;
    }

    public int getProductID() { return productID; }
    public String getName() { return name; }
    public double getPrice() { return price; }
    public String getCategory() { return category; }

}

//<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
//<configuration>
//    <application-config application-id="myapp1" enable-cache-exception="true" default-region-name="DefaultRegion" key-case-sensitivity="false">
//        <cache-regions>
//            <region name="com.alachisoft.ncache.samples.Product:Product" cache-name="demoCache" priority="BelowNormal" expiration-type="Absolute" expiration-period="8"/>
//            <region name="CustomRegion" cache-name="demoCache" priority="default" expiration-type="sliding" expiration-period="8"/>
//            <region name="DefaultRegion" cache-name="demoCache" priority="default" expiration-type="none" expiration-period="0"/>
//            <region name="default-update-timestamps-region" cache-name="demoCache" priority="default" expiration-type="none" expiration-period="0"/>
//            <region name="default-query-results-region" cache-name="demoCache" priority="default" expiration-type="none" expiration-period="0"/>
//        </cache-regions>
//    </application-config>
//</configuration>
