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
using FluentNHibernate.Mapping;

namespace NHibernateSample.Mappings
{
    /// <summary>
    /// Class to help NHibernate map Product to database Products table
    /// </summary>
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            // Setting database table name
            Table("Products");

            // Setting cache region
            Cache.ReadWrite().Region("default");

            // Mapping primary key
            Id(x => x.ProductID)
                .Column("ProductID")
                .CustomType("Int64")
                .GeneratedBy.Identity();

            // Mapping properties
            Map(x => x.ProductName).Column("ProductName");
            Map(x => x.SupplierID).Column("SupplierID");
            Map(x => x.CategoryID).Column("CategoryID");
            Map(x => x.QuantityPerUnit).Column("QuantityPerUnit");
            Map(x => x.UnitPrice).Column("UnitPrice");
            Map(x => x.UnitsInStock).Column("UnitsInStock");
            Map(x => x.UnitsOnOrder).Column("UnitsOnOrder");
            Map(x => x.ReorderLevel).Column("ReorderLevel");
            Map(x => x.Discontinued).Column("Discontinued");
            Map(x => x.LastModify).Column("LastModify");
        }
    }
}
