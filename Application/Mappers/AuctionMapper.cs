using Application.DTOs.Response;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class AuctionMapper
    {
        public static AuctionResponse ToResponse(Auction subasta)
        {
            var pujaActual = subasta.Pujas.Any()
                ? subasta.Pujas.Max(p => p.Monto)
                : subasta.PrecioBase;

            return new AuctionResponse
            {
                Id = subasta.Id,
                Titulo = subasta.Titulo,
                Categoria = subasta.Categoria.Nombre,
                UrlImagen = subasta.UrlImagen,
                PrecioBase = subasta.PrecioBase,
                PujaActual = pujaActual,
                CantidadPujas = subasta.Pujas.Count,
                FechaInicio = subasta.FechaInicio,
                FechaFin = subasta.FechaFin,
                Estado = subasta.Estado.ToString()
            };
        }

        public static AuctionDetailResponse ToDetalleResponse(Auction subasta)
        {
            var pujaActual = subasta.Pujas.Any()
                ? subasta.Pujas.Max(p => p.Monto)
                : subasta.PrecioBase;

            return new AuctionDetailResponse
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

        public static ListingResponse ToPublicacionResponse(Auction subasta)
        {
            var tienePujas = subasta.Pujas.Any();

            var precioActual = tienePujas
                ? subasta.Pujas.Max(p => p.Monto)
                : subasta.PrecioBase;

            return new ListingResponse
            {
                Id = subasta.Id,
                Titulo = subasta.Titulo,
                Categoria = subasta.Categoria.Nombre,
                Estado = subasta.Estado.ToString(),
                CantidadPujas = subasta.Pujas.Count,
                PrecioActual = precioActual,
                Recaudacion =
                    subasta.Estado == AuctionStatus.Finalizada && tienePujas
                        ? precioActual
                        : 0,
                EstadoAdjudicacion = subasta.Estado switch
                {
                    AuctionStatus.Finalizada when tienePujas => "Adjudicada",
                    AuctionStatus.Desierta => "Sin adjudicar",
                    AuctionStatus.Activa => "En curso",
                    _ => "Pendiente"
                },
                FechaFin = subasta.FechaFin
            };
        }
    }
}
