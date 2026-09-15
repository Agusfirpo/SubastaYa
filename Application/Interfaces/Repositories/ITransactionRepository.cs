using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task AgregarAsync(LedgerTransaction transaccion,CancellationToken cancellationToken);

        Task<IList<LedgerTransaction>> ObtenerPorBilleteraIdAsync(int billeteraId,CancellationToken cancellationToken);

    }
}