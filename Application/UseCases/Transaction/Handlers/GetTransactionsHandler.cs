using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Mappers;
using Application.UseCases.Transaccion.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Transaccion.Handler
{
    public class GetTransactionsHandler
    {
        private readonly IWalletRepository _billeteraRepository;
        private readonly ITransactionRepository _transaccionRepository;
        public GetTransactionsHandler(IWalletRepository billeteraRepository,ITransactionRepository transaccionRepository)
        {
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
        }

        public async Task<IList<TransactionResponse>?> Handle(GetTransactionsQuery query)
        {
            var billetera = await _billeteraRepository.ObtenerPorUsuarioAsync(query.UsuarioId);

            if (billetera == null)
                return null;

            var transacciones = await _transaccionRepository.ObtenerPorBilleteraIdAsync(billetera.Id);

            return transacciones.Select(TransactionMapper.ToResponse).ToList();
        }
    }
}