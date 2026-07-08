# EFCORE CACHING SAMPLE

## Table of contents

* [Introduction](#introduction)
* [Query Caching](#query-caching)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Database Setup](#database-setup)
* [Notes](#notes)
* [References](#references)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

## Introduction

This .NET 8.0 console sample demonstrates integrating NCache with Entity Framework Core using the `Alachisoft.NCache.EntityFrameworkCore` provider. The examples show how to cache EF Core query results and entity objects in NCache to reduce database load and improve read performance. This sample demonstrates:

### Query Caching

- Query caching involves storing transactional query results in cache using the EF Core extension APIs (`FromCache`/`FromCacheAsync`).
- Upon execution of a query, the cache is checked for the query result.
    - If found, the database is queried and the result is cached.
    - Otherwise the result is fetched from the cache instead of the database. 


## Prerequisites

Before the sample application is executed, make sure that:
- .NET 8.0 SDK or Visual Studio (recommended) installed.
- Database server (Microsoft SQL Server or SQL Server Express) installed.
    - Ensure the server is reachable, running and accessible (connection string with credentials is available)
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html
- NuGet packages required (references already included in the project):
	- `EntityFrameworkCore.NCache.OpenSource`
	- `Microsoft.EntityFrameworkCore`
	- `Microsoft.EntityFrameworkCore.SqlServer`
	- `Microsoft.EntityFrameworkCore.tools`

## Build and Run the Sample

To run the sample, first you need to setup the database, build the code and run it.

### Database Setup

This sample uses a small subset of the Microsoft's Northwind schema. The scripts create and populate the tables used by the sample (Products, Suppliers, Customers, Orders). You can either use the scripts for the full schema:
- Full Northwind scripts (GitHub):\
https://github.com/microsoft/sql-server-samples/tree/main/samples/databases/northwind-pubs
  - Clone or download the repo and run the appropriate .sql script(s) to create/populate the full Northwind dataset.

Or use the included sample script: `PopulateEFCoreDB.sql` (creates and populates the tables used by this sample).
- Run the script included with the sample: `PopulateEFCoreDB.sql` (located in this sample folder).
- Using SQL Server Management Studio (SSMS):
    - Connect to your SQL Server / SQL Express instance (e.g. .\SQLEXPRESS).
    - Open PopulateEFCoreDB.sql and click Execute.
- Using sqlcmd (Windows):
  - Integrated security:
    ```bash
    sqlcmd -S .\SQLEXPRESS -E -i "<path-to-sample>\PopulateEFCoreDB.sql"
    ```
  - SQL authentication:
    ```bash
    # Replace <server> with server IP or name, <user> and <password> with credentials
    sqlcmd -S <server> -U <user> -P <password> -i "<path-to-sample>\PopulateEFCoreDB.sql"
    ```
- Verify tables/data:
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

## Notes

- Ensure the `ConnectionString` you changed in the `App.config` points to the same database you populated. 
- The SQL account must have permissions to update data.
- If NuGet packages are not restored properly:
    - Visual Studio: Right‑click the solution → Restore NuGet Packages.
    - Command line: run `dotnet restore` in the sample folder (see Build section).
    - If using nuget.exe: run `nuget restore EFCoreCaching.sln`.

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/entity-framework-core-caching.html

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