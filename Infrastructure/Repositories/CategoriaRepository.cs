using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;
        public CategoriaRepository(AppDbContext context) 
        { 
            _context = context;
        }
        public async Task<IList<Categoria>> ObtenerTodasAsync()
        {
            return await _context.Categorias.ToArrayAsync();
        }
    }
}
