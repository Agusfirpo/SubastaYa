using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces.Handlers
{
    public interface INotificadorSubasta
    {
        Task NotificarNuevaPuja(int subastaUd, decimal monto, int compradorId, DateTime fechafin, bool tiempoExtendido);
    }
}
