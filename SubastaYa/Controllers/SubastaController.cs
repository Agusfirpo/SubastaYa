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
        private readonly ListarSubastasHandler _listar;
        private readonly CrearSubastaHandler _crear;
        private readonly ObtenerSubastaPorIdHandler _obtener;

        public SubastaController(ListarSubastasHandler listar, CrearSubastaHandler crear,ObtenerSubastaPorIdHandler obtener)
        {
            _listar = listar;
            _crear = crear;
            _obtener = obtener;
        }

        [HttpGet]
        public async Task<ActionResult<SubastasPaginadasResponse>> ObtenerTodas(
            [FromQuery] string? estado,
            [FromQuery] string? busqueda,
            [FromQuery] int? categoriaId,
            [FromQuery] decimal? precioMinimo,
            [FromQuery] decimal? precioMaximo,
            [FromQuery] string? orden,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanioPagina = 10)
        {
            var resultado = await _listar.Handle(new ListarSubastasQuery
                {
                    Estado = estado,
                    Busqueda = busqueda,
                    CategoriaId = categoriaId,
                    PrecioMinimo = precioMinimo,
                    PrecioMaximo = precioMaximo,
                    Orden = orden,
                    Pagina = pagina,
                    TamanioPagina = tamanioPagina
                });

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<CrearSubastaResponse>> Crear(CrearSubastaRequest request)
        {
            var resultado = await _crear.Handle(new CrearSubastaCommand
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
                });

            return Created($"/api/v1/subastas/{resultado.Id}",resultado);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleSubastaResponse>> ObtenerPorId(int id)
        {
            var resultado = await _obtener.Handle(new ObtenerSubastaPorIdQuery { Id = id });

            return Ok(resultado);
        }
    }
}