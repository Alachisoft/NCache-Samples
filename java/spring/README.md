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
- **application.properties** file changed according to configuration.
  - **spring.datasource.url:** contains the database url, change database name IP address if needed.
  - **spring.datasource.username:** contains datasource username. 
  - **spring.datasource.password:** contains datasource password.
- ByDefault this sample uses 'demoCache', make sure cache is running.
### Build and Run the sample

- Run the following commands:
  ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
- OR cd to **target**(where jars are copied) folder and run this command:
  - ``` cd target ```
  - ``` java -jar ncache-spring-0.0.1-SNAPSHOT.war ```
  
- After running this sample application for the first time a table with a name book will be created in a 
  database. Initially, the table will empty there won't be any data, so there are two ways to create a new book:
  - either by creating a book one by one from the sample application by clicking on a Create new book button, 
    from this you can also check each book you have created is also cached in NCache by checking the cache count.
  - A quick way to add books to the table is by running an SQL script(sqlDataScript.sql) shipped with the sample, 
    and you can find it in resources. After running the script refresh the application in the browser, and you will 
    see the data.
  - After creating the items in the table you can cache any item either by searching them with ISBN or by updating 
    them. Both operations will cache the data in NCache, and the item will be removed after the expiration, or you 
    can also remove it by deleting the item by pressing the delete button.

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