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
        private readonly ObtenerSubastaPorIdHandler _obtenerSubastaPorIdHandler;
        public SubastaController(
         ListarSubastasHandler listarSubastasHandler,
         CrearSubastaHandler crearSubastaHandler,
          ObtenerSubastaPorIdHandler obtenerSubastaPorIdHandler)
        {
            _listarSubastasHandler = listarSubastasHandler;
            _crearSubastaHandler = crearSubastaHandler;
            _obtenerSubastaPorIdHandler = obtenerSubastaPorIdHandler;
        }

        [HttpGet]
        public async Task<ActionResult<SubastasPaginadasResponse>>ObtenerTodas(
        [FromQuery] string? estado,
        [FromQuery] int? categoriaId,
        [FromQuery] decimal? precioMinimo,
        [FromQuery] decimal? precioMaximo,
        [FromQuery] string? orden,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanioPagina = 10)
        {
            try
            {
                var query = new ListarSubastasQuery
                {
                    Estado = estado,
                    CategoriaId = categoriaId,
                    PrecioMinimo = precioMinimo,
                    PrecioMaximo = precioMaximo,
                    Orden = orden,
                    Pagina = pagina,
                    TamanioPagina = tamanioPagina
                };

                var resultado = await _listarSubastasHandler.Handle(query);

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CrearSubastaResponse>> Crear(CrearSubastaRequest request)
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

                var resultado = await _crearSubastaHandler.Handle(command);

                return Created($"/api/v1/subastas/{resultado.Id}",resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleSubastaResponse>> ObtenerPorId(int id)
        {
            var query = new ObtenerSubastaPorIdQuery
            {
                Id = id
            };

            var resultado = await _obtenerSubastaPorIdHandler.Handle(query);

            if (resultado == null)
            {
                return NotFound(new
                {
                    mensaje = "La subasta no existe."
                });
            }

            return Ok(resultado);
        }
    }
}