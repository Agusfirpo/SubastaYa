using Aplicacion.DTOs.Response;
using Aplicacion.Helpers;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.Mappers;
using Aplicacion.UseCases.Subasta.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class ObtenerSubastaPorIdHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        public ObtenerSubastaPorIdHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }
        public async Task<DetalleSubastaResponse?> Handle(ObtenerSubastaPorIdQuery query)
        {
            var subasta = await _subastaRepository.ObtenerPorIdAsync(query.Id);

            if (subasta == null)
                return null;

            var pujaActual = subasta.Pujas.Any() ? subasta.Pujas.Max(p => p.Monto) : subasta.PrecioBase;

            return SubastaMapper.ToDetalleResponse(subasta);
        }
    }
}