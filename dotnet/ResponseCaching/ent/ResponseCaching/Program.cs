// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.ResponseCaching;
using Microsoft.EntityFrameworkCore;
using ResponseCaching.Models;
using System;

// ===============================================================================
// NCache response caching is a powerful feature that enhances the performance of
// ASP.NET Core applications by storing frequently accessed responses in a
// distributed cache. This reduces server load and speeds up response times for
// end-users by serving cached content directly from NCache. Some use cases for
// NCache response caching include:
//   1. Caching frequently requested data (e.g., product listings, user profiles).
//   2. Reducing database load by caching results of expensive queries.
//   3. Improving scalability by distributing cached responses across multiple
//      cache servers.
//   4. Enhancing user experience by delivering faster page loads.
//   5. Caching API responses to reduce latency for mobile and web applications.
// NCache response caching is particularly beneficial for applications with high
// read-to-write ratios, where the same data is requested multiple times.
// ===============================================================================

// Creating and configuring the WebApplication builder instance
var builder = WebApplication.CreateBuilder(args);

// Add MVC services (controllers + views)
builder.Services.AddControllersWithViews();

// Configuring DbContext
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DemoDBServer"]));

// Registering repositories
builder.Services.AddScoped<IProductsRepository, ProductRepository>();

// Reading NCache-specific configurations from appsettings.json
builder.Services.AddOptions()
    .Configure<NCacheConfiguration>(builder.Configuration.GetSection("NCacheSettings"));

// ===============================================================================
// NCache Integrating is done through the AddNCacheResponseCachingServices()
// extension method provided by the NCache Response Caching integration package.
// This method sets up NCache as the response caching provider for ASP.NET Core
// applications, allowing responses to be cached in a distributed cache.
// ===============================================================================
builder.Services.AddNCacheResponseCachingServices();

var app = builder.Build();

// Configuring the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirecting HTTP requests to HTTPS
app.UseHttpsRedirection();

// Serving static files like CSS, JS, images, etc
app.UseStaticFiles();

// Routes incoming requests to matching controller actions
app.UseRouting();

// Checking user permissions before accessing resources
app.UseAuthorization();

// Defining default route for controllers and actions
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{ProductID?}");

// Runs the application
app.Run();
