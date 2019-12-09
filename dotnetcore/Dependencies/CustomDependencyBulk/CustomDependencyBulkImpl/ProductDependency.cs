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
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Alachisoft.NCache.Runtime.Dependencies;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Implementation of BulkExtensibleDependency.
    /// </summary>
    [Serializable]
    public class ProductDependency : BulkExtensibleDependency
    { 
        private int productId;
        private int unitsInStock;
        private string _connString;

        /// <summary>
        /// Custom constructor initialize dependency
        /// </summary>
        public ProductDependency(string connString, int Id, int units)
        {
            _connString = connString;
            productId = Id;
            unitsInStock = units;
        }

        /// <summary>
        /// Initialize custom dependency
        /// </summary>
        public override bool Initialize()
        { 
            return true;
        }

        ///<summary>
        /// Get units in stock of each product
        /// </summary>
        private Dictionary<int,int> UnitsInStockStatus(List<int> productIds)
        {
            using (var connection = new SqlConnection(_connString))
            {

                var ids =
                productIds.Select(i => i.ToString(CultureInfo.InvariantCulture))
                    .Aggregate((s1, s2) => s1 + ", " + s2);

                string queryString =
                    "select ProductID, UnitsInStock " +
                    "from Products " +
                    $"WHERE ProductID in ({ids})";

                Dictionary<int, int> productsInfo = new Dictionary<int, int>();

                using (var cmd = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            int units = 0;
                            Int32.TryParse(reader["UnitsInStock"].ToString(), out units);
                            productsInfo.Add((int)reader.GetValue(0), units);
                        }
                    }
                }

                return productsInfo;
            }
        }

        /// <summary>
        /// This method is used to invalidate a cached items on clean interval
        /// </summary> 
        public override void EvaluateBulk(IEnumerable<BulkExtensibleDependency> dependencies)
        {
            List<ProductDependency> dependencyList = new List<ProductDependency>();
            foreach (ProductDependency productDependency in dependencies)
            {
                dependencyList.Add(productDependency);
            }

            List<int> productIds = GetDependencyProductIds(dependencyList);
            var productsInfo = UnitsInStockStatus(productIds);

            foreach (ProductDependency productDependency in dependencyList)
            {
                if (productsInfo.ContainsKey(productDependency.productId))
                {
                    if (productsInfo[productDependency.productId] != productDependency.unitsInStock)
                    {
                        productDependency.Expire();
                    }
                }
            }
        }

        /// <summary>
        /// Get list of productID of Products with bulk dependency
        /// </summary>
        /// <param name="dependencies">IEnumerable BulkExtensibleDependency collection</param>
        /// <returns>List of productId Products with bulk dependency</returns>
        private List<int> GetDependencyProductIds(IEnumerable<BulkExtensibleDependency> dependencies)
        {
            List<int> productIds = new List<int>();

            foreach(ProductDependency productDependency in dependencies)
            {
                productIds.Add(productDependency.productId);
            }
            
            return productIds;
        }

        /// <summary>
        /// Dispose custom dependency
        /// </summary>

        protected override void DependencyDispose()
        {
            base.DependencyDispose();
        }

        public override string ToString()
        {
            return$"Product ID: {productId} UnitsinStock: {unitsInStock}";
        }

    }

}
