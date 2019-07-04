# CONTINUOUS QUERY

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample explains how you can use the NCache Continuous Query. It uses customer instance and stores it to the cache, then queries the cache for the item against the criteria.

This sample uses SampleData project as a reference for model class "Customer".

### Prerequisites

Before the sample application is executed make sure that:

- app.config have been changed according to the configurations. 
	- Change the cache name

- To use this sample, you must first specify the indexes of the objects you want to query in the cache.
	- Build 'SampleData' project and Start NCache Web Manager and create a clustered cache with the name specified in app.config. 
	- Now select the 'Query Indexes' tab in the "Advanced Settings" of cache's details page.
	- Add a query index by browsing 'sampledata.dll'. 
    - Select the 'Customer' class and click 'Add Selected Classes'.
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
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

[C] Copyright 2019 Alachisoft 