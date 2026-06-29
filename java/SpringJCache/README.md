# Spring JCache integration with NCache

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample application demonstrates how to use NCache as a JCache (JSR-107) provider in a Spring application.

Spring’s JCache abstraction enables applications to interact with caches using standard JSR-107 APIs. 
By integrating NCache, this sample shows how to leverage distributed and scalable caching while keeping the 
application decoupled from the underlying cache implementation.

### PREREQUISITES

Ensure that:
- **application.properties** file exists in the resources folder and 
 JCache provider properties are defined in the following manner: 

 ```properties
spring.cache.type=jcache
spring.cache.jcache.provider=com.alachisoft.ncache.jsr107.spi.NCacheCachingProvider
spring.cache.cache-names=demoCache
```

 - By default, this sample uses 'demoCache', make sure the cache is running.


### Build and Run the sample

- Open the sample in your favourite IDE.
- Run the application from SpringSample.java.
- Open the web browser and type in http://localhost:8080/products.

If there are any startup errors regarding dependencies, run the following command from terminal while
in this project's directory:

**For Windows/Linux/macOS:**
```bash
mvn clean install
```

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

[C] Copyright 2026 Alachisoft 