# ASYNCHRONOUS OPERATIONS SAMPLE

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

Asynchronous cache operations enable applications to perform cache reads and writes without blocking the calling thread. Rather than waiting for each cache operation to complete, the application can continue executing other tasks while NCache processes the request in the background.

This sample is a .NET 8.0 console application that demonstrates how to use NCache [asynchronous APIs](https://www.alachisoft.com/resources/docs/ncache/prog-guide/async-crud-operations.html) to perform CRUD operations on data stored in the cache. It shows how to [connect to a running cache](https://www.alachisoft.com/resources/docs/ncache/prog-guide/connect-to-cache.html) and use [AddAsync](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Client.ICache.AddAsync.html), [InsertAsync](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Client.ICache.InsertAsync.html#Alachisoft_NCache_Client_ICache_InsertAsync_), and [RemoveAsync](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Client.ICache.RemoveAsync.html#Alachisoft_NCache_Client_ICache_RemoveAsync_) methods to add, update, and remove cache items asynchronously. The sample disposes of the cache instance before the application exits to release resources.

This sample references the `SampleData` project, which contains the `Customer` model used throughout the sample.

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
- SampleData project (which contains the `Customer` model) must be present in the directory as this sample.

## Build and Run the Sample

Before building this sample, if you need to change the cache name, open `App.config` from the project root directory and change the `CacheName` value:

```xml
<appSettings>
  <add key="CacheName" value="demoCache"/>
</appSettings>
```

From Visual Studio (Recommended):
- Open `AsynchronousOperations.sln`
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\AsynchronousOperations
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
  
- If NuGet packages are not restored correctly:
	- **Visual Studio**: Right‑click the solution → Restore NuGet Packages.
	- **Command line**: Run `dotnet restore` in the sample folder (see [Build and Run](#build-and-run-the-sample) section).
	- **nuget.exe**: Run `nuget restore AsynchronousOperations.sln`.

### Cannot Connect to Cache

- Verify the NCache service is running.
- Verify that the configured cache (for example, `demoCache`) exists and is running.
- Verify that the cache name in `App.config` matches the running cache.
- Verify that [firewall rules](https://www.alachisoft.com/resources/docs/ncache/install-guide/firewalls-ports.html) allow communication with the cache server.

## References

For more information about Asynchronous Cache Operations, see:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/async-crud-operations.html

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