using System;
using System.Collections.Generic;

namespace EFCoreCaching.Models;

/// <summary>
/// Represents a customer in the Northwind database.
/// </summary>
public partial class Customer
{
    /// <summary>
    /// Gets or sets the unique identifier for the customer.
    /// </summary>
    public string CustomerId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the company associated with the customer.
    /// </summary>
    public string CompanyName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the contact person's full name.
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// Gets or sets the job title of the contact person.
    /// </summary>
    public string? ContactTitle { get; set; }

    /// <summary>
    /// Gets or sets the street address of the customer.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the city where the customer is located.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets the region or state of the customer.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Gets or sets the postal code of the customer.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the country where the customer resides.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Gets or sets the primary phone number of the customer.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the fax number of the customer.
    /// </summary>
    public string? Fax { get; set; }

    /// <summary>
    /// Navigation property — collection of orders associated with this customer.
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
