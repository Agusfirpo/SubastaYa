using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AppDbContext _context;
        public AuctionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IList<Auction>> ObtenerTodasAsync()
        {
            return await _context.Subastas.Include(s => s.Categoria).ToListAsync();
        }
        public async Task AgregarAsync(Auction subasta)
        {
            await _context.Subastas.AddAsync(subasta);

             
        }
        public async Task<Auction?> ObtenerPorIdAsync(int id)
        {
            return await _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Vendedor)
                .Include(s => s.Pujas)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<IList<Auction>> ObtenerPorVendedorIdAsync(int vendedorId)
        {
            return await _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Pujas)
                .Where(s => s.VendedorId == vendedorId)
                .ToListAsync();
        }
        public async Task<Auction?> ObtenerPorIdParaActualizarAsync(int id)
        {
            return await _context.Subastas.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<IList<Auction>> ObtenerVencidasParaActualizarAsync(DateTime fechaActual)
        {
            return await _context.Subastas
                .Include(s => s.Pujas)
                .Where(s =>
                    s.Estado == AuctionStatus.Activa &&
                    s.FechaFin <= fechaActual)
                .ToListAsync();
        }
        public async Task<(IList<Auction> Items, int TotalItems)>ObtenerTodasAsync(
                string? estado,
                int? categoriaId,
                decimal? precioMinimo,
                decimal? precioMaximo,
                string? orden,
                int pagina,
                int tamanioPagina,
                string? busqueda)
        {
            var query = _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Pujas)
                .AsQueryable();

            // FILTRO POR ESTADO
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
                query = query.Where(s =>
                    s.Titulo.Contains(busqueda));
            }

            // FILTRO POR CATEGORÍA
            if (categoriaId.HasValue)
            {
                query = query.Where(s => s.CategoriaId == categoriaId.Value);
            }

            // PRECIO MÍNIMO
            if (precioMinimo.HasValue)
            {
                query = query.Where(s => s.PrecioBase >= precioMinimo.Value);
            }

            // PRECIO MÁXIMO
            if (precioMaximo.HasValue)
            {
                query = query.Where(s => s.PrecioBase <= precioMaximo.Value);
            }

            // ORDENAMIENTO
            if (orden == "tiempo")
            {
                query = query.OrderBy(s => s.FechaFin);
            }
            else if (orden == "puja")
            {
                query = query.OrderByDescending(s => s.Pujas.Any() ? s.Pujas.Max(p => p.Monto) : s.PrecioBase);
            }
            else
            {
                query = query.OrderBy(s => s.Id);
            }

            // TOTAL ANTES DE PAGINAR
            var totalItems = await query.CountAsync();

            // PAGINACIÓN
            var items = await query.Skip((pagina - 1) * tamanioPagina).Take(tamanioPagina).ToListAsync();

            return (items, totalItems);
        }
        public async Task<IList<Auction>> ObtenerProgramadasParaProcesarAsync(DateTime ahora)
        {
            return await _context.Subastas
                .Where(s =>
                    s.Estado == AuctionStatus.Programada &&
                    s.FechaInicio <= ahora)
                .ToListAsync();
        }
    }
}