# DISTRIBUTED LUCENE

### Table of contents

* [Introduction](#introduction)
* [Prerequisites]
	- Lucene Indexing should be enabled from NCache Web Manager.
	- Client Activity Notifications should be enabled from NCache Web Manager.
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample program demonstrates how to use the NCache Distributed Lucene API. 
It shows how to initialize the cache instance and perform Lucene related tasks from NCache.

### Prerequisites

Before the sample application is executed make sure that:

- A Partitioned OR Partitioned-of-Replica cache is created.
- app.config have been changed according to the configurations. 
	- Change the cache name as required.
	- Change the index name as required.
- By default this sample uses 'myPartitionedCache' and 'MyIndex', make sure that cache is running.

- To use this sample, you must first enable Lucene Indexes on cache. 
	- Start NCache Web Manager and create a clustered cache with the name specified in app.config. 
	- Now select the 'Full Text Search Index' tab in the "Advanced Settings" of cache's details page.
	- Check the 'Enable Lucene Index' and specify custom path where your Lucene indexes will be created in 'Index Path' or default path "[installDir]\lucene" will be used. 
	- Save changes.
	- Now select the 'Options' tab in the "Advanced Settings" of cache's details page. 
	- Check the 'Enable Client Activity Notifications' on the option's page. This is to dispose off instances used by Lucene when a client is detected dead. 
	- Save changes. 

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