// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright � Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.DatasourceProviders;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Samples.Utility;

namespace Alachisoft.NCache.Samples.Providers
{
	/// <summary>
	/// Contains methods used to save/update an object to the master data source. 
	/// </summary>
	public class SqlWriteThruProvider : IWriteThruProvider
	{
		/// <summary>
		/// Object used to communicate with the Datasource.
		/// </summary>
		private SqlDatasource sqlDatasource;

        /// <summary>
        /// Perform tasks like allocating resources or acquiring connections
        /// </summary>
        /// <param name="parameters">Startup paramters defined in the configuration</param>
        /// <param name="cacheId">Define for which cache provider is configured</param>
        public void Init(IDictionary parameters, string cacheId)
        {
            object connString = parameters["connstring"];
            sqlDatasource = new SqlDatasource();
            sqlDatasource.Connect(connString == null ? "" : connString.ToString());
        }

		/// <summary>
		/// Perform tasks associated with freeing, releasing, or resetting resources.
		/// </summary>
		public void Dispose()
		{
			sqlDatasource.DisConnect();
		}

        #region IWriteThruProvider Members

        /// <summary>
        /// Write multiple cache operations to data source
        /// </summary>
        /// <param name="operations">Collection of <see cref="WriteOperation"/></param>
        /// <returns>Collection of <see cref="OperationResult"/> to cache</returns>
        public ICollection<OperationResult> WriteToDataSource (ICollection<WriteOperation> operations)
        {
            // initialize collection of results to return to cache
            ICollection<OperationResult> operationResults = new List<OperationResult>();
            // initialize variable for confirmation of write operation
            bool result = false;
            // iterate over each operation sent by cache
            foreach (var item in operations)
            {
                // initialize operation result with failure
                OperationResult operationResult = new OperationResult(item, OperationResult.Status.Failure);
                // get object from provider cache item
                Customer value = item.ProviderItem.GetValue<Customer>();
                // check if the type is what you need
                if (value.GetType().Equals(typeof(Customer)))
                {
                    // perform write command and get confirmation from data source
                    result = sqlDatasource.SaveCustomer((Customer)value);
                    // if write operation is successful, change status of operationResult
                    if (result) operationResult.OperationStatus = OperationResult.Status.Success;
                }
                // insert result operation to collect of results
                operationResults.Add(operationResult);
            }
            
            // send result to cache
            return operationResults;
            
        }

        /// <summary>
        /// Write multiple data type cache operations to data source
        /// </summary>
        /// <param name="dataTypeWriteOperations">Collection of <see cref="DataTypeWriteOperation"/></param>
        /// <returns>Collection of <see cref="OperationResult"/> to cache</returns>
        public ICollection<OperationResult> WriteToDataSource (ICollection<DataTypeWriteOperation> dataTypeWriteOperations)
        {
            // initialize collection of results to return to cache
            ICollection<OperationResult> operationResults = new List<OperationResult>();
            // initialize variable for confirmation of write operation
            bool result = false;
            // iterate over each operation sent by cache
            foreach (DataTypeWriteOperation operation in dataTypeWriteOperations)
            {
                // initialize operation result with failure
                OperationResult operationResult = new OperationResult(operation, OperationResult.Status.Failure);
                // determine the type of data structure
                switch(operation.DataType)
                {
                    // for counters, get value from ProviderItem.Counter
                    case DistributedDataType.Counter:
                        result = sqlDatasource.SaveCounter(operation.ProviderItem.Counter);
                        if (result) operationResult.OperationStatus = OperationResult.Status.Success;
                        break;
                    // for every other data structure, the new entry is sent as object from cache
                    default:
                        if(operation.OperationType.Equals(DatastructureOperationType.UpdateDataType))
                            result = sqlDatasource.SaveCustomer((Customer) operation.ProviderItem.Data);
                        else if (operation.OperationType.Equals(DatastructureOperationType.AddToDataType))
                            result = sqlDatasource.AddCustomer((Customer)operation.ProviderItem.Data);
                        else if (operation.OperationType.Equals(DatastructureOperationType.DeleteFromDataType))
                            result = sqlDatasource.RemoveCustomer((Customer)operation.ProviderItem.Data);
                        if (result) operationResult.OperationStatus = OperationResult.Status.Success;
                        break;
                }
                // add result to list of operation results
                operationResults.Add(operationResult);
            }
            // return list of operations to cache
            return operationResults;
        }

        /// <summary>
        /// write an operation to data source
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public OperationResult WriteToDataSource (WriteOperation operation)
        {
            // initialize variable for confirmation of write operation
            bool result = false;
            // initialize operation result with failure
            OperationResult operationResult = new OperationResult(operation, OperationResult.Status.Failure);
            // get value of object
            Customer value = operation.ProviderItem.GetValue<Customer>();
            // check if value is the type you need
            if (value.GetType().Equals(typeof(Customer)))
            {
                // send data to cache for writing
                result = sqlDatasource.SaveCustomer((Customer)value);
                // if write operatio is success, change status of operation result
                if (result) operationResult.OperationStatus = OperationResult.Status.Success;
            }
            // return result to cache
            return operationResult;
        }

        #endregion

        //////////////////////////////////////////////
    }
}
