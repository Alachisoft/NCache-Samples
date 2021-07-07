# SearchUsingSQL

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample program demonstrates how to use the Object Query Language in NCache. 
This sample provides you with 3 examples of OQL. Following Queries are implemented in this sample:
- 'SELECT System.String WHERE Supplier = "Walmart"' done through NamedTags.
- 'SELECT Alachisoft.NCache.sample.Data.Product WHERE Category ="Edibles"' done through object index definition. 
- 'SELECT Name, Supplier FROM Alachisoft.NCache.Sample.Data.Product WHERE Category = "Edibles"' done through Projection.

This sample uses product.json document as a reference for model class "Product".

### Prerequisites

Before the sample application is executed make sure that:

- app.config.json have been changed according to the configurations. 
	- change the cache name
- To use this sample, you must first specify the indexes of the objects you want to query in the cache.
          - Start NCache Web Manager and create a clustered cache with the name specified in app.config.json. 
          - Now select the 'Query Indexes' tab in the "Advanced Settings" of cache's details page.
          - Add a query index by browsing 'product.json'. 
          - Select the 'Alachisoft.NCache.Sample.Data.Product' class and click 'Add Selected Classes'.
          - Check the class and all of its attributes and click OK.
          - Save changes.

- By default this sample uses 'demoCache', make sure that cache is running. 

### Build and Run the Sample
    
- Run command 
	- '''npm install'''
- To Run the sample application, execute following command
	- ''' node ./index.js'''  
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