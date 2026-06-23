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
    /// Class to help NHibernate map Customer to database Customers table
    /// </summary>
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            // Setting database table name
            Table("Customers");

            // Setting cache region
            Cache.NonStrictReadWrite().Region("default");

            // Mapping primary key
            Id(x => x.CustomerID)
                .Column("CustomerID")
                .CustomType("String")
                .GeneratedBy.Assigned();

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
            Map(x => x.LastModify).Column("LastModify");
        }
    }
}
