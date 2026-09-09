using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Exceptions;
using Application.UseCases.Puja.Command;
using Application.UseCases.Puja.Handler;
using Application.UseCases.Puja.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api_SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/subastas/{subastaId:int}/pujas")]
    public class BidsController : ControllerBase
    {
        private readonly GetBidsByAuctionHandler _listar;
        private readonly PlaceBidHandler _realizar;
        public BidsController(GetBidsByAuctionHandler listar, PlaceBidHandler realizar)
        {
            _listar = listar;
            _realizar = realizar;
        }

        [HttpGet]
        public async Task<ActionResult<IList<BidResponse>>> ObtenerPorSubasta(int subastaId)
        {
            var resultado = await _listar.Handle(new GetBidsByAuctionQuery
                {
                    SubastaId = subastaId
                });

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<PlaceBidResponse>> Realizar(int subastaId,PlaceBidRequest request)
        {
            var resultado = await _realizar.Handle(new PlaceBidCommand
                {
                    SubastaId = subastaId,
                    CompradorId = request.CompradorId,
                    Monto = request.Monto
                });

            return Created($"/api/v1/subastas/{subastaId}/pujas",resultado);
        }
    }
}