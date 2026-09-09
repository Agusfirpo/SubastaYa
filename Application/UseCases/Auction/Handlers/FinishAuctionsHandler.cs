using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Command;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;


namespace Application.UseCases.Subasta.Handler
{
    public class FinishAuctionsHandler
    {
        private readonly IAuctionRepository _subastaRepository;
        private readonly IWalletRepository _billeteraRepository;
        private readonly ITransactionRepository _transaccionRepository;
        private readonly IAuditRepository _auditoriaRepository;
        private readonly IUnitOfWork _unidadTrabajo;

        public FinishAuctionsHandler(
            IAuctionRepository subastaRepository,
            IWalletRepository billeteraRepository,
            ITransactionRepository transaccionRepository,
            IAuditRepository auditoriaRepository,
            IUnitOfWork unidadTrabajo)
        {
            _subastaRepository = subastaRepository;
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
            _auditoriaRepository = auditoriaRepository;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task Handle(
            FinishAuctionsCommand command)
        {
            var subastas = await _subastaRepository.ObtenerVencidasParaActualizarAsync(command.FechaActual);

            foreach (var subasta in subastas)
            {
                await FinalizarSubasta(subasta,command.FechaActual);
            }
        }

        private async Task FinalizarSubasta(Domain.Entities.Auction subasta,DateTime fechaActual)
        {
            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                // SUBASTA SIN PUJAS
                if (!subasta.Pujas.Any())
                {
                    subasta.Estado = AuctionStatus.Desierta;
                    subasta.Version++;

                    await _auditoriaRepository.AgregarAsync(
                        new AuditLog
                        {
                            Entidad = "Subasta",
                            EntidadId = subasta.Id,
                            Accion = "CIERRE_DESIERTA",
                            UsuarioId = null,
                            DetalleJson =
                                "{\"motivo\":\"Subasta finalizada sin pujas\"}",
                            Fecha = fechaActual
                        });

                    return;
                }

                // DETERMINAR GANADOR
                var pujaGanadora = subasta.Pujas
                    .OrderByDescending(p => p.Monto)
                    .First();

                var billeteraComprador =
                    await _billeteraRepository.ObtenerPorUsuarioAsync(
                        pujaGanadora.CompradorId);

                var billeteraVendedor =
                    await _billeteraRepository.ObtenerPorUsuarioAsync(
                        subasta.VendedorId);

                if (billeteraComprador == null ||
                    billeteraVendedor == null)
                {
                    throw new NotFoundException(
                        "No se encontraron las billeteras necesarias para liquidar la subasta.");
                    throw new NotFoundException("No se encontraron las billeteras necesarias para liquidar la subasta.");
                }

                // LIQUIDACIÓN
                billeteraComprador.SaldoRetenido -= pujaGanadora.Monto;
                billeteraComprador.SaldoTotal -= pujaGanadora.Monto;
                billeteraComprador.Version++;

                billeteraVendedor.SaldoTotal += pujaGanadora.Monto;
                billeteraVendedor.Version++;

                // LEDGER COMPRADOR
                await _transaccionRepository.AgregarAsync(
                    new LedgerTransaction
                    {
                        BilleteraId = billeteraComprador.Id,
                        Tipo = TransactionType.Pago,
                        Monto = pujaGanadora.Monto,
                        Fecha = fechaActual,
                        SubastaId = subasta.Id
                    });

                // LEDGER VENDEDOR
                await _transaccionRepository.AgregarAsync(
                    new LedgerTransaction
                    {
                        BilleteraId = billeteraVendedor.Id,
                        Tipo = TransactionType.Cobro,
                        Monto = pujaGanadora.Monto,
                        Fecha = fechaActual,
                        SubastaId = subasta.Id
                    });

                // FINALIZAR SUBASTA
                subasta.Estado = AuctionStatus.Finalizada;
                subasta.Version++;

                // AUDITORÍA
                await _auditoriaRepository.AgregarAsync(
                    new AuditLog
                    {
                        Entidad = "Subasta",
                        EntidadId = subasta.Id,
                        Accion = "CIERRE_CON_GANADOR",
                        UsuarioId = null,
                        DetalleJson =
                            $"{{\"ganadorId\":{pujaGanadora.CompradorId},\"monto\":{pujaGanadora.Monto}}}",
                        Fecha = fechaActual
                    });
            });
        }
    }
}