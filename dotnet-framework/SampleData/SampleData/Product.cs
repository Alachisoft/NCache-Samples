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
    /// Model class for products
    /// </summary>
    [Serializable]
    public class Product
    {
        /// <summary>
        /// Unique ProductID assigned to each product
        /// </summary>
        public int ProductID { set; get; }

        /// <summary>
        /// Price of one unit of this product
        /// </summary>
        public Decimal UnitPrice { set; get; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string ProductName { set; get; }

        public string ClassName { set; get; }


        /// <summary>
        /// Quantity per unit of the product
        /// </summary>
        public string QuantityPerUnit { set; get; }

        /// <summary>
        /// Unit in stock of the product
        /// </summary>
        public short UnitsInStock { set; get; }

        /// <summary>
        /// Units available of the product
        /// </summary>
        public int UnitsAvailable { set; get; }

        /// <summary>
        /// Category of the product
        /// </summary>
        public string Category { set; get; }

        /// <summary>
        /// This method returns the information about this product in string format.
        /// </summary>
        /// <returns> Returns the information about this product. </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("Name ");
            builder.Append(ProductName);
            builder.Append(", Quantity/Unit ");
            builder.Append(QuantityPerUnit);
            builder.Append(", UnitPrice ");
            builder.Append(UnitPrice);
            builder.Append(", UnitsInStock ");
            builder.Append(UnitsInStock);
            builder.Append("]");
            return builder.ToString();
        }
    }
}