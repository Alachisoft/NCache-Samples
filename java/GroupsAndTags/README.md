# GROUPS AND TAGS

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample program demonstrates how to use the Groups and Sub-groups, Tags and NamedTags features of NCache. 
This sample demonstrates the use of Group/Subgroup, Tags and NamedTags feature of NCache. 
	Group/Subgroup:
		Adds items with Group/Subgroup into the cache. and uses API to get the items using Group/Subgroup.
	Tags:
		Adds items with Tags, and use different Tag API to fetch the tagged items from the cache.
	NamedTags:
		Adds items with NamedTags, and query the NCache using OQL to fetch the named tagged items.

This sample uses SampleData project as a reference for model class "Product" and "Order".

### Prerequisites

Before the sample application is executed make sure that:

- config.properties have been changed according to the configurations. 
	- change the cache name
	
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
- ``` mvn exec:java -Dexec.mainClass=com.alachisoft.ncache.samples.Main ```
- OR 
	- To run with java:
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