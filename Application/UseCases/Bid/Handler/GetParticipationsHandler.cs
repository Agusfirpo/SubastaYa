using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.UseCases.Puja.Queries;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Puja.Handler
{
    public class GetParticipationsHandler
    {
        private readonly IBidRepository _pujaRepository;

        public GetParticipationsHandler(
            IBidRepository pujaRepository)
        {
            _pujaRepository = pujaRepository;
        }
        public async Task<IList<ParticipationResponse>> Handle(GetParticipationsQuery query)
        {
            var pujas =await _pujaRepository.ObtenerPorCompradorIdAsync(query.CompradorId);
            var participaciones = pujas.GroupBy(p => p.SubastaId);
            var resultado = new List<ParticipationResponse>();

            foreach (var grupo in participaciones)
            {
                var unaPuja = grupo.First();
                var subasta = unaPuja.Subasta;
                var pujaActual = subasta.Pujas.Max(p => p.Monto);
                var miUltimaPuja = grupo.Max(p => p.Monto);
                var ganador = subasta.Pujas.OrderByDescending(p => p.Monto).First();
                var esLider = ganador.CompradorId ==query.CompradorId;
                string estadoResultado;

                if (subasta.Estado == AuctionStatus.Finalizada)
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

                resultado.Add(new ParticipationResponse
                    {
                        SubastaId = subasta.Id,
                        Titulo = subasta.Titulo,
                        EstadoSubasta =subasta.Estado.ToString(),
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