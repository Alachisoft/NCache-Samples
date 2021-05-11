# SQL DEPENDENCY

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

- This sample explains how NCache supports dependent items to be added, and what is the behaviour if dependency has a change.
- Sample adds one item dependent upon a record in the database.
- Any modification in the database record will result in the invalidation of the cache item and thus the item will be removed from the cache.
- This sample deals with the Database dependency feature of NCache.
- This sample uses northwind database. 

### Prerequisites

Before the sample application is executed make sure that:

- Enable the service broker in SQL Server 2005 or above before NCache can use SqlCacheDependency
```
[SQL]
ALTER DATABASE Northwind SET ENABLE_BROKER;
GO
```
- The following permissions need to be enabled in order to use NCache SQL cache dependency if the user does not have database permissions. 
Database permissions are defined for two different modes: Default mode and Custom mode.
- Default Mode
```
GRANT SUBSCRIBE QUERY NOTIFICATIONS TO <database_principal>
GRANT CREATE QUEUE TO <database_principal>
GRANT CREATE SERVICE TO <database_principal>
GRANT CREATE PROCEDURE TO <database_principal>
```
- Custom Mode
```
CREATE QUEUE "NCacheSQLQueue-[IP-Address]";
CREATE SERVICE "NCacheSQLService-[IP-Address]"
ON QUEUE."NCacheSQLQueue-[IP-Address]"([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);
GRANT SUBSCRIBE QUERY NOTIFICATIONS TO[User Name];
GRANT RECEIVE ON :: "NCacheSQLQueue-[IP-Address]"TO[User Name];
GRANT RECEIVE ON :: QueryNotificationErrorsQueue TO[User Name];
GRANT SEND ON SERVICE :: "NCacheSQLService-[IP-Address]"to[User Name];
```
- app.config.json have been changed according to the configurations. 
	- Change the cache name 
	- conn-string to connect with database.
	
- By default this sample uses 'demoCache', make sure that cache is running. 

### Build and Run the Sample
   
- Run command 
	- '''npm install'''
- To Run the sample application, execute following command
	- ''' node ./index.js'''     

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