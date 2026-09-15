using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task EjecutarEnTransaccionAsync(Func<Task> action, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await action();

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync(CancellationToken.None);

                _context.ChangeTracker.Clear();

                throw;
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None);

                throw;
            }
        }
    }
}