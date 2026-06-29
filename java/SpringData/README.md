# Spring Sample

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
- **ncache-spring.xml** file changed according to the configuration.
  - **ncache-instance**: Each Spring cache is mapped on a specific cache instance. Multiple Spring caches can be
    configured to use the same cache instance. NCache cacheManager for Spring will keep record of each Spring's
    cache data in cache.
  - **name**: Spring cache name.
- ByDefault this sample uses 'demoCache', make sure cache is running.


### Build and Run the sample

#### From Terminal (Windows and Linux/macOS)
- Navigate to this projects directory and run the following commands:

```bash
mvn clean package
cd target
java -jar Spring-1.0-SNAPSHOT.jar
```

#### From IDE
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