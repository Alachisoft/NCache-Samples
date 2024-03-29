# CACHE ITEM VERSIONING

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

Cache item versioning enables optimistic locking to makes possible the concurrency checks on cacheitems. 
CacheItemVersion is a property associated with every cache item. It is basically a numeric value that is 
used to represent the version of the cached item which changes with every update to an item. This property 
allows you to track whether any change occurred in an item or not. When you fetch an item from cache, you also 
fetch its current version form cache.

This sample program demonstrates how to use Optimistic locking in NCache through item versioning. 
It shows how to Add, Get, Update and Delete object(s) using cache item version from NCache.

This sample uses SampleData project as a reference for model class "Customer".

### Prerequisites

Before the sample application is executed make sure that:

- app.config have been changed according to the configurations. 
	- Change the cache name
- By default this sample uses 'demoCache', make sure that cache is running. 

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