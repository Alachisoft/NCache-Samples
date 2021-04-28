# Hibernate Sample

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction
NCache provides Hibernate's second level cache provider. NCache provider can be configured as Hibernate's 2nd level 
cache without any code change. Using NCache as 2nd level cache for Hibernate enables applications to use NCache's 
distributed caching features.


### Prerequisites

Before the sample application executed make sure that:

- **client.ncconf**, **NCacheHibernate.xml** have been changed according to the configurations. 
	- change the cache name in **client.ncconf** and **NCacheHibernate.xml**.
- ByDefault this sample uses 'demoCache', make sure the cache is running. 
- Oracle database must be configured.
- Database UserName, password and connection-string must be configured in **NCacheHibernate.xml**.

### Build and Run the Sample
- Run the following commands:
    ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
- OR cd to target and run this command: 
``` java -cp * hibernator.BLL.Main ```

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for 
    your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, 
    please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

[C] Copyright 2021 Alachisoft 