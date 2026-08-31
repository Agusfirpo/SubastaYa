using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Aplicacion.Interfaces.Repositories
{
    public interface IPujaRepository
    {
        Task<IList<Puja>> ObtenerPorSubastaIdAsync(int subastaId);

        Task<Puja?> ObtenerMayorPorSubastaIdAsync(int subastaId);

        Task AgregarAsync(Puja puja);
        
    }
}