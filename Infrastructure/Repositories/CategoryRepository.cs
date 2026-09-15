using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Category>> ObtenerTodasAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Categorias
                .ToArrayAsync(cancellationToken);
        }
    }
}