// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Alachisoft.NCache.Caching.Distributed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

// ===============================================================================
// NCache Distributed Caching enables storing and retrieving data across multiple
// application instances. It provides a shared, fast, in-memory cache that
// improves performance and scalability in distributed or load-balanced systems.
// Some simple use cases include:
//   1. Caching frequently accessed data to reduce database load (e.g., product
//      catalogs, configuration settings).
//   2. Storing user session or authentication data across multiple servers in a
//      web farm.
//   3. Sharing temporary application state between different application
//      instances.
//   4. Caching results of expensive or complex queries to improve application
//      performance.
//   5. Providing a shared data layer for real-time applications like dashboards
//      or collaborative apps.
// In this sample, we use NCache as a distributed cache to store and retrieve data
// objects using the IDistributedCache interface.
// ===============================================================================

// Creating the web application builder
var builder = WebApplication.CreateBuilder(args);

// ===============================================================================
// NCache is registered as the underlying distributed caching provider using the
// AddNCacheDistributedCache extension method. This method is part of the
// Alachisoft.NCache.Caching.Distributed namespace. It takes a parameter of type
// IConfigurationSection that specifies the NCache configuration settings. This
// includes details such as the cache name, server addresses, and other relevant
// options. Once added, it enables the IDistributedCache interface throughout all
// application instances to perform caching operations (Set, Get, Remove, etc.)
// ===============================================================================
builder.Services.AddNCacheDistributedCache(builder.Configuration.GetSection("NCacheSettings"));

// Adding MVC (or controllers with views)
builder.Services.AddControllersWithViews();

// Building application
var app = builder.Build();

// Configuring error handling based on environment
if (app.Environment.IsDevelopment())
{
    // Detailed error page for development
    app.UseDeveloperExceptionPage();
}
else
{
    // Generic error handler for production
    app.UseExceptionHandler("/Error");
}

// Enabling static files, routing, and authorization
app.UseStaticFiles();

// Setting up routing and authorization
app.UseRouting();
app.UseAuthorization();
app.MapDefaultControllerRoute();

// Caching the current time on application start
var cache = app.Services.GetRequiredService<IDistributedCache>();

// Running the application
app.Run();