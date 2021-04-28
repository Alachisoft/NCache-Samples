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
namespace Alachisoft.NCache.Sample.Data
{
    /// <summary>
    /// Model class for orders
    /// </summary>
    [Serializable]
    public class Order
    {
        /// <summary>
        /// Unique Id of the customer
        /// </summary>
        public int OrderID
        {
            get;
            set;
        }
        /// <summary>
        /// Unique Id of the customer
        /// </summary>
        public string CustomerID
        {
            get;
            set;
        }

        /// <summary>
        /// Unique Id of the customer
        /// </summary>
        public string Address
        {
            get;
            set;
        }

    }
}
