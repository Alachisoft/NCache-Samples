// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Web.SessionState;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ===============================================================================
// NCache Session Sharing allows multiple applications to share the same user
// session state regardless of the platform they are built on. This is especially
// useful for scenarios where you have applications running on different .NET
// platforms such as one in .NET Core and the other in .NET Framework. Some simple
// use cases include:
//   1. Migrating legacy .NET Framework apps to .NET Core without forcing users to
//      log in again.
//   2. Sharing user session state across multiple web applications or modules.
//   3. Maintaining session consistency in load-balanced or multi-server systems.
//   4. Enabling real-time collaboration features that rely on shared session
//      data.
//   5. Preserving user-specific data (like shopping carts or preferences) across
//      different platforms.
// ===============================================================================

// Creating and configuring the WebApplication builder instance
var builder = WebApplication.CreateBuilder(args);

// Configuring application settings
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Adding MVC controllers and views to the dependency injection container
builder.Services.AddControllersWithViews();

//Adding a Shared Encryption Key
builder.Services.AddDataProtection()
    .SetApplicationName("NCacheSharedSessionApp") // IMPORTANT: must be identical in all apps
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\SharedKeys"));
// ===============================================================================
// NCache is registered as the session provider in the ASP.NET Core dependency
// injection container by the AddNCacheSession extension method. This method takes
// configuration settings as input to set up NCache for session management. The
// configuration parameters (like cache name, session options, etc.) are read from
// the "NCacheSettings" section of appsettings.json.
// ===============================================================================
builder.Services.AddNCacheSession(builder.Configuration.GetSection("NCacheSettings"));

// Building the WebApplication instance
var app = builder.Build();

// Configuring middleware for error handling based on the environment
if (app.Environment.IsDevelopment())
{
    // Enabling detailed error pages in development environment
    app.UseDeveloperExceptionPage();
}
else
{
    // Configuring a generic error handler and enabling HSTS in production environment
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirecting all HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enabling serving of static files like CSS, JavaScript, and images
app.UseStaticFiles();

// Enabling routing to map requests to controllers and actions
app.UseRouting();

// ===============================================================================
// This middleware activates NCache session state management in the request
// pipeline. It ensures that session data is stored and retrieved from NCache
// instead of the default in-memory or SQL-based session store.
// ===============================================================================
app.UseNCacheSession();

// Enabling authorization middleware for protecting endpoints
app.UseAuthorization();

// Configuring routes for MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "guess",
    pattern: "{controller=Home}/{action=Guess}/{id?}");

// Running the application
app.Run();