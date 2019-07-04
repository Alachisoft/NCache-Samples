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
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using Alachisoft.NCache.Sample.Data;

namespace Alachisoft.NCache.Samples.Utility
{
    /// <summary>
    /// Class that helps read and write customer information to SQL Server
    /// datasource.
    /// </summary>
    internal class SqlDatasource
    {
        private SqlConnection _connection;
        private string _connString;

        /// <summary>
        /// Returns the connection string of this datasource.
        /// </summary>
        public string ConnString { get { return _connString; } }

        /// <summary>
        /// Establish connection with the datasource.
        /// </summary>
        /// <param name="connString"></param>
        public void Connect(string connString)
        {
            if (connString != "")
                _connString = connString;
            _connection = new SqlConnection(_connString);
            _connection.Open();
        }

        /// <summary>
        /// Releases the connection.
        /// </summary>
        public void DisConnect()
        {
            if (_connection != null)
                _connection.Close();
        }

        /// <summary>
        /// Loads a <see cref="Customer"/> object from the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object LoadCustomer(Object key)
        {
            object[] customerId = { key };
            SqlDataReader reader = null;

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                "Select CustomerID, ContactName, CompanyName, Address," +
                                "City, Country, PostalCode, Phone, Fax" +
                                " From dbo.Customers where CustomerID = '{0}'",
                                customerId);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Customer objCustomer = new Customer();

                if (!reader.IsDBNull(0))
                {
                    objCustomer.CustomerID = reader.GetString(0) ?? "";
                }
                if (!reader.IsDBNull(1))
                {
                    objCustomer.ContactName = reader.GetString(1) ?? "";
                }
                if (!reader.IsDBNull(2))
                {
                    objCustomer.CompanyName = reader.GetString(2) ?? "";
                }
                if (!reader.IsDBNull(3))
                {
                    objCustomer.Address = reader.GetString(3) ?? "";
                }
                if (!reader.IsDBNull(4))
                {
                    objCustomer.City = reader.GetString(4) ?? "";
                }
                if (!reader.IsDBNull(5))
                {
                    objCustomer.Country = reader.GetString(5) ?? "";
                }
                if (!reader.IsDBNull(6))
                {
                    objCustomer.PostalCode = reader.GetString(6) ?? "";
                }
                if (!reader.IsDBNull(7))
                {
                    objCustomer.ContactNo = reader.GetString(7) ?? "";
                }
                if (!reader.IsDBNull(8))
                {
                    objCustomer.Fax = reader.GetString(8) ?? "";
                }

                reader.Close();
                return objCustomer;
            }
            reader.Close();

            return null;
        }

        /// <summary>
        /// Loads a <see cref="Customer"/> object from the datasource.
        /// </summary>
        /// <param name="requestedCompanyName"></param>
        /// <returns></returns>
        public long GetCustomerCountByCompanyName(Object requestedCompanyName)
        {
            object[] companyName = { requestedCompanyName };
            SqlDataReader reader = null;

            long result = 0;

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                "Select COUNT(CustomerID)" +
                                " From dbo.Customers where CompanyName = '{0}'",
                                companyName);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result = reader.GetInt32(0);
            }
            reader.Close();

            return result;
        }

        /// <summary>
        /// Loads a <see cref="Customer"/> object from the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, object> LoadCustomersByCity(string cityName)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            SqlDataReader reader = null;

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                "Select CustomerID, ContactName, CompanyName, Address," +
                                "City, Country, PostalCode, Phone, Fax" +
                                " From dbo.Customers where City = '{0}'",
                                cityName);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Customer objCustomer = new Customer();

                if (!reader.IsDBNull(0))
                {
                    objCustomer.CustomerID = reader.GetString(0) ?? "";
                }
                if (!reader.IsDBNull(1))
                {
                    objCustomer.ContactName = reader.GetString(1) ?? "";
                }
                if (!reader.IsDBNull(2))
                {
                    objCustomer.CompanyName = reader.GetString(2) ?? "";
                }
                if (!reader.IsDBNull(3))
                {
                    objCustomer.Address = reader.GetString(3) ?? "";
                }
                if (!reader.IsDBNull(4))
                {
                    objCustomer.City = reader.GetString(4) ?? "";
                }
                if (!reader.IsDBNull(5))
                {
                    objCustomer.Country = reader.GetString(5) ?? "";
                }
                if (!reader.IsDBNull(6))
                {
                    objCustomer.PostalCode = reader.GetString(6) ?? "";
                }
                if (!reader.IsDBNull(7))
                {
                    objCustomer.ContactNo = reader.GetString(7) ?? "";
                }
                if (!reader.IsDBNull(8))
                {
                    objCustomer.Fax = reader.GetString(8) ?? "";
                }

                result.Add(reader["CustomerID"].ToString(), objCustomer);
            }
            reader.Close();

            return result;
        }

        /// <summary>
        /// Loads a <see cref="Customer"/> object from the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<object> LoadCustomersFromCountry(Object key)
        {
            List<object> result = new List<object>();
            object[] customerId = { key };
            SqlDataReader reader = null;

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                "Select CustomerID, ContactName, CompanyName, Address," +
                                "City, Country, PostalCode, Phone, Fax" +
                                " From dbo.Customers where Country = '{0}'",
                                customerId);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Customer objCustomer = new Customer();

                if (!reader.IsDBNull(0))
                {
                    objCustomer.CustomerID = reader.GetString(0) ?? "";
                }
                if (!reader.IsDBNull(1))
                {
                    objCustomer.ContactName = reader.GetString(1) ?? "";
                }
                if (!reader.IsDBNull(2))
                {
                    objCustomer.CompanyName = reader.GetString(2) ?? "";
                }
                if (!reader.IsDBNull(3))
                {
                    objCustomer.Address = reader.GetString(3) ?? "";
                }
                if (!reader.IsDBNull(4))
                {
                    objCustomer.City = reader.GetString(4) ?? "";
                }
                if (!reader.IsDBNull(5))
                {
                    objCustomer.Country = reader.GetString(5) ?? "";
                }
                if (!reader.IsDBNull(6))
                {
                    objCustomer.PostalCode = reader.GetString(6) ?? "";
                }
                if (!reader.IsDBNull(7))
                {
                    objCustomer.ContactNo = reader.GetString(7) ?? "";
                }
                if (!reader.IsDBNull(8))
                {
                    objCustomer.Fax = reader.GetString(8) ?? "";
                }

                result.Add(objCustomer);
            }
            reader.Close();

            return result;
        }

        /// <summary>
        /// Loads a <see cref="Customer"/> object from the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Queue<object> LoadCustomersByOrder(string orderField)
        {
            Queue<object> result = new Queue<object>();
            SqlDataReader reader = null;

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                "Select CustomerID, ContactName, CompanyName, Address," +
                                "City, Country, PostalCode, Phone, Fax" +
                                " From dbo.Customers order by {0} ASC",
                                orderField);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Customer objCustomer = new Customer();

                if (!reader.IsDBNull(0))
                {
                    objCustomer.CustomerID = reader.GetString(0) ?? "";
                }
                if (!reader.IsDBNull(1))
                {
                    objCustomer.ContactName = reader.GetString(1) ?? "";
                }
                if (!reader.IsDBNull(2))
                {
                    objCustomer.CompanyName = reader.GetString(2) ?? "";
                }
                if (!reader.IsDBNull(3))
                {
                    objCustomer.Address = reader.GetString(3) ?? "";
                }
                if (!reader.IsDBNull(4))
                {
                    objCustomer.City = reader.GetString(4) ?? "";
                }
                if (!reader.IsDBNull(5))
                {
                    objCustomer.Country = reader.GetString(5) ?? "";
                }
                if (!reader.IsDBNull(6))
                {
                    objCustomer.PostalCode = reader.GetString(6) ?? "";
                }
                if (!reader.IsDBNull(7))
                {
                    objCustomer.ContactNo = reader.GetString(7) ?? "";
                }
                if (!reader.IsDBNull(8))
                {
                    objCustomer.Fax = reader.GetString(8) ?? "";
                }

                result.Enqueue(objCustomer);
            }
            reader.Close();

            return result;
        }

        /// <summary>
        /// Loads a <see cref="Customer"/> object from the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HashSet<int> LoadOrderIDsByCustomer(Object key)
        {
            HashSet<int> result = new HashSet<int>();
            object[] customerId = { key };
            SqlDataReader reader = null;

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                "Select OrderID" +
                                " From dbo.Orders where CustomerID = '{0}'",
                                customerId);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader.GetInt32(0));
            }
            reader.Close();

            return result;
        }

        /// <summary>
        /// Add <see cref="Customer"/> information to datasource.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool AddCustomer(Customer val)
        {
            int rowsChanged = 0;
            string[] customer = {val.CustomerID,val.ContactName,val.CompanyName,
                                    val.Address,val.City,val.Country,val.PostalCode,
                                    val.ContactNo,val.Fax};

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                            "Insert into dbo.Customers" +
                                            "(CustomerID, ContactName,CompanyName,Address,City,Country,PostalCode,Phone,Fax)" +
                                            "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", customer);
            rowsChanged = cmd.ExecuteNonQuery();
            if (rowsChanged > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save <see cref="Customer"/> information to datasource.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool SaveCustomer(Customer val)
        {
            int rowsChanged = 0;
            string[] customer = {val.CustomerID,val.ContactName,val.CompanyName,
                                    val.Address,val.City,val.Country,val.PostalCode,
                                    val.ContactNo,val.Fax};

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                            "Update dbo.Customers " +
                                            "Set CustomerID='{0}'," +
                                            "ContactName='{1}',CompanyName='{2}'," +
                                            "Address='{3}',City='{4}'," +
                                            "Country='{5}',PostalCode='{6}'," +
                                            "Phone='{7}',Fax='{8}'" +
                                            " Where CustomerID = '{0}'", customer);
            rowsChanged = cmd.ExecuteNonQuery();
            if (rowsChanged > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete <see cref="Customer"/> information from datasource.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool RemoveCustomer(Customer val)
        {
            int rowsChanged = 0;
            string[] customer = { val.CustomerID };

            SqlCommand cmd = _connection.CreateCommand();
            cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
                                            "delete from dbo.Customers " +
                                            " Where CustomerID = '{0}'", customer);
            rowsChanged = cmd.ExecuteNonQuery();
            if (rowsChanged > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save counter value to datasource.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool SaveCounter(long counterValue)
        {
            /**
             * Perform command to save counter value to data source
             **/
            //int rowsChanged = 0;
            //SqlCommand cmd = _connection.CreateCommand();
            //cmd.CommandText = String.Format(CultureInfo.InvariantCulture,
            //                                "Update dbo.Counter " +
            //                                "Set CounterValue='{0}", counterValue);
            //rowsChanged = cmd.ExecuteNonQuery();
            //if (rowsChanged > 0)
            //{
            //    return true;
            //}
            //return false;

            return true;
        }
    }
}
