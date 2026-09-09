using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;
        public WalletRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task<Wallet?> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Billeteras.FirstOrDefaultAsync(b => b.Id == usuarioId);
        }
    }
}
