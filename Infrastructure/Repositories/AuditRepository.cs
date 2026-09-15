using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _context;

        public AuditRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(
            AuditLog auditoria,
            CancellationToken cancellationToken)
        {
            await _context.AuditoriaLogs.AddAsync(
                auditoria,
                cancellationToken);
        }
    }
}