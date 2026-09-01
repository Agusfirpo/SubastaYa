using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Billetera.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Billetera.Handler
{
    public class ObtenerBilleteraHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;

        public ObtenerBilleteraHandler(IBilleteraRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }
        public async Task<BilleteraResponse?> Handle(ObtenerBilleteraQuery query)
        {
            var billetera = await _billeteraRepository.ObtenerPorUsuarioAsync(query.UsuarioId);

            if (billetera == null)
                return null;

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
