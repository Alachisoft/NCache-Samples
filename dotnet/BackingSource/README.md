# BACKING SOURCE

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

- This sample explains how NCache keeps the cached items synchronized with the database, and how NCache silently loads the expired items from the 
	datasource using its 'ReadThru' feature and similarly silently updates items using its 'WriteThru' feature.

- This sample deals with the following features of NCache.
	i)	 ReadThru
	ii)	 WriteThru
	iii) ReSync Expired Items
	
This sample uses SampleData project as a reference for model class "Customer".

### Prerequisites

Before the sample application is executed make sure that:

- app.config have been changed according to the configurations. 
	- change the cache name
- By default this sample uses 'myPartitionedCache', make sure that cache is running. 
- Northwind database must be configured in Sql Server.

- Before running this sample make sure backing source is enable and following providers are registered.
	For Read Thru
		SqlReadThruProvider
	For Write Thru
		SqlWriteThruProvider
		
- To enable and register backing source,
	- Start NCache Web Manager and create a clustered cache with the name specified in app.config. 
	- Now select the 'Backing Source' tab in the "Advanced Settings" of cache's details page. 
	- Provide the assembly names and classes to use and name these providers as described above. 
	- Specify connection string as 'connstring' parameter for database that specified in app.config. 
	- Click 'Deploy Backing Source Provider' to deploy backing source. 
	- Locate and select 'BackingSourceProvider.Sql.dll', 'SampleData.dll' and all other dependent assemblies for providers.
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

[C] Copyright 2019 Alachisoft 