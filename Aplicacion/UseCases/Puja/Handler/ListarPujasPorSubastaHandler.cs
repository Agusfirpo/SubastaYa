using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Puja.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Puja.Handler
{
    public class ListarPujasPorSubastaHandler
    {
        private readonly IPujaRepository _pujaRepository;

        public ListarPujasPorSubastaHandler(IPujaRepository pujaRepository)
        {
            _pujaRepository = pujaRepository;
        }
        public async Task<IList<PujaResponse>> Handle(ListarPujasPorSubastaQuery query)
        {
            var pujas = await _pujaRepository.ObtenerPorSubastaIdAsync(query.SubastaId);

            return pujas.Select(p => new PujaResponse
            {
                Id = p.Id,
                SubastaId = p.SubastaId,
                Monto = p.Monto,
                FechaPuja = p.FechaPuja,
                Usuario = $"Usuario***{p.CompradorId}"
            })
                .ToList();
        }
    }
}
