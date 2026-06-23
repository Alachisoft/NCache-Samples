// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.AspNetCore.SignalR;
using SignalRChatNCache.Hubs;

// ===============================================================================
// NCache can be integrated as a backplane for SignalR to provide a scalable
// solution for real-time web applications. This allows multiple servers to share
// real-time messages efficiently, ensuring that all connected clients receive
// updates regardless of the server or platform they are connected to. Some use
// cases include:
//   1. Live notifications (e.g., chat messages, alerts, or announcements)
//   2. Real-time dashboards and live data updates
//   3. Collaborative applications where multiple users see changes instantly
//   4. Multiplayer gaming applications
//   5. Live sports updates and scoreboards
// ===============================================================================

// Creating the web application builder
var builder = WebApplication.CreateBuilder(args);

// Loading application configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Adding MVC controllers and SignalR services to the application
builder.Services.AddControllersWithViews();

// Configuring NCache settings from configuration file
builder.Services.Configure<NCacheConfiguration>(builder.Configuration.GetSection("NCacheConfiguration"));

// ===============================================================================
// NCache with SignalR Integration is done through the AddNCache() extension
// method provided by the NCache SignalR integration package. It allows NCache to
// be configured as the backplane for SignalR by specifying the cache name and
// event key used for communication between SignalR servers. The integration is set
// up by chaining the AddNCache() method to the AddSignalR().
// ================================================================================
builder.Services.AddSignalR().AddNCache(options =>
{
    // Retrieving NCache configuration values from appsettings.json
    options.CacheName = builder.Configuration["NCacheConfiguration:CacheName"];

    options.EventKey = builder.Configuration["NCacheConfiguration:EventKey"]; // here change property name to ApplicationID

});

// Building the web application
var app = builder.Build();

// Configuring middleware pipeline for handling requests
if (app.Environment.IsDevelopment())
{
    // Using developer exception page during development
    app.UseDeveloperExceptionPage();
}
else
{
    // Using global error handler and enabling HSTS for production
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Enabling HTTPS redirection and serving static content
app.UseHttpsRedirection();
app.UseStaticFiles();

// Enabling routing for incoming requests
app.UseRouting();

// Mapping the SignalR hub endpoint
app.MapHub<MessageHub>("/messages");

// Mapping default MVC controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Announcement}/{action=Index}/{id?}"
);

// Running the application
app.Run();
