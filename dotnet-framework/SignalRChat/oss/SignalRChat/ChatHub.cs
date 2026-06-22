using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task SendMessageToAll(string username, string message)
    {
        Clients.All.receiveMessage(username, message);
    }

    public Task SendMessageToSpecificConnection(string connectionId, string username, string message)
    {
        Clients.Client(connectionId).receiveMessage(username, message);
        return Task.CompletedTask;
    }

    public Task SendMessageToSpecificGroup(string groupName, string username, string message)
    {
        Clients.Group(groupName)
               .receiveMessage($"{username}({groupName})", message);
        return Task.CompletedTask;
    }

    public Task JoinGroup(string groupName)
    {
        Groups.Add(Context.ConnectionId, groupName);

        Clients.Caller.receiveMessage("System", $"Joined group: {groupName}");
        return Task.CompletedTask;
    }

    public Task LeaveGroup(string groupName)
    {
        Groups.Remove(Context.ConnectionId, groupName);

        Clients.Caller.receiveMessage("System", $"Left group: {groupName}");
        return Task.CompletedTask;
    }
}