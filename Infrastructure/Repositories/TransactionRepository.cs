using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(
            LedgerTransaction transaccion,
            CancellationToken cancellationToken)
        {
            await _context.TransaccionLedgers.AddAsync(
                transaccion,
                cancellationToken);
        }

        public async Task<IList<LedgerTransaction>> ObtenerPorBilleteraIdAsync(
            int billeteraId,
            CancellationToken cancellationToken)
        {
            return await _context.TransaccionLedgers
                .AsNoTracking()
                .Where(t => t.BilleteraId == billeteraId)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync(cancellationToken);
        }
    }
}