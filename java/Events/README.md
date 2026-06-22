# EVENTS OPERATIONS SAMPLE

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

This sample is a Java (JDK 21) Maven console application which demonstrates NCache Events using demo `Product` data from the SampleData project. The sample shows how to subscribe to and handle both cache‑level and item‑level notifications so external logic can react to cache changes in real time.

### Cache‑level events
- Enables subscribing to notifications for additions, updates and removals across the entire cache.
- Workflow: register cache events (RegisterCacheEvents), trigger events via Add/Insert/Remove, then unregister handlers.

### Item‑level events
- Enables subscribing to notifications for a **specific key** to receive fine‑grained updates for that item only.
- Workflow: register item events for a key, perform operations to trigger events (Insert/Remove), then unregister item events.

This sample uses SampleData project as a reference for model class `Product`.

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
- Maven dependency required: `ncache-client` (version 5.3.6 or later).
- `SampleData` Maven project (contains the `Customer` model) is available locally.
  - It is located at: `../SampleData/` relative to this sample.
- Terminal commands are listed in each section but you can use any JAVA IDE (e.g. IntelliJ IDEA, NetBeans, Eclipse, VSCode, etc.) to achieve the same goal.

## Build and Run the Sample

This sample is a Maven wrapped Java application which means that there is no need to manually setup Maven in your environment. Maven wrapper will install all required Maven dependencies automatically. Before packaging the sample, ensure the following:

### Setting up SampleData dependency

 The `SampleData` Maven project (located at `../SampleData/`) needs to be installed in your environment first. To do this:
- Open a command prompt and navigate to the `SampleData` project location where the `pom.xml` file is located.
- Run the following commands to install the project:
    - For Windows:
      ```bash
      .\mvnw.cmd clean
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
      ./mvnw install
      ```

### Configuring Cache Name

By default, the cache name is set to `demoCache`. This is the cache created after installing NCache. If you have a different cache and need to change the cache name:
- Open `src/main/resources/app.properties` in any text editor.
- Replace `demoCache` with your desired cache name:
    ```bash
    CacheName=demoCache
    ```

### Enabling Events

- Follow these steps to enable cache-level events in the NCache Web Manager:
1. Open NCache Web Manager (Default URL: `http://localhost:8251/`)
    - Select your cache (this sample uses `demoCache`).
    - If the cache is running, stop it before making configuration changes.
2. Enable cache-level events
	- Go to `Advanced Settings`.
	- Click on `Options`.
	- Check `Item add notification. (OnItemAdded)`.
	- Check `Item remove notification. (OnItemRemoved)`.
	- Check `Item update notification. (OnItemUpdated)`.
	- Click `Save Changes`.

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
https://www.alachisoft.com/resources/docs/ncache/prog-guide/events-overview.html

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