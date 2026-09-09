using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces.Repositories;
using Application.UseCases.Puja.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Puja.Handler
{
    public class GetBidsByAuctionHandler
    {
        private readonly IBidRepository _pujaRepository;

        public GetBidsByAuctionHandler(IBidRepository pujaRepository)
        {
            _pujaRepository = pujaRepository;
        }
        public async Task<IList<BidResponse>> Handle(GetBidsByAuctionQuery query)
        {
            var pujas = await _pujaRepository.ObtenerPorSubastaIdAsync(query.SubastaId);

            return pujas.Select(BidMapper.ToResponse).ToList();
        }
    }
}
