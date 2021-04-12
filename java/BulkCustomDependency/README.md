# BULK CUSTOM DEPENDENCY

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample implements the Bulk Custom dependency feature of the NCache. In custom dependency, you can implement your 
custom logic which defines when a certain data becomes invalid for cached items. This sample implements Bulk Custom 
Dependency and adds items to cache with this bulk custom dependency and then updates item in database. Bulk Custom depedency 
invalidate data from database and when the dependency expires, the items are removed  from the cache.

### Prerequisites

Before the sample application executed make sure:

- **config.properties** have been changed according to the configurations. 
	- The default cache name is "demoCache".
	- connString: the connection string used to connect to the database. ByDefault it connects to the localhost.
	- user: the user name of the database.
	- pass: the password of the database.
- To deploy Custom Dependency, Build 'CustomDependencyImpl' project.
	- Start NCache Web Manager and create a clustered cache with the name specified in config.properties.
	- Click on the **View Details** and scroll down to the **Advanced Settings**.
	- Click on 'Deploy Providers' in the Cache Setting(...) of clustered cache's page.
	- Select BulkCustomDependency.jar and click Ok.
	- Provide connString, userName and password as the parameters.
	- Save changes.

- ByDefault this sample uses 'demoCache', make sure the cache is running. 

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

[C] Copyright 2021 Alachisoft 