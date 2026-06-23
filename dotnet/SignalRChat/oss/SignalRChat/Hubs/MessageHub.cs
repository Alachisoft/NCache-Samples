
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
        public Task SendMessageToAll(string username, string message)
        {
            // Broadcast to all OTHER clients
            return Clients.All.SendAsync("ReceiveMessage", username, message);
        }

        public Task SendMessageToSpecificConnection(string connectionId, string username, string message)
        {
            return Clients.Client(connectionId)
                .SendAsync("ReceiveMessage", username, message);
        }

        public Task SendMessageToSpecificGroup(string groupName, string username, string message)
        {
            return Clients.Group(groupName)
                .SendAsync("ReceiveMessage", $"{username}({groupName})", message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("ReceiveMessage", "System", $"Joined group: {groupName}");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.SendAsync("ReceiveMessage", "System", $"Left group: {groupName}");
        }
    }
}
