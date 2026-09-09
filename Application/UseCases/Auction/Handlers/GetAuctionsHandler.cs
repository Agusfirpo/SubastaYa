using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Application.UseCases.Subasta.Handler
{
    public class GetAuctionsHandler
    {
        private readonly IAuctionRepository _subastaRepository;
        public GetAuctionsHandler(
            IAuctionRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }
        public async Task<PagedAuctionsResponse> Handle(GetAuctionsQuery query)
        {
            if (query.Pagina <= 0)
            {
                throw new ValidationException ("La página debe ser mayor a cero.");
            }

            if (query.TamanioPagina <= 0 || query.TamanioPagina > 100)
            {
                throw new ValidationException("El tamaño de página debe estar entre 1 y 100.");
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

            var items = resultado.Items.Select(AuctionMapper.ToResponse).ToList();

            var totalPaginas = (int)Math.Ceiling(resultado.TotalItems / (double)query.TamanioPagina);

            return new PagedAuctionsResponse
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