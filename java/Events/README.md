# EVENTS

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample program demonstrates how to use the NCache Events and Event Notifications. 
It shows how to initialize the cache instance and Add, Get, Update and Delete object(s) from NCache and receive events.
These events cover Selective events, General events and Custom events.

This sample uses SampleData project as a reference for model class "Product".

### Prerequisites

Before the sample application is executed make sure that:

- config.properties have been changed according to the configurations. 
	- change the cache name
- Event notifications must be enabled in NCache Web Manager 
	- Start NCache Web Manager and create a clustered cache with the name specified in app.config. 
	- Now select the Options' tab in the "Advanced Settings" of cache's details page. 
	- Enable event notifications (item-add, item-remove, item-update)
	- Save changes.
- By default, this sample uses 'demoCache', make sure that cache is running. 
- This sample requires sample data project. Build SampleData project and install its mvn package using the following commands:
    - Open command prompt.
    - cd to the SampleData directory.
    - run: 
        ``` mvn clean package ```

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
- ``` mvn exec:java -Dexec.mainClass=com.alachisoft.ncache.samples.Events ```
- OR cd to target and run this command: 
	- ``` cd target ```
	- ``` java -cp * com.alachisoft.ncache.samples.Events ```

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