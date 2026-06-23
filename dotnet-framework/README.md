# NCache ASP.NET Framework Samples

This folder contains **.NET Framework** samples demonstrating various features and capabilities of [NCache](https://www.alachisoft.com/ncache/). These samples target .NET Framework 4.6.1, 4.7.2, and 4.8 platforms.

## Overview

NCache is a 100% native .NET in-memory distributed cache that provides extreme performance and scalability for .NET Framework applications.

These samples provide practical, ready-to-run examples that help developers quickly integrate NCache features into their ASP.NET Framework applications.

Each sample includes complete source code, configuration files, and detailed README instructions to get you started quickly.

## Available Samples

1. **[AsynchronousOperations](./AsynchronousOperations)**
   - Demonstrates asynchronous cache operations for non-blocking Add, Get, Update, and Remove operations.
2. **[BackingSource](./BackingSource)**
   - Shows how to configure and use read-through, write-through, and write-behind operations with backing source.
3. **[BasicCachingOperations](./BasicCachingOperations)**
   - Demonstrates fundamental cache CRUD operations including Add, Get, Update, Remove, and Contains.
4. **[BulkOperations](./BulkOperations)**
   - Shows how to perform bulk cache operations like AddBulk, GetBulk, UpdateBulk, and RemoveBulk for improved performance.
5. **[CacheItemLocking](./CacheItemLocking)**
   - Demonstrates pessimistic locking mechanism to lock and unlock cached items for concurrent access control.
6. **[CacheItemVersioning](./CacheItemVersioning)**
   - Shows how to use optimistic locking with item versioning to handle concurrent updates to cache items.
7. **[CacheLoaderRefresher](./CacheLoaderRefresher)**
   - Demonstrates how to implement custom cache loader and refresher to automatically populate and refresh cache data.
8. **[DataStructures](./DataStructures)**
   - Shows how to use NCache distributed data structures like Counter, List, Queue, and Dictionary.
9. **[Events](./Events)**
   - Shows how to register and handle item-level event notifications for cache operations like Add, Update, and Remove.
10. **[NHibernate](./NHibernate)**
    - Demonstrates NCache as a second-level cache provider for NHibernate ORM to improve database query performance.
11. **[PubSub](./PubSub)**
    - Implements publish/subscribe messaging pattern using NCache topics for real-time message distribution.
12. **[SearchUsingLINQ](./SearchUsingLINQ)**
    - Shows how to search and query cached objects using LINQ expressions with NCache.
13. **[SearchUsingSQL](./SearchUsingSQL)**
    - Demonstrates searching cached objects using SQL-like query syntax in NCache.
14. **[SessionSharing](./SessionSharing)**
    - Shows how to share ASP.NET session state across multiple applications using NCache.
15. **[SignalRChat](./SignalRChat)**
    - Demonstrates using NCache as a backplane for ASP.NET SignalR to scale out real-time applications.
16. **[TagsNamedTags](./TagsNamedTags)**
    - Shows how to use tags and named tags to logically group and search cached items for bulk operations.
17. **[ViewState](./ViewState)**
    - Demonstrates caching ASP.NET view state in NCache to reduce page size and improve performance.

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