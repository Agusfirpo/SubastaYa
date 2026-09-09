using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Command;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handler
{
    public class ProcessScheduledAuctionsHandler
    {
        private readonly IAuctionRepository _subastaRepository;
        private readonly IUnitOfWork _unidadTrabajo;
        private readonly IAuditRepository _auditoriaRepository;
        public ProcessScheduledAuctionsHandler(IAuctionRepository subastaRepository,IUnitOfWork unidadTrabajo, IAuditRepository auditoriaRepository)
        {
            _subastaRepository = subastaRepository;
            _unidadTrabajo = unidadTrabajo;
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(ProcessScheduledAuctionsCommand command)
        {
            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                var subastas = await _subastaRepository.ObtenerProgramadasParaProcesarAsync(command.FechaActual);

                foreach (var subasta in subastas)
                {
                    subasta.Estado = AuctionStatus.Activa;
                    subasta.Version++;

                    await _auditoriaRepository.AgregarAsync(
                        new AuditLog
                        {
                            Entidad = "Subasta",
                            EntidadId = subasta.Id,
                            Accion = "INICIO_SUBASTA",
                            UsuarioId = null,
                            DetalleJson =
                                "{\"estadoAnterior\":\"Programada\",\"estadoNuevo\":\"Activa\"}",
                            Fecha = command.FechaActual
                        });
                }
            });
        }
    }
}