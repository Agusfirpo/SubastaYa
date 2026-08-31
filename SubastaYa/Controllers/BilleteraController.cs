using Aplicacion.DTOs.Request;
using Aplicacion.DTOs.Response;
using Aplicacion.UseCases.Billetera.Command;
using Aplicacion.UseCases.Billetera.Handler;
using Aplicacion.UseCases.Billetera.Queries;
using Aplicacion.UseCases.Transaccion.Handler;
using Aplicacion.UseCases.Transaccion.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios/{usuarioId:int}/billetera")]
    public class BilleteraController : ControllerBase
    {
        private readonly ObtenerBilleteraHandler _obtenerBilleteraHandler;
        private readonly AcreditarSaldoHandler _acreditarSaldoHandler;
        private readonly ListarTransaccionesHandler _listarTransaccionesHandler;
        public BilleteraController(ObtenerBilleteraHandler obtenerBilleteraHandler, AcreditarSaldoHandler acreditarSaldoHandler,ListarTransaccionesHandler listarTransaccionesHandler)
        {
            _obtenerBilleteraHandler = obtenerBilleteraHandler;
            _acreditarSaldoHandler = acreditarSaldoHandler;
            _listarTransaccionesHandler = listarTransaccionesHandler;
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

        [HttpPost("depositos")]
        public async Task<ActionResult<BilleteraResponse>> AcreditarSaldo(int usuarioId, AcreditarSaldoRequest request)
        {
            try
            {
                var command = new AcreditarSaldoCommand
                {
                    UsuarioId = usuarioId,
                    Monto = request.Monto
                };

                var resultado = await _acreditarSaldoHandler.Handle(command);

                if (resultado == null)
                {
                    return NotFound(new
                    {
                        mensaje = "No se encontró la billetera del usuario."
                    });
                }

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

        [HttpGet("transacciones")]
        public async Task<ActionResult<IList<TransaccionResponse>>> ObtenerTransacciones(int usuarioId)
        {
            var query = new ListarTransaccionesQuery
            {
                UsuarioId = usuarioId
            };

            var resultado = await _listarTransaccionesHandler.Handle(query);

            if (resultado == null)
            {
                return NotFound(new
                {
                    mensaje =
                        "No se encontró la billetera del usuario."
                });
            }

            return Ok(resultado);
        }
    }
}