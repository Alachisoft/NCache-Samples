# NCache Spring Sessions

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)


### Introduction

This sample application demonstrates how to use NCache as a backing store for Spring Session.

Spring Session simplifies session management by externalizing HTTP session data to a centralized store. 
In this sample, NCache acts as the distributed cache provider, enabling scalable, fault-tolerant session 
storage across multiple application instances.

### PREREQUISITES

Before running the application please make sure that:
	
  - Files ncache-spring.xml and client.ncconf must pe present inside NCHOME/config.
  - Cache(s) specified in ncache-spring.xml must exist in the 'client.ncconf' with correct server ip address and these caches should be started.

Example configuration for ncache-spring.xml:
```
<application-config default-cache-name="demoCache">
<caches>
<cache name="demoCache" ncacheid-instance="demoCache" priority="normal" expiration-type="absolute" expiration-period="300"/>
</caches>
</application-config>
```


## Build and Run the sample

- Open your project in your favourite IDE and run SessionSpringApplication.

- Access the following url: 
  http://localhost:8080/session (specify correct host and port 
  according to the system specifications).

If there are any startup errors regarding dependencies, run the following command from terminal while
in this project's directory

**For Windows/Linux/macOS:**
```bash
mvn clean install
```
  
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