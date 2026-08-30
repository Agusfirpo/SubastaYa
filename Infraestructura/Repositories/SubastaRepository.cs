using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class SubastaRepository : ISubastaRepository
    {
        private readonly AppDbContext _context;

        public SubastaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Subasta>> ObtenerTodasAsync()
        {
            return await _context.Subastas
                .Include(s => s.Categoria)
                .ToListAsync();
        }

        public async Task AgregarAsync(Subasta subasta)
        {
            await _context.Subastas.AddAsync(subasta);

            await _context.SaveChangesAsync();
        }

        public async Task<Subasta?> ObtenerPorIdAsync(int id)
        {
            return await _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Vendedor)
                .Include(s => s.Pujas)
                .FirstOrDefaultAsync(s => s.Id == id);
        }



        public async Task<IList<Subasta>> ObtenerPorVendedorIdAsync(int vendedorId)
        {
            return await _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Pujas)
                .Where(s => s.VendedorId == vendedorId)
                .ToListAsync();
        }
    }
}