using Aplicacion.DTOs.Request;
using Aplicacion.DTOs.Response;
using Aplicacion.Exceptions;
using Aplicacion.UseCases.Puja.Command;
using Aplicacion.UseCases.Puja.Handler;
using Aplicacion.UseCases.Puja.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/subastas/{subastaId:int}/pujas")]
    public class PujaController : ControllerBase
    {
        private readonly ListarPujasPorSubastaHandler _listar;
        private readonly RealizarPujaHandler _realizar;
        public PujaController(ListarPujasPorSubastaHandler listar, RealizarPujaHandler realizar)
        {
            _listar = listar;
            _realizar = realizar;
        }

        [HttpGet]
        public async Task<ActionResult<IList<PujaResponse>>> ObtenerPorSubasta(int subastaId)
        {
            var resultado = await _listar.Handle(new ListarPujasPorSubastaQuery
                {
                    SubastaId = subastaId
                });

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<RealizarPujaResponse>> Realizar(int subastaId,RealizarPujaRequest request)
        {
            var resultado = await _realizar.Handle(new RealizarPujaCommand
                {
                    SubastaId = subastaId,
                    CompradorId = request.CompradorId,
                    Monto = request.Monto
                });

            return Created($"/api/v1/subastas/{subastaId}/pujas",resultado);
        }
    }
}