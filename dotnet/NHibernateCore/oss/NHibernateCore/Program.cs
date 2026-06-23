// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Threading.Tasks;
using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Criterion;
using NHibernateSample.Mappings;
using Alachisoft.NCache.Client;

// ===============================================================================
// NHibernate is a mature and widely used ORM for .NET applications. NCache
// provides seamless integration with NHibernate, enabling developers to leverage
// distributed caching capabilities within their NHibernate applications. This
// integration automatically improves performance without major code changes.
// It supports both HQL and Criteria queries, as well as synchronous and
// asynchronous operations, offering configurable options like expiration, cache
// regions, and lock modes to ensure data consistency. Some use cases for caching
// with NHibernate using NCache include:
//   1. Caching frequently accessed entities, such as customers or products, to
//      reduce database load and improve response times.
//   2. Caching collections, such as orders per customer or product lists, to
//      minimize repeated queries.
//   3. Caching lookup or reference data to avoid repeated reads from the database.
//   4. Caching results of complex or expensive queries, including HQL and LINQ
//      queries, to enhance application performance.
//   5. Caching reports, dashboards, or analytics data to speed up retrieval time.
// ===============================================================================

namespace NHibernateSample
{
    public class Program
    {
        /// <summary>
        /// Provides a static instance of the session factory used to create sessions.
        /// </summary>
        private static ISessionFactory _sessionFactory;

        /// <summary>
        /// Main method - Entry point of the application 
        /// </summary>
        /// <param name="args">Arguments for main method</param>
        static async Task Main(string[] args)
        {
            // Fetching connection string from App.Config
            string conn_str = ConfigurationManager.AppSettings["ConnectionString"];

            // Configuring NHibernate with database details
            _sessionFactory = Fluently.Configure().Database(
               MsSqlConfiguration.MsSql2008
                   .ConnectionString(conn_str) // Passing connection string
                   .Dialect<NHibernate.Dialect.MsSql2008Dialect>()
            )
            // Adding Mappings for entities
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CustomerMap>())
            // Enabling Second Level Cache with NCache
            .ExposeConfiguration(cfg =>
            {
               // Enabling NCache second-level cache (configuration is fetched from hibernate.cfg.xml)
               cfg.Configure();
            })
            // Building session factory
            .BuildSessionFactory();

            Console.WriteLine("Fluent NHibernate + NCache initialized.\n");

            // Opening a session
            var session = _sessionFactory.OpenSession();

            Console.WriteLine("Lock test");
            session.Lock(new Supplier { SupplierID = 1 }, null);

            // Fetching Customer (ALFKI) asynchronously from database and storing in cache
            await GetCustomerAsync(session, "ALFKI");

            // Fetching Order via Customer (ALFKI) from database and storing in cache
            await GetCustomerOrdersAsync(session, "ALFKI");

            // Fetching all Suppliers and Products from database and storing in cache
            await FetchAllSuppliersAndProductsAsync(session);

            // Fetching all Suppliers from New York city from cache
            await GetSuppliersAsync(session, "New York");

            Console.WriteLine("Fetch again");

            // Fetching all Suppliers from New York city from cache
            await GetSuppliersAsync(session, "New York");

            // Creating another New York city Supplier
            Supplier supplier = new Supplier
            {
                CompanyName = "New Supplier",
                ContactName = "Alice Smith",
                ContactTitle = "Sales Manager",
                Address = "456 Elm St",
                City = "New York",
                Region = "IL",
                PostalCode = "60601",
                Country = "USA",
                Phone = "312-555-1234",
                Fax = "312-555-5678",
                HomePage = "http://www.globalsupplies.com",
                LastModify = DateTime.UtcNow
            };

            // Adding supplier to database and cache
            //AddSupplier(session, supplier);

            //// Fetching all Suppliers from New York city from cache again
            //await GetSuppliersAsync(session, "New York");

            // Fetching available Products from cache
            await GetAvailableProducts(session);

            // Creating new available products
            Product product = new Product
            {
                ProductName = "New Product",
                SupplierID = 1, // Make sure this supplier exists
                CategoryID = null,
                QuantityPerUnit = "10",
                UnitPrice = 99.99,
                UnitsInStock = 100,
                UnitsOnOrder = 0,
                ReorderLevel = 10,
                Discontinued = false,
                LastModify = DateTime.UtcNow
            };

            // Adding product to database and cache
            AddProduct(session, product);

            // Fetching available Products from cache again
            await GetAvailableProducts(session);

            // Waiting for user input before evicting cached items
            Console.WriteLine("\nPress Enter to evict all cached items and exit...");
            Console.Read();

            // Evicting all cached items
            _sessionFactory.Evict(typeof(Order));
            _sessionFactory.Evict(typeof(Customer));
            _sessionFactory.Evict(typeof(Supplier));
            _sessionFactory.Evict(typeof(Product));
            _sessionFactory.EvictQueries();

            // Disposing session
            session.Dispose();
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to retrieve Suppliers and Products from database and cache them
        /// </summary>
        /// <param name="session">ISession instance on which to operate</param>
        /// <returns>Task instance</returns>
        static async Task FetchAllSuppliersAndProductsAsync(ISession session)
        {
            // Fetching suppliers from database via nhibernate
            var suppliers = session.CreateCriteria<Supplier>()
                .SetCacheable(true)
                .List<Supplier>();

            // Fetching products from database via nhibernate
            var products = await session.CreateCriteria<Product>()
                .SetCacheable(true)
                .ListAsync<Product>();

          

            Console.WriteLine("\nSuppliers and products loaded into cache.");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to retrieve Customers from data source asynchronously
        /// </summary>
        /// <param name="session">ISession instance on which to operate</param>
        /// <param name="customerId">Customer ID by which to search for Customer</param>
        /// <returns>Task instance</returns>
        static async Task GetCustomerAsync(ISession session, string customerId)
        {
            // Retrieving Customer via ID
            // NOTE: We can also use UniqueResult for synchronous operation
            var customer = await session.CreateCriteria<Customer>()
                .Add(Restrictions.Eq("CustomerID", customerId))
                .SetCacheable(true)
                .UniqueResultAsync<Customer>();

            // Printing fetched customer
            Console.WriteLine($"\nCustomer fetched: {customer.CustomerID} - {customer.ContactName}");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to retrieve Orders via Customer ID asynchronously
        /// </summary>
        /// <param name="session">ISession instance on which to operate</param>
        /// <param name="customerId">Customer ID by which to search for Order</param>
        /// <returns>Task instance</returns>
        static async Task GetCustomerOrdersAsync(ISession session, string customerId)
        {
            // Retrieving Order via CustomerID
            // NOTE: We can also use List for synchronous operation
            var orders = await session.CreateCriteria<Order>()
                .Add(Restrictions.Eq("CustomerID", customerId))
                .SetCacheable(true)
                .ListAsync<Order>();

            // Printing all fetched Orders
            Console.WriteLine($"\nOrders for {customerId}:");
            foreach (var o in orders)
                Console.WriteLine($"OrderID: {o.OrderID}, ShipName: {o.ShipName}, Date: {o.OrderDate}");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to retrieve Suppliers via city asynchronously
        /// </summary>
        /// <param name="session">ISession instance on which to operate</param>
        /// <param name="city">City by which to search for Supplier</param>
        /// <returns>Task instance</returns>
        static async Task GetSuppliersAsync(ISession session, string city)
        {
            // Fetching Supplier via city
            // NOTE: We can also use List for synchronous operation
            
            
            var suppliers = await session.CreateCriteria<Supplier>()
                .Add(Restrictions.Eq("City", city))
                .SetCacheable(true)
                .ListAsync<Supplier>();

            // Printing all fetched Suppliers
            Console.WriteLine("\nSuppliers fetched:");
            foreach (var s in suppliers)
                Console.WriteLine($"{s.SupplierID,-5} {s.CompanyName,-40} {s.Phone}");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to retrieve available Products asynchronously
        /// </summary>
        /// <param name="session">ISession instance on which to operate</param>
        /// <returns>Task instance</returns>
        static async Task GetAvailableProducts(ISession session)
        {
            // Fetching all available products
            // NOTE: We can also use List for synchronous operation
            var available = await session.CreateCriteria<Product>()
                .Add(Restrictions.Eq("Discontinued", false))
                .SetCacheable(true)
                .ListAsync<Product>();

            // Printing all fetched products
            Console.WriteLine("\nAvailable Products:");
            foreach (var p in available)
                Console.WriteLine($"{p.ProductID,-5} {p.ProductName,-40} {p.QuantityPerUnit}");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to add Product to database and cache
        /// </summary>
        /// <param name="session">ISession instance on which to operate</param>
        /// <param name="product">Product instance to save to cache and database</param>
        static void AddProduct(ISession session, Product product)
        {
            // Saving product to data sources
            session.Save(product);

            // Committing transaction
            session.Flush();

            Console.WriteLine($"\nProduct added: {product.ProductID} - {product.ProductName}");
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to add Supplier to database and cache
        /// </summary>
        /// <param name="session">ISession instance on which to operate</param>
        /// <param name="supplier">Supplier instance to save to cache and database</param>
        static void AddSupplier(ISession session, Supplier supplier)
        {
            // Saving supplier to data sources
            session.Save(supplier);

            // Committing transaction
            session.Flush();

            Console.WriteLine($"\nSupplier added: {supplier.SupplierID} - {supplier.CompanyName}");
        }
    }
}
