using Aplicacion.Interfaces.Repositories;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class ProcesarSubastasProgramadasHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProcesarSubastasProgramadasHandler(
            ISubastaRepository subastaRepository,
            IUnidadTrabajo unidadTrabajo)
        {
            _subastaRepository = subastaRepository;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task Handle()
        {
            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                var subastas = await _subastaRepository
                    .ObtenerProgramadasParaProcesarAsync(DateTime.UtcNow);

                foreach (var subasta in subastas)
                {
                    subasta.Estado = EstadoSubasta.Activa;
                    subasta.Version++;
                }
            });
        }
    }
}