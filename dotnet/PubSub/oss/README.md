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

This .NET 8.0 console sample demonstrates NCache Publish/Subscribe (Pub/Sub) which lets apps broadcast typed messages to named topics and lets consumers subscribe (durable or non‑durable) to receive them. Use Pub/Sub for real‑time updates, notifications, dashboards, or decoupled event flows.

This sample shows publishing (synchronous) and non‑durable subscription patterns. There are three .NET 8.0 console application projects in this sample:

### Publisher
-	Demonstrates three publishing modes: synchronous (Publish).
	- Ensures topics (ElectronicsOrders) exist or creates them.
	- Creates tasks to publishes messages (sync).
	- Starts the tasks to send fixed number of messages.
	- Waits for a keypress from user; then deletes topics and disposes cache.

### NonDurableSubscriber
- 	Demonstrates non‑durable subscriptions for topics and pattern that receive messages only while connected.
	- Creates non‑durable subscriptions for specific topics.
	- Prints received message notifications.
	- Waits for a keypress from user; then unsubscribes topics and disposes cache.
	
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
- NuGet package required: **Alachisoft.NCache.Opensource.SDK (>= 5.3.6.2)**. The package reference is already included in the project file.
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

## Notes

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