# NCache ASP.NET Samples

This folder contains **.NET Core** samples demonstrating various features and capabilities of [NCache](https://www.alachisoft.com/ncache/). These samples target .NET Core 3.1, .NET 5, .NET 6, .NET 7, and .NET 8+ platforms.

## Overview

NCache is a 100% native .NET in-memory distributed cache that provides extreme performance and scalability for .NET Core applications.

These samples provide practical, ready-to-run examples that help developers quickly integrate NCache features into their ASP.NET Core and .NET Core applications.

Each sample includes complete source code, configuration files, and detailed README instructions to get you started quickly.

## Available Samples

1. **[AsynchronousOperations](./AsynchronousOperations)**\
   Demonstrates asynchronous cache operations for non-blocking Add, Get, Update, and Remove operations.
2. **[BasicCachingOperations](./BasicCachingOperations)**\
   Demonstrates fundamental cache CRUD operations including Add, Get, Update, Remove, and Contains.
3. **[BulkOperations](./BulkOperations)**\
   Shows how to perform bulk cache operations like AddBulk, GetBulk, UpdateBulk, and RemoveBulk for improved performance.
4. **[CacheItemLocking](./CacheItemLocking)**\
   Demonstrates pessimistic locking mechanism to lock and unlock cached items for concurrent access control.
5. **[CacheItemVersioning](./CacheItemVersioning)**\
   Shows how to use optimistic locking with item versioning to handle concurrent updates to cache items.
6. **[CacheLoaderRefresher](./CacheLoaderRefresher)**\
   Demonstrates how to implement custom cache loader and refresher to automatically populate and refresh cache data.
7. **[DataStructures](./DataStructures)**\
   Shows how to use NCache distributed data structures like Counter, List, Queue, and Dictionary.
8. **[EFCoreCaching](./EFCoreCaching)**\
   Demonstrates caching Entity Framework Core queries using NCache as a second-level cache provider.
9. **[IDistributedCache](./IDistributedCache)**\
    Implements Microsoft's IDistributedCache interface using NCache as the backing store for ASP.NET Core applications.
10. **[Microservices](./Microservices)**\
    Demonstrates implementing distributed caching patterns in microservices architecture using NCache.
11. **[NHibernateCore](./NHibernateCore)**\
    Demonstrates NCache as a second-level cache provider for NHibernate Core ORM to improve database query performance.
12. **[PubSub](./PubSub)**\
    Implements publish/subscribe messaging pattern using NCache topics for real-time message distribution.
13. **[ResponseCaching](./ResponseCaching)**\
    Demonstrates ASP.NET Core response caching middleware using NCache to cache HTTP responses.
14. **[SearchUsingLINQ](./SearchUsingLINQ)**\
    Shows how to search and query cached objects using LINQ expressions with NCache. 
15. **[SearchUsingSQL](./SearchUsingSQL)**\
    Demonstrates searching cached objects using SQL-like query syntax in NCache.
16. **[SessionSharing](./SessionSharing)**\
    Shows how to share ASP.NET Core session state across multiple applications using NCache.
17. **[SignalRChat](./SignalRChat)**\
    Demonstrates using NCache as a backplane for ASP.NET Core SignalR to scale out real-time applications.
28. **[TagsNamedTags](./TagsNamedTags)**\
    Shows how to use tags and named tags to logically group and search cached items for bulk operations.

## Additional Resources

### Playground

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

