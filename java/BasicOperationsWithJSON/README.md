# BASIC OPERATIONS WITH JSON

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample program demonstrates how to use the NCache API to perform CRUD operations with JSON objects. 
It shows how to initialize the cache instance and Add, Get, Update and Remove JSON object(s) from NCache.

This sample uses SampleData project as a reference for model class "Customer" and uses Newtonsoft for JSON serialization.

### Prerequisites

Before the sample application executed make sure that:

- **config.properties** have been changed according to the configurations. 
	- Change the cache name
- ByDefault this sample uses 'demoCache', make sure the cache is running. 
- jdk 11 or higher is required.
- NCache Java client installed on the machine.   

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
    - ``` mvn clean package ```
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

[C] Copyright 2020 Alachisoft 