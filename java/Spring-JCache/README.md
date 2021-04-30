# Spring JCache Sample

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample demonstrates the usage of NCache with Spring Boot.
NCache provides the integration with Spring Framework as well as Spring Boot and the user can easily plug in NCache to
their existing Spring applications. The only need is to add in the configuration files to take advantage of the NCache
distributed system.

### PREREQUISITES

Before running this sample make sure that:
- If you have more than one provider of JCache in your .classpath, then the tags **spring.cache.jcache.provider** and
  **spring.cache.type** need to be explicitly added in the **application.properties** file.
```
  spring.cache.jcache.provider=com.alachisoft.ncache.jsr107.spi.NCacheCachingProvider
  spring.cache.type=jcache
```
- Configure Caches:
    - To configure your cache for JCache Spring application, if you want to initialize your cache at the application
      startup, then for that you need to add the **spring.cache.cache-names** tag in the **application.properties** file.
      The cache name should be the same as configured in Web Manager.
```
  spring.cache.cache-names=demoCache
```
- Also, the cache can also be configured through the **JCacheManagerCustomizer** class which initializes the cache at
  runtime with the desired configuration.
```
  cacheManager.createCache("demoCache", mutableConfiguration);
```
- ByDefault this sample uses 'demoCache', make sure cache is running.

### Build and Run the sample

- Run the following commands:
  ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
- OR cd to **target**(where jars are copied) folder and run this command:
    - ``` cd target ```
    - ``` java -cp * ncache.spring.jcache-0.0.1-SNAPSHOT.war ```

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