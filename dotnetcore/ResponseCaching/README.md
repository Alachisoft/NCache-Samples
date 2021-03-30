# Response Caching

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample illustrates the use of a Response Caching using NCache. The sample contains multiple pages such as,
	- Index page, display list of products, create SQL dependency and cache the response.   
	- View page, display product detail and cache the response
	- Edit page, updates the database, if sucessfully updated, database changes trigger the SQL dependency that removes the index page from the cache and redirects you to Success page.
	-Success page , in case your view is successfully updated it removes it from cache using the invalidation feature.

Sample uses a partitioned cache named, 'myPartitionedCache',
which is registered on your machine upon the installation of NCache. 

To start or re-register this cache later, you can use NCache Web Manager or NCache command line tools.


### Prerequisites

Requirements:

- Visual Studio 2015 or later.
- .net core  3.1 or later.

Before the sample application is executed make sure that:
- appsettings.json have been changed according to the configurations. 
	- Change the cache name
		- By default this sample uses 'myPartitionedCache', make sure that cache is running. 
	- Change the connection string
		- By default this sample uses 'Mydbstring', make sure that connection string connect with sql server. 

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