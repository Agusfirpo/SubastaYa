using Application.DTOs.Response;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.UseCases.Billetera.Queries;
using Domain.Exceptions;

namespace Application.UseCases.Billetera.Handler
{
    public class GetWalletHandler
    {
        private readonly IWalletRepository _billeteraRepository;
        public GetWalletHandler(IWalletRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }

        public async Task<WalletResponse> Handle(GetWalletQuery query)
        {
            var billetera = await _billeteraRepository.ObtenerPorUsuarioAsync(query.UsuarioId);

            if (billetera == null)
                throw new NotFoundException("No se encontró la billetera del usuario.");

            return new WalletResponse
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
