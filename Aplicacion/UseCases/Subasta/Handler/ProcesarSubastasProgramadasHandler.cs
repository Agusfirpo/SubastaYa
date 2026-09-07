using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Subasta.Command;
using Dominio.Enums;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class ProcesarSubastasProgramadasHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProcesarSubastasProgramadasHandler(ISubastaRepository subastaRepository,IUnidadTrabajo unidadTrabajo)
        {
            _subastaRepository = subastaRepository;
            _unidadTrabajo = unidadTrabajo;
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
                }
            });
        }
    }
}