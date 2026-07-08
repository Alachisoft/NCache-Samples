using System;
using System.Collections.Generic;

namespace EFCoreCaching.Models;

/// <summary>
/// Represents a customer order in the Northwind database.
/// </summary>
public partial class Order
{
    /// <summary>
    /// Gets or sets the unique identifier for the order.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the customer who placed the order.
    /// </summary>
    public string? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the employee who processed the order.
    /// </summary>
    public int? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the date when the order was placed.
    /// </summary>
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// Gets or sets the date by which the order is required to be delivered.
    /// </summary>
    public DateTime? RequiredDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the order was shipped.
    /// </summary>
    public DateTime? ShippedDate { get; set; }

    /// <summary>
    /// Gets or sets the shipping method identifier.
    /// </summary>
    public int? ShipVia { get; set; }

    /// <summary>
    /// Gets or sets the freight cost for shipping the order.
    /// </summary>
    public decimal? Freight { get; set; }

    /// <summary>
    /// Gets or sets the name of the recipient or shipping company.
    /// </summary>
    public string? ShipName { get; set; }

    /// <summary>
    /// Gets or sets the street address where the order is shipped.
    /// </summary>
    public string? ShipAddress { get; set; }

    /// <summary>
    /// Gets or sets the city where the order is shipped.
    /// </summary>
    public string? ShipCity { get; set; }

    /// <summary>
    /// Gets or sets the region or state where the order is shipped.
    /// </summary>
    public string? ShipRegion { get; set; }

    /// <summary>
    /// Gets or sets the postal code for the shipping address.
    /// </summary>
    public string? ShipPostalCode { get; set; }

    /// <summary>
    /// Gets or sets the country where the order is shipped.
    /// </summary>
    public string? ShipCountry { get; set; }

    /// <summary>
    /// Navigation property — the customer associated with this order.
    /// </summary>
    public virtual Customer? Customer { get; set; }
}
