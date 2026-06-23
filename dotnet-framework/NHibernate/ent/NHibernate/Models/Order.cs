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

namespace NHibernateSample
{
    /// <summary>
    /// Order class definition
    /// </summary>
    public class Order
    {
        public virtual long OrderID { get; set; }
        public virtual string CustomerID { get; set; }
        public virtual long? EmployeeID { get; set; }
        public virtual DateTime? OrderDate { get; set; }
        public virtual DateTime? RequiredDate { get; set; }
        public virtual DateTime? ShippedDate { get; set; }
        public virtual int? ShipVia { get; set; }
        public virtual decimal Freight { get; set; }
        public virtual string ShipName { get; set; }
        public virtual string ShipAddress { get; set; }
        public virtual string ShipCity { get; set; }
        public virtual string ShipRegion { get; set; }
        public virtual string ShipPostalCode { get; set; }
        public virtual string ShipCountry { get; set; }
        public virtual DateTime LastModify { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
