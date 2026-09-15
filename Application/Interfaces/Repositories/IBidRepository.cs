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
        Task<IList<Bid>> ObtenerPorSubastaIdAsync(int subastaId,CancellationToken cancellationToken);

        Task<Bid?> ObtenerMayorPorSubastaIdAsync(int subastaId,CancellationToken cancellationToken);

        Task AgregarAsync(Bid puja,CancellationToken cancellationToken);

        Task<IList<Bid>> ObtenerPorCompradorIdAsync(int compradorId,CancellationToken cancellationToken);

    }
}