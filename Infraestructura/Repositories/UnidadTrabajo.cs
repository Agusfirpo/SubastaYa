using Aplicacion.Exceptions;
using Aplicacion.Interfaces.Repositories;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly AppDbContext _context;
        public UnidadTrabajo(AppDbContext context)
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

                throw new ConcurrenciaException("La subasta fue modificada por otro usuario.");
            }
            catch
            {
                await transaccion.RollbackAsync();

                throw;
            }
        }
    }
}