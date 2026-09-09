using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _context;
        public AuditRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AgregarAsync(AuditLog auditoria)
        {
            await _context.AuditoriaLogs.AddAsync(auditoria);
        }
    }
}