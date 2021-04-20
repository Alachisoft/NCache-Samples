# CACHE LOADER

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Sample Cache Loader creation](#sample-cache-loader-creation)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

NCache provides Cache Startup Loader through which cache can be preloaded with data as it starts up. This is
useful for applications which depend on certain data and that data should be available to the application
immediately after it begins executing. 

This sample explains how NCache has the ability to run the user provided code at the Cache startup and how can 
we implement and use Cache startup loader. This sample demonstrates working of cache loader on multiple nodes 
depending on the distribution hints.

This sample uses SampleData project as a reference for model class "Order".

### Prerequisites

Before the sample application executed make sure that:

- **config.properties** have been changed according to the configurations. 
	- Change the cache name
- By default, this sample uses 'demoCache', make sure cache is running. 

### Sample-cache-loader-creation

-  Besides, implementation, NCache requires user specified configuration to enable the cache loader. A user can configure and enable the CacheStartupLoader settings through NCache Web Manager. Following are the steps to configure and deploy Cache Loader.
	- Start NCache Web Manager and create a clustered cache with the name specified in an app.config.
	- Now select the 'Cache Loader' tab in the "Advanced Settings" of cache's details page.
	- Check the 'Enable Cache Startup Loader' and provide the Library Name(CacheLoaderImpl) and Class Name(com.alachisoft.ncache.samples.cacheLoaderImpl.Refresher) of the implementation.
	- Provide the parameter "connString" as the database connection string.
	- Provide the parameter "user" for databasr user name.
	- Provide the parameter "password" for database password.
	- These parameters provided to the cache loader assembly. 
	- A multi-node environment requires distribution hints to run the loader.
	- To deploy cache loader assemblies click 'Deploy Cache Loader' and select CacheLoaderImpl and SampleData assemblies and click 'OK'.
	- Save changes.

### Build and Run the Sample
- Run the following commands:
    ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
- OR cd to **target** (where jars are copied) folder and run this command:
	- ``` cd target ```
	- ``` java -cp * com.alachisoft.ncache.samples.Mian ```

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