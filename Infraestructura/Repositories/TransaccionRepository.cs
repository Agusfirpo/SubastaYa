using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
        public async Task<IList<TransaccionLedger>> ObtenerPorBilleteraIdAsync(int billeteraId)
        {
            return await _context.TransaccionLedgers.AsNoTracking().Where(t=>t.BilleteraId == billeteraId).OrderByDescending(t =>t.Fecha).ToListAsync();
        }
    }
}