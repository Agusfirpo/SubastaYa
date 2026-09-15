using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AppDbContext _context;

        public BidRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Bid>> ObtenerPorSubastaIdAsync(
            int subastaId,
            CancellationToken cancellationToken)
        {
            return await _context.Pujas
                .AsNoTracking()
                .Where(p => p.SubastaId == subastaId)
                .OrderByDescending(p => p.FechaPuja)
                .ToListAsync(cancellationToken);
        }

        public async Task<Bid?> ObtenerMayorPorSubastaIdAsync(
            int subastaId,
            CancellationToken cancellationToken)
        {
            return await _context.Pujas
                .AsNoTracking()
                .Where(p => p.SubastaId == subastaId)
                .OrderByDescending(p => p.Monto)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AgregarAsync(
            Bid puja,
            CancellationToken cancellationToken)
        {
            await _context.Pujas.AddAsync(
                puja,
                cancellationToken);
        }

        public async Task<IList<Bid>> ObtenerPorCompradorIdAsync(
            int compradorId,
            CancellationToken cancellationToken)
        {
            return await _context.Pujas
                .Include(p => p.Subasta)
                .ThenInclude(s => s.Pujas)
                .Where(p => p.CompradorId == compradorId)
                .ToListAsync(cancellationToken);
        }
    }
}