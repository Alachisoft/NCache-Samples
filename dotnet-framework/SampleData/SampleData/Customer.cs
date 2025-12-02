// ===============================================================================
// Alachisoft (R) NCache Sample Code
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Text;

namespace Alachisoft.NCache.Sample.Data
{
    /// <summary>
    /// Model class for Customers
    /// </summary>
    [Serializable]
    public class Customer
    {
        /// <summary>
        /// Unique CustomerID of the customer
        /// </summary>
        public string CustomerID { set; get; }

        /// <summary>
        /// Contact name of the customer
        /// </summary>
        public string ContactName { set; get; }

        /// <summary>
        /// Company the customer works for
        /// </summary>
        public string CompanyName { set; get; }

        /// <summary>
        /// Contact number of the customer
        /// </summary>
        public string ContactNo { set; get; }

        /// <summary>
        /// Residential address of the customer
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// Residence city of the customer
        /// </summary>
        public string City { set; get; }

        /// <summary>
        /// Nationality of the customer
        /// </summary>
        public string Country { set; get; }

        /// <summary>
        /// Postal code of the customer
        /// </summary>
        public string PostalCode { set; get; }

        /// <summary>
        /// Fax number of the customer
        /// </summary>
        public string Fax { set; get; }
    }
}
