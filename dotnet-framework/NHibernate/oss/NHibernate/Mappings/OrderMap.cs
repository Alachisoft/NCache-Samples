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
    /// Class to help NHibernate map Order to database Orders table
    /// </summary>
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            // Setting database table name
            Table("Orders");

            // Setting cache region
            Cache.ReadWrite().Region("default");

            // Mapping primary key
            Id(x => x.OrderID)
                .Column("OrderID")
                .CustomType("Int64")
                .GeneratedBy.Identity();

            // Mapping properties
            Map(x => x.CustomerID).Column("CustomerID");
            Map(x => x.EmployeeID).Column("EmployeeID");
            Map(x => x.OrderDate).Column("OrderDate");
            Map(x => x.RequiredDate).Column("RequiredDate");
            Map(x => x.ShippedDate).Column("ShippedDate");
            Map(x => x.ShipVia).Column("ShipVia");
            Map(x => x.Freight).Column("Freight");
            Map(x => x.ShipName).Column("ShipName");
            Map(x => x.ShipAddress).Column("ShipAddress");
            Map(x => x.ShipCity).Column("ShipCity");
            Map(x => x.ShipRegion).Column("ShipRegion");
            Map(x => x.ShipPostalCode).Column("ShipPostalCode");
            Map(x => x.ShipCountry).Column("ShipCountry");
            Map(x => x.LastModify).Column("LastModify");

            // Many-to-one relationship
            References(x => x.Customer)
                .Class<Customer>()
                .Column("CustomerID")
                .Not.Nullable()
                .Not.Insert()
                .Not.Update();
        }
    }
}
