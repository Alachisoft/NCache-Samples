# SESSION SHARING SAMPLE

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

This sample demonstrates Session Sharing between `.NET 8.0` and `.NET Framework 4.8.1` web application platforms using NCache Session Services as the common session store. Both applications run independently but share a single distributed session, allowing them to behave like one unified application.

The Core application registers NCache as the session provider using `AddNCacheSession`. The .NET Framework application configures session sharing through `Web.config`, where the NCache Session State Provider is added under <sessionState>.

To illustrate cross-platform session sharing, both applications host the same Guess-the-Number game. Both applications run the same logic and use the same session key, ensuring they read and update the same session data. The gameplay is simple:
- When the first request reaches either application, a random secret number is generated and stored in the shared session.
- Each user guess is appended to a shared list of attempts (also stored in session)
	- Switching between the Core and Framework applications shows the same secret number, the same guess history and the same in-progress game state because both are reading from the same distributed session in NCache.
- If the user correctly guesses the number in either application:
	- The game is won and the session is reset.
	- Refreshing the other application immediately starts a new game, demonstrating full cross-platform session synchronization.

Session Sharing enables both apps to maintain the same session, the same guess history, and synchronized game state regardless of where the user interacts.

On starting the single page web application, the index page showcases the game description and attempts history. It provides the option to input a number in order to provide a guess by filling the textbox with a valid number and clicking `Guess!`.

![Session Sharing Page](../../../../docs/assets/sample-images/SessionSharing-Sample-frameowrk-ent.png)


## Prerequisites

Before the sample application is executed, make sure that:

- .NET 8.0 SDK and .NET Framework 4.8.1 or Visual Studio (recommended) installed.
- NCache is installed and running in an accessible location.
  - If not, visit the following link to get started:\
  https://www.alachisoft.com/resources/docs/ncache/getting-started/ncache.html
- Ensure that `demoCache` (or another cache of your choice) is running.
  - This is created during installation, otherwise you can create a new cache via this link:\
  https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html
- NuGet package required (The package references are already included in the project file).
	- `Alachisoft.NCache.SDK`
	- `AspNet.SessionState.NCache`
	- `AspNetCore.Session.NCache`

## Build and Run the Sample

Before building this sample, if you need to change the cache name, open `appsettings.json` from the project root directory and change the `CacheName` value:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "NCacheSettings": {
    "CacheName": "demoCache",
    "SessionAppId": "NCacheSharedSessionApp",
    "EnableSessionLocking": false,
    "SessionLockingRetry": -1,
    "EnableLogs": false,
    "EnableDetailLogs": false,
    "ExceptionsEnabled": false,
    "OperationRetry": 0,
    "operationRetryInterval": 0,
    "EnableSessionSharing": true,
    "SessionOptions": {
      "CookieName": "ASP.NET_CORE_SessionId",
      "CookieDomain": null,
      "CookiePath": "/",
      "CookieHttpOnly": "True",
      "IdleTimeout": "20",
      "CookieSecure": "None"
    }
  }
}
```

Also change `cacheName` in `Web.Config`:

```xml
<sessionState cookieless="false" regenerateExpiredSessionId="true" mode="Custom" customProvider="NCacheSessionProvider" timeout="20" cookieName="ASP.NET_CORE_SessionId">
  <providers>
    <add name="NCacheSessionProvider" type="Alachisoft.NCache.Web.SessionState.NSessionStoreProvider" exceptionsEnabled="true" enableSessionLocking="true" emptySessionWhenLocked="false" sessionLockingRetry="-1" sessionAppId="NCacheSharedSessionApp" useInProc="false" enableLogs="false" cacheName="demoCache" writeExceptionsToEventLog="false" AsyncSession="false" enableSessionSharing="true" useJsonSerialization="true"/>
  </providers>
</sessionState>
```

### Build

From Visual Studio (Recommended):
- Open `SessionSharing.sln`.
- Let NuGet packages restore.
- Build and run.

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine,
# e.g. d:\ncache-samples\SessionSharing
cd "<path-to-sample>"
dotnet restore
dotnet build
```

### Run

You need to run the SessionSharing projects (`GameNetCore` and `GameNetFramework`).

From Visual Studio (Recommended):
- Setup both games as multiple startup projects by following the instructions on:\
https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects

From command line (optional):
```bash
# Replace <path-to-sample> with the sample location on your machine
# Example: d:\ncache-samples\SessionSharing
# --- Start the ASP.NET Core MVC application ---
cd "<path-to-sample>\GameNetCore"
dotnet run
# --- Start the .NET Framework Webforms application ---
"C:\Program Files\IIS Express\iisexpress.exe" /path:"<path-to-sample>\GameNetFramework" /port:<portNumber>
```

## Notes

- If NuGet packages are not restored properly:
	- Visual Studio: Right‑click the solution → Restore NuGet Packages.
	- Command line: run `dotnet restore` in the sample folder (see Build and Run section).
	- If using nuget.exe: run `nuget restore SessionSharing.sln`.

## References

Reference documentation is available at:\
https://www.alachisoft.com/resources/docs/ncache/prog-guide/aspnet.html


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