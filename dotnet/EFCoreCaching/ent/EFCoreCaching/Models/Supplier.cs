using System;
using System.Collections.Generic;

namespace EFCoreCaching.Models;

/// <summary>
/// Represents a supplier that provides products in the Northwind database.
/// </summary>
public partial class Supplier
{
    /// <summary>
    /// Gets or sets the unique identifier for the supplier.
    /// </summary>
    public int SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the name of the supplier company.
    /// </summary>
    public string CompanyName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the contact person's name at the supplier company.
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// Gets or sets the title of the contact person.
    /// </summary>
    public string? ContactTitle { get; set; }

    /// <summary>
    /// Gets or sets the supplier's street address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the city where the supplier is located.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets the region or state of the supplier.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Gets or sets the postal code of the supplier's address.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the country where the supplier is based.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Gets or sets the supplier's phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the supplier's fax number.
    /// </summary>
    public string? Fax { get; set; }

    /// <summary>
    /// Gets or sets the supplier's homepage or website URL.
    /// </summary>
    public string? HomePage { get; set; }

    /// <summary>
    /// Navigation property — collection of products supplied by this supplier.
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
