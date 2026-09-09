using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBidRepository
    {
        Task<IList<Bid>> ObtenerPorSubastaIdAsync(int subastaId);

        Task<Bid?> ObtenerMayorPorSubastaIdAsync(int subastaId);

        Task AgregarAsync(Bid puja);

        Task<IList<Bid>> ObtenerPorCompradorIdAsync(int compradorId);

    }
}