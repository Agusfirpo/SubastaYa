using Aplicacion.DTOs.Response;
using Aplicacion.UseCases.Puja.Handler;
using Aplicacion.UseCases.Puja.Queries;
using Aplicacion.UseCases.Subasta.Handler;
using Aplicacion.UseCases.Subasta.Queries;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.DTOs.Request;
using Aplicacion.UseCases.Usuario.Command;
using Aplicacion.UseCases.Usuario.Handler;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly ListarSubastasPorVendedorHandler _listarSubastasPorVendedorHandler;
        private readonly ListarParticipacionesHandler _listarParticipacionesHandler;
        private readonly LoginHandler _loginHandler;
        public UsuarioController(ListarSubastasPorVendedorHandler listarSubastasPorVendedorHandler, ListarParticipacionesHandler listarParticipacionesHandler, LoginHandler loginHandler)
        {
            _listarSubastasPorVendedorHandler = listarSubastasPorVendedorHandler;
            _listarParticipacionesHandler = listarParticipacionesHandler;
            _loginHandler = loginHandler;   
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

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var resultado = await _loginHandler.Handle(new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            });

            if (resultado == null)
                return Unauthorized(new
                {
                    mensaje = "Email o contraseña incorrectos."
                });

            return Ok(resultado);
        }



    }
}