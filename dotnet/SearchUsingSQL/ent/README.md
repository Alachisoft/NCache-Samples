# SEARCH USING SQL SAMPLE

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

This sample is a .NET 8.0 console application which demonstrates performing SQL‑like queries against cached objects in NCache. The sample inserts Product objects into the cache with associated Tags and NamedTags. It then demonstrates SQL-like queries against named tags and indexed object attributes, along with projection queries. Each query uses `QueryCommand` and reads results through an `ICacheReader`, and finally disposes the cache.

This sample specifically demonstrates:
- Loading data into NCache initially using `AddSampleData`.
- Running SQL-like queries to:
    - Retrieve products which are still under warranty.
    - Retrieve products which have a unit price greater than 100.
    - Retrieve product name and category which have a unit price of greater than 100.

This sample uses SampleData project as a reference for model class `Product`.

## Prerequisites

Before the sample application is executed, make sure that:

- .NET 8.0 SDK or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
    https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-distributed-cache.html
- NuGet package required: Alachisoft.NCache.SDK (>= 5.3.6.1). The package reference is already included in the project file.
- SampleData project (which contains the `Product` model) must be present in the directory as this sample.

## Build and Run the Sample

To run the sample, first you need to build the sample and then configure the cache to index the `Product` objects and then run the code.

### Build

If you need to change the cache name, open `App.config` from the project root directory and change the `CacheName` value:

```xml
<appSettings>
  <add key="CacheName" value="demoCache"/>
</appSettings>
```

From Visual Studio (Recommended):
- Open `SearchUsingSQL.sln`.
- Let NuGet packages restore.
- Build.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\SearchUsingSQL
cd "<path-to-sample>"
dotnet restore
dotnet build
```

### Configuration (Query Indexes)

Configure the cache to index the Product class so queries run efficiently:

1. Build SampleData
    - Build the solution so `SampleData.dll` is produced in: `SampleData\bin\Debug\net8.0\SampleData.dll` (or release folder if built in Release).
2. Open the NCache Management Center (Default URL: `http://localhost:8251/`).
    - Select your cache (this sample uses `demoCache`).
    - If the cache is running, stop it before making configuration changes.
3. Configure query index
    - Click `View Details` and select `Query Indexes` tab in the `Advanced Settings (Clustered Cache)` section.
    - Click `Add` then `Browse`, locate and select the `SampleData.dll` from the build output folder.
    - In `Browsed Assemblies` expand the assembly, locate and check the "Product" class as below and click `Add Selected Classes` button.
        - [ ] `SampleData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`
            - [ ] `Alachisoft.NCache.Sample.Data`
                - [x] `Alachisoft.NCache.Sample.Data.Product`
4. Select attributes to index
    - In the `Select Attributes`, check all the properties (checking the parent will select all).
    - Click `OK` button 
5. Save changes and Start Cache
    - Click the `Save Changes` button.
    - Start the cache.

### Run the sample

After configuring the cache, you can run the sample from Visual Studio. If you are using command line, navigate to the path (see Build section) and use the following command:
```bash
dotnet run
```


## Troubleshooting

### Assembly Does Not Appear or Classes Are Missing

- Verify that you selected the correct assembly (`SampleData.dll`) when configuring the cache.
- Rebuild the sample project if the assembly was recently modified.
- Ensure the assembly contains public classes that can be discovered.

### Cache Fails to Start After Adding Indexes

If the cache fails to start after adding indexes:

1. Remove the newly added index.
2. Start the cache.
3. Resolve any issues in the assembly (such as invalid types or missing dependencies).
4. Rebuild the assembly and re-add the index.


### Missing SampleData Project

- Ensure the sample solution references SampleData before building:
   - **Visual Studio**: Right-click the solution → Add → Existing Project → `SampleData\SampleData\SampleData.csproj`
   - **Command line**: Add the project reference or edit the solution file to include the SampleData project.

### NuGet Packages Are Not Restored

- If NuGet packages are not restored properly:
	- **Visual Studio**: Right‑click the solution → Restore NuGet Packages.
	- **Command line**: Run `dotnet restore` in the sample folder (see Build section).
	- **nuget.exe**: Run `nuget restore SearchUsingSQL.sln`.


## References

For more information about Searching Cache with SQL, see:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/search-cache-with-sql.html

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