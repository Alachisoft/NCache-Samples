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

using Microsoft.AspNetCore.SignalR;
using SignalRChatNCache.Controllers;

namespace SignalRChatNCache.Hubs
{
    /// <summary>
    /// Represents a hub for managing real-time messaging between connected clients.
    /// </summary>
    public class MessageHub : Hub
    {
        /// <summary>
        /// Method that sends a message from a user to all other connected clients
        /// </summary>
        /// <param name="user">The name of the user sending the message. Cannot be null or empty.</param>
        /// <param name="message">The message content to be sent. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        public Task SendMessage(string user, string message)
        {
            // Save message for persistence
            AnnouncementController.MessageStore.Messages.Add((user, message));

            // Broadcast to all OTHER clients
            return Clients.Others.SendAsync("ReceiveMessage", user, message);
        }
    }
}
