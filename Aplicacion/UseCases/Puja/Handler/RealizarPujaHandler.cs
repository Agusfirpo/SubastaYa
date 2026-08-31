using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Puja.Command;
using Dominio.Entities;
using Dominio.Enums;

namespace Aplicacion.UseCases.Puja.Handler
{
    public class RealizarPujaHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IPujaRepository _pujaRepository;
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public RealizarPujaHandler(
            ISubastaRepository subastaRepository,
            IPujaRepository pujaRepository,
            IBilleteraRepository billeteraRepository,
            ITransaccionRepository transaccionRepository,
            IAuditoriaRepository auditoriaRepository,
            IUnidadTrabajo unidadTrabajo)
        {
            _subastaRepository = subastaRepository;
            _pujaRepository = pujaRepository;
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
            _auditoriaRepository = auditoriaRepository;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<RealizarPujaResponse> Handle(
            RealizarPujaCommand command)
        {
            RealizarPujaResponse? resultado = null;

            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                var ahora = DateTime.UtcNow;

                // =============================
                // SUBASTA
                // =============================

                var subasta =
                    await _subastaRepository
                        .ObtenerPorIdParaActualizarAsync(
                            command.SubastaId);

                if (subasta == null)
                    throw new ArgumentException(
                        "La subasta no existe.");

                if (subasta.Estado != EstadoSubasta.Activa)
                    throw new InvalidOperationException(
                        "La subasta no está activa.");

                if (ahora < subasta.FechaInicio)
                    throw new InvalidOperationException(
                        "La subasta todavía no comenzó.");

                if (ahora >= subasta.FechaFin)
                    throw new InvalidOperationException(
                        "La subasta ya finalizó.");


                // =============================
                // PUJA ACTUAL
                // =============================

                var pujaAnterior =
                    await _pujaRepository
                        .ObtenerMayorPorSubastaIdAsync(
                            command.SubastaId);

                decimal montoMinimo;

                if (pujaAnterior == null)
                {
                    montoMinimo = subasta.PrecioBase;
                }
                else
                {
                    montoMinimo =
                        pujaAnterior.Monto +
                        subasta.IncrementoMinimo;
                }

                if (command.Monto < montoMinimo)
                {
                    throw new ArgumentException(
                        $"La puja mínima es ${montoMinimo}.");
                }


                // =============================
                // BILLETERA NUEVO POSTOR
                // =============================

                var billeteraNueva =
                    await _billeteraRepository.ObtenerPorUsuarioAsync(
                            command.CompradorId);

                if (billeteraNueva == null)
                    throw new ArgumentException(
                        "El comprador no posee billetera.");


                // ==================================
                // SI EL MISMO LÍDER VUELVE A OFERTAR
                // ==================================

                if (pujaAnterior != null &&
                    pujaAnterior.CompradorId == command.CompradorId)
                {
                    var diferencia =
                        command.Monto - pujaAnterior.Monto;

                    if (billeteraNueva.SaldoDisponible < diferencia)
                    {
                        throw new ArgumentException(
                            "Saldo insuficiente.");
                    }

                    billeteraNueva.SaldoRetenido += diferencia;
                    billeteraNueva.Version++;

                    await _transaccionRepository.AgregarAsync(
                        new TransaccionLedger
                        {
                            BilleteraId = billeteraNueva.Id,
                            Tipo = TipoTransaccion.Retencion,
                            Monto = diferencia,
                            Fecha = ahora,
                            SubastaId = subasta.Id
                        });
                }
                else
                {
                    // =================================
                    // NUEVO LÍDER
                    // =================================

                    if (billeteraNueva.SaldoDisponible < command.Monto)
                    {
                        throw new ArgumentException(
                            "Saldo insuficiente.");
                    }


                    // Liberar líder anterior
                    if (pujaAnterior != null)
                    {
                        var billeteraAnterior =
                            await _billeteraRepository.ObtenerPorUsuarioAsync(
                                    pujaAnterior.CompradorId);

                        if (billeteraAnterior != null)
                        {
                            billeteraAnterior.SaldoRetenido -=
                                pujaAnterior.Monto;

                            billeteraAnterior.Version++;

                            await _transaccionRepository.AgregarAsync(
                                new TransaccionLedger
                                {
                                    BilleteraId =
                                        billeteraAnterior.Id,

                                    Tipo =
                                        TipoTransaccion.Liberacao,

                                    Monto =
                                        pujaAnterior.Monto,

                                    Fecha = ahora,

                                    SubastaId =
                                        subasta.Id
                                });
                        }
                    }


                    // Retener al nuevo líder
                    billeteraNueva.SaldoRetenido += command.Monto;

                    billeteraNueva.Version++;

                    await _transaccionRepository.AgregarAsync(
                        new TransaccionLedger
                        {
                            BilleteraId = billeteraNueva.Id,

                            Tipo = TipoTransaccion.Retencion,

                            Monto = command.Monto,

                            Fecha = ahora,

                            SubastaId = subasta.Id
                        });
                }


                // =============================
                // REGISTRAR PUJA
                // =============================

                await _pujaRepository.AgregarAsync(
                    new Dominio.Entities.Puja
                    {
                        SubastaId = subasta.Id,

                        CompradorId = command.CompradorId,

                        Monto = command.Monto,

                        FechaPuja = ahora
                    });


                // =============================
                // ANTI-SNIPING
                // =============================

                var tiempoExtendido = false;

                var tiempoRestante =
                    subasta.FechaFin - ahora;

                if (tiempoRestante <=
                    TimeSpan.FromSeconds(60))
                {
                    subasta.FechaFin =
                        subasta.FechaFin.AddMinutes(2);

                    tiempoExtendido = true;

                    await _auditoriaRepository.AgregarAsync(
                        new AuditoriaLog
                        {
                            Entidad = "Subasta",

                            EntidadId = subasta.Id,

                            Accion =
                                "EXTENSION_ANTI_SNIPING",

                            UsuarioId =
                                command.CompradorId,

                            DetalleJson =
                                $"{{\"nuevaFechaFin\":\"{subasta.FechaFin:O}\"}}",

                            Fecha = ahora
                        });
                }


                // MUY IMPORTANTE
                // Cada puja modifica la versión de la subasta.
                subasta.Version++;


                resultado = new RealizarPujaResponse
                {
                    SubastaId = subasta.Id,

                    Monto = command.Monto,

                    SaldoDisponible =
                        billeteraNueva.SaldoDisponible,

                    FechaFin = subasta.FechaFin,

                    TiempoExtendido = tiempoExtendido
                };
            });

            return resultado!;
        }
    }
}