using Application.Exceptions;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task EjecutarEnTransaccionAsync(Func<Task> accion)
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                await accion();

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaccion.RollbackAsync();

                _context.ChangeTracker.Clear();

                throw new ConcurrencyException("La información fue modificada por otro usuario.");
            }
            catch
            {
                await transaccion.RollbackAsync();

                throw;
            }
        }
    }
}