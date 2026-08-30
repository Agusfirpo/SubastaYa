using Aplicacion.DTOs.Response;
using Aplicacion.UseCases.Billetera.Handler;
using Aplicacion.UseCases.Billetera.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios/{usuarioId:int}/billetera")]
    public class BilleteraController : ControllerBase
    {
        private readonly ObtenerBilleteraHandler _obtenerBilleteraHandler;

        public BilleteraController(ObtenerBilleteraHandler obtenerBilleteraHandler)
        {
            _obtenerBilleteraHandler = obtenerBilleteraHandler;
        }

        [HttpGet]

        public async Task<ActionResult<BilleteraResponse>> Get(int usuarioId)
        {
            var query = new ObtenerBilleteraQuery
            {
                UsuarioId = usuarioId
            };

            var resultado = await _obtenerBilleteraHandler.Handle(query);

            if (resultado == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontro la billetera del usuario."
                });
            }

            return Ok(resultado);
        }
    }
}
