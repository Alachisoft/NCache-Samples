# Microservices Sample

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

This sample demonstrates two .NET 8.0 minimal API microservices communicating with each other using NCache Pub/Sub messaging — `OrderService` and `InventoryService`. A `Client` console application is used to send operational commands to both services using RestAPI. The workflow for these projects:

### `Client` console application
- Creates 5 sample orders to be sent.
- Iterates and sends HTTP requests to the `OrderService` to add the order.
- On each successful response, sends HTTP requests to `InventoryService` to print current stock value.
- If any response is failure, stops sending orders
- Performs cleanup before exit.

### `OrderService` web API microservice:
- Creates a `DistributedList` for storing orders in cache.
- Creates a boolean flag if the inventory is `Out-Of-Stock` and sets it to false.
- Creates/gets topics for publishing/subscribing messages to/from `InventoryService`.
- When order request is received from `Client`:
    - If `Out-Of-Stock` is set to false:
        - That order is added to the cache.
        - A message is published to the `InventoryService` for notification.
        - A success response is sent back.
    - If `Out-Of-Stock` is set to true:
        - An out of stock message is sent as a failed response and order is rejected.
- When "out-of-stock" message is received from `InventoryService` via subscription:
    - The `Out-Of-Stock` is set to true.
- When cleanup request is received from `Client`:
    - `DistributedList` is removed from cache.

### `InventoryService` web API microservice:
- Creates a `DistributedCounter` for storing stock in cache with initial value 3.
- Creates/gets topics for publishing/subscribing messages to/from `OrderService`.
- When order message is received from `OrderService` via subscription:
    - Reduce the counter value by 1.
    - If the counter value is below 1:
        - An out of stock message is published to the `OrderService` for notification.
- When stock request is received from `Client`:
    - The counter value is sent back as response.
- When cleanup request is received from `Client`:
    - `DistributedCounter` is removed from cache.

This sample uses SampleData project as a reference for model class "Order".

## Prerequisites

Before the sample application is executed, make sure that:

- .NET 8.0 SDK or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html/
- NuGet package required: Alachisoft.NCache.SDK (>= 5.3.6.1). The package reference is already included in the project files.
- SampleData project (which contains the "Order" model) must be present in the directory as this sample.
- Verify that each service runs on a different HTTPS port and are correctly specified in Program.cs of Client App.

## Build and Run the Sample

Before building this sample, if you need to change the cache name, open `appsettings.json` from the project root directory and change the `CacheName` value. You need to do this for both `OrderService` and `InventoryService` projects.

```json
"NCache": {
  "CacheName": "demoCache"
}
```

### Build

From Visual Studio (Recommended):
- Open `Microservices.sln`.
- Let NuGet packages restore.
- Build all the projects in solution.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\Microservices
cd "<path-to-sample>"
dotnet restore
dotnet build
```

### Run

You need to run the microservice projects (`OrderService` and `InventoryService`) first and then run the `Client` console app.

From Visual Studio (Recommended):
- Setup both microservices as multiple startup projects by following the instructions on:\
https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects
- When both projects are running, start the `Client` by:
    - Right-clicking `Client` project → `Debug` → `Start New Instance`.
    - Or select `Client` project → `Debug` from top menu → `Start New Instance`.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\Microservices
# Start both services one by one
cd "<path-to-sample>"\OrderService
dotnet run
cd "<path-to-sample>"\InventoryService
dotnet run
# Wait for the services to start and then run the client
cd "<path-to-sample>"\Client
dotnet run
```

## Notes

- Ensure the sample solution references SampleData before building:
   - In Visual Studio: Right-click the solution → Add → Existing Project → `SampleData\SampleData\SampleData.csproj`
   - Command line: add the project reference or edit the solution file to include the SampleData project.
- If NuGet packages are not restored properly:
    - Visual Studio: Right‑click the solution → Restore NuGet Packages.
    - Command line: run `dotnet restore` in the sample folder (see Build and Run section).
    - If using nuget.exe: run `nuget restore Microservices.sln`.

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/publish-subscribe-ncache.html

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