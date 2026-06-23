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

using Alachisoft.NCache.AspNet.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;
using System.Threading.Tasks;

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

// Specify the OWIN startup class
[assembly: OwinStartup(typeof(SignalRChat.Startup))]

namespace SignalRChat
{
    /// <summary>
    /// Startup class to configure SignalR with NCache backplane
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Method to configure SignalR and NCache backplane
        /// </summary>
        /// <param name="app">The OWIN application builder.</param>
        public void Configuration(IAppBuilder app)
        {
            // Read NCache configuration settings from Web.config
            string cache = ConfigurationManager.AppSettings["cache"];
            string eventKey = ConfigurationManager.AppSettings["eventKey"];

            // Configure NCache as the backplane for SignalR
            GlobalHost.DependencyResolver.UseNCache(cache, eventKey);

            // Map SignalR hubs (i.e., ChatHub) to the app builder pipeline
            app.MapSignalR();
        }
    }
}
