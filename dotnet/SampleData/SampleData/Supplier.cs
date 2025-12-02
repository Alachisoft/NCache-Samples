// ===============================================================================
// Alachisoft (R) NCache Sample Code
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

namespace Alachisoft.NCache.Sample.Data
{
    /// <summary>
    /// Model class for suppliers
    /// </summary>
    [Serializable]
    public class Supplier
    {
        /// <summary>
        /// Unique SupplierID assigned to each supplier
        /// </summary>
        public int SupplierID { get; set; }

        /// <summary>
        /// Name of the supplier company
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Contact name of the supplier
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Address of the supplier
        /// </summary>
        public string Address { get; set; }

    }
}
