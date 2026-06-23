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
    /// Class to help NHibernate map Supplier to database Suppliers table
    /// </summary>
    public class SupplierMap : ClassMap<Supplier>
    {
        public SupplierMap()
        {
            // Setting database table name
            Table("Suppliers");

            // Setting cache region
            Cache.ReadWrite().Region("default");

            // Mapping primary key
            Id(x => x.SupplierID)
                .Column("SupplierID")
                .CustomType("Int64")
                .GeneratedBy.Native();

            // Mapping properties
            Map(x => x.CompanyName).Column("CompanyName");
            Map(x => x.ContactName).Column("ContactName");
            Map(x => x.ContactTitle).Column("ContactTitle");
            Map(x => x.Address).Column("Address");
            Map(x => x.City).Column("City");
            Map(x => x.Region).Column("Region");
            Map(x => x.PostalCode).Column("PostalCode");
            Map(x => x.Country).Column("Country");
            Map(x => x.Phone).Column("Phone");
            Map(x => x.Fax).Column("Fax");
            Map(x => x.HomePage).Column("HomePage");
            Map(x => x.LastModify).Column("LastModify");
        }
    }
}
