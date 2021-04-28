# NCache JCache Sample

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [HOW TO CONFIGURE](#HOW TO CONFIGURE)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction
 
This sample program demonstrates the usage of JCache APIs.

### Prerequisites

Before the sample application is executed make sure that:
    config.properties have been changed according to the configurations.
        Change the cache name
    By default this sample uses 'demoCache', make sure that cache is running.


### HOW TO CONFIGURE

There are 3 ways to load NCacheCachingProvider
	1 - The META-INF/services/javax.cache.spi.CachingProvider service definition that is located in the ncache-client maven
	package assures that applications using the javax.cache.Caching bootstrap class use the Coherence JCache provider by default.

	2 - The second way is to use the Caching.getCachingProvider(String) or Caching.getCachingProvider(String, ClassLoader)
	methods to explicitly request the Coherence JCache provider implementation.  

	3 - Third way is to override the default cache provider using the javax.cache.spi.cachingprovider system property 
	and specifying the fully qualified name of the JCache provider implementation class. 

	NOTE : Service definition is located in the ncache-client maven package so that user can use ncache-provider without configuring it.

Build and Run the sample
=========================
    Run the following commands: mvn clean package
    Open your project in your favourite compiler and run the sample application.
    OR cd to target and run this command:
        cd target
        java -cp * com.alachisoft.ncache.samples.JCacheSample
        
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