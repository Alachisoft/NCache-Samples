# EVENTS SAMPLE

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

This .NET Framework 4.8.1 console sample demonstrates NCache Events using demo Product data from the SampleData project. The sample shows how to subscribe to and handle both cache‑level and item‑level notifications so external logic can react to cache changes in real time.

### Cache‑level events
- Enables subscribing to notifications for additions, updates and removals across the entire cache.
- Workflow: register cache events (RegisterCacheEvents), trigger events via Add/Insert/Remove, then unregister handlers.

### Item‑level events
- Enables subscribing to notifications for a **specific key** to receive fine‑grained updates for that item only.
- Workflow: register item events for a key, perform operations to trigger events (Insert/Remove), then unregister item events.

This sample uses SampleData project as a reference for model class "Product".

## Prerequisites

Before the sample application is executed, make sure that:

- .NET Framework 4.8.1 Developer Pack or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html
- NuGet package required: Alachisoft.NCache.Opensource.SDK (>= 5.3.6.2). The package reference is already included in the project file.
- SampleData project (which contains the "Product" model) must be present in the directory as this sample.

## Build and Run the Sample

Before building this sample, if you need to change the cache name, open `App.config` from the project root directory and change the `CacheName` value:

```xml
<appSettings>
  <add key="CacheName" value="demoCache"/>
</appSettings>
```

### Configuration (Enabling Events)

- Follow these steps to enable cache-level events in the NCache Web Manager:
1. Open NCache Web Manager (Default URL: `http://localhost:8251/`)
    - Select your cache (this sample uses `demoCache`).
    - If the cache is running, stop it before making configuration changes.
2. Enable cache-level events
	- Go to `Advanced Settings`.
	- Click on `Options`.
	- Check `Item add notification. (OnItemAdded)`.
	- Check `Item remove notification. (OnItemRemoved)`.
	- Check `Item update notification. (OnItemUpdated)`.
	- Click `Save Changes`.

From Visual Studio (Recommended):
- Open `Events.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\Events
cd "<path-to-sample>"
nuget restore Events.sln
msbuild Events.sln /p:Configuration=Debug
Events\bin\Debug\Events.exe
```

## Notes

- Ensure the sample solution references SampleData before building:
   - In Visual Studio: Right-click the solution → Add → Existing Project → `SampleData\SampleData\SampleData.csproj`
   - Command line: add the project reference or edit the solution file to include the SampleData project.
- If NuGet packages are not restored properly:
	- Visual Studio: Right‑click the solution → Restore NuGet Packages.
	- Command line: run `nuget restore Events.sln` in the sample folder (see Build and Run section).

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/events-overview.html

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