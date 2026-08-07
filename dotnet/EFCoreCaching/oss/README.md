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

## Prerequisites

Before running the sample, ensure that:

- .NET 8.0 SDK and Visual Studio (recommended) are installed.
- Database server (Microsoft SQL Server or SQL Server Express) is installed.
    - Ensure the server is reachable, running and accessible (connection string with credentials is available)
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-distributed-cache.html
- NuGet packages required (references already included in the project):
	- `EntityFrameworkCore.NCache.OpenSource`
	- `Microsoft.EntityFrameworkCore`
	- `Microsoft.EntityFrameworkCore.SqlServer`
	- `Microsoft.EntityFrameworkCore.Tools`

## Build and Run the Sample

To run the sample, first you need to setup the database, build the code and run it.

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

### Run the sample

After configuring the cache, you can run the sample from visual studio. If you are using command line, navigate to the path (see Build section) and use the following command:
```bash
dotnet run
```

## Troubleshooting

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

