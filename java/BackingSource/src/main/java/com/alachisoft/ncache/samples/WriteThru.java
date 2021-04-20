// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Write Through sample class.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
package com.alachisoft.ncache.samples;

import com.alachisoft.ncache.runtime.datasourceprovider.*;
import com.alachisoft.ncache.samples.data.Customer;
import com.alachisoft.ncache.samples.data.DataLayer;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Map;

public class WriteThru implements WriteThruProvider
{
    DataLayer _dataLayer;

    /**
     * Perform tasks like allocating resources or acquiring connections
     * @param map Startup paramters defined in the configuration
     * @param s Define for which cache provider is configured
     * @throws Exception
     */
    @Override
    public void init(Map<String, String> map, String s) throws Exception {
        String connString = map.get("connString");
        String user = map.get("user");
        String pass = map.get("pass");
        _dataLayer = new DataLayer();
        _dataLayer.Init(connString, user, pass);
    }

    /**
     * write an operation to data source
     * @param wo
     * @return
     * @throws Exception
     */
    @Override
    public OperationResult writeToDataSource(WriteOperation wo) throws Exception
    {
        // initialize variable for confirmation of write operation
        boolean result = false;
        // get value of object
        Customer value =  wo.getProviderCacheItem().getValue(Customer.class);
        // initialize operation result with failure
        OperationResult operationResult = new OperationResult(wo, OperationResult.Status.Failure);

        // check if value is the type you need
        if(value.getClass().equals(Customer.class)){
            // send data to cache for writing
            result = _dataLayer.saveCustomer(value);
            // id write ooperation is success, change status of operation result
            if(result) operationResult.setOperationStatus(OperationResult.Status.Success);
        }
        // return result to cache
        return operationResult;
    }

    /**
     * Write multiple data type cache operations to data source
     * @param collection Collection of WriteOperation
     * @return Collection of OperationResult to cache
     * @throws Exception
     */
    @Override
    public Collection<OperationResult> writeToDataSource(Collection<WriteOperation> collection) throws Exception {
        // initialize collection of results to return to cache
        List<OperationResult> operationResults = new ArrayList<>();

        // initialize variable for confirmation of write operation
        boolean result = false;

        // iterate over each operation sent by cache
        for(WriteOperation writeOperation : collection)
        {
            // initialize operation result with failure
            OperationResult operationResult = new OperationResult(writeOperation, OperationResult.Status.Failure);

            // get object from provider cache item
            Customer value = writeOperation.getProviderCacheItem().getValue(Customer.class);

            // check if value is the type you need
            if(value.getClass().equals(Customer.class)){
                // send data to cache for writing
                result = _dataLayer.saveCustomer(value);
                // id write operation is success, change status of operation result
                if(result) operationResult.setOperationStatus(OperationResult.Status.Success);
            }
        }
        return operationResults;
    }

    /**
     * Write multiple data type cache operations to data source
     * @param collection Collection of DataStructureWriteOperation
     * @return Collection of OperationResult to cache
     * @throws Exception
     */
    @Override
    public Collection<OperationResult> writeDataStructureToDataSource(Collection<DataStructureWriteOperation> collection) throws Exception {
        Collection<OperationResult> operationResults = new ArrayList<>();

        boolean result = false;

        for(DataStructureWriteOperation operation : collection){
            // initialize operation result with failure
            OperationResult operationResult = new OperationResult(operation, OperationResult.Status.Failure);

            // for every other data structure, the new entry is sent as object from cache
            if(operation.getOperationType().equals(DataStructureOperationType.UpdateDataType))
                result = _dataLayer.saveCustomer((Customer) operation.getProviderItem().getData());
            else if(operation.getOperationType().equals(DataStructureOperationType.AddToDataType))
                result = _dataLayer.addToSource((Customer) operation.getProviderItem().getData());
            else if(operation.getOperationType().equals(DataStructureOperationType.DeleteFromDataType))
                result = _dataLayer.removeFromSource((Customer) operation.getProviderItem().getData());
            if(result) operationResult.setOperationStatus(OperationResult.Status.Success);

            operationResults.add(operationResult);
        }
        return operationResults;
    }


    @Override
    public void close() throws SQLException {
        _dataLayer.closeConnection();
    }
}
