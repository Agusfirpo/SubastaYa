using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces.Repositories
{
    public interface IUnidadTrabajo
    {
        Task EjecutarEnTransaccionAsync(Func<Task> accion);
    }
}
