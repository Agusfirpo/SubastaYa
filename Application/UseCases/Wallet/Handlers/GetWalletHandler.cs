using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Mappers;
using Application.UseCases.Billetera.Queries;
using Domain.Exceptions;

namespace Application.UseCases.Billetera.Handler
{
    public class GetWalletHandler
    {
        private readonly IWalletRepository _billeteraRepository;

        public GetWalletHandler(
            IWalletRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }

        public async Task<WalletResponse> Handle(GetWalletQuery query , CancellationToken cancellationToken)
        {
            var billetera = await _billeteraRepository.ObtenerPorUsuarioAsync(query.UsuarioId , cancellationToken);

            if (billetera == null)
            {
                throw new NotFoundException("No se encontró la billetera del usuario.");
            }

            return WalletMapper.ToResponse(billetera);
        }
    }
}
