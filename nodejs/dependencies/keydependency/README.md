# KEY DEPENDENCY

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

- This sample explains how NCache supports dependent items to be added, and what is the behaviour if dependency has a change.
- Sample adds one item dependent upon another item.
- Any modification in the dependee item will result in the invalidation of the dependent item and thus the item will be removed from the cache.
- This sample deals with the Key dependency features of NCache.
	
### Prerequisites

Before the sample application is executed make sure that:

- app.config.json have been changed according to the configurations. 
	- Change the cache name
	
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