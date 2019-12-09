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
using System.Globalization;
using System.Data.SqlClient;
using Alachisoft.NCache.Runtime.Dependencies;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Implementation of ExtensibleDependency.
    /// </summary>
    [Serializable]
	public class Dependency : ExtensibleDependency
	{
		private string _connString;
		private int _productID;
		
		[NonSerialized]
        SqlConnection _connection;

        /// <summary>
        /// Custom constructor initialize dependency
        /// </summary>
        public Dependency(int productID, string connString)
        {
            _connString = connString;
            _productID = productID;
        }

        /// <summary>
        /// Initialize custom dependency
        /// </summary>
        public override bool Initialize() 
        {
            _connection = new SqlConnection(_connString);
            _connection.Open();

            return true; 
        }

        /// <summary>
        /// This method gets available unit stock from database for specified product
        /// </summary>
        /// <param name="productID"> Product ID to be used to get product quantity </param>
		internal int GetAvailableUnits(int productID)
		{
            if (_connection == null)
            {
                _connection = new SqlConnection(_connString);
                _connection.Open();
            }

			int availableUnits = -1;

            SqlCommand command = _connection.CreateCommand();
			command.CommandText = String.Format(CultureInfo.InvariantCulture, 
				"Select UnitsInStock From Products" +
				" where ProductID = {0}", productID);

			var reader = command.ExecuteReader();
					
			if(reader.Read())
			{
				availableUnits = Convert.ToInt32(reader["UnitsInStock"].ToString());
			}
			reader.Close();
			
			return availableUnits;
	    }

        /// <summary>
        /// This property is used to invalidate a cached item on clean interval
        /// </summary>
        /// <returns> return true to evict this item from cache </returns>
		public override bool HasChanged
		{
			get
			{
                if (GetAvailableUnits(_productID) < 100)
                    return true;
                return false;
			}
		}

        /// <summary>
        /// Dispose custom dependency
        /// </summary>
        protected override void DependencyDispose()
        {
            if (_connection != null)
                _connection.Close();

            base.DependencyDispose();
        }
	}
}
