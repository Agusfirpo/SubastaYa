using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Subasta.Queries;
using Dominio.Enums;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class ListarSubastasPorVendedorHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ListarSubastasPorVendedorHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IList<PublicacionResponse>> Handle(
            ListarSubastasPorVendedorQuery query)
        {
            var subastas = await _subastaRepository
                .ObtenerPorVendedorIdAsync(query.VendedorId);

            return subastas.Select(s =>
            {
                var tienePujas = s.Pujas.Any();

                var precioActual = tienePujas
                    ? s.Pujas.Max(p => p.Monto)
                    : s.PrecioBase;

                var recaudacion =s.Estado == EstadoSubasta.Finalizada && tienePujas ? precioActual : 0;

                var adjudicacion = s.Estado switch
                {
                    EstadoSubasta.Finalizada when tienePujas
                        => "Adjudicada",

                    EstadoSubasta.Desierta
                        => "Sin adjudicar",

                    EstadoSubasta.Activa
                        => "En curso",

                    EstadoSubasta.Programada
                        => "Pendiente",

                    _ => "Pendiente"
                };

                return new PublicacionResponse
                {
                    Id = s.Id,
                    Titulo = s.Titulo,
                    Categoria = s.Categoria.Nombre,
                    Estado = s.Estado.ToString(),

                    CantidadPujas = s.Pujas.Count,
                    PrecioActual = precioActual,

                    Recaudacion = recaudacion,
                    EstadoAdjudicacion = adjudicacion,

                    FechaFin = s.FechaFin
                };
            }).ToList();
        }
    }
}