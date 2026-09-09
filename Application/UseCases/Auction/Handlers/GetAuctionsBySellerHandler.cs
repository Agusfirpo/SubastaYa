using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Queries;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handler
{
    public class GetAuctionsBySellerHandler
    {
        private readonly IAuctionRepository _subastaRepository;

        public GetAuctionsBySellerHandler(IAuctionRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IList<ListingResponse>> Handle(
            GetAuctionsBySellerQuery query)
        {
            var subastas = await _subastaRepository
                .ObtenerPorVendedorIdAsync(query.VendedorId);

            return subastas.Select(s =>
            {
                var tienePujas = s.Pujas.Any();

                var precioActual = tienePujas
                    ? s.Pujas.Max(p => p.Monto)
                    : s.PrecioBase;

                var recaudacion =s.Estado == AuctionStatus.Finalizada && tienePujas ? precioActual : 0;

                var adjudicacion = s.Estado switch
                {
                    AuctionStatus.Finalizada when tienePujas
                        => "Adjudicada",

                    AuctionStatus.Desierta
                        => "Sin adjudicar",

                    AuctionStatus.Activa
                        => "En curso",

                    AuctionStatus.Programada
                        => "Pendiente",

                    _ => "Pendiente"
                };

                return new ListingResponse
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