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
- By default this sample uses 'demoCache', make sure that cache is running. 
- Northwind database must be configured in Sql Server.

- Before running this sample make sure backing source is enable and following providers are registered.
	For Read Thru
		1) SqlReadThruProvider
	For Write Thru
		1) SqlWriteThruProvider
		
- To enable and register backing source,
	- Start NCache Web Manager and create a clustered cache with the name specified in app.config. 
	- Now select the 'Backing Source' tab in the "Advanced Settings" of cache's details page. 
	- To enable Read Through Backing Source,
		- Click the checkbox labelled "Enable Read Through". Click on "Add Provider" button next next to this checkbox.
		- Provide a unique provider name ("SqlReadThruProvider", for example).
		- Click on "Browse" button for library field. Select library "BackingSource.Providers.Sql.Netcore.dll".
		- Select class "Alachisoft.NCache.Samples.Providers.SqlReadThruProvider" from the now populated drop down list.
		- Specify connection string as 'connstring' parameter for database that specified in app.config. 
	- Similarly, to enable Write Thru backing source, follow the same steps as above. Choose "Alachisoft.NCache.Samples.Providers.SqlWriteThruProvider" from the class drop down list.
	- Backing source provider files need to be deployed.
		- Click 'Deploy Backing Source Provider' to deploy backing source. 
		- Locate and select 'BackingSourceProvider.Sql.Netcore.dll', 'SampleData.dll' and all other dependent assemblies for providers.
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