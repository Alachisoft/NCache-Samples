# BACKING SOURCE

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction



### Prerequisites

Before the sample application executed make sure that:

- **client.ncconf**, **NCacheHibernate.xml** have been changed according to the configurations. 
	- change the cache name in **client.ncconf** and **NCacheHibernate.xml**.
- ByDefault this sample uses 'demoCache', make sure the cache is running. 
- Oracle database must be configured.
- Database UserName, password and connection-string must be configured in **NCacheHibernate.xml**.

### Build and Run the Sample
    
- Install com.alachisoft.ncache ncache-hibernate mvn package.(To get latest NCache client Maven package go to this link)
- https://mvnrepository.com/artifact/com.alachisoft.ncache/ncache-hibernate
- Add it in the **pom.xml** file inside **dependencies** section like this:
    - ```
      <dependencies>
              <dependency>
                  <groupId>com.alachisoft.ncache</groupId>
                  <artifactId>ncache-hibernate</artifactId>
                  <version>5.2.0</version>
              </dependency>
              <!-- https://mvnrepository.com/artifact/org.hibernate/hibernate-core -->
              <dependency>
                  <groupId>org.hibernate</groupId>
                  <artifactId>hibernate-core</artifactId>
                  <version>5.4.4.Final</version>
              </dependency>
              <!-- https://mvnrepository.com/artifact/org.hibernate/hibernate-jcache -->
              <dependency>
                  <groupId>org.hibernate</groupId>
                  <artifactId>hibernate-jcache</artifactId>
                  <version>5.4.4.Final</version>
              </dependency>
      .....
      </dependencies> 
      ```
- Run the following commands:
    ``` mvn clean package ```
- Open your project in your favourite compiler and run the sample application.
- Or open command prompt and go to the directory where you have saved the project.
- ``` mvn exec:java -Dexec.mainClass=com.alachisoft.ncache.samples.Main ```
- OR cd to target and run this command: 
``` java -cp * com.alachisoft.ncache.samples.Main ```

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