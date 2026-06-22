using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using EFCoreCaching.Models;
using Alachisoft.NCache.EntityFrameworkCore;

namespace EFCoreCaching;

/// <summary>
/// Represents the Entity Framework Core DbContext for the Northwind database.
/// Integrates with NCache to enable distributed caching for entities.
/// </summary>
public partial class NorthwindContext : DbContext
{
    /// <summary>
    /// Default constructor used when no external options are provided.
    /// </summary>
    public NorthwindContext() { }

    /// -------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of NorthwindContext using the specified options.
    /// </summary>
    /// <param name="options">The configuration options for this DbContext.</param>
    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options) { }

    // DbSets — Map entity classes to database tables
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }

    /// -------------------------------------------------------------------------------
    /// <summary>
    /// Configures the EF Core context to use SQL Server and NCache.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Retrieve connection string from App.config
            string? connString = ConfigurationManager.AppSettings["ConnString"];
            if (string.IsNullOrEmpty(connString))
                throw new InvalidOperationException("Connection string 'ConnString' not found in app.config.");

            // Retrieve NCache cache name from App.config
            string? cacheName = ConfigurationManager.AppSettings["CacheName"];
            if (string.IsNullOrEmpty(cacheName))
                throw new InvalidOperationException("Cache name 'CacheName' not found in app.config.");

            // Configure NCache integration for EF Core
            NCacheConfiguration.Configure(cacheName, DependencyType.SqlServer);

            // Configure SQL Server as EF Core provider
            optionsBuilder.UseSqlServer(connString);
        }
    }

    /// -------------------------------------------------------------------------------
    /// <summary>
    /// Configures the schema for the Northwind database entities.
    /// </summary>
    /// <param name="modelBuilder">Model builder used to configure entity mappings.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Customer entity configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.City, "City");
            entity.HasIndex(e => e.CompanyName, "CompanyName");
            entity.HasIndex(e => e.PostalCode, "PostalCode");
            entity.HasIndex(e => e.Region, "Region");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("CustomerID");

            entity.Property(e => e.Address).HasMaxLength(60);
            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.CompanyName).HasMaxLength(40);
            entity.Property(e => e.ContactName).HasMaxLength(30);
            entity.Property(e => e.ContactTitle).HasMaxLength(30);
            entity.Property(e => e.Country).HasMaxLength(15);
            entity.Property(e => e.Fax).HasMaxLength(24);
            entity.Property(e => e.Phone).HasMaxLength(24);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Region).HasMaxLength(15);
        });

        // Order entity configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "CustomerID");
            entity.HasIndex(e => e.CustomerId, "CustomersOrders");
            entity.HasIndex(e => e.EmployeeId, "EmployeeID");
            entity.HasIndex(e => e.EmployeeId, "EmployeesOrders");
            entity.HasIndex(e => e.OrderDate, "OrderDate");
            entity.HasIndex(e => e.ShipPostalCode, "ShipPostalCode");
            entity.HasIndex(e => e.ShippedDate, "ShippedDate");
            entity.HasIndex(e => e.ShipVia, "ShippersOrders");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Freight).HasDefaultValue(0m).HasColumnType("money");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.RequiredDate).HasColumnType("datetime");
            entity.Property(e => e.ShipAddress).HasMaxLength(60);
            entity.Property(e => e.ShipCity).HasMaxLength(15);
            entity.Property(e => e.ShipCountry).HasMaxLength(15);
            entity.Property(e => e.ShipName).HasMaxLength(40);
            entity.Property(e => e.ShipPostalCode).HasMaxLength(10);
            entity.Property(e => e.ShipRegion).HasMaxLength(15);
            entity.Property(e => e.ShippedDate).HasColumnType("datetime");

            // One-to-many: Customer → Orders
            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customers");
        });

        // Product entity configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "CategoriesProducts");
            entity.HasIndex(e => e.CategoryId, "CategoryID");
            entity.HasIndex(e => e.ProductName, "ProductName");
            entity.HasIndex(e => e.SupplierId, "SupplierID");
            entity.HasIndex(e => e.SupplierId, "SuppliersProducts");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProductName).HasMaxLength(40);
            entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
            entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.UnitPrice).HasDefaultValue(0m).HasColumnType("money");
            entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
            entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);

            // One-to-many: Supplier → Products
            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Products_Suppliers");
        });

        // Supplier entity configuration
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasIndex(e => e.CompanyName, "CompanyName");
            entity.HasIndex(e => e.PostalCode, "PostalCode");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.Address).HasMaxLength(60);
            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.CompanyName).HasMaxLength(40);
            entity.Property(e => e.ContactName).HasMaxLength(30);
            entity.Property(e => e.ContactTitle).HasMaxLength(30);
            entity.Property(e => e.Country).HasMaxLength(15);
            entity.Property(e => e.Fax).HasMaxLength(24);
            entity.Property(e => e.HomePage).HasColumnType("ntext");
            entity.Property(e => e.Phone).HasMaxLength(24);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Region).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    /// -------------------------------------------------------------------------------
    /// <summary>
    /// Allows further customization of model creation in a partial class.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance.</param>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
