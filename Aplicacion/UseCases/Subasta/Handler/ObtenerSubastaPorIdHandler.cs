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
    public class ObtenerSubastaPorIdHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubastaPorIdHandler(
            ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<DetalleSubastaResponse?> Handle(
            ObtenerSubastaPorIdQuery query)
        {
            var subasta =
                await _subastaRepository.ObtenerPorIdAsync(query.Id);

            if (subasta == null)
                return null;

            var pujaActual = subasta.Pujas.Any()
                ? subasta.Pujas.Max(p => p.Monto)
                : subasta.PrecioBase;

            return new DetalleSubastaResponse
            {
                Id = subasta.Id,
                Titulo = subasta.Titulo,
                Descripcion = subasta.Descripcion,
                UrlImagen = subasta.UrlImagen,
                Categoria = subasta.Categoria.Nombre,
                Vendedor = subasta.Vendedor.Nombre,
                PrecioBase = subasta.PrecioBase,
                IncrementoMinimo = subasta.IncrementoMinimo,
                PujaActual = pujaActual,
                CantidadPujas = subasta.Pujas.Count,
                FechaInicio = subasta.FechaInicio,
                FechaFin = subasta.FechaFin,
                Estado = subasta.Estado.ToString()
            };
        }
    }
}