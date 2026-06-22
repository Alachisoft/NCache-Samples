using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ResponseCaching.Models;

/// <summary>
/// Represents the database context for the application.
/// This class manages the connection to the database and provides access to entity sets (tables) 
/// It inherits from <see cref="DbContext"/> to enable Entity Framework Core operations such as querying and saving data.
/// </summary>
public partial class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    public AppDbContext()
    {
    }

    /// -------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class with specified options.
    /// </summary>
    /// <param name="options">Configuration options for the context, such as the database provider and connection string.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// -------------------------------------------------------------------------------
    /// <summary>
    /// Represents the <c>Products</c> table in the database.
    /// </summary>
    public virtual DbSet<Product> Products { get; set; }


}   
