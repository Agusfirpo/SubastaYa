using Application.DTOs.Response;
using Application.UseCases.Puja.Handler;
using Application.UseCases.Puja.Queries;
using Application.UseCases.Subasta.Handler;
using Application.UseCases.Subasta.Queries;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Request;
using Application.UseCases.Usuario.Command;
using Application.UseCases.Usuario.Handler;

namespace Api_SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsersController : ControllerBase
    {
        private readonly GetAuctionsBySellerHandler _publicaciones;
        private readonly GetParticipationsHandler _participaciones;
        private readonly LoginHandler _login;

        public UsersController(GetAuctionsBySellerHandler publicaciones, GetParticipationsHandler participaciones, LoginHandler login)
        {
            _publicaciones = publicaciones;
            _participaciones = participaciones;
            _login = login;
        }

        [HttpGet("{usuarioId:int}/subastas")]
        public async Task<ActionResult<IList<ListingResponse>>> ObtenerPublicaciones(int usuarioId)
        {
            var resultado = await _publicaciones.Handle(new GetAuctionsBySellerQuery
            {
                VendedorId = usuarioId
            });

            return Ok(resultado);
        }

        [HttpGet("{usuarioId:int}/pujas")]
        public async Task<ActionResult<IList<ParticipationResponse>>> ObtenerParticipaciones(int usuarioId)
        {
            var resultado = await _participaciones.Handle(new GetParticipationsQuery
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