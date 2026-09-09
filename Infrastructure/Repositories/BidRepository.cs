using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AppDbContext _context;
        public BidRepository(AppDbContext context) 
        { 
            _context = context; 
        }
        public async Task<IList<Bid>> ObtenerPorSubastaIdAsync (int subastaId)
        {
            return await _context.Pujas
                .AsNoTracking()
                .Where(p => p.SubastaId == subastaId)
                .OrderByDescending(p => p.FechaPuja)
                .ToListAsync();
        }
        public async Task<Bid?> ObtenerMayorPorSubastaIdAsync(int subastaId)
        {
            return await _context.Pujas
                .AsNoTracking()
                .Where(p => p.SubastaId == subastaId)
                .OrderByDescending(p => p.Monto)
                .FirstOrDefaultAsync();
        }
        public async Task AgregarAsync(Bid puja)
        {
            await _context.Pujas.AddAsync(puja);
        }
        public async Task<IList<Bid>> ObtenerPorCompradorIdAsync(int compradorId)
        {
            return await _context.Pujas
                .Include(p => p.Subasta)
                .ThenInclude(s => s.Pujas)
                .Where(p => p.CompradorId == compradorId)
                .ToListAsync();
        }
    }
}
