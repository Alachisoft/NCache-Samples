# EFCORE CACHING SAMPLE

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

Entity Framework (EF) is a .NET Object-Relational Mapping (ORM) framework that simplifies database operations by allowing developers to work with data as .NET objects.

This .NET 8.0 console sample demonstrates integrating NCache with Entity Framework Core using the `Alachisoft.NCache.EntityFrameworkCore` provider. The examples show how to cache EF Core query results and entity objects in NCache to reduce database load and improve read performance. 

### Query Caching

- Query caching involves storing transactional query results in cache using the EF Core extension APIs (`FromCache`/`FromCacheAsync`).
- Upon execution of a query, the cache is checked for the query result.
    - If the result is found in the cache, it is returned directly without querying the database.
    - Otherwise, the database is queried, and the result is stored in the cache for subsequent requests. 

### Reference Dataset Caching

- Reference dataset caching loads entire datasets into the cache and then directly queries the cache to avoid database hits. 
- Datasets are first cached from the database via extensions APIs (`LoadIntoCache`/`LoadIntoCacheAsync`). 
- Once cached, you can use the `FromCacheOnly` or `FromCacheOnlyAsync` extension methods to retrieve query results exclusively from the cache, without accessing the database.

## Prerequisites

Before running the sample, ensure that:

- .NET 8.0 SDK and Visual Studio (recommended) are installed.
- Database server (Microsoft SQL Server or SQL Server Express) is installed.
    - Ensure the server is reachable, running and accessible (connection string with credentials is available).
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-distributed-cache.html
- NuGet packages required (references already included in the project):
	- `EntityFrameworkCore.NCache`
	- `Microsoft.EntityFrameworkCore`
	- `Microsoft.EntityFrameworkCore.SqlServer`
	- `Microsoft.EntityFrameworkCore.Tools`

## Build and Run the Sample

To run the sample, first you need to set up the database, build the code, and run it.

### Database Setup

This sample uses a small subset of the Microsoft's Northwind schema. The scripts create and populate the tables used by the sample (Products, Suppliers, Customers, Orders). You can either use the scripts for the full schema:
- **Option 1: Use the full Northwind database**:
    - Download or clone the official Microsoft Northwind scripts from:\
    https://github.com/microsoft/sql-server-samples/tree/main/samples/databases/northwind-pubs
    - Run the appropriate .sql script(s) to create and populate the complete Northwind database.
- **Option 2: Use the sample database script**:
    - Run the included `PopulateEFCoreDB.sql` script, which creates and populates only the tables required by this sample.
    
You can execute either script using one of the following methods:
- **Using SQL Server Management Studio (SSMS)**:
    - Connect to your SQL Server / SQL Express instance (e.g. `.\SQLEXPRESS`).
    - Open `PopulateEFCoreDB.sql` and click Execute.
- **Using sqlcmd (Windows)**:
  - Integrated security:
    ```bash
    sqlcmd -S .\SQLEXPRESS -E -i "<path-to-sample>\PopulateEFCoreDB.sql"
    ```
  - SQL authentication:
    ```bash
    # Replace <server> with server IP or name, <user> and <password> with credentials
    sqlcmd -S <server> -U <user> -P <password> -i "<path-to-sample>\PopulateEFCoreDB.sql"
    ```

After the script completes, verify that the tables were created and populated:

    ```sql
    SELECT COUNT(*) FROM Customers;
    SELECT TOP 5 * FROM Customers;
    SELECT COUNT(*) FROM Orders;
    SELECT TOP 5 * FROM Orders;
    SELECT COUNT(*) FROM Products;
    SELECT TOP 5 * FROM Products;
    SELECT COUNT(*) FROM Suppliers;
    SELECT TOP 5 * FROM Suppliers;
    ```

### Build

The connection string needs to be placed in the `App.config` in the project root directory in the `ConnString` value. If you need to change the cache name, open `App.config` from the project root directory and change the `CacheName` value:

```xml
<appSettings>
	<add key="CacheName" value="demoCache"/>
    <!-- Replace db-conn-string with your database connection string.
         Examples:
         - Integrated Security (SQL Express): Server=.\SQLEXPRESS;Database=EFCoreCacheDb;Integrated Security=True;
         - SQL Auth: Server=MYSERVER;Database=EFCoreCacheDb;User Id=myuser;Password=mypassword;
    -->
	<add key="ConnString" value="db-conn-string" />
</appSettings>
```

From Visual Studio (Recommended):
- Open `EFCoreCaching.sln`.
- Let NuGet packages restore.
- Build.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\EFCoreCaching
cd "<path-to-sample>"
dotnet restore
dotnet build
```

### Configure Query Indexes

Configure the cache to index the model classes so queries can run efficiently:

1. Build EFCoreCaching
    - Build the solution so `EFCoreCaching.dll` is produced in: `EFCoreCaching\bin\Debug\net8.0\EFCoreCaching.dll` (or release folder if built in Release).
2. Open the NCache Management Center (Default URL: `http://localhost:8251/`).
    - Select your cache (this sample uses `demoCache`).
    - If the cache is running, stop it before making configuration changes.
3. Configure query indexes
    - Click `View Details` and select `Query Indexes` tab in the `Advanced Settings (Clustered Cache)` section.
    - Click `Add` then `Browse`, locate and select the `EFCoreCaching.dll` from the build output folder.
    - In `Browsed Assemblies` expand the assembly, locate and check the `EFCoreCaching.Models` to check all classes as below and click `Add Selected Classes` button.
        - [ ] `EFCoreCaching, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`
            - [x] `EFCoreCaching.Models`
                - [x] `EFCoreCaching.Models.Customer`
                - [x] `EFCoreCaching.Models.Order`
                - [x] `EFCoreCaching.Models.Product`
                - [x] `EFCoreCaching.Models.Supplier`
4. Select attributes to index
    - In the `Select Attributes`, check all the properties (checking the model will select all its attributes).
    - Click `OK` button 
5. Save changes and Start Cache
    - Click the `Save Changes` button.
    - Start the cache.

To learn more about configuring query indexes, please see [NCache Docs](https://www.alachisoft.com/resources/docs/ncache/admin-guide/configure-query-index.html). 

### Run the sample

After configuring the cache, you can run the sample from visual studio. If you are using command line, navigate to the path (see Build section) and use the following command:

```bash
dotnet run
```

## Troubleshooting

### Assembly Does Not Appear or Classes Are Missing

- Verify that you selected the correct assembly (`EFCoreCaching.dll`) when configuring the cache.
- Rebuild the sample project if the assembly was recently modified.
- Ensure the assembly contains public classes that can be discovered.

### Cache Fails to Start After Adding Indexes

If the cache fails to start after adding indexes:

1. Remove the newly added index.
2. Start the cache.
3. Resolve any issues in the assembly (such as invalid types or missing dependencies).
4. Rebuild the assembly and re-add the index.

### Cannot Connect to Cache

- Verify the NCache service is running.
- Verify that the configured cache (for example, `demoCache`) exists and is running.
- Verify that the cache name in `App.config` matches the running cache.
- Verify that [firewall rules](https://www.alachisoft.com/resources/docs/ncache/install-guide/firewalls-ports.html) allow communication with the cache server.

### Database Issues

- Ensure the `ConnString` in `App.config` points to the same database that was populated with the sample data.
- Verify that the SQL Server account has sufficient permissions to read and update data.

### NuGet Packages Are Not Restored

- If NuGet packages are not restored properly:
	- **Visual Studio**: Right‑click the solution → Restore NuGet Packages.
	- **Command line**: Run `dotnet restore` in the sample folder, see Build section.
	- **nuget.exe**: Run `nuget restore EFCoreCaching.sln`.

## References

For more information about EF Core Caching, see:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/entity-framework-core-caching.html

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

