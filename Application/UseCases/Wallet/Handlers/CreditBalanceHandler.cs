using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.UseCases.Billetera.Command;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.UseCases.Billetera.Handler
{
    public class CreditBalanceHandler
    {
        private readonly IWalletRepository _billeteraRepository;
        private readonly ITransactionRepository _transaccionRepository;
        private readonly IAuditRepository _auditoriaRepository;
        private readonly IUnitOfWork _unidadTrabajo;

        public CreditBalanceHandler(IWalletRepository billeteraRepository,ITransactionRepository transaccionRepository,IAuditRepository auditoriaRepository,IUnitOfWork unidadTrabajo)
        {
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
            _auditoriaRepository = auditoriaRepository;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<WalletResponse> Handle(CreditBalanceCommand command)
        {
            if (command.Monto <= 0)
                throw new ValidationException("El monto a acreditar debe ser mayor a cero.");

            WalletResponse? resultado = null;

            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                var billetera = await _billeteraRepository.ObtenerPorUsuarioAsync(command.UsuarioId);

                if (billetera == null)
                    throw new NotFoundException("No se encontró la billetera del usuario.");

                billetera.SaldoTotal += command.Monto;
                billetera.Version++;

                var ahora = DateTime.UtcNow;

                await _transaccionRepository.AgregarAsync(new LedgerTransaction
                    {
                        BilleteraId = billetera.Id,
                        Tipo = TransactionType.Deposito,
                        Monto = command.Monto,
                        Fecha = ahora,
                        SubastaId = null
                    });

                await _auditoriaRepository.AgregarAsync(new AuditLog
                    {
                        Entidad = "Billetera",
                        EntidadId = billetera.Id,
                        Accion = "ACREDITACION_SALDO",
                        UsuarioId = command.UsuarioId,
                        DetalleJson =
                            $"{{\"monto\":{command.Monto}}}",
                        Fecha = ahora
                    });

                resultado = new WalletResponse
                {
                    Id = billetera.Id,
                    UsuarioId = billetera.UsuarioId,
                    SaldoTotal = billetera.SaldoTotal,
                    SaldoRetenido = billetera.SaldoRetenido,
                    SaldoDisponible = billetera.SaldoDisponible
                };
            });

            return resultado!;
        }
    }
}