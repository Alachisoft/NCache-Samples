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
    /// Product class definition
    /// </summary>
    public class Product
    {
        public virtual long ProductID { get; set; }
        public virtual string ProductName { get; set; }
        public virtual long SupplierID { get; set; }
        public virtual long? CategoryID { get; set; }
        public virtual string QuantityPerUnit { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual short UnitsInStock { get; set; }
        public virtual short UnitsOnOrder { get; set; }
        public virtual short ReorderLevel { get; set; }
        public virtual bool Discontinued { get; set; }
        public virtual DateTime LastModify { get; set; }
    }
}
