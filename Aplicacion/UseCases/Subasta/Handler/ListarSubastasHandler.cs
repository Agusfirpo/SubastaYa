using Aplicacion.DTOs.Response;
using Aplicacion.Helpers;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Subasta.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    query.TamanioPagina,
                    query.Busqueda
                    );

            var items = resultado.Items.Select(SubastaMapper.ToResponse).ToList();

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