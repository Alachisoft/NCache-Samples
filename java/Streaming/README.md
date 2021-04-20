# STREAMING

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample program demonstrates how to use the NCache Stream API to perform operations. 
Streaming allows to read data from cache in chunks just like any buffered stream.

### Prerequisites

Before the sample application is executed make sure that:
- **config.properties** have been changed according to the configurations. 
- Change the cache name
- ByDefault this sample uses 'demoCache', make sure cache is running. 

### Build and Run the Sample
    
- Run the following commands:
    ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
- OR cd to **target**(where jars are copied) folder and run this command:
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