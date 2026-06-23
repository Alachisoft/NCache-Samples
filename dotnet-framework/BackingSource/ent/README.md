# BACKING SOURCE SAMPLE

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

This sample demonstrates NCache ReadThru and WriteThru backing source features to keep cached data synchronized with the underlying database. These features are essential in scenarios where applications frequently read or update data and require the cache to always remain consistent with the datastore. It is implemented as a .NET Framework 4.8.1 console application; the runnable project is BackingSource. The logic for reading and writing data resides in the `BackingSourceProvider` class library project.

Before running the sample, the cache must be configured with the required ReadThru and WriteThru providers (see `Build and Run the sample` → `Configuration` section), and the database must be set up and populated (see `Build and Run the sample` → `Database Setup` → `Populating the database` section). Once done, the cache will be able to fetch items on-demand from the database and update the database whenever cached items are modified which is being demonstrated in `BackingSourceUsage`.

The `BackingSourceUsage`, once executed first fetches a customer object using read through and then updates the same customer object in the database using write through. Afterwards it briefly touches on all distributed data structures (counter, dictionary, list, queue and hashset) with the help of read throughs.
	
This sample uses SampleData project as a reference for model class `Customer`.

## Prerequisites

Before the sample application is executed, make sure that:

- .NET Framework 4.8.1 Developer Pack or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html
- Ensure that `ReadThruProviderName` and `WriteThruProviderName` are set according to the provider names in configuration (see Configuration).
- NuGet packages required (references already included in the project):
	- `Alachisoft.NCache.SDK`

## Build and Run the Sample

Before building this sample, if you need to change the cache name and providers, open `App.config` from the project root directory and change the `CacheName`, `ReadThruProviderName` and `WriteThruProviderName` values:

```xml
<configuration>
	<appSettings>
		<add key="CacheName" value="demoCache"/>
		<add key="ReadThruProviderName" value ="SqlReadThruProvider"/>
		<add key="WriteThruProviderName" value ="SqlWriteThruProvider"/>
	</appSettings>
</configuration>
```

From Visual Studio (Recommended):
- Open `BackingSource.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\BackingSource
cd "<path-to-sample>"
nuget restore BackingSource.sln
msbuild BackingSource.sln /p:Configuration=Debug
```

### Database Setup

#### Populating the database

For this sample, the Northwind database is required. You can download and set it up using the script provided by Microsoft.
Use the following link to access the official Northwind SQL script and detailed setup instructions:\
https://github.com/microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs

Follow the steps given in the repository to create and populate the Northwind database on your SQL Server instance.

### Configuration (Backing Soruce)

Follow these steps to deploy and configure the BackingSource provider in NCache Web Manager:
1. Build the provider assembly
	- Build the solution so `BackingSourceProvider.dll` is produced in: `BackingSource\bin\Debug\BackingSourceProvider.dll` (or release folder if built in Release).
2. Open NCache Web Manager (Default URL: `http://localhost:8251/`)
	- Select your cache (this sample uses `demoCache`).
	- If the cache is running, stop it before making configuration changes.
3. Enable Backing Source Providers
   - Click `View Details`, select `Backing Source` tab in the `Advanced Settings (Clustered Cache)` section.
   - Check "Enable Read Through".
   - Check "Enable Write Through".
4. Deploy the assembly and select class
   - Click `Add Provider` in the Read Through section and select `BackingSourceProvider.dll` from the build output.
   - In "Class Name" choose the provider class (e.g. ReadThruProvider).
   - Click `Add Provider` in the Write Through section and select `BackingSourceProvider.dll` from the build output.
   - In "Class Name" choose the provider class (e.g. WriteThruProvider).
5. Add Parameter
   - Add `ConnectionString` as parameter and the connection string of your populated database in the value field then click the add button.
7. Deploy providers and save changes
   - Click `Deploy Backing Source Provider` to register the provider assembly.
   - Browse to the directory where `BackingSourceProvider.dll` is present and select it.
   - Click ok.
   - Click `Save Changes`.
   - Start the cache.
	

#### Troubleshooting

- Ensure the SQL account has permissions to read the tables and that the connection string is correct.

### Run the sample

After configuring the cache, you can run the sample from Visual Studio. If you are using command line, navigate to the path (see Build section) and use the following command:
```bash
BackingSourceUsage\bin\Debug\BackingSourceUsage.exe
```

## Notes

- If NuGet packages are not restored properly:
	- Visual Studio: Right‑click the solution → Restore NuGet Packages.
	- Command line: run `nuget restore BackingSource.sln` in the sample folder (see Build and Run section).

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/data-source-provider.html

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