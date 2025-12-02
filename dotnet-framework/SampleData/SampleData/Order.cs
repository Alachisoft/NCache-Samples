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
        /// Unique OrderID assigned to each order
        /// </summary>
        public int OrderID { set; get; }

        /// <summary>
        /// The time when order was made
        /// </summary>
        public DateTime OrderDate { set; get; }

        /// <summary>
        /// Name of the person whom order is to be delivered
        /// </summary>
        public string ShipName { set; get; }

        /// <summary>
        /// The address where order is to be delivered
        /// </summary>
        public string ShipAddress { set; get; }

        /// <summary>
        /// City where order is to be delivered
        /// </summary>
        public string ShipCity { set; get; }

        /// <summary>
        /// Country where order is to be delivered
        /// </summary>
        public string ShipCountry { set; get; }

        /// <summary>
        /// Generates an order of specified type with current date
        /// </summary>
        public static T GenerateOrder<T>() where T : Order, new()
        {
            T order = new T();
            order.OrderDate = DateTime.Now;
            return order;
        }
    }

    [Serializable]
    public class ElectronicsOrder : Order
    {
        public ElectronicsOrder()
        {
            OrderID = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
        }
    }
    [Serializable]
    public class GarmentsOrder : Order
    {
        public GarmentsOrder()
        {
            OrderID = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
        }
    }

}
