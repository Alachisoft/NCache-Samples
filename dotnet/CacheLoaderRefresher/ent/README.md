# CACHE LOADER AND REFRESHER SAMPLE

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

This sample demonstrates NCache Cache Loader and Refresher feature, which loads data from a data source into the cache when the cache starts and periodically refreshes the cached data to keep it up to date. This is useful for applications that require frequently accessed data to be available immediately at startup while ensuring it remains synchronized with the underlying data source

The sample consists of two .NET 8.0 projects:

- **LoaderRefresherUsage**: A console application that connects to the cache and displays the cached data.
- **LoaderRefresherLib**: A class library that implements the Cache Loader and Refresher logic.

Before running the sample, the cache must be configured (see `Build and Run the sample` → `Configuration` section) and database must be setup (see `Build and Run the sample` → `Database Setup` → `Populating the database` section). Once done, the database will be populated and on startup, the cache will also be loaded with data. The `LoaderRefresherUsage` console app is used to display the contents of cache.

After cache startup and loader execution, the console app will display the data from cache loaded from the database. Once confirmed, close the console app and execute the database update query script (see `Build and Run the sample` → `Database Setup` → `Updating the database` section). This time, running the console app after the refresh interval will display the updated data. These results demonstrate the loader and refresher features of NCache.

This sample uses SampleData project as a reference for model class `Product` and `Supplier`.

## Prerequisites

Before running the sample, ensure that:

- .NET 8.0 SDK and Visual Studio (recommended) are installed.
- Database server (Microsoft SQL Server or SQL Server Express) is installed.
    - Ensure the server is reachable, running and accessible (connection string with credentials is available).
- SQL Server Management Studio (SSMS) or any other database tool is installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-distributed-cache.html
- NuGet package required: **Alachisoft.NCache.SDK (>= 5.3.6.1)**. The package reference is already included in the project file.
- SampleData project (which contains the `Product` and `Supplier` models) must be present in the directory as this sample.

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
- Open `CacheLoaderRefresher.sln`.
- Let NuGet packages restore.
- Build.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\CacheLoaderRefresher
cd "<path-to-sample>"
dotnet restore
dotnet build
```

### Database Setup

#### Populating the database

- Run the script included with the sample: `CreateDatabase.sql` (located in this sample folder).
- Using SQL Server Management Studio (SSMS):
    - Connect to your SQL Server / SQL Express instance (e.g. .\SQLEXPRESS).
    - Open `CreateDatabase.sql` and click Execute.
- Using sqlcmd (Windows):
  - **Integrated security**:
    ```bash
    sqlcmd -S .\SQLEXPRESS -E -i "<path-to-sample>\CreateDatabase.sql"
    ```
  - **SQL authentication**:
    ```bash
    # Replace <server> with server IP or name, <user> and <password> with credentials
    sqlcmd -S <server> -U <user> -P <password> -i "<path-to-sample>\CreateDatabase.sql"
    ```
- Verify tables/data:
    ```sql
    SELECT COUNT(*) FROM Products;
    SELECT TOP 5 * FROM Products;
    SELECT COUNT(*) FROM Suppliers;
    SELECT TOP 5 * FROM Suppliers;
    ```

#### Updating the database

- You will need to update the database once cache loader data is verified after running the sample.
- Once that is done, you will need to update the database data to verify refresher functionality.
- An update script `UpdateDatabase.sql` is provided in the sample, run it the same way as the populate script.
- Or execute a manual update to simulate changed source data, for example:
    ```sql
    UPDATE Products SET Price = Price + 1 WHERE ProductId = 1;
    ```
- Use sqlcmd or SSMS as shown above to apply updates.
- After applying updates, wait for the refresher interval configured in the provider, then run the console app to observe refreshed data.

### Configuration (CacheLoader/Refresher)

Follow these steps to deploy and configure the Loader/Refresher provider in the NCache Management Center:
1. Build the provider assembly
    - Build the solution so `LoaderRefresherLib.dll` is produced in: `LoaderRefresherLib\bin\Debug\net8.0\LoaderRefresherLib.dll` (or release folder if built in Release).
2. Open the NCache Management Center (Default URL: `http://localhost:8251/`).
    - Select your cache (this sample uses `demoCache`).
    - If the cache is running, stop it before making configuration changes.
3. Enable Cache Loader/Refresher
   - Click `View Details`, select `CacheLoader/Refresher` tab in the `Advanced Settings (Clustered Cache)` section.
   - Check "Enable Cache Loader and Refresher".
4. Deploy the assembly and select class
   - Click `Browse` and select `LoaderRefresherLib.dll` from the build output.
   - In "Class Name" choose the provider class (e.g. LoaderAndRefresher).
5. Add Parameter and DataSets
   - Add `ConnectionString` as parameter and the connection string of your populated database in the value field then click the add button.
   - Add dataset used by the provider. You need to add `products` and `suppliers` datasets.
   - For each dataset, click `Add Dataset` button, enter dataset name, check `Refresh this dataset at the following schedule`, enter 1 minute as interval and click `OK` button.
7. Deploy providers and save changes
   - Click `Deploy Cache Loader` to register the provider assembly.
   - Click `Save Changes`.
   - Start the cache.

For more information about configuring Cache Loader and Refresher, please see:\
https://www.alachisoft.com/resources/docs/ncache/admin-guide/loader-referesher-overview.html


### Run the Sample

After configuring the cache, you can run the sample from Visual Studio. If you are using command line, navigate to the path (see Build section) and use the following command:

```bash
dotnet run
```

## Troubleshooting

### Missing SampleData Project

- Ensure the sample solution references the `SampleData` project before building.
  - **Visual Studio**: Right-click the solution → Add → Existing Project → `SampleData\SampleData\SampleData.csproj`
  - **Command line**: Add the project reference or edit the solution file to include the `SampleData` project.

### NuGet Packages Are Not Restored

- **Visual Studio**: Right-click the solution → Restore NuGet Packages.
- **Command line**: Run `dotnet restore` in the sample folder.
- **nuget.exe**: Run `nuget restore CacheLoaderRefresher.sln`.

### Database Issues

- Ensure the `ConnectionString` parameter you add to the Loader/Refresher provider in the NCache Management Center points to the database created using `CreateDatabase.sql`
- Verify that the SQL Server instance is running and accessible.
- The SQL account must have permissions to create tables and insert/update data. If you see permission errors, use an account with the required privileges or run SSMS as Administrator.

### Cache Loader Configuration

- Verify that the correct `LoaderRefresherLib.dll` assembly is deployed.
- Check Web Manager logs for loader/refresher errors and database connectivity issues.
- Ensure the provider class is selected correctly.
- Verify that the `products` and `suppliers` datasets are configured.
- If the cache fails to start, remove the provider, correct the configuration, redeploy the provider, and start the cache again.

### Cannot Connect to Cache

- Verify the NCache service is running.
- Verify that the configured cache exists and is running.
- Verify that firewall rules allow communication with the cache server.

## References

For more information about Cache Loader and Refresher, see:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/cache-startup-loader.html

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