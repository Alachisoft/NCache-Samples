# COMPACT SERIALIZATION

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

- This is a sample application based on 'Compact Serialization' feature. 
- This program performs Add operation 100 times in a loop. It uses the iteration number as a key and user picked data 
    type as value to be added to the cache. This Sample contains some objects which are compact serializable. 
    These objects include: 
	- BiggerObject
	- BigObject
	- NormalObject
	- Small Object
- All these objects implement ICompactSerialization. Notice that they implement Serialize and Deserialize functions of 
    ICompactSerialize interface.

### Prerequisites

Before the sample application executed make sure that:

- config.properties have been changed according to the configurations. 
	- Change the cache name
- By default, this sample uses 'demoCache', make sure cache is running. 

- To use your compact serializable objects with NCache, you must first 'Register your application objects with NCache'.
	- Start NCache Web Manager and create a clustered cache with the name specified in config.properties. 
	- Now select the 'Compact Serialization' tab in the "Advanced Settings" of cache's details page 
	- Click the 'Add Types' button to add your types. Browse for the 'CompactSerialization-5.2.0.jar' file.
	  If specified dll is not present already, you will have to build 'CompactSerializationImpl' project.
	- Add compact types by browsing 'CompactSerialization-5.2.0.jar'.
	- Select the com.alachisoft.ncache.samples.compactSerializationImpl types from the list shown in the assembly browser and click 'Add Classes'.
	- Save changes.

### Build and Run the Sample
- Run the following commands:
    - ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
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
