using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    public class BilleteraRepository : IBilleteraRepository
    {
        private readonly AppDbContext _context;
        public BilleteraRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task<Billetera?> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Billeteras.FirstOrDefaultAsync(b => b.Id == usuarioId);
        }
    }
}
