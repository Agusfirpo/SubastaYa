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
        private readonly ListarSubastasPorVendedorHandler _publicaciones;
        private readonly ListarParticipacionesHandler _participaciones;
        private readonly LoginHandler _login;

        public UsuarioController(ListarSubastasPorVendedorHandler publicaciones, ListarParticipacionesHandler participaciones, LoginHandler login)
        {
            _publicaciones = publicaciones;
            _participaciones = participaciones;
            _login = login;
        }

        [HttpGet("{usuarioId:int}/subastas")]
        public async Task<ActionResult<IList<PublicacionResponse>>> ObtenerPublicaciones(int usuarioId)
        {
            var resultado = await _publicaciones.Handle(new ListarSubastasPorVendedorQuery
            {
                VendedorId = usuarioId
            });

            return Ok(resultado);
        }

        [HttpGet("{usuarioId:int}/pujas")]
        public async Task<ActionResult<IList<ParticipacionResponse>>> ObtenerParticipaciones(int usuarioId)
        {
            var resultado = await _participaciones.Handle(new ListarParticipacionesQuery
            {
                CompradorId = usuarioId
            });

            return Ok(resultado);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var resultado = await _login.Handle(new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            });

            return resultado == null
                ? Unauthorized(new
                {
                    mensaje = "Email o contraseña incorrectos."
                })
                : Ok(resultado);
        }
    }
}