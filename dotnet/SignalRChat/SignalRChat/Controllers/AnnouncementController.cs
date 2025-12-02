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

using Microsoft.AspNetCore.Mvc;

namespace SignalRChatNCache.Controllers
{
    /// <summary>
    /// This controller demonstrates how to use SignalR with NCache as a backplane
    /// to broadcast messages to all connected clients
    /// </summary>
    public class AnnouncementController : Controller
    {
        /// <summary>
        /// Method to display the index view with existing messages
        /// </summary>
        /// <returns>Index view</returns>
        [HttpGet]
        public IActionResult Index()
        {
            // Passing existing messages to the view
            return View(MessageStore.Messages);
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method stores static in-memory messages for the duration of the application's lifetime
        /// </summary>
        public static class MessageStore
        {
            // List that holds (User, Message) pairs
            public static List<(string User, string Message)> Messages { get; } = new();
        }
    }
}
