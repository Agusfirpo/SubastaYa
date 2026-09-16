using Application.DTOs.Response;
using Application.Interfaces.Handlers;
using Application.Interfaces.Repositories;
using Application.UseCases.Puja.Command;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.UseCases.Puja.Handler
{
    public class PlaceBidHandler
    {
        private readonly IAuctionRepository _subastaRepository;
        private readonly IBidRepository _pujaRepository;
        private readonly IWalletRepository _billeteraRepository;
        private readonly ITransactionRepository _transaccionRepository;
        private readonly IAuditRepository _auditoriaRepository;
        private readonly IUnitOfWork _unidadTrabajo;
        private readonly IAuctionNotifier _notificadorSubasta;

        public PlaceBidHandler(IAuctionRepository subastaRepository,IBidRepository pujaRepository,IWalletRepository billeteraRepository,ITransactionRepository transaccionRepository,IAuditRepository auditoriaRepository,IUnitOfWork unidadTrabajo,IAuctionNotifier notificadorSubasta)
        {
            _subastaRepository = subastaRepository;
            _pujaRepository = pujaRepository;
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
            _auditoriaRepository = auditoriaRepository;
            _unidadTrabajo = unidadTrabajo;
            _notificadorSubasta = notificadorSubasta;
        }

        public async Task<PlaceBidResponse> Handle(PlaceBidCommand command,CancellationToken cancellationToken)
        {
            PlaceBidResponse? resultado = null;

            await _unidadTrabajo.EjecutarEnTransaccionAsync(
                async () =>
                {
                    var ahora = DateTime.UtcNow;

                    // SUBASTA
                    var subasta = await _subastaRepository.ObtenerPorIdParaActualizarAsync(command.SubastaId,cancellationToken);

                    if (subasta == null)
                        throw new NotFoundException("La subasta no existe.");

                    if (subasta.VendedorId == command.CompradorId)
                    {
                        throw new ValidationException("No podés pujar en una subasta creada por vos.");
                    }

                    if (subasta.Estado != AuctionStatus.Activa)
                    {
                        throw new ValidationException("La subasta no está activa.");
                    }

                    if (ahora < subasta.FechaInicio)
                    {
                        throw new ValidationException("La subasta todavía no comenzó.");
                    }

                    if (ahora >= subasta.FechaFin)
                    {
                        throw new ValidationException("La subasta ya finalizó.");
                    }

                    // PUJA ACTUAL
                    var pujaAnterior =await _pujaRepository.ObtenerMayorPorSubastaIdAsync(command.SubastaId,cancellationToken);

                    // El líder actual no puede volver a ofertar
                    if (pujaAnterior != null &&pujaAnterior.CompradorId == command.CompradorId)
                    {
                        throw new ValidationException("Debés esperar a que otro usuario realice una puja antes de volver a ofertar.");
                    }

                    // MONTO MÍNIMO
                    var montoMinimo = pujaAnterior == null
                            ? subasta.PrecioBase
                            : pujaAnterior.Monto +
                              subasta.IncrementoMinimo;

                    if (command.Monto < montoMinimo)
                    {
                        throw new ValidationException($"La puja mínima es ${montoMinimo:N2}.");
                    }

                    // BILLETERA DEL NUEVO POSTOR
                    var billeteraNueva =await _billeteraRepository.ObtenerPorUsuarioAsync(command.CompradorId,cancellationToken);

                    if (billeteraNueva == null)
                    {
                        throw new NotFoundException("El comprador no posee billetera.");
                    }

                    if (billeteraNueva.SaldoDisponible < command.Monto)
                    {
                        throw new ValidationException("Saldo insuficiente.");
                    }

                    // LIBERAR SALDO DEL LÍDER ANTERIOR
                    if (pujaAnterior != null)
                    {
                        var billeteraAnterior = await _billeteraRepository.ObtenerPorUsuarioAsync(pujaAnterior.CompradorId,cancellationToken);

                        if (billeteraAnterior != null)
                        {
                            billeteraAnterior.SaldoRetenido -= pujaAnterior.Monto;

                            await _transaccionRepository.AgregarAsync(
                                new LedgerTransaction
                                {
                                    BilleteraId = billeteraAnterior.Id,
                                    Tipo = TransactionType.Liberacion,
                                    Monto = pujaAnterior.Monto,
                                    Fecha = ahora,
                                    SubastaId = subasta.Id
                                },
                                cancellationToken);
                        }
                    }

                    // RETENER SALDO DEL NUEVO LÍDER
                    billeteraNueva.SaldoRetenido += command.Monto;

                    await _transaccionRepository.AgregarAsync(
                        new LedgerTransaction
                        {
                            BilleteraId = billeteraNueva.Id,
                            Tipo = TransactionType.Retencion,
                            Monto = command.Monto,
                            Fecha = ahora,
                            SubastaId = subasta.Id
                        },
                        cancellationToken);

                    // REGISTRAR PUJA
                    await _pujaRepository.AgregarAsync(
                        new Bid
                        {
                            SubastaId = subasta.Id,
                            CompradorId = command.CompradorId,
                            Monto = command.Monto,
                            FechaPuja = ahora
                        },
                        cancellationToken);

                    // ANTI-SNIPING
                    var tiempoExtendido = false;
                    var tiempoRestante = subasta.FechaFin - ahora;

                    if (tiempoRestante <=
                        TimeSpan.FromSeconds(60))
                    {
                        subasta.FechaFin = subasta.FechaFin.AddMinutes(2);

                        tiempoExtendido = true;

                        await _auditoriaRepository.AgregarAsync(
                            new AuditLog
                            {
                                Entidad = "Subasta",
                                EntidadId = subasta.Id,
                                Accion = "EXTENSION_ANTI_SNIPING",
                                UsuarioId = command.CompradorId,
                                DetalleJson =
                                    $"{{\"nuevaFechaFin\":\"{subasta.FechaFin:O}\"}}",
                                Fecha = ahora
                            },
                            cancellationToken);
                    }

                    // NO se incrementa Version manualmente.
                    // RowVersion es manejado automáticamente
                    // por SQL Server / Entity Framework Core.
                    subasta.FechaFin = subasta.FechaFin.AddTicks(1);
                    resultado = new PlaceBidResponse
                    {
                        SubastaId = subasta.Id,
                        Monto = command.Monto,
                        SaldoDisponible = billeteraNueva.SaldoDisponible,
                        FechaFin = subasta.FechaFin,
                        TiempoExtendido = tiempoExtendido
                    };
                },
                cancellationToken);

            await _notificadorSubasta.NotificarNuevaPuja(
                resultado!.SubastaId,
                resultado.Monto,
                command.CompradorId,
                resultado.FechaFin,
                resultado.TiempoExtendido,
                cancellationToken);

            return resultado;
        }
    }
}