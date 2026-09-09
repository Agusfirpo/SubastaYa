using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.UseCases.Subasta.Command;
using Application.UseCases.Subasta.Handler;
using Application.UseCases.Subasta.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api_SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/subastas")]
    public class AuctionsController : ControllerBase
    {
        private readonly GetAuctionsHandler _listar;
        private readonly CreateAuctionHandler _crear;
        private readonly GetAuctionByIdHandler _obtener;

        public AuctionsController(GetAuctionsHandler listar, CreateAuctionHandler crear,GetAuctionByIdHandler obtener)
        {
            _listar = listar;
            _crear = crear;
            _obtener = obtener;
        }

        [HttpGet]
        public async Task<ActionResult<PagedAuctionsResponse>> ObtenerTodas(
            [FromQuery] string? estado,
            [FromQuery] string? busqueda,
            [FromQuery] int? categoriaId,
            [FromQuery] decimal? precioMinimo,
            [FromQuery] decimal? precioMaximo,
            [FromQuery] string? orden,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanioPagina = 10)
        {
            var resultado = await _listar.Handle(new GetAuctionsQuery
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
        public async Task<ActionResult<CreateAuctionResponse>> Crear(CreateAuctionRequest request)
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
        public async Task<ActionResult<AuctionDetailResponse>> ObtenerPorId(int id)
        {
            var resultado = await _obtener.Handle(new GetAuctionByIdQuery { Id = id });

            return Ok(resultado);
        }
    }
}