# PUBSUB SAMPLE

## Table of Contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the Sample](#build-and-run-the-sample)
* [Troubleshooting](#troubleshooting)
* [References](#references)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

## Introduction

This .NET 8.0 console sample demonstrates NCache Publish/Subscribe (Pub/Sub) which lets applications broadcast typed messages to named topics and allows subscribers to receive those messages through durable or non-durable subscriptions. Pub/Sub is commonly used for real-time notifications, event-driven communication, dashboards, and other loosely coupled messaging scenarios.

The sample demonstrates synchronous, asynchronous, and bulk message publishing, as well as durable and non-durable subscription patterns. There are three .NET 8.0 console application projects in this sample:

### Publisher
-  Demonstrates three publishing modes: synchronous (Publish), asynchronous (PublishAsync), and bulk (PublishBulk).
	- Creates or retrieves the `ElectronicsOrders` and `GarmentsOrders` topics.
	- Creates tasks for synchronous, asynchronous, and bulk publishing.
	- Starts the tasks to publish a fixed number of messages to the topics.
	- Waits for user input before deleting the topics and disposing of the cache.

### NonDurableSubscriber
- 	Demonstrates non‑durable subscriptions for topics and pattern that receive messages only while the subscriber is connected.
	- Creates non‑durable subscriptions for specific topics.
	- Creates a pattern-based subscription for *Orders.
	- Prints received message notifications.
	- Waits for user input before unsubscribing and disposing of the cache.

### DurableSubscriber
-	Demonstrates durable (persistent) subscriptions, which continue to receive messages that were published while the subscriber was offline. Supports shared and exclusive policies and pattern subscriptions.
	- Creates durable subscriptions (with expiration).
	- Receives pending and live messages.
	- Waits for user input before unsubscribing and disposing of the cache.
	
This sample uses SampleData project as a reference for model class `Order`.

## Prerequisites

Before running the sample, ensure that:

- .NET 8.0 SDK and Visual Studio (recommended) are installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-distributed-cache.html
- NuGet package required: **Alachisoft.NCache.SDK (>= 5.3.6.1)**. The package reference is already included in the project file.
- SampleData project (which contains the `Order` model) must be present in the directory as this sample.

## Build and Run the Sample

Before building this sample, if you need to change the cache name, open `App.config` from the project root directory and change the `CacheName` value:

```xml
<appSettings>
  <add key="CacheName" value="demoCache"/>
</appSettings>
```

### Build the Sample

From Visual Studio (Recommended):
- Open `PubSub.sln`.
- Let NuGet packages restore.
- Build all projects in solution.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\PubSub
cd "<path-to-sample>"
dotnet restore
dotnet build
```

### Run the Sample

You need to run the projects `DurableSubscriber` and `NonDurableSubscriber` first and then run the `Publisher` project.

From Visual Studio (Recommended):
- Setup both projects as multiple startup projects by following the instructions on:\
https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects
- When both projects are running, start the `Publisher` by:
    - Right-clicking `Publisher` project → `Debug` → `Start New Instance`.
    - Or select `Publisher` project → `Debug` from top menu → `Start New Instance`.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\PubSub
# Run both projects one by one
cd "<path-to-sample>"\DurableSubscriber
dotnet run
cd "<path-to-sample>"\NonDurableSubscriber
dotnet run
# Wait for both subscribers to start, then run the publisher.
cd "<path-to-sample>"\Publisher
dotnet run
```

## Troubleshooting

### Missing SampleData Project

- Ensure the sample solution references SampleData before building:
   - **Visual Studio**: Right-click the solution → Add → Existing Project → `SampleData\SampleData\SampleData.csproj`
   - **Command line**: Add the project reference or edit the solution file to include the SampleData project.

### NuGet Packages Are Not Restored

- If NuGet packages are not restored properly:
	- **Visual Studio**: Right‑click the solution → Restore NuGet Packages.
	- **Command line**: Run `dotnet restore` in the sample folder (see [Build and Run](#build-and-run-the-sample) section).
	- **nuget.exe**: Run `nuget restore PubSub.sln`.

## References

For more information about PubSub, see:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/publish-subscribe-ncache.html

## Additional Resources

### Samples & Playground

For more samples of NCache features on various platforms:\
https://github.com/Alachisoft/NCache-Samples/

You can also visit NCache Playground for an interactive feature demo:\
https://www.alachisoft.com/nclive/

### Documentation

The complete online documentation for NCache is available at:\
https://www.alachisoft.com/resources/docs/

### Developer's Guide

The complete developer's guide of NCache is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/

## Technical Support

Alachisoft&copy; provides various sources of technical support. 

- Please refer to https://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

## Copyrights

Copyright 2026 Alachisoft&copy;