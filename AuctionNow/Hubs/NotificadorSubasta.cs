using Aplicacion.Interfaces.Handlers;
using Microsoft.AspNetCore.SignalR;

namespace SubastaYa.Hubs
{
    public class NotificadorSubasta : INotificadorSubasta
    {
        private readonly IHubContext<SubastaHub> _hubContext;

        public NotificadorSubasta(IHubContext<SubastaHub> hubContext)
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