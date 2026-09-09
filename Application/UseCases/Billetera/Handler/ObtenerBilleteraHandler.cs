using Aplicacion.DTOs.Response;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Billetera.Queries;
using Dominio.Exceptions;

namespace Aplicacion.UseCases.Billetera.Handler
{
    public class ObtenerBilleteraHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;
        public ObtenerBilleteraHandler(IBilleteraRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }

        public async Task<BilleteraResponse> Handle(ObtenerBilleteraQuery query)
        {
            var billetera = await _billeteraRepository.ObtenerPorUsuarioAsync(query.UsuarioId);

            if (billetera == null)
                throw new RecursoNoEncontradoException("No se encontró la billetera del usuario.");

            return new BilleteraResponse
            {
                Id = billetera.Id,
                UsuarioId = billetera.UsuarioId,
                SaldoTotal = billetera.SaldoTotal,
                SaldoRetenido = billetera.SaldoRetenido,
                SaldoDisponible = billetera.SaldoDisponible
            };
        }
    }
}
