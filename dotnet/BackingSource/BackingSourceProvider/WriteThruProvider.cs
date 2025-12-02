// ===============================================================================
// Alachisoft (R) NCache Sample Code (Modified for direct ADO.NET access)
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
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Sample.Data;

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

namespace Alachisoft.NCache.Samples.BackingSource
{
    /// <summary>
    /// Contains methods used to save/update an object to the master data source directly.
    /// </summary>
    public class SqlWriteThruProvider : IWriteThruProvider
    {
        /// Connection string for communicating with the data source
        private string? _connectionString;

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
            object? connString = parameters["ConnectionString"];
            _connectionString = connString?.ToString() ??
                throw new ArgumentException("Connection string not provided in parameters.");

            // Testing connection to verify database accessibility
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
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
                        // Casting provider item data to Customer
                        var customer = op.ProviderItem.Data as Customer;
                        if (customer != null)
                        {
                            // Executing specific database action based on operation type
                            success = op.OperationType switch
                            {
                                DatastructureOperationType.UpdateDataType => SaveCustomer(customer),
                                DatastructureOperationType.AddToDataType => AddCustomer(customer),
                                DatastructureOperationType.DeleteFromDataType => RemoveCustomer(customer),
                                _ => false
                            };

                            // Marking result as successful if database operation succeeds
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
            Customer? value = operation.ProviderItem.GetValue<Customer>();

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
            using var conn = new SqlConnection(_connectionString);
            // Creating SQL command for save or update operation
            using var cmd = new SqlCommand(query, conn);

            // Adding parameters to SQL command
            cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
            cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ContactName", customer.ContactName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);

            // Opening connection and executing command
            conn.Open();

            // Executing command and getting affected rows
            var result = cmd.ExecuteNonQuery();

            // Closing connection after execution
            conn.Close();
            return result > 0;
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
            using var conn = new SqlConnection(_connectionString);
            // Creating SQL command for insert operation
            using var cmd = new SqlCommand(query, conn);

            // Adding parameters to SQL command
            cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
            cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ContactName", customer.ContactName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);

            // Opening connection and executing command
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
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
            using var conn = new SqlConnection(_connectionString);
            // Creating SQL command for delete operation
            using var cmd = new SqlCommand(query, conn);
            // Adding key parameter
            cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);

            // Opening connection and executing delete command
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
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
            using var conn = new SqlConnection(_connectionString);
            // Creating SQL command for counter save operation
            using var cmd = new SqlCommand(query, conn);

            // Adding parameters for counter name and value
            cmd.Parameters.AddWithValue("@Name", "CustomerCounter");
            cmd.Parameters.AddWithValue("@Value", counterValue);

            // Opening connection and executing merge command
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}