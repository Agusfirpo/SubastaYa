using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;

        public WalletRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet?> ObtenerPorUsuarioAsync(int usuarioId, CancellationToken cancellationToken)
        {
            return await _context.Billeteras.FirstOrDefaultAsync(b => b.Id == usuarioId,cancellationToken);
        }
    }
}