using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Infraestructura.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly AppDbContext _context;

        public TransaccionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(TransaccionLedger transaccion)
        {
            await _context.TransaccionLedgers.AddAsync(transaccion);
        }
    }
}