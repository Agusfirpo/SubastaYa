using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Dominio.Enums;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    public class SubastaRepository : ISubastaRepository
    {
        private readonly AppDbContext _context;

        public SubastaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Subasta>> ObtenerTodasAsync()
        {
            return await _context.Subastas
                .Include(s => s.Categoria)
                .ToListAsync();
        }

        public async Task AgregarAsync(Subasta subasta)
        {
            await _context.Subastas.AddAsync(subasta);

            await _context.SaveChangesAsync();
        }

        public async Task<Subasta?> ObtenerPorIdAsync(int id)
        {
            return await _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Vendedor)
                .Include(s => s.Pujas)
                .FirstOrDefaultAsync(s => s.Id == id);
        }



        public async Task<IList<Subasta>> ObtenerPorVendedorIdAsync(int vendedorId)
        {
            return await _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Pujas)
                .Where(s => s.VendedorId == vendedorId)
                .ToListAsync();
        }


        public async Task<Subasta?> ObtenerPorIdParaActualizarAsync(int id)
        {
            return await _context.Subastas
                .FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task<IList<Subasta>> ObtenerVencidasParaActualizarAsync(DateTime fechaActual)
        {
            return await _context.Subastas
                .Include(s => s.Pujas)
                .Where(s =>
                    s.Estado == EstadoSubasta.Activa &&
                    s.FechaFin <= fechaActual)
                .ToListAsync();
        }

        public async Task<IList<Subasta>> ObtenerTodasAsync(string? estado,int? categoriaId,decimal? precioMinimo,decimal? precioMaximo,string? orden)
        {
            var query = _context.Subastas
                .AsNoTracking()
                .Include(s => s.Categoria)
                .Include(s => s.Pujas)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
            {
                if (Enum.TryParse<EstadoSubasta>(
                    estado,
                    true,
                    out var estadoSubasta))
                {
                    query = query.Where(
                        s => s.Estado == estadoSubasta);
                }
            }

            if (categoriaId.HasValue)
            {
                query = query.Where(
                    s => s.CategoriaId == categoriaId.Value);
            }

            if (precioMinimo.HasValue)
            {
                query = query.Where(
                    s => s.PrecioBase >= precioMinimo.Value);
            }

            if (precioMaximo.HasValue)
            {
                query = query.Where(
                    s => s.PrecioBase <= precioMaximo.Value);
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

            return await query.ToListAsync();
        }




    }
}