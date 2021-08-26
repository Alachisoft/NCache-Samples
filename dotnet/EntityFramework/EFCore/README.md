# EFCoreCaching

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

- This is a sample application based on NCache's EFCoreCaching provider. NCache's EFCoreCaching provider implementation allows you to store your entity data to NCache’s Distributed/OutProc cache. This sample caches entity objects and query result sets in cache. 
	
- It is a console based application. The sample demonstrates how NCache works as a caching provider for entity framework core.
	- <b>LINQ Query Extension Methods</b> section demonstrates different extension methods on the database context that intercept EFCore queries and cache data using different strategies.
	- <b>Direct Cache Access Methods</b> section demonstrates how entities can still be added or removed from cache without querying the database.
	- <b>Data Invalidation</b> section shows how data in cache can be made reliable.
	- <b>Aggregate Operation Caching</b> section shows how results of aggregate operations can be cached.

### Prerequisites

Following are the prerequisites for this sample.

- Northwind is configured in SqlServer. 
- Visual Studio 2015 is installed.
- .Net Framework 4.6.1 is installed
- Entity Framework Core 2.2.1

Before the sample application is executed make sure that:

- app.config has been changed according to the configurations. 
	- Change the cache name 
	- Change connection string for the SQL database in app.config.
- Make sure SQL service is running and server hosts Northwind sample database.
- By default this sample uses 'demoCache', make sure that cache is running. 
- Query indexes for the Northwind model in the application need to be configured in NCache before the application is used. Follow the given steps for that,
	- Build the sample; generating the executable file.
	- Start NCache Web Manager and create a clustered cache with the name specified in app.config.
	- Now select the 'Query Indexes' tab in the "Advanced Settings" of cache's details page.
	- Add a query index by browsing DLL files of the model.
	- Since our application generates an executable file and not a DLL assembly, therefore, NCache Web Manager needs to be provided the executable to extract the indexes from it.
	- In the file chooser dialogue, go to the folder where your executable file is placed.
	- Type '*.exe' in the file name field and press enter. This will list all the executable files in the directory.
	- Simply choose the executable file containing the northwind model's build and set up the indexes from it.
	- Save changes.

### Run the Sample
    
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