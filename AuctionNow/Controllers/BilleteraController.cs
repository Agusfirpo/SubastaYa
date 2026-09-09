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
        private readonly ObtenerBilleteraHandler _obtener;
        private readonly AcreditarSaldoHandler _acreditar;
        private readonly ListarTransaccionesHandler _transacciones;

        public BilleteraController(ObtenerBilleteraHandler obtener,AcreditarSaldoHandler acreditar,ListarTransaccionesHandler transacciones)
        {
            _obtener = obtener;
            _acreditar = acreditar;
            _transacciones = transacciones;
        }

        [HttpGet]
        public async Task<ActionResult<BilleteraResponse>> Get(int usuarioId)
        {
            var resultado = await _obtener.Handle(new ObtenerBilleteraQuery { UsuarioId = usuarioId });
            return Ok(resultado);
        }

        [HttpPost("depositos")]
        public async Task<ActionResult<BilleteraResponse>> AcreditarSaldo(int usuarioId,AcreditarSaldoRequest request)
        {
            var resultado = await _acreditar.Handle(new AcreditarSaldoCommand
                {
                    UsuarioId = usuarioId,
                    Monto = request.Monto
                });

            return Ok(resultado);
        }

        [HttpGet("transacciones")]
        public async Task<ActionResult<IList<TransaccionResponse>>> ObtenerTransacciones(int usuarioId)
        {
            var resultado = await _transacciones.Handle(new ListarTransaccionesQuery
                {
                    UsuarioId = usuarioId
                });

            return Ok(resultado);
        }
    }
}