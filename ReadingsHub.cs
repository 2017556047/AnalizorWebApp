using Microsoft.AspNetCore.SignalR;

namespace AnalizorWebApp.Hubs
{
    public class ReadingsHub : Hub
    {
        public Task JoinDeviceGroup(int deviceId)
            => Groups.AddToGroupAsync(Context.ConnectionId, $"device-{deviceId}");

        public Task LeaveDeviceGroup(int deviceId)
            => Groups.RemoveFromGroupAsync(Context.ConnectionId, $"device-{deviceId}");
    }
}