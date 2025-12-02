// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat
{
    /// <summary>
    /// ChatHub class to handle real-time chat functionality
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>
        /// Sends a message from a specified user to all connected clients.
        /// </summary>
        /// <remarks>This method broadcasts the specified message to all clients currently connected to
        /// the server.</remarks>
        /// <param name="user">The name of the user sending the message. Cannot be null or empty.</param>
        /// <param name="message">The content of the message to send. Cannot be null or empty.</param>
        public void SendMessage(string user, string message)
        {
            // Broadcast the message to all connected clients
            Clients.All.broadcastMessage(user, message);
        }
    }
}