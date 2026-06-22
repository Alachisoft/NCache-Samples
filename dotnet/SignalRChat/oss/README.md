# SIGNALR CHAT SAMPLE

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

This sample is a .NET 8.0 MVC application that demonstrates how to integrate ASP.NET SignalR with NCache to build a scalable, real-time chat system. SignalR enables real-time communication between the server and connected clients, while NCache acts as a distributed back-plane to ensure that messages are delivered instantly to all the connected clients.NCache is registered as the SignalR backplane using the `AddSignalR().AddNCache()` configuration.

The Home Page provides an enhanced real-time communication interface built using SignalR. It includes connection management, group-based messaging, and targeted message delivery.

### Connection Initialization
When the application starts, the client establishes a SignalR connection with the server.
Each connected client is assigned a unique Connection ID, which is displayed on the UI.
Opening the same URL in multiple tabs creates independent client connections, each with its own Connection ID.
### Group Management
Users can join or leave groups dynamically.\
Available actions:
- Join Group – Adds the current connection to a specified group.
- Leave Group – Removes the current connection from the specified group.
### Messaging Features

The application supports multiple messaging modes:

- Send to All - Broadcasts a message to all connected clients.
- Send to Connection - Sends a private message to a specific client using their Connection ID.
- Send to Group - Sends a message to all clients within a specified group.

### Message Flow
- The client enters a message and selects the desired sending mode.
- The message is sent to the SignalR Hub.
- The Hub processes the request and routes the message based on:
    - All clients
    - A specific connection
    - A specific group

- Messages are delivered to recipients in real time.


![SignalR Chat Page](../../../../docs/assets/sample-images/SignalRChat-Sample-dotnet-oss.png)

## Prerequisites

Before the sample application is executed, make sure that:

- .NET 8.0 SDK or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html
- Ensure SignalR applications use `SignalRChatApp` as the event key.
	- By default, the event key `SignalRChatApp` is configured in `appsettings.json`. Adjust as needed.
- NuGet packages required (references already included in the project):
	- `Alachisoft.NCache.OpenSource.SDK`
	- `AspNetCore.SignalR.NCache.OpenSource`

## Build and Run the Sample

Before building this sample, if you need to change the cache name and event key, open `appsettings.json` from the project root directory and change the `CacheName` and `EventKey` values:

```json
{
  "NCacheConfiguration": {
    "CacheName": "demoCache",
    "EventKey": "SignalRChatApp"
	}
}
```

From Visual Studio (Recommended):
- Open `SignalRChat.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\SignalRChat
cd "<path-to-sample>"
dotnet restore
dotnet build
dotnet run
```

## Notes

- If NuGet packages are not restored properly:
	- Visual Studio: Right‑click the solution → Restore NuGet Packages.
	- Command line: run `dotnet restore` in the sample folder (see Build and Run section).
	- If using nuget.exe: run `nuget restore SignalRChat.sln`.

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/ncache-extension-signalr-core.html?tabs=net

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