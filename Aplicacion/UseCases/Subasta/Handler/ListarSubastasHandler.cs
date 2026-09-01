using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Subasta.Queries;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class ListarSubastasHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        public ListarSubastasHandler(
            ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }
        public async Task<SubastasPaginadasResponse> Handle(ListarSubastasQuery query)
        {
            if (query.Pagina <= 0)
            {
                throw new ArgumentException("La página debe ser mayor a cero.");
            }

            if (query.TamanioPagina <= 0 || query.TamanioPagina > 100)
            {
                throw new ArgumentException("El tamaño de página debe estar entre 1 y 100.");
            }

            var resultado =
                await _subastaRepository.ObtenerTodasAsync(
                    query.Estado,
                    query.CategoriaId,
                    query.PrecioMinimo,
                    query.PrecioMaximo,
                    query.Orden,
                    query.Pagina,
                    query.TamanioPagina);

            var items = resultado.Items
                .Select(s => new SubastaResponse
                {
                    Id = s.Id,
                    Titulo = s.Titulo,
                    Categoria = s.Categoria.Nombre,
                    UrlImagen = s.UrlImagen,
                    PrecioBase = s.PrecioBase,
                    PujaActual = s.Pujas.Any()? s.Pujas.Max(p => p.Monto): s.PrecioBase,
                    CantidadPujas = s.Pujas.Count,
                    FechaInicio = s.FechaInicio,
                    FechaFin = s.FechaFin,
                    Estado = s.Estado.ToString()
                })
                .ToList();

            var totalPaginas = (int)Math.Ceiling(resultado.TotalItems / (double)query.TamanioPagina);

            return new SubastasPaginadasResponse
            {
                Items = items,
                Pagina = query.Pagina,
                TamanioPagina = query.TamanioPagina,
                TotalItems = resultado.TotalItems,
                TotalPaginas = totalPaginas
            };
        }
    }
}