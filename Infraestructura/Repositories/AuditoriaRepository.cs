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
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly AppDbContext _context;
        public AuditoriaRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AgregarAsync(AuditoriaLog auditoria)
        {
            await _context.AuditoriaLogs.AddAsync(auditoria);
        }
    }
}