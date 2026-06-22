# NCache Samples

This repository contains source code samples for **[NCache](https://www.alachisoft.com/ncache/)** demonstrating various features and capabilities. Each sample contains a *Readme.md* file that has instructions and pre-requisites to run that sample.

**NCache** is a 100% .NET/.NET Core in-memory distributed cache & datastore which provides an extremely fast and highly scalable cache for application data to reduce expensive database trips.

## Overview

These samples help developers get started with NCache by providing practical examples of implementing caching solutions for different scenarios. Each sample includes complete source code with detailed instructions for setup and execution.

## Platforms

- .NET Core Samples (`/dotnet`)
	- Samples targeting .NET Core, .NET 5, .NET 6, .NET 7, and .NET 8+ platforms.
- .NET Framework Samples (`/dotnet-framework`)
	- Samples targeting .NET Framework 4.6.1, 4.7.2, 4.8, and higher versions.

## Prerequisites

Before running these samples, ensure you have:

1. **NCache** installed on your machine
   - Download from [Alachisoft](https://www.alachisoft.com/download-ncache.html)
   - Follow the [Installation Guide](https://www.alachisoft.com/resources/docs/ncache/install-guide/)
2. **Development Environment**
   - Visual Studio 2019 or later (for .NET Framework samples)
   - Visual Studio 2019/2022 or VS Code (for .NET Core samples)
   - .NET Framework 4.6.1+ and/or .NET Core 3.1+ / .NET 5+
3. **NCache Running**
   - Ensure NCache service is running
   - Create and start a cache named `demoCache` (or modify the cache name in samples)

## Getting Started

1. **Clone the Repository**
2. **Choose Your Platform**
	- Navigate to either `dotnet` (for .NET Core) or `dotnet-framework` (for .NET Framework) folder based on your target platform
3. **Open a Sample**
	- Open the solution file (`.sln`) in Visual Studio
	- Restore NuGet packages
	- Update the cache name in configuration if needed
4. **Build and Run**
	- Build the solution
	- Run the sample application
	- Follow the on-screen instructions or refer to the sample's README

## Sample Structure

- Each sample includes:
	- Complete source code
	- README.md with detailed instructions
	- Configuration files
	- Step-by-step execution guide
	- Expected output examples

## Key Features Demonstrated

- **Cache CRUD Operations** - Add, Get, Update, Delete, Bulk operations
- **Events & Notifications** - Item-level event notifications
- **Pub/Sub Messaging** - Topic-based publish/subscribe pattern
- **Session Management** - ASP.NET and ASP.NET Core session state
- **Response Caching** - HTTP response caching for web applications
- **Lock/Unlock** - Concurrent access control
- **Expiration** - Absolute and sliding expiration policies
- **Eviction** - Cache eviction strategies

## Additional Resources

### Playground

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