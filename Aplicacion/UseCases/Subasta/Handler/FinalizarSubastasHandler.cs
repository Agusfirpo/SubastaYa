using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Dominio.Enums;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class FinalizarSubastasHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public FinalizarSubastasHandler(
            ISubastaRepository subastaRepository,
            IBilleteraRepository billeteraRepository,
            ITransaccionRepository transaccionRepository,
            IAuditoriaRepository auditoriaRepository,
            IUnidadTrabajo unidadTrabajo)
        {
            _subastaRepository = subastaRepository;
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
            _auditoriaRepository = auditoriaRepository;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task Handle()
        {
            var ahora = DateTime.UtcNow;

            var subastas =
                await _subastaRepository
                    .ObtenerVencidasParaActualizarAsync(ahora);

            foreach (var subasta in subastas)
            {
                await FinalizarSubasta(subasta);
            }
        }

        private async Task FinalizarSubasta(
            Dominio.Entities.Subasta subasta)
        {
            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                var ahora = DateTime.UtcNow;

                // ============================
                // SUBASTA SIN PUJAS
                // ============================

                if (!subasta.Pujas.Any())
                {
                    subasta.Estado = EstadoSubasta.Cancelada;
                    subasta.Version++;

                    await _auditoriaRepository.AgregarAsync(
                        new AuditoriaLog
                        {
                            Entidad = "Subasta",
                            EntidadId = subasta.Id,
                            Accion = "CIERRE_DESIERTA",
                            UsuarioId = null,
                            DetalleJson =
                                "{\"motivo\":\"Subasta finalizada sin pujas\"}",
                            Fecha = ahora
                        });

                    return;
                }


                // ============================
                // DETERMINAR GANADOR
                // ============================

                var pujaGanadora = subasta.Pujas
                    .OrderByDescending(p => p.Monto)
                    .First();

                var billeteraComprador =
                    await _billeteraRepository
                        .ObtenerPorUsuarioAsync(
                            pujaGanadora.CompradorId);

                var billeteraVendedor =
                    await _billeteraRepository
                        .ObtenerPorUsuarioAsync(
                            subasta.VendedorId);

                if (billeteraComprador == null ||
                    billeteraVendedor == null)
                {
                    throw new InvalidOperationException(
                        "No se encontraron las billeteras necesarias para liquidar la subasta.");
                }


                // ============================
                // LIQUIDACIÓN
                // ============================

                billeteraComprador.SaldoRetenido -=
                    pujaGanadora.Monto;

                billeteraComprador.SaldoTotal -=
                    pujaGanadora.Monto;

                billeteraComprador.Version++;


                billeteraVendedor.SaldoTotal +=
                    pujaGanadora.Monto;

                billeteraVendedor.Version++;


                // ============================
                // LEDGER COMPRADOR
                // ============================

                await _transaccionRepository.AgregarAsync(
                    new TransaccionLedger
                    {
                        BilleteraId = billeteraComprador.Id,
                        Tipo = TipoTransaccion.Pago,
                        Monto = pujaGanadora.Monto,
                        Fecha = ahora,
                        SubastaId = subasta.Id
                    });


                // ============================
                // LEDGER VENDEDOR
                // ============================

                await _transaccionRepository.AgregarAsync(
                    new TransaccionLedger
                    {
                        BilleteraId = billeteraVendedor.Id,
                        Tipo = TipoTransaccion.Cobro,
                        Monto = pujaGanadora.Monto,
                        Fecha = ahora,
                        SubastaId = subasta.Id
                    });


                // ============================
                // FINALIZAR
                // ============================

                subasta.Estado = EstadoSubasta.Finalizada;
                subasta.Version++;


                // ============================
                // AUDITORÍA
                // ============================

                await _auditoriaRepository.AgregarAsync(
                    new AuditoriaLog
                    {
                        Entidad = "Subasta",
                        EntidadId = subasta.Id,
                        Accion = "CIERRE_CON_GANADOR",
                        UsuarioId = null,
                        DetalleJson =
                            $"{{\"ganadorId\":{pujaGanadora.CompradorId},\"monto\":{pujaGanadora.Monto}}}",
                        Fecha = ahora
                    });
            });
        }
    }
}