# DISTRIBUTED COUNTER SAMPLE

## Table of Contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the Sample](#build-and-run-the-sample)
* [Troubleshooting](#troubleshooting)
* [Reference](#reference)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

## Introduction

A Distributed Counter is a cluster-wide, atomic long value stored in the cache that can be safely incremented, decremented, read, and reset by multiple clients concurrently. All operations are performed atomically across the cache cluster, ensuring consistency in distributed environments.

This .NET 8.0 console sample demonstrates the NCache Distributed Counter data structure. It shows how to create or retrieve a counter from the cache, perform atomic increment and decrement operations, retrieve its current value, reset it, and remove it from the cache.

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

## Build and Run the Sample

Before building this sample, if you need to change the cache name, open `App.config` from the project root directory and change the `CacheName` value:

```xml
<appSettings>
  <add key="CacheName" value="demoCache"/>
</appSettings>
```

From Visual Studio (Recommended):
- Open `DistributedCounter.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\DataStructures\DistributedCounter
cd "<path-to-sample>"
dotnet restore
dotnet build
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
	- **nuget.exe**: Run `nuget restore DistributedCounter.sln`.

### Cannot Connect to Cache

- Verify the NCache service is running.
- Verify that the configured cache (for example, `demoCache`) exists and is running.
- Verify that the cache name in `App.config` matches the running cache.
- Verify that [firewall rules](https://www.alachisoft.com/resources/docs/ncache/install-guide/firewalls-ports.html) allow communication with the cache server.

## References

For more information about Distributed Counters, see:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/counter-datatype.html

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



