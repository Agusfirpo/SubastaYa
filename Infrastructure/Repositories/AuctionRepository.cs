using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AppDbContext _context;

        public AuctionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Auction>> ObtenerTodasAsync(CancellationToken cancellationToken)
        {
            return await _context.Subastas.Include(s => s.Categoria).ToListAsync(cancellationToken);
        }

        public async Task AgregarAsync(Auction subasta,CancellationToken cancellationToken)
        {
            await _context.Subastas.AddAsync(subasta,cancellationToken);
        }
        public async Task<Auction?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Vendedor)
                .Include(s => s.Pujas)
                .FirstOrDefaultAsync(s => s.Id == id
                ,cancellationToken);
        }

        public async Task<IList<Auction>> ObtenerPorVendedorIdAsync(int vendedorId, CancellationToken cancellationToken)
        {
            return await _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Pujas)
                .Where(s => s.VendedorId == vendedorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Auction?> ObtenerPorIdParaActualizarAsync(int id,CancellationToken cancellationToken)
        {
            return await _context.Subastas
                .FirstOrDefaultAsync(
                    s => s.Id == id,
                    cancellationToken);
        }

        public async Task<IList<Auction>> ObtenerVencidasParaActualizarAsync(DateTime fechaActual,CancellationToken cancellationToken)
        {
            return await _context.Subastas
                .Include(s => s.Pujas)
                .Where(s =>
                    s.Estado == AuctionStatus.Activa &&
                    s.FechaFin <= fechaActual)
                .ToListAsync(cancellationToken);
        }

        public async Task<(IList<Auction> Items, int TotalItems)> ObtenerTodasAsync(
            string? estado,
            int? categoriaId,
            decimal? precioMinimo,
            decimal? precioMaximo,
            string? orden,
            int pagina,
            int tamanioPagina,
            string? busqueda,
            CancellationToken cancellationToken)
        {
            var query = _context.Subastas.AsNoTracking().Include(s => s.Categoria).Include(s => s.Pujas).AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
            {
                if (Enum.TryParse<AuctionStatus>(
                    estado,
                    true,
                    out var estadoSubasta))
                {
                    query = query.Where(s => s.Estado == estadoSubasta);
                }
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                query = query.Where(s => s.Titulo.Contains(busqueda));
            }

            if (categoriaId.HasValue)
            {
                query = query.Where(s => s.CategoriaId == categoriaId.Value);
            }

            if (precioMinimo.HasValue)
            {
                query = query.Where(s => s.PrecioBase >= precioMinimo.Value);
            }

            if (precioMaximo.HasValue)
            {
                query = query.Where(s => s.PrecioBase <= precioMaximo.Value);
            }

            if (orden == "tiempo")
            {
                query = query.OrderBy(s => s.FechaFin);
            }
            else if (orden == "puja")
            {
                query = query.OrderByDescending(
                    s => s.Pujas.Any()
                        ? s.Pujas.Max(p => p.Monto)
                        : s.PrecioBase);
            }
            else
            {
                query = query.OrderBy(s => s.Id);
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query.Skip((pagina - 1) * tamanioPagina).Take(tamanioPagina).ToListAsync(cancellationToken);

            return (items, totalItems);
        }

        public async Task<IList<Auction>> ObtenerProgramadasParaProcesarAsync(DateTime ahora, CancellationToken cancellationToken)
        {
            return await _context.Subastas
                .Where(s =>
                    s.Estado == AuctionStatus.Programada &&
                    s.FechaInicio <= ahora)
                .ToListAsync(cancellationToken);
        }
    }
}