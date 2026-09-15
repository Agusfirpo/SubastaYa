using Microsoft.AspNetCore.SignalR;

namespace Api_SubastaYa.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task UnirseASubasta(int subastaId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"subasta-{subastaId}");
        }

        public async Task SalirDeSubasta(int subastaId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"subasta-{subastaId}");
        }
    }
}
