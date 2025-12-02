// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Sample.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ===============================================================================
// Ncache Write-Thru Provider is used to write data to the master data source
// whenever data is added/updated in the cache. This mechanism ensures that
// the master data source remains synchronized with the cache, providing data
// consistency across the system. Some use cases of Write-Thru include:
//   1. In e-commerce to ensure synchronized data between the cache and database.
//   2. In financial systems to maintain the integrity of data.
//   3. In applications that perform real-time analytics on streaming data to
//      ensure data points are also stored in the master data source for long-term
//      analysis and reporting.
// ===============================================================================

namespace Alachisoft.NCache.Samples.BackingSourceProvider
{
    /// <summary>
    /// Contains methods used to save/update an object to the master data source directly.
    /// </summary>
    public class WriteThruProvider : IWriteThruProvider
    {
        /// Connection string for communicating with the data source
        private string _connectionString;

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to initialize the write-thru provider
        /// </summary>
        /// <param name="parameters">Startup parameters defined in the configuration</param>
        /// <param name="cacheName">Name of the cache</param>
        public void Init(IDictionary parameters, string cacheName)
        {
            // Validating input parameters
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            // Extracting connection string from parameters
            object connString = parameters["ConnectionString"];
            _connectionString = connString?.ToString() ??
                throw new ArgumentException("Connection string not provided in parameters.");

            // Test connection
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
            }
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to dispose the write-thru provider
        /// </summary>
        public void Dispose()
        {
            // Clearing connection string on disposal
            _connectionString = null;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to write multiple cache operations to the data source
        /// </summary>
        /// <param name="operations">Collection of <see cref="WriteOperation"/> to be written to data source</param>
        /// <returns>Collection of <see cref="OperationResult"/> returned to cache</returns>
        public ICollection<OperationResult> WriteToDataSource(ICollection<WriteOperation> operations)
        {
            // Initializing collection for holding operation results
            ICollection<OperationResult> operationResults = new List<OperationResult>();

            // Iterating through all write operations
            foreach (var op in operations)
            {
                // Processing each operation individually
                var result = WriteToDataSource(op);
                operationResults.Add(result);
            }

            // Returning collected results to cache
            return operationResults;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to write multiple data type operations to the data source
        /// </summary>
        /// <param name="dataTypeWriteOperations">Collection of <see cref="DataTypeWriteOperation"/> to
        /// be written to data source</param>
        /// <returns>Collection of <see cref="OperationResult"/> returned to cache</returns>
        public ICollection<OperationResult> WriteToDataSource(ICollection<DataTypeWriteOperation> dataTypeWriteOperations)
        {
            // Initializing list to store results
            ICollection<OperationResult> operationResults = new List<OperationResult>();

            // Iterating through all data type write operations
            foreach (var op in dataTypeWriteOperations)
            {
                // Creating a result with default failure status
                OperationResult operationResult = new OperationResult(op, OperationResult.Status.Failure);
                bool success = false;

                // Checking which distributed data type is being written
                switch (op.DataType)
                {
                    case DistributedDataType.Counter:
                        // Writing counter value to data source
                        success = SaveCounter(op.ProviderItem.Counter);
                        if (success) operationResult.OperationStatus = OperationResult.Status.Success;
                        break;

                    default:
                        Customer customer = op.ProviderItem.Data as Customer;
                        if (customer != null)
                        {
                            switch (op.OperationType)
                            {
                                case DatastructureOperationType.UpdateDataType:
                                    success = SaveCustomer(customer);
                                    break;

                                case DatastructureOperationType.AddToDataType:
                                    success = AddCustomer(customer);
                                    break;

                                case DatastructureOperationType.DeleteFromDataType:
                                    success = RemoveCustomer(customer);
                                    break;
                            }

                            if (success)
                                operationResult.OperationStatus = OperationResult.Status.Success;
                        }
                        break;
                }

                // Adding operation result to result list
                operationResults.Add(operationResult);
            }

            // Returning list of results back to cache
            return operationResults;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to write a single cache operation to the data source
        /// </summary>
        /// <param name="operation">Single cache operation to be written</param>
        /// <returns>Result of the write operation</returns>
        public OperationResult WriteToDataSource(WriteOperation operation)
        {
            // Initializing variable for success status
            bool result = false;

            // Creating operation result with failure status initially
            OperationResult operationResult = new OperationResult(operation, OperationResult.Status.Failure);

            // Extracting object value from cache item
            Customer value = operation.ProviderItem.GetValue<Customer>();

            // Checking if value is of expected type
            if (value != null)
            {
                // Saving or updating customer in database
                result = SaveCustomer(value);

                // Updating operation status on success
                if (result)
                    operationResult.OperationStatus = OperationResult.Status.Success;
            }

            // Returning operation result back to cache
            return operationResult;
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to save or update a Customer record in the data source
        /// </summary>
        /// <param name="customer">Customer object to save</param>
        /// <returns>True if operation succeeds, otherwise false</returns>
        private bool SaveCustomer(Customer customer)
        {
            // SQL query to update existing record or insert new one if it doesn't exist
            const string query = @"
                UPDATE Customers 
                SET CompanyName = @CompanyName, ContactName = @ContactName, City = @City, Country = @Country 
                WHERE CustomerID = @CustomerID;

                IF @@ROWCOUNT = 0
                INSERT INTO Customers (CustomerID, CompanyName, ContactName, City, Country)
                VALUES (@CustomerID, @CompanyName, @ContactName, @City, @Country);";

            // Creating SQL connection
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", (object)customer.CompanyName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ContactName", (object)customer.ContactName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@City", (object)customer.City ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Country", (object)customer.Country ?? DBNull.Value);

                conn.Open();
                // Executing command and getting affected rows
                var result = cmd.ExecuteNonQuery();

                // Closing connection after execution
                conn.Close();
                return result > 0;
            }
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to add a new Customer record to the data source
        /// </summary>
        /// <param name="customer">Customer object to add</param>
        /// <returns>True if operation succeeds, otherwise false</returns>
        private bool AddCustomer(Customer customer)
        {
            const string query = @"
                INSERT INTO Customers (CustomerID, CompanyName, ContactName, City, Country)
                VALUES (@CustomerID, @CompanyName, @ContactName, @City, @Country);";

            // Creating SQL connection
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", (object)customer.CompanyName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ContactName", (object)customer.ContactName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@City", (object)customer.City ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Country", (object)customer.Country ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to remove a Customer record from the data source
        /// </summary>
        /// <param name="customer">Customer object to remove</param>
        /// <returns>True if operation succeeds, otherwise false</returns>
        private bool RemoveCustomer(Customer customer)
        {
            const string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";

            // Creating SQL connection
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// ------------------------------------------------------------------------------------------
        /// <summary>
        /// Method to save or update a counter value in the data source
        /// </summary>
        /// <param name="counterValue">Counter value to store</param>
        /// <returns>True if operation succeeds, otherwise false</returns>
        private bool SaveCounter(long counterValue)
        {
            const string query = @"
                MERGE INTO Counters WITH (HOLDLOCK) AS Target
                USING (SELECT @Name AS Name, @Value AS Value) AS Source
                ON Target.Name = Source.Name
                WHEN MATCHED THEN UPDATE SET Target.Value = Source.Value
                WHEN NOT MATCHED THEN INSERT (Name, Value) VALUES (Source.Name, Source.Value);";

            // Creating SQL connection
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", "CustomerCounter");
                cmd.Parameters.AddWithValue("@Value", counterValue);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
