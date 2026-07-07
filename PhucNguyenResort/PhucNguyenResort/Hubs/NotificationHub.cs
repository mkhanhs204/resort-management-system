using Microsoft.AspNetCore.SignalR;

namespace PhucNguyenResort.Hubs;

/// <summary>
/// SignalR Hub for real-time notification delivery.
/// Uses the ClaimTypes.NameIdentifier claim as the user identifier 
/// to match ASP.NET Core cookie authentication.
/// </summary>
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
