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
    public class ListarSubastasHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ListarSubastasHandler(
            ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IList<SubastaResponse>> Handle(
            ListarSubastasQuery query)
        {
            var subastas =
                await _subastaRepository.ObtenerTodasAsync(
                    query.Estado,
                    query.CategoriaId,
                    query.PrecioMinimo,
                    query.PrecioMaximo,
                    query.Orden);

            return subastas
                .Select(s => new SubastaResponse
                {
                    Id = s.Id,
                    Titulo = s.Titulo,
                    Categoria = s.Categoria.Nombre,
                    UrlImagen = s.UrlImagen,
                    PrecioBase = s.PrecioBase,


                    PujaActual = s.Pujas.Any()
                        ? s.Pujas.Max(p => p.Monto)
                        : s.PrecioBase,

                    CantidadPujas = s.Pujas.Count,

                    FechaInicio = s.FechaInicio,
                    FechaFin = s.FechaFin,
                    Estado = s.Estado.ToString()
                })
                .ToList();
        }
    }
}