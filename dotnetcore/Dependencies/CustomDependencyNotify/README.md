# CUSTOM DEPENDENCY NOTIFY

### Note

Please note that .NET Core based 'CUSTOM DEPENDENCY NOTIFY' sample will only run with .NET Core based NCache servers.

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
Items are added in the cache with custom dependency along with the parameters needed to create Notify Extensible Dependency object and as the data is changed in the CosmosDB database, items are removed from the cache.

This sample uses Models project as a reference for model class "Customers".

### Prerequisites

Before the sample application is executed make sure that:
- Requirements may differ depending upon the database you are using. This sample is configured to work with Azure CosmosDB. To use with other
databases, you may have to comment/uncomment some code in the sample code.

- Create a new database in CosmosDb named DemoDatabase and a collection "Customers" with a partition key set to '/id'.
- After this, you can insert the seed customer data. Add a new container named "leases" with partitionkey of "/id" to monitor changes in database.
- Change the cache name, EndPoint, auth key  in app.config
- By default this sample uses 'myPartitionedCache', make sure that cache is running.
- Before running this sample make sure custom dependency is enabled and "NotifyCustomDependencyProvider" is registered.
- To enable and register backing source,
	- Start NCache Web Manager and create a clustered cache with the name specified in app.config. 
	- Now select the 'Custom Dependency' tab in the "Advanced Settings" of cache's details page. 
	- To enable Custom Dependency,
		- Click the checkbox labelled "Enable Custom Dependency". Click on "Add Provider" button next to this checkbox.
		- Provide a unique provider name ("NotifyCustomDependencyProvider", for example).Make sure the name matches with the one specified in the provider in CustomDependencyNotifyImpl project.
		- Click on "Browse" button for library field. Select library "CustomDependencyNotifyImpl.dll".
		- Select class "Alachisoft.NCache.Samples.Providers.NotifyCustomDependencyProvider" from the now populated drop down list.
		- Specify connection string as 'connstring' parameter for database that specified in app.config. 
	- Custom Dependency provider files need to be deployed.
		- Click 'Deploy Custom Dependency Provider' to deploy custom dependency. 
		- Locate and select 'CustomDependencyNotifyImpl.dll', 'Models.dll' and all other dependent assemblies for providers.
	- Click 'Ok' and save changes.
    

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

[C] Copyright 2021 Alachisoft 