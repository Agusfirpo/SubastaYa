using Aplicacion.DTOs.Response;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Billetera.Command;
using Dominio.Entities;
using Dominio.Enums;
using Dominio.Exceptions;

namespace Aplicacion.UseCases.Billetera.Handler
{
    public class AcreditarSaldoHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public AcreditarSaldoHandler(IBilleteraRepository billeteraRepository,ITransaccionRepository transaccionRepository,IAuditoriaRepository auditoriaRepository,IUnidadTrabajo unidadTrabajo)
        {
            _billeteraRepository = billeteraRepository;
            _transaccionRepository = transaccionRepository;
            _auditoriaRepository = auditoriaRepository;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<BilleteraResponse> Handle(AcreditarSaldoCommand command)
        {
            if (command.Monto <= 0)
                throw new DomainException("El monto a acreditar debe ser mayor a cero.");

            BilleteraResponse? resultado = null;

            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                var billetera = await _billeteraRepository.ObtenerPorUsuarioAsync(command.UsuarioId);

                if (billetera == null)
                    throw new RecursoNoEncontradoException("No se encontró la billetera del usuario.");

                billetera.SaldoTotal += command.Monto;
                billetera.Version++;

                var ahora = DateTime.UtcNow;

                await _transaccionRepository.AgregarAsync(new TransaccionLedger
                    {
                        BilleteraId = billetera.Id,
                        Tipo = TipoTransaccion.Deposito,
                        Monto = command.Monto,
                        Fecha = ahora,
                        SubastaId = null
                    });

                await _auditoriaRepository.AgregarAsync(new AuditoriaLog
                    {
                        Entidad = "Billetera",
                        EntidadId = billetera.Id,
                        Accion = "ACREDITACION_SALDO",
                        UsuarioId = command.UsuarioId,
                        DetalleJson =
                            $"{{\"monto\":{command.Monto}}}",
                        Fecha = ahora
                    });

                resultado = new BilleteraResponse
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