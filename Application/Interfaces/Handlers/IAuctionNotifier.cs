using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Handlers
{
    public interface IAuctionNotifier
    {
        Task NotificarNuevaPuja(int subastaUd, decimal monto, int compradorId, DateTime fechafin, bool tiempoExtendido);
    }
}
