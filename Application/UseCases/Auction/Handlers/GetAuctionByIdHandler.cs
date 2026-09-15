using Application.DTOs.Response;
using Application.Mappers;
using Application.Interfaces.Repositories;
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
        public async Task<AuctionDetailResponse?> Handle(GetAuctionByIdQuery query, CancellationToken cancellationToken)
        {
            var subasta = await _subastaRepository.ObtenerPorIdAsync(query.Id,cancellationToken);

            if (subasta == null)
                return null;

            return AuctionMapper.ToDetalleResponse(subasta);
        }
    }
}