using Aplicacion.DTOs.Request;
using Aplicacion.DTOs.Response;
using Aplicacion.UseCases.Subasta.Command;
using Aplicacion.UseCases.Subasta.Handler;
using Aplicacion.UseCases.Subasta.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/subastas")]
    public class SubastaController : ControllerBase
    {
        private readonly ListarSubastasHandler _listarSubastasHandler;
        private readonly CrearSubastaHandler _crearSubastaHandler;

        public SubastaController(
            ListarSubastasHandler listarSubastasHandler,
            CrearSubastaHandler crearSubastaHandler)
        {
            _listarSubastasHandler = listarSubastasHandler;
            _crearSubastaHandler = crearSubastaHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IList<SubastaResponse>>> ObtenerTodas()
        {
            var query = new ListarSubastasQuery();

            var resultado =
                await _listarSubastasHandler.Handle(query);

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<CrearSubastaResponse>> Crear(
            CrearSubastaRequest request)
        {
            try
            {
                var command = new CrearSubastaCommand
                {
                    VendedorId = request.VendedorId,
                    CategoriaId = request.CategoriaId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    UrlImagen = request.UrlImagen,
                    PrecioBase = request.PrecioBase,
                    IncrementoMinimo = request.IncrementoMinimo,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin
                };

                var resultado =
                    await _crearSubastaHandler.Handle(command);

                return Created(
                    $"/api/v1/subastas/{resultado.Id}",
                    resultado);
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