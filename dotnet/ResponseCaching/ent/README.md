# RESPONSE CACHING SAMPLE

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

This sample is a .NET 8.0 MVC application that demonstrates how to use NCache as a distributed response Caching provider. This enables entire views (HTML output) to be stored in NCache, reducing database load and significantly improving performance, especially in multi-server environments. NCache is registered using `AddNCacheResponseCachingServices` and the configuration is loaded from `appsettings.json` (cache name, event key, etc.).

On starting the single page web application, the index page displays a grid for various `Product` records with pagination. First time calling this page, data will be fetched from the database and then cached. On subsequent calls, data will be fetched from NCache, reducing database traffic.

![Product Grid Page](../../../../docs/assets/sample-images/ResponseCaching-ProductList-Sample-dotnet-ent.png)

When a user clicks `View` against a `Product`, the product details page is displayed. The full HTML of this page is also cached in NCache for quick retrieval for future page requests.

![Product View Page](../../../../docs/assets/sample-images/ResponseCaching-ViewProduct-Sample-dotnet-ent.png)

When a user clicks `Edit` against a `Product`, the product details page is displayed. The full HTML of this page is also cached in NCache for quick retrieval for subscaquent page requests.After submitting changes, the data is updated via the repository and the SQL Server triggers the SQL dependency linked to the cached Index page. NCache automatically invalidates the stale cached response. The user is then redirected to a success page.

![Product Edit Page](../../../../docs/assets/sample-images/ResponseCaching-EditProduct-Sample-dotnet-ent.png)

Before running the sample, the database must be set up and populated (see `Build and Run the sample` → `Database Setup` → `Populating the database` section).

## Prerequisites

Before the sample application is executed, make sure that:

- .NET 8.0 SDK or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html
- Database server (Microsoft SQL Server or SQL Server Express) installed.
  - Ensure the server is reachable, running and accessible (connection string with credentials is available).
  - Broker service must be enabled in database. The following link contains instructions:\
    https://learn.microsoft.com/en-us/sql/database-engine/service-broker/how-to-activate-service-broker-message-delivery-in-databases-transact-sql 
- SQL Server Management Studio (SSMS) or any other database tool installed.
- Ensure that database is set up and contains required tables (see Database Setup) and connection string in `appsettings.json` is able to connect to the database.
- NuGet packages required (references already included in the project):
	- `Alachisoft.NCache.SDK`
	- `AspNetCore.ResponseCache.NCache`

## Build and Run the Sample

Before building this sample, you need to change the connection string to point to your database, open `appsettings.json` from the project root directory and change `<db-conn-string>` to your connection string in `DemoDBServer`. If you want to change the cache name, change the `demoCache` value of `CacheName` as well.

```json
{
  ...
	"NCacheSettings": {
		"CacheName": "demoCache", 
    ...
	},
	"ConnectionStrings": {
		"DemoDBServer": "<db-conn-string>"
	}
}
```

From Visual Studio (Recommended):
- Open `ResponseCaching.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\ResponseCaching
cd "<path-to-sample>"
dotnet restore
dotnet build
```

### Database Setup

#### Populating the database

For this sample, the Northwind database is required. You can download and set it up using the script provided by Microsoft.
Use the following link to access the official Northwind SQL script and detailed setup instructions:\
https://github.com/microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs

Follow the steps given in the repository to create and populate the Northwind database on your SQL Server instance.

#### Troubleshooting

- Ensure the SQL account has permissions to read the tables and that the connection string is correct.

### Run the sample

After configuring the cache, you can run the sample from Visual Studio. If you are using command line, navigate to the path (see Build section) and use the following command:
```bash
dotnet run
```

## Notes

- If NuGet packages are not restored properly:
	- Visual Studio: Right‑click the solution → Restore NuGet Packages.
	- Command line: run `dotnet restore` in the sample folder (see Build and Run section).
	- If using nuget.exe: run `nuget restore ResponseCaching.sln`.

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/response-caching-overview.html

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