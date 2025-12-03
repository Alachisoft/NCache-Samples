# TAGS AND NAMED TAGS SAMPLE

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

This sample is a .NET Framework 4.8.1 console application which demonstrates the use of NCache Tags and Named Tags and how they are used to index and query cached items.

### Tags  
- Tags let you label and group cached items so you can perform set-oriented queries (many-to-many relationships between items and tags).
- Use tags to fetch or remove groups of items that share a common label (for example: "electronics", "clearance").
- This sample uses the Product model (from the SampleData project) to show typical workflows:
	- Adding products with tags
	- Querying products by one or more tags
	- Removing groups of tagged products.

### Named Tags
- Named Tags attach key/value attributes (strings, numbers, booleans, dates) to items to enable attribute-based queries and finer-grained filtering.
- In this sample named tags are demonstrated using sample image filenames: 
	- Add named-tag attributes to image items
	- Query or remove items by named-tag values
	- Remove named tags from items.

This sample uses the SampleData project for the "Product" model.

## Prerequisites

Before the sample application is executed, make sure that:

- .NET Framework 4.8.1 Developer Pack or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html
- NuGet package required: Alachisoft.NCache.SDK (>= 5.3.6.1). The package reference is already included in the project file.
- SampleData project (which contains the "Product" model) must be present in the directory as this sample.

## Build and Run the Sample

Before building this sample, if you need to change the cache name, open `App.config` from the project root directory and change the `CacheName` value:

```xml
<appSettings>
  <add key="CacheName" value="demoCache"/>
</appSettings>
```

From Visual Studio (Recommended):
- Open `TagsNamedTags.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\TagsNamedTags
cd "<path-to-sample>"
nuget restore TagsNamedTags.sln
msbuild TagsNamedTags.sln /p:Configuration=Debug
TagsNamedTags\bin\Debug\TagsNamedTags.exe
```

## Notes

- Ensure the sample solution references SampleData before building:
   - In Visual Studio: Right-click the solution → Add → Existing Project → `SampleData\SampleData\SampleData.csproj`
   - Command line: add the project reference or edit the solution file to include the SampleData project.
- If NuGet packages are not restored properly:
	- Visual Studio: Right‑click the solution → Restore NuGet Packages.
	- Command line: run `nuget restore TagsNamedTags.sln` in the sample folder (see Build and Run section).

## References

Reference documentation for tags is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/tag-cache-data.html

Reference documentation for named tags is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/named-tags.html

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

Copyright 2025 Alachisoft&copy;