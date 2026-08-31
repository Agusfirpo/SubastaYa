using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Puja.Queries;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Puja.Handler
{
    public class ListarParticipacionesHandler
    {
        private readonly IPujaRepository _pujaRepository;

        public ListarParticipacionesHandler(
            IPujaRepository pujaRepository)
        {
            _pujaRepository = pujaRepository;
        }

        public async Task<IList<ParticipacionResponse>> Handle(ListarParticipacionesQuery query)
        {
            var pujas =await _pujaRepository.ObtenerPorCompradorIdAsync(query.CompradorId);

            var participaciones = pujas.GroupBy(p => p.SubastaId);

            var resultado = new List<ParticipacionResponse>();

            foreach (var grupo in participaciones)
            {
                var unaPuja = grupo.First();

                var subasta = unaPuja.Subasta;

                var pujaActual = subasta.Pujas.Max(p => p.Monto);

                var miUltimaPuja = grupo.Max(p => p.Monto);

                var ganador = subasta.Pujas.OrderByDescending(p => p.Monto).First();

                var esLider = ganador.CompradorId ==query.CompradorId;

                string estadoResultado;

                if (subasta.Estado == EstadoSubasta.Finalizada)
                {
                    estadoResultado =
                        esLider
                            ? "Ganada"
                            : "No ganada";
                }
                else
                {
                    estadoResultado =
                        esLider
                            ? "Liderando"
                            : "Superado";
                }

                resultado.Add(new ParticipacionResponse
                    {
                        SubastaId = subasta.Id,
                        Titulo = subasta.Titulo,
                        EstadoSubasta =
                            subasta.Estado.ToString(),

                        MiUltimaPuja = miUltimaPuja,
                        PujaActual = pujaActual,

                        EsLider = esLider,

                        Resultado = estadoResultado,

                        FechaFin = subasta.FechaFin
                    });
            }

            return resultado;
        }
    }
}