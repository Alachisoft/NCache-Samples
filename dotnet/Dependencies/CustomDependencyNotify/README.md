# CUSTOM DEPENDENCY NOTIFY

### Note

Please note that .NET Framework based 'CUSTOM DEPENDENCY NOTIFY' sample will only run with .NET Framework based NCache servers.

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)



### Introduction

This sample explains how NCache keeps the cached items synchronized with the databases that give data change notification features.
In this sample, we are using "Notify Custom Dependency" feature to synchronize cache with CosmosDB.
Items are added in the cache with a dependency and as the data is changed in the CosmosDB database, items are removed from the cache.

This sample uses Models project as a reference for model class "Customers".

### Prerequisites

Before the sample application is executed make sure that:
- Requirements may differ depending upon the database you are using. This sample is configured to work with Azure CosmosDB. To use with other
databases, you may have to comment/uncomment some code in the sample code.

- Create a new database in CosmosDb named DemoDatabase and a collection "Customers" with a partition key set to '/id'.
- After this, you can insert the seed customer data. Add a new container named "leases" with partitionkey of "/id" to monitor changes in database.
- Change the cache name, EndPoint, auth key  in app.config
- By default this sample uses 'myPartitionedCache', make sure that cache is running.

### Build and Run the Sample
    
- Run the sample application.

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

[C] Copyright 2019 Alachisoft 