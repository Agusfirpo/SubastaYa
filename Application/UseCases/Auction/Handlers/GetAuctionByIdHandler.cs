using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.Mappers;
using Application.UseCases.Subasta.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Subasta.Handler
{
    public class GetAuctionByIdHandler
    {
        private readonly IAuctionRepository _subastaRepository;
        public GetAuctionByIdHandler(IAuctionRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }
        public async Task<AuctionDetailResponse?> Handle(GetAuctionByIdQuery query)
        {
            var subasta = await _subastaRepository.ObtenerPorIdAsync(query.Id);

            if (subasta == null)
                return null;

            var pujaActual = subasta.Pujas.Any() ? subasta.Pujas.Max(p => p.Monto) : subasta.PrecioBase;

            return AuctionMapper.ToDetalleResponse(subasta);
        }
    }
}