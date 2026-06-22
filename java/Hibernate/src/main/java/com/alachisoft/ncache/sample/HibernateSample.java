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

package com.alachisoft.ncache.sample;

import java.util.List;

import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.cfg.Configuration;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;

import nhibernatesample.entities.Customer;
import nhibernatesample.entities.Order;
import nhibernatesample.entities.Product;
import nhibernatesample.entities.Supplier;

/*
 * ===============================================================================
 * Hibernate is a mature and widely used ORM for Java applications. NCache
 * provides seamless integration with Hibernate, enabling developers to leverage
 * distributed caching capabilities within their Hibernate applications. This
 * integration automatically improves performance without major code changes.
 * It supports both HQL and Criteria queries, offering configurable options
 * like expiration, cache regions, and lock modes to ensure data consistency.
 * Some use cases for caching with Hibernate using NCache include:
 *   1. Caching frequently accessed entities, such as customers or products, to
 *      reduce database load and improve response times.
 *   2. Caching collections, such as orders per customer or product lists, to
 *      minimize repeated queries.
 *   3. Caching lookup or reference data to avoid repeated reads from the database.
 *   4. Caching results of complex or expensive queries, including HQL and LINQ
 *      queries, to enhance application performance.
 *   5. Caching reports, dashboards, or analytics data to speed up retrieval time.
 * ===============================================================================
 */

/**
 * Main application class for running NHibernate + NCache sample in Java
 *
 */
public class HibernateSample {

    /**
     * Provides a static instance of the session factory used to create sessions.
     *
     */
    private static SessionFactory sessionFactory;

    /**
     * Main method - Entry point of the application.
     *
     * @param args Arguments for main method
     */
    public static void main(String[] args) throws Exception {

        // Fetching connection string from hibernate.cfg.xml
        Configuration cfg = new Configuration();
        cfg.configure(); // loads hibernate.cfg.xml

        // Adding annotated entity classes or mapping files
        cfg.addAnnotatedClass(Customer.class);
        cfg.addAnnotatedClass(Order.class);
        cfg.addAnnotatedClass(Supplier.class);
        cfg.addAnnotatedClass(Product.class);

        // NCache second-level cache is enabled through hibernate.cfg.xml

        // Building session factory
        sessionFactory = cfg.buildSessionFactory();

        System.out.println("Hibernate + NCache initialized.\n");

        // Opening a session
        Session session = sessionFactory.openSession();

        // Fetching Customer (ALFKI) asynchronously from database and storing in cache
        getCustomer(session, "ALFKI");

        // Fetching Order via Customer (ALFKI) from database and storing in cache
        getCustomerOrders(session, "ALFKI");

        // Fetching all Suppliers and Products from database and storing in cache
        FetchAllSuppliersAndProducts(session);

        // Fetching all Suppliers from New York city from cache
        getSuppliers(session, "New York");

        // Creating another New York city Supplier
        Supplier supplier = new Supplier();
        supplier.setCompanyName("New Supplier");
        supplier.setContactName("Alice Smith");
        supplier.setContactTitle("Sales Manager");
        supplier.setAddress("456 Elm St");
        supplier.setCity("New York");
        supplier.setRegion("IL");
        supplier.setPostalCode("60601");
        supplier.setCountry("USA");
        supplier.setPhone("312-555-1234");
        supplier.setFax("312-555-5678");
        supplier.setHomePage("http://www.globalsupplies.com");

        // Adding supplier to database and cache
        addSupplier(session, supplier);

        // Fetching all Suppliers from New York city from cache again
        getSuppliers(session, "New York");

        // Fetching available Products
        getAvailableProducts(session);

        // Creating new available products
        Product product = new Product();
        product.setProductName("New Product");
        product.setSupplierID(1L);
        product.setCategoryID(null);
        product.setQuantityPerUnit("10");
        product.setUnitPrice(99.99);
        product.setUnitsInStock((short) 100);
        product.setUnitsOnOrder((short) 0);
        product.setReorderLevel((short) 10);
        product.setDiscontinued(false);

        // Adding product to database and cache
        addProduct(session, product);

        // Fetching available Products from cache again
        getAvailableProducts(session);
Thread.sleep(5000);
        // Evicting all cached items
        sessionFactory.getCache().evictEntityData(Order.class);
        sessionFactory.getCache().evictEntityData(Customer.class);
        sessionFactory.getCache().evictEntityData(Supplier.class);
        sessionFactory.getCache().evictEntityData(Product.class);
        sessionFactory.getCache().evictQueryRegions();

        // Disposing session
        session.close();
        sessionFactory.close();
    }

    /**
     * Method to retrieve Suppliers and Products from database and cache them
     *
     * @param session Session instance on which to operate
     */
    static void FetchAllSuppliersAndProducts(Session session) {

        CriteriaBuilder cb = session.getCriteriaBuilder();

        // Fetching suppliers from database via hibernate
        CriteriaQuery<Supplier> cqSup = cb.createQuery(Supplier.class);
        Root<Supplier> rootSup = cqSup.from(Supplier.class);
        cqSup.select(rootSup);
        List<Supplier> suppliers = session.createQuery(cqSup)
                .setCacheable(true)
                .getResultList();

        // Fetching products from database via hibernate
        CriteriaQuery<Product> cqProd = cb.createQuery(Product.class);
        Root<Product> rootProd = cqProd.from(Product.class);
        cqProd.select(rootProd);
        List<Product> products = session.createQuery(cqProd)
                .setCacheable(true)
                .getResultList();

        System.out.println("\nSuppliers and products loaded into cache.");
    }

    /**
     * Method to retrieve Customers from data source asynchronously
     *
     * @param session    Session instance on which to operate
     * @param customerId Customer ID by which to search for Customer
     */
    static void getCustomer(Session session, String customerId) {

        CriteriaBuilder cb = session.getCriteriaBuilder();

        // Fetching customer from db via hibernate
        CriteriaQuery<Customer> cq = cb.createQuery(Customer.class);
        Root<Customer> root = cq.from(Customer.class);
        cq.select(root).where(cb.equal(root.get("customerID"), customerId));

        Customer customer = session.createQuery(cq)
                .setCacheable(true)
                .getSingleResult();

        System.out.println("\nCustomer fetched: " + customer.getCustomerID()
                + " - " + customer.getContactName());
    }

    /**
     * Method to retrieve Orders via Customer ID asynchronously
     *
     * @param session    Session instance on which to operate
     * @param customerId Customer ID by which to search for Order
     */
    static void getCustomerOrders(Session session, String customerId) {

        CriteriaBuilder cb = session.getCriteriaBuilder();

        // Fetching customer order from db via hibernate
        CriteriaQuery<Order> cq = cb.createQuery(Order.class);
        Root<Order> root = cq.from(Order.class);
        cq.select(root).where(cb.equal(root.get("customerID"), customerId));

        List<Order> orders = session.createQuery(cq)
                .setCacheable(true)
                .getResultList();

        System.out.println("\nOrders for " + customerId + ":");
        for (Order o : orders) {
            System.out.println("OrderID: " + o.getOrderID() +
                    ", ShipName: " + o.getShipName());
        }
    }

    /**
     * Method to retrieve Suppliers via city asynchronously
     *
     * @param session Session instance on which to operate
     * @param city    City by which to search for Supplier
     */
    static void getSuppliers(Session session, String city) {

        CriteriaBuilder cb = session.getCriteriaBuilder();

        // Fetching all suppliers via city from cache via hibernate
        // (all suppliers were loaded into cache earlier)
        CriteriaQuery<Supplier> cq = cb.createQuery(Supplier.class);
        Root<Supplier> root = cq.from(Supplier.class);
        cq.select(root).where(cb.equal(root.get("city"), city));

        List<Supplier> suppliers = session.createQuery(cq)
                .setCacheable(true)
                .getResultList();

        System.out.println("\nSuppliers fetched:");
        for (Supplier s : suppliers) {
            System.out.printf("%-5s %-40s %s\n",
                    s.getSupplierID(), s.getCompanyName(), s.getPhone());
        }
    }

    /**
     * Method to retrieve available Products asynchronously
     *
     * @param session Session instance on which to operate
     */
    static void getAvailableProducts(Session session) {

        CriteriaBuilder cb = session.getCriteriaBuilder();

        // Fetching all available products from cache via hibernate
        // (all products were loaded into cache earlier)
        CriteriaQuery<Product> cq = cb.createQuery(Product.class);
        Root<Product> root = cq.from(Product.class);
        cq.select(root).where(cb.equal(root.get("discontinued"), false));

        List<Product> available = session.createQuery(cq)
                .setCacheable(true)
                .getResultList();

        System.out.println("\nAvailable Products:");
        for (Product p : available) {
            System.out.printf("%-5s %-40s %s\n",
                    p.getProductID(), p.getProductName(), p.getQuantityPerUnit());
        }
    }

    /**
     * Method to add Product to database and cache
     *
     * @param session Session instance on which to operate
     * @param product Product instance to save to cache and database
     */
    static void addProduct(Session session, Product product) {

        // Adding product into cache and db via hibernate
        session.beginTransaction();
        session.persist(product);
        session.getTransaction().commit();

        System.out.println("\nProduct added: " +
                product.getProductID() + " - " + product.getProductName());
    }

    /**
     * Method to add Supplier to database and cache.
     *
     * @param session  ISession instance on which to operate
     * @param supplier Supplier instance to save to cache and database
     */
    static void addSupplier(Session session, Supplier supplier) {

        // Adding supplier into cache and db via hibernate
        session.beginTransaction();
        session.persist(supplier);
        session.getTransaction().commit();

        System.out.println("\nSupplier added: " +
                supplier.getSupplierID() + " - " + supplier.getCompanyName());
    }
}
