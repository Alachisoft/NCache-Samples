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

This sample program demonstrates how to use the Cache Item Version to perform CRUD operations. 
It shows how to Add, Get, Update and Delete an object(s) using cache an item version from NCache.

This sample uses SampleData project as a reference for model class "Customer".

### Prerequisites

Before the sample application executed make sure that:

- **config.properties** have been changed according to the configurations. 
	- Change the cache name
- By default, this sample uses 'demoCache', make sure cache is running. 


### Build and Run the Sample
    
- Install com.alachisoft.ncache client mvn package.(To get latest NCache client Maven package go to this link)
- https://mvnrepository.com/artifact/com.alachisoft.ncache/ncache-client
- Add it in the **pom.xml** file inside **dependencies** section like this:
    - ```
      <dependencies>
      <!-- https://mvnrepository.com/artifact/com.alachisoft.ncache/ncache-client -->
            <dependency>
                <groupId>com.alachisoft.ncache</groupId>
                <artifactId>ncache-client</artifactId>
                <version>5.2.0</version>
            </dependency>
      .....
      </dependencies> 
      ```
- Run the following commands:
    ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
- Or open command prompt and go to the directory where you have saved the project.
    - ``` mvn exec:java -Dexec.mainClass=com.alachisoft.ncache.samples.Main ```
- OR cd to target and run this command: 
	- ``` cd target ```
    - ``` java -cp * com.alachisoft.ncache.samples.Main ```

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