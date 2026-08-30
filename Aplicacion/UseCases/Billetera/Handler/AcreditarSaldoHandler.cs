using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Billetera.Command;
using Dominio.Entities;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Billetera.Handler
{
    public class AcreditarSaldoHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public AcreditarSaldoHandler(
            IBilleteraRepository billeteraRepository,
            ITransaccionRepository transaccionRepository,
            IAuditoriaRepository auditoriaRepository,
            IUnidadTrabajo unidadTrabajo)
        {
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
            _auditoriaRepository = auditoriaRepository;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<BilleteraResponse?> Handle(
            AcreditarSaldoCommand command)
        {
            if (command.Monto <= 0)
            {
                throw new ArgumentException(
                    "El monto a acreditar debe ser mayor a cero.");
            }

            BilleteraResponse? resultado = null;

            await _unidadTrabajo.EjecutarEnTransaccionAsync(
                async () =>
                {
                    var billetera =
                        await _billeteraRepository.ObtenerPorUsuarioAsync(command.UsuarioId);

                    if (billetera == null)
                        return;

                    billetera.SaldoTotal += command.Monto;

                    // Como Version es int y concurrency token.
                    billetera.Version++;

                    var transaccion = new TransaccionLedger
                    {
                        BilleteraId = billetera.Id,
                        Tipo = TipoTransaccion.Deposito,
                        Monto = command.Monto,
                        Fecha = DateTime.UtcNow,
                        SubastaId = null
                    };

                    await _transaccionRepository.AgregarAsync(transaccion);

                    var auditoria = new AuditoriaLog
                    {
                        Entidad = "Billetera",
                        EntidadId = billetera.Id,
                        Accion = "ACREDITACION_SALDO",
                        UsuarioId = command.UsuarioId,
                        DetalleJson =
                            $"{{\"monto\":{command.Monto}}}",
                        Fecha = DateTime.UtcNow
                    };

                    await _auditoriaRepository.AgregarAsync(auditoria);

                    resultado = new BilleteraResponse
                    {
                        Id = billetera.Id,
                        UsuarioId = billetera.UsuarioId,
                        SaldoTotal = billetera.SaldoTotal,
                        SaldoRetenido = billetera.SaldoRetenido,
                        SaldoDisponible = billetera.SaldoDisponible
                    };
                });

            return resultado;
        }
    }
}