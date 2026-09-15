using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Mappers;
using Application.UseCases.Subasta.Queries;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Subasta.Handler
{
    public class GetAuctionsBySellerHandler
    {
        private readonly IAuctionRepository _subastaRepository;

        public GetAuctionsBySellerHandler(
            IAuctionRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IList<ListingResponse>> Handle(GetAuctionsBySellerQuery query,CancellationToken cancellationToken)
        {
            var subastas =await _subastaRepository.ObtenerPorVendedorIdAsync(query.VendedorId,cancellationToken);

            return subastas.Select(AuctionMapper.ToPublicacionResponse).ToList();
        }
    }
}