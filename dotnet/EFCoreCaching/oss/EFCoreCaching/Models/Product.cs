using System;
using System.Collections.Generic;

namespace EFCoreCaching.Models;

/// <summary>
/// Represents a product available in the Northwind database.
/// </summary>
public partial class Product
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string ProductName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the supplier providing the product.
    /// </summary>
    public int? SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the product category.
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the quantity description, such as units per box or package.
    /// </summary>
    public string? QuantityPerUnit { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the number of units currently in stock.
    /// </summary>
    public short? UnitsInStock { get; set; }

    /// <summary>
    /// Gets or sets the number of units currently on order.
    /// </summary>
    public short? UnitsOnOrder { get; set; }

    /// <summary>
    /// Gets or sets the reorder level for the product.
    /// </summary>
    public short? ReorderLevel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is discontinued.
    /// </summary>
    public bool Discontinued { get; set; }

    /// <summary>
    /// Navigation property — the supplier associated with this product.
    /// </summary>
    public virtual Supplier? Supplier { get; set; }
}
