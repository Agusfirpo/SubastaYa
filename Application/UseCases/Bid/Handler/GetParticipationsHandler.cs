using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Mappers;
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

        public GetParticipationsHandler(IBidRepository pujaRepository)
        {
            _pujaRepository = pujaRepository;
        }

        public async Task<IList<ParticipationResponse>> Handle(GetParticipationsQuery query,CancellationToken cancellationToken)
        {
            var pujas =await _pujaRepository.ObtenerPorCompradorIdAsync(query.CompradorId,cancellationToken);

            return pujas.GroupBy(p => p.SubastaId)
                .Select(grupo =>
                {
                    var ultimaPujaUsuario = grupo.OrderByDescending(p => p.FechaPuja).First();

                    return BidMapper.ToParticipacionResponse(ultimaPujaUsuario);
                })
                .ToList();
        }
    }
}