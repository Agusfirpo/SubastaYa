using Aplicacion.DTOs.Response;
using Aplicacion.UseCases.Puja.Handler;
using Aplicacion.UseCases.Puja.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/subastas/{subastaId:int}/pujas")]
    public class PujaController : ControllerBase
    {
        private readonly ListarPujasPorSubastaHandler _listarPujasPorSubastaHandler;

        public PujaController(ListarPujasPorSubastaHandler listarPujasPorSubastaHandler)
        {
            _listarPujasPorSubastaHandler = listarPujasPorSubastaHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IList<PujaResponse>>> ObtenerPorSubasta(
            int subastaId)
        {
            var query = new ListarPujasPorSubastaQuery
            {
                SubastaId = subastaId
            };

            var resultado =
                await _listarPujasPorSubastaHandler.Handle(query);

            return Ok(resultado);
        }
    }
}