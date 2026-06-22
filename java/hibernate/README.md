# NHIBERNATE SAMPLE

## Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Notes](#notes)
* [References](#references)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

## Introduction

This Java (JDK 21) Maven console application shows how to integrate NCache into Hibernate applications by using NCache as a second-level cache. The application shows how to cache database items and queries using Hibernate in order to reduce database load.

Key features covered include query caching (`getResultList`), single entry retrievals (`getSingleResult`), addition of entries (`persist`) and removal of entries and queries (`evictEntityData` and `evictQueryRegions`) from database and cache. The sample uses an altered version of `Microsoft's Northwind dataset` to illustrate realistic scenarios.

## Prerequisites

Before building the sample application, make sure that:

- JDK 21 (required) is installed and configured. Verify using:
  ```bash
  java --version
  ```
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html
- Maven dependency required: 
  - ncache-hibernate (version 5.3.6 or later).
  - hibernate-core (version 5.6.16-Final)
  - javax.persistence-api (version 2.2)
  - jaxb-core (version 2.3.0.1)
  - jaxb-impl (version 2.3.3)
  - validation-api (version 2.0.1.Final)
- All process commands are defined in CLI but optionally any JAVA IDE (e.g. IntelliJ IDEA, NetBeans, Eclipse, VSCode, etc.) can be used to achieve the same goal.

## Build and Run the Sample

This sample is a Maven wrapped Java application which means that there is no need to manually setup Maven in your environment. Maven wrapper will install all required Maven dependencies automatically. Before building and running the sample, ensure the following:

### Configuring Cache Name

By default, the cache name is set to `demoCache`. This is the cache created after installing NCache. If you have a different cache and need to change the cache name:
- Open `src/main/resources/ncache-hibernate.xml`
- Modify the value of `cache-name` property to your desired cache name.
```xml
<cache-regions>
	<region name="default" cache-name="demoCache" priority="default" expiration-type="none" expiration-period="0" use-async="false"/>
</cache-regions>
```

### Database Setup

#### Populating the database

- Run the script included with the sample: `HibernateScript.sql` (located in this sample folder).
- Using SQL Server Management Studio (SSMS):
	- Connect to your SQL Server / SQL Express instance (e.g. .\SQLEXPRESS).
	- Open NHibernateScript.sql and click Execute.
- Using sqlcmd (Windows):
  - Integrated security:
	```bash
	sqlcmd -S .\SQLEXPRESS -E -i "<path-to-sample>\HibernateScript.sql"
	```
  - SQL authentication:
	```bash
	# Replace <server> with server IP or name, <user> and <password> with credentials
	sqlcmd -S <server> -U <user> -P <password> -i "<path-to-sample>\HibernateScript.sql"

#### Troubleshooting

- Ensure the SQL account has permissions to read the tables and that the connection string is correct.

### Running the Sample

To run the sample application, open a command prompt and navigate to the sample project location where the `pom.xml` file is located. Then execute the following commands:

- For Windows:
    ```bash
    .\mvnw.cmd clean install
    .\mvnw.cmd exec:java
    ```
- For Linux/macOS:
  - First grant execution permissions to the wrapper script:

    ```bash
    chmod +x mvnw
    ```
  - Run the following commands to execute the sample:
    ```bash
    ./mvnw clean install
    ./mvnw exec:java
    ```
    
## Notes

- If Maven dependencies are not loaded or configured properly:
    - For Windows:
    ```bash
    .\mvnw.cmd clean
    .\mvnw.cmd dependency:resolve
    .\mvnw.cmd install
    ```
    - For Linux/macOS:
    ```bash
    ./mvnw clean
    ./mvnw dependency:resolve
    ./mvnw install
    ```

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/basic-cache-operations.html

## Additional Resources

### Samples & Playground

For more samples of NCache features on various platforms:\
https://github.com/Alachisoft/NCache-Samples/

You can also visit NCache Playground for an interactive feature demo:\
https://www.alachisoft.com/nclive/

### Documentation

The complete online documentation for NCache is available at:\
http://www.alachisoft.com/resources/docs/#ncache

### Programmer's Guide
The complete programmer's guide of NCache is available at:\
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

## Technical Support

Alachisoft&copy; provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

## Copyrights

Copyright 2026 Alachisoft&copy;