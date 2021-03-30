# CUSTOM DEPENDENCY

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample implements the Custom dependency feature of the NCache. In custom dependency, you can implement your 
custom logic which defines when a certain data becomes invalid for cached items. This sample implements Custom 
Dependency and adds items to cache with custom dependency and then updates item in database. Custom depedency 
invalidate data from database and when the dependency expires, the items are removed  from the cache.

### Prerequisites

Before the sample application is executed make sure that:

- app.config have been changed according to the configurations. 
	- Change the cache name
	- connectionString:	the connection string used to connect to the database. By default it connects to the localhost.

- By default this sample uses 'demoCache', make sure that cache is running.

- Before running this sample make sure custom dependency is enabled and "CustomDependencyProvider" is registered.

- To enable and register custom dependency,
	- Start NCache Web Manager and create a clustered cache with the name specified in app.config. 
	- Now select the 'Custom Dependency' tab in the "Advanced Settings" of cache's details page. 
	- To enable Custom Dependency,
		- Click the checkbox labelled "Enable Custom Dependency". Click on "Add Provider" button next to this checkbox.
		- Provide a unique provider name ("CustomDependencyProvider", for example).Make sure the name matches with the one specified in the provider in the CustomDependencyImpl project.
		- Click on "Browse" button for library field. Select library "CustomDependencyImpl.dll".
		- Select class "Alachisoft.NCache.Samples.Providers.CustomDependencyProvider" from the now populated drop down list.
		- Specify connection string as 'connstring' parameter for database that specified in app.config. 
	- Custom Dependency provider files need to be deployed.
		- Click 'Deploy Custom Dependency Provider' to deploy custom dependency. 
		- Locate and select 'CustomDependencyImpl.dll', 'SampleData.dll' and all other dependent assemblies for providers.
	- Click 'Ok' and save changes. 

### Build and Run the Sample

- Build 'CustomDependencyUsage' project.
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