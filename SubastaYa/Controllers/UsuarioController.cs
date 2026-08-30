using Aplicacion.DTOs.Response;
using Aplicacion.UseCases.Subasta.Handler;
using Aplicacion.UseCases.Subasta.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly ListarSubastasPorVendedorHandler
            _listarSubastasPorVendedorHandler;

        public UsuarioController(
            ListarSubastasPorVendedorHandler
                listarSubastasPorVendedorHandler)
        {
            _listarSubastasPorVendedorHandler =
                listarSubastasPorVendedorHandler;
        }

        [HttpGet("{usuarioId:int}/subastas")]
        public async Task<ActionResult<IList<PublicacionResponse>>>
            ObtenerPublicaciones(int usuarioId)
        {
            var query = new ListarSubastasPorVendedorQuery
            {
                VendedorId = usuarioId
            };

            var resultado =
                await _listarSubastasPorVendedorHandler.Handle(query);

            return Ok(resultado);
        }
    }
}