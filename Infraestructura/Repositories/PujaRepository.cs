using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    public class PujaRepository : IPujaRepository
    {
        private readonly AppDbContext _context;

        public PujaRepository(AppDbContext context) 
        { 
            _context = context; 
        }

        public async Task<IList<Puja>> ObtenerPorSubastaIdAsync (int subastaId)
        {
            return await _context.Pujas
                .AsNoTracking()
                .Where(p => p.SubastaId == subastaId)
                .OrderByDescending(p => p.FechaPuja)
                .ToListAsync();
        }
    }
}
