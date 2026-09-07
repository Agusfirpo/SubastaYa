using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Subasta.Command;
using Dominio.Entities;
using Dominio.Enums;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class ProcesarSubastasProgramadasHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IAuditoriaRepository _auditoriaRepository;
        public ProcesarSubastasProgramadasHandler(ISubastaRepository subastaRepository,IUnidadTrabajo unidadTrabajo, IAuditoriaRepository auditoriaRepository)
        {
            _subastaRepository = subastaRepository;
            _unidadTrabajo = unidadTrabajo;
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task Handle(ProcesarSubastasProgramadasCommand command)
        {
            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                var subastas = await _subastaRepository.ObtenerProgramadasParaProcesarAsync(command.FechaActual);

                foreach (var subasta in subastas)
                {
                    subasta.Estado = EstadoSubasta.Activa;
                    subasta.Version++;

                    await _auditoriaRepository.AgregarAsync(
                        new AuditoriaLog
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