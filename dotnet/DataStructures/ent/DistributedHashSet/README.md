# DISTRIBUTED HASHSET SAMPLE

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

A Distributed HashSet is a cluster‑wide, thread‑safe collection of unique values that supports atomic operations and efficient enumeration from any client in the cache cluster. This .NET 8.0 console sample demonstrates the NCache Distributed HashSet data structure.

The sample is organized into focused scenarios that demonstrate common HashSet operations:
- **Basic Operations Using Weekdays**: Demonstrates adding multiple values with AddRange, adding individual values with Add (including an attempt to add a duplicate), checking for value existence with Contains, removing values with Remove, and iterating through all values in the distributed HashSet.
- **Performing a Union**: Combine two distributed sets into a single result set using two sets of numbers divisible by 3 and 5.
- **Performing an Intersection**: Compute the common elements between sets using two sets of numbers divisible by 3 and 5.
- **Performing a Complement (Difference)**: Remove the elements of one set from another to determine the remaining values using two sets of names, one containing random names and the other containing only four-letter names.

## Prerequisites

Before running the sample, ensure that:

- .NET 8.0 SDK and Visual Studio (recommended) are installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-distributed-cache.html
- NuGet package required: **Alachisoft.NCache.SDK (>= 5.3.6.1)**. The package reference is already included in the project file.

## Build and Run the Sample

From Visual Studio (Recommended):
- Open `DistributedHashSet.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\DistributedHashSet
cd "<path-to-sample>"
dotnet restore
dotnet build
dotnet run
```

If you need to change the cache name, open `App.config` from the project root directory and change the `CacheName` value:

```xml
<appSettings>
  <add key="CacheName" value="demoCache"/>
</appSettings>
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
	- **nuget.exe**: Run `nuget restore DistributedHashSet.sln`.

### Unsupported Data Types

NCache Distributed HashSet supports **only primitive and string data types**.

- Creating a Distributed HashSet with **unsupported data types does not throw an exception**.
- An exception is thrown when you **perform operations on the HashSet (such as adding or retrieving values)**.

### Cannot Connect to Cache

- Verify the NCache service is running.
- Verify that the configured cache (for example, `demoCache`) exists and is running.
- Verify that the cache name in `App.config` matches the running cache.
- Verify that [firewall rules](https://www.alachisoft.com/resources/docs/ncache/install-guide/firewalls-ports.html) allow communication with the cache server.

## References

For more information about Distributed HashSet, see:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/set-datatype.html

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

