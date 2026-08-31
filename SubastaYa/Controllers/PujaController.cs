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
        private readonly ListarPujasPorSubastaHandler _listarPujasPorSubastaHandler;
        private readonly RealizarPujaHandler _realizarPujaHandler;

        public PujaController(ListarPujasPorSubastaHandler listarPujasPorSubastaHandler , RealizarPujaHandler realizarPujaHandler)
        {
            _listarPujasPorSubastaHandler = listarPujasPorSubastaHandler;
            _realizarPujaHandler = realizarPujaHandler; 
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


        [HttpPost]
        public async Task<ActionResult<RealizarPujaResponse>> Realizar(int subastaId,RealizarPujaRequest request)
        {
            try
            {
                var command = new RealizarPujaCommand
                {
                    SubastaId = subastaId,
                    CompradorId = request.CompradorId,
                    Monto = request.Monto
                };

                var resultado =
                    await _realizarPujaHandler.Handle(command);

                return Created(
                    $"/api/v1/subastas/{subastaId}/pujas",
                    resultado);
            }
            catch (ConcurrenciaException ex)
            {
                return Conflict(new
                {
                    mensaje = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    mensaje = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }



    }
}