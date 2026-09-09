using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.Mappers;
using Aplicacion.UseCases.Transaccion.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Transaccion.Handler
{
    public class ListarTransaccionesHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionRepository _transaccionRepository;
        public ListarTransaccionesHandler(IBilleteraRepository billeteraRepository,ITransaccionRepository transaccionRepository)
        {
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
        }

        public async Task<IList<TransaccionResponse>?> Handle(ListarTransaccionesQuery query)
        {
            var billetera = await _billeteraRepository.ObtenerPorUsuarioAsync(query.UsuarioId);

            if (billetera == null)
                return null;

            var transacciones = await _transaccionRepository.ObtenerPorBilleteraIdAsync(billetera.Id);

            return transacciones.Select(TransaccionMapper.ToResponse).ToList();
        }
    }
}