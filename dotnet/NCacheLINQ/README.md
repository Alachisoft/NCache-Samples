# NCACHE LINQ

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample program demonstrates how to use LINQ Queries with NCache.
This sample provides you with 3 examples of LINQ Queries.
There are two modules in this sample:
- SampleData (Class library)
	- This module some attributes of Products. The Product class and its
		attributes are also implemented in this module.
- NCacheLINQ (Console application)
	- Simple console application that presents you a menu to select the query you want to execute against the cache. This application uses 'myPartitionedCache' by default. You can specify any other from app.config. 
	- Following LINQ Queries are implemented in this sample:
		- FROM product in products WHERE product.ProductID > 10 SELECT product;
		- FROM product in products WHERE product.Category == 4 SELECT product;
		- FROM product in products WHERE product.ProductID < 10 && product.Supplier == 1 SELECT product;

This sample uses SampleData project as a reference for model class "Product".

### Prerequisites

Before the sample application is executed make sure that:

- This feature requires NCache version 4.6 or higher.
- app.config have been changed according to the configurations. 
	- change the cache name
- To use this sample, you must first specify the indexes of the objects you want to query in the cache.
          - Start NCache Web Manager and create a clustered cache with the name specified in app.config. 
          - Now select the 'Query Indexes' tab in the "Advanced Settings" of cache's details page.
          - Add a query index by browsing 'sampledata.dll'. 
          - Select the 'Product' class and click 'Add Selected Classes'.
          - Check the class and all of its attributes and click OK.
          - Save changes.	
	
- By default this sample uses 'myPartitionedCache', make sure that cache is running. 

### Build and Run the Sample
    
- Run the sample application.

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/ncache-programmers-guide.pdf

### Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

[C] Copyright 2021 Alachisoft 