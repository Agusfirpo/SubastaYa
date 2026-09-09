using Application.Interfaces.Handlers;
using Microsoft.AspNetCore.SignalR;

namespace Api_SubastaYa.Hubs
{
    public class AuctionNotifier : IAuctionNotifier
    {
        private readonly IHubContext<AuctionHub> _hubContext;

        public AuctionNotifier(IHubContext<AuctionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotificarNuevaPuja(int subastaId,decimal monto,int compradorId,DateTime fechaFin,bool tiempoExtendido)
        {
            await _hubContext.Clients.Group($"subasta-{subastaId}").SendAsync("NuevaPuja",
                new
                    {
                        subastaId,
                        monto,
                        compradorId,
                        fechaFin,
                        tiempoExtendido
                    });
        }
    }
}