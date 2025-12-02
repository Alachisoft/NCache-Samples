# NHIBERNATE SAMPLE

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

This .NET Framework 4.8.1 console application shows how to integrate NCache into NHibernate applications by using NCache as a second-level cache. The application shows how to cache database items and queries using NHibernate in order to reduce database load.

Key features covered include query caching (`List`/`ListAsync`), single entry retrievals (`UniqueResult`/`UniqueResultAsync`), addition of entries (`Save` and `Flush`) and removal of entries and queries (`Evict` and `EvictQueries`) from database and cache. The sample uses an altered version of `Microsoft's Northwind dataset` to illustrate realistic scenarios.

## Prerequisites

Before the sample application is executed, make sure that:

- .NET Framework 4.8.1 Developer Pack or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
- Ensure that `demoCache` (or another cache of your choice) is running.
	- By default, the cache name `demoCache` is configured in `NCacheNHibernate.xml` and `hibernate.cfg.xml`. Adjust as needed.
- Ensure that a database of your choice is configured with required permissions and change the Connection String in `App.config`.
- NuGet packages required (references already included in the project):
	- `Alachisoft.NCache.SDK`
	- `FluentNhibernate`
	- `NHibernate.NCache`

## Build and Run the Sample

### Configuration

Before building this sample, the connection string needs to be changed in `App.Config`. Open the file and navigate to the `configuration` → `appSettings` section. Change the value of key `ConnectionString` from `<db-conn-string>` to your database connection string.

```xml
</configuration>
	...
    <appSettings>
		...
        <!-- Replace <db-conn-string> with your database connection string. -->
        <add key="ConnectionString" value="<db-conn-string>" />
    </appSettings>
</configuration>
```

If you need to change the cache name and providers, open `NCacheNHibernate.xml` from the project root directory and change the `cache-name` value:

```xml
<cache-regions>
	<region name="default" cache-name="demoCache" priority="default" expiration-type="none" expiration-period="0" use-async="false"/>
</cache-regions>
```

Also open `hibernate.cfg.xml` change the `cache.cacheName` value in the `session-factory` region:

```xml
<session-factory>
	<property name="cache.provider_class">Alachisoft.NCache.Integrations.NHibernate.Cache.NCacheProvider, Alachisoft.NCache.Integrations.NHibernate.Cache</property>
	<property name="cache.use_second_level_cache">true</property>
	<property name="cache.use_query_cache">true</property>
		<property name="cache.cacheName">demoCache</property>
</session-factory>
```

### Database Setup

#### Populating the database

- Run the script included with the sample: `NHibernateScript.sql` (located in this sample folder).
- Using SQL Server Management Studio (SSMS):
	- Connect to your SQL Server / SQL Express instance (e.g. .\SQLEXPRESS).
	- Open NHibernateScript.sql and click Execute.
- Using sqlcmd (Windows):
  - Integrated security:
	```bash
	sqlcmd -S .\SQLEXPRESS -E -i "<path-to-sample>\NHibernateScript.sql"
	```
  - SQL authentication:
	```bash
	# Replace <server> with server IP or name, <user> and <password> with credentials
	sqlcmd -S <server> -U <user> -P <password> -i "<path-to-sample>\NHibernateScript.sql"
	```

#### Troubleshooting

- Ensure the SQL account has permissions to read the tables and that the connection string is correct.

### Build

From Visual Studio (Recommended):
- Open `NHibernate.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\NHibernate
cd "<path-to-sample>"
nuget restore NHibernate.sln
msbuild NHibernate.sln /p:Configuration=Debug
```

### Run the sample

After configuring the cache, you can run the sample from Visual Studio. If you are using command line, navigate to the path (see Build section) and use the following command:
```bash
NHibernate\bin\Debug\NHibernateSample.exe
```

## Notes

- If NuGet packages are not restored properly:
	- Visual Studio: Right‑click the solution → Restore NuGet Packages.
	- Command line: run `nuget restore NHibernate.sln` in the sample folder (see Build and Run section).

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/ncache-as-nhibernate-second-level-cache.html

## Additional Resources

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

Copyright 2025 Alachisoft&copy;