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
    }
}