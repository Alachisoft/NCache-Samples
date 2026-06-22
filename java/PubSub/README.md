# PUBSUB OPERATIONS SAMPLE

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

This sample is a Java (JDK 21) Maven console application which demonstrates NCache Publish/Subscribe (Pub/Sub) which lets apps broadcast typed messages to named topics and lets consumers subscribe (durable or non‑durable) to receive them. Use Pub/Sub for real‑time updates, notifications, dashboards, or decoupled event flows.

This sample shows publishing (synchronous, asynchronous, bulk) and both durable and non‑durable subscription patterns. There are three .NET 8.0 console application projects in this sample:

### Publisher
-	Demonstrates three publishing modes: synchronous (Publish), asynchronous (PublishAsync), and bulk (PublishBulk).
	- Ensures topics (ElectronicsOrders, GarmentsOrders) exist or creates them.
	- Creates tasks to publishes messages (sync, bulk and async).
	- Starts the tasks to send fixed number of messages.
	- Waits for a keypress from user; then deletes topics and disposes cache.

### NonDurableSubscriber
- 	Demonstrates non‑durable subscriptions for topics and pattern that receive messages only while connected.
	- Creates non‑durable subscriptions for specific topics.
	- Creates a pattern-based subscription (*Orders).
	- Prints received message notifications.
	- Waits for a keypress from user; then unsubscribes topics and disposes cache.

### DurableSubscriber
-	Demonstrate durable (persistent) subscriptions that can receive messages published while the subscriber was offline. Supports shared and exclusive policies and pattern subscriptions.
	- Creates durable subscriptions (with expiration).
	- Receives pending and live messages.
	- Waits for a keypress from user; then unsubscribes topics and disposes cache.

This sample uses SampleData project as a reference for model class `Order`.

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
    .\mvnw.cmd compile exec:java "-Dexec.mainClass=com.alachisoft.ncache.samples.subscriber.DurableSubscriber.DurableSubscriber"

    .\mvnw.cmd compile exec:java "-Dexec.mainClass=com.alachisoft.ncache.samples.subscriber.NonDurableSubscriber.NonDurableSubscriber"

    .\mvnw.cmd compile exec:java "-Dexec.mainClass=com.alachisoft.ncache.samples.Publisher.Publisher"
   ```

- For Linux/macOS:
  - First grant execution permissions to the wrapper script:

    ```bash
    chmod +x mvnw
    ```
  - Run the following commands to execute the sample:
    ```bash
    ./mvnw clean install
    ./mvnw compile exec:java "-Dexec.mainClass=com.alachisoft.ncache.samples.subscriber.DurableSubscriber.DurableSubscriber"

    ./mvnw compile exec:java "-Dexec.mainClass=com.alachisoft.ncache.samples.subscriber.NonDurableSubscriber.NonDurableSubscriber"

    ./mvnw compile exec:java "-Dexec.mainClass=com.alachisoft.ncache.samples.Publisher.Publisher"
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
https://www.alachisoft.com/resources/docs/ncache/prog-guide/publish-subscribe-ncache.html

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