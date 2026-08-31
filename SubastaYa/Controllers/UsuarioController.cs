using Aplicacion.DTOs.Response;
using Aplicacion.UseCases.Puja.Handler;
using Aplicacion.UseCases.Puja.Queries;
using Aplicacion.UseCases.Subasta.Handler;
using Aplicacion.UseCases.Subasta.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly ListarSubastasPorVendedorHandler _listarSubastasPorVendedorHandler;
        private readonly ListarParticipacionesHandler _listarParticipacionesHandler;
        public UsuarioController(ListarSubastasPorVendedorHandler listarSubastasPorVendedorHandler, ListarParticipacionesHandler listarParticipacionesHandler)
        {
            _listarSubastasPorVendedorHandler = listarSubastasPorVendedorHandler;
            _listarParticipacionesHandler = listarParticipacionesHandler;
        }

        [HttpGet("{usuarioId:int}/subastas")]
        public async Task<ActionResult<IList<PublicacionResponse>>>ObtenerPublicaciones(int usuarioId)
        {
            var query = new ListarSubastasPorVendedorQuery
            {
                VendedorId = usuarioId
            };

            var resultado = await _listarSubastasPorVendedorHandler.Handle(query);

            return Ok(resultado);
        }
        [HttpGet("{usuarioId:int}/pujas")]
        public async Task<ActionResult<IList<ParticipacionResponse>>>ObtenerParticipaciones(int usuarioId)
        {
            var query = new ListarParticipacionesQuery
            {
                CompradorId = usuarioId
            };

            var resultado = await _listarParticipacionesHandler.Handle(query);

            return Ok(resultado);
        }
    }
}