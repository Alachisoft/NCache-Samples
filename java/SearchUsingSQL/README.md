# SEARCH USING SQL SAMPLE

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

This sample is a Java (JDK 21) Maven console application demonstrates performing SQL‑like queries against cached objects in NCache. It inserts `Product` objects as `CacheItems` with `Tags` and `NamedTags`, then shows how to run parameterized queries (named‑tag filters), indexed attribute queries, and projection queries to retrieve or project specific fields from the cache. Each query uses `QueryCommand` and reads results through an `ICacheReader`, and finally dispose the cache.

This sample specifically demonstrates:
- Loading data into NCache initially using `AddSampleData`.
- Running LINQ to perform the following actions:
    - Retrieve products which are still under warranty.
    - Retrieve products which have a unit price greater than 100.
    - Retrieve product name and category which have a unit price of greater than 100.

This sample uses the SampleData Maven project as a reference for model class `Product`.

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
- Maven dependency required: ncache-client (version 5.3.6 or later).
- `SampleData` Maven project (contains the `Product` model) is available locally.
  - It is located at: `../SampleData/` relative to this sample.
- All process commands are defined in CLI but optionally any JAVA IDE (e.g. IntelliJ IDEA, NetBeans, Eclipse, VSCode, etc.) can be used to achieve the same goal.

## Build and Run the Sample

This sample is a Maven wrapped Java application which means that there is no need to manually setup Maven in your environment. Maven wrapper will install all required Maven dependencies automatically. Before building and running the sample, ensure the following:

### Setting up SampleData dependency

 The `SampleData` Maven project (located at `../SampleData/`) needs to be built and installed in your environment first. To do this:
- Open a command prompt and navigate to the `SampleData` project location where the `pom.xml` file is located.
- Run the following commands to build and install the project:
    - For Windows:
      ```bash
      .\mvnw.cmd clean
      .\mvnw.cmd package
      .\mvnw.cmd install
      ```
    - For Linux/macOS:
      - First grant execution permissions to the wrapper script:

        ```bash
        chmod +x mvnw
        ``` 
      - Then execute the following:
      ```bash
      ./mvnw clean
      ./mvnw package
      ./mvnw install
      ```

### Configuration (Query Indexes)

Configure the cache to index the Product class so queries run efficiently:

1. Package SampleData
    - Package the project so `SampleData-1.0-SNAPSHOT.jar` is produced in: `SampleData\target\SampleData-1.0-SNAPSHOT.jar` (or release folder if built in Release).
2. Open NCache Web Manager (Default URL: `http://localhost:8251/`).
    - Select your cache (this sample uses `demoCache`).
    - If the cache is running, stop it before making configuration changes.
3. Configure query index
    - Click `View Details` and select `Query Indexes` tab in the `Advanced Settings (Clustered Cache)` section.
    - Click `Add` then `Browse`, locate and select the `SampleData-1.0-SNAPSHOT.jar` from the build output folder.
    - In `Browsed Assemblies` expand the assembly, locate and check the "Product" class as below and click `Add Selected Classes` button.
        - [ ] `SampleData-1.0-SNAPSHOT.jar`
            - [ ] `com.alachisoft.ncache.samples.data`
                - [x] `com.alachisoft.ncache.samples.data.Product`
4. Select attributes to index
    - In the `Select Attributes`, check all the properties (checking the parent will select all).
    - Click `OK` button 
5. Save changes and Start Cache
    - Click the `Save Changes` button.
    - Start the cache.

### Configuring Cache Name

By default, the cache name is set to `demoCache`. This is the cache created after installing NCache. If you have a different cache and need to change the cache name:
- Open `src/main/resources/app.properties`
- Modify the value of `CacheName` property to your desired cache name.
```bash
CacheName=demoCache
```

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