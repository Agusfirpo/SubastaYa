using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Subasta.Queries;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class ListarSubastasPorVendedorHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ListarSubastasPorVendedorHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IList<PublicacionResponse>> Handle(ListarSubastasPorVendedorQuery query)
        {
            var subastas = await _subastaRepository.ObtenerPorVendedorIdAsync(query.VendedorId);

            return subastas
                .Select(s => new PublicacionResponse
                {
                    Id = s.Id,
                    Titulo = s.Titulo,
                    Categoria = s.Categoria.Nombre,
                    Estado = s.Estado.ToString(),
                    CantidadPujas = s.Pujas.Count,
                    PrecioActual = s.Pujas.Any()? s.Pujas.Max(p => p.Monto) : s.PrecioBase,
                    FechaFin = s.FechaFin
                })
                .ToList();
        }
    }
}