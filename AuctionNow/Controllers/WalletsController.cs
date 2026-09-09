using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.UseCases.Billetera.Command;
using Application.UseCases.Billetera.Handler;
using Application.UseCases.Billetera.Queries;
using Application.UseCases.Transaccion.Handler;
using Application.UseCases.Transaccion.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api_SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios/{usuarioId:int}/billetera")]
    public class WalletsController : ControllerBase
    {
        private readonly GetWalletHandler _obtener;
        private readonly CreditBalanceHandler _acreditar;
        private readonly GetTransactionsHandler _transacciones;

        public WalletsController(GetWalletHandler obtener,CreditBalanceHandler acreditar,GetTransactionsHandler transacciones)
        {
            _obtener = obtener;
            _acreditar = acreditar;
            _transacciones = transacciones;
        }

        [HttpGet]
        public async Task<ActionResult<WalletResponse>> Get(int usuarioId)
        {
            var resultado = await _obtener.Handle(new GetWalletQuery { UsuarioId = usuarioId });
            return Ok(resultado);
        }

        [HttpPost("depositos")]
        public async Task<ActionResult<WalletResponse>> AcreditarSaldo(int usuarioId,CreditBalanceRequest request)
        {
            var resultado = await _acreditar.Handle(new CreditBalanceCommand
                {
                    UsuarioId = usuarioId,
                    Monto = request.Monto
                });

            return Ok(resultado);
        }

        [HttpGet("transacciones")]
        public async Task<ActionResult<IList<TransactionResponse>>> ObtenerTransacciones(int usuarioId)
        {
            var resultado = await _transacciones.Handle(new GetTransactionsQuery
                {
                    UsuarioId = usuarioId
                });

            return Ok(resultado);
        }
    }
}