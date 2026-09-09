using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AgregarAsync(LedgerTransaction transaccion)
        {
            await _context.TransaccionLedgers.AddAsync(transaccion);
        }
        public async Task<IList<LedgerTransaction>> ObtenerPorBilleteraIdAsync(int billeteraId)
        {
            return await _context.TransaccionLedgers.AsNoTracking().Where(t=>t.BilleteraId == billeteraId).OrderByDescending(t =>t.Fecha).ToListAsync();
        }
    }
}