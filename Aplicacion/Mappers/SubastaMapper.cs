using Aplicacion.DTOs.Response;
using Dominio.Entities;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Helpers
{
    public static class SubastaMapper
    {
        public static SubastaResponse ToResponse(Subasta subasta)
        {
            var pujaActual = subasta.Pujas.Any()
                ? subasta.Pujas.Max(p => p.Monto)
                : subasta.PrecioBase;

            return new SubastaResponse
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

        public static DetalleSubastaResponse ToDetalleResponse(Subasta subasta)
        {
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

        public static PublicacionResponse ToPublicacionResponse(Subasta subasta)
        {
            var tienePujas = subasta.Pujas.Any();

            var precioActual = tienePujas
                ? subasta.Pujas.Max(p => p.Monto)
                : subasta.PrecioBase;

            return new PublicacionResponse
            {
                Id = subasta.Id,
                Titulo = subasta.Titulo,
                Categoria = subasta.Categoria.Nombre,
                Estado = subasta.Estado.ToString(),
                CantidadPujas = subasta.Pujas.Count,
                PrecioActual = precioActual,
                Recaudacion =
                    subasta.Estado == EstadoSubasta.Finalizada && tienePujas
                        ? precioActual
                        : 0,
                EstadoAdjudicacion = subasta.Estado switch
                {
                    EstadoSubasta.Finalizada when tienePujas => "Adjudicada",
                    EstadoSubasta.Desierta => "Sin adjudicar",
                    EstadoSubasta.Activa => "En curso",
                    _ => "Pendiente"
                },
                FechaFin = subasta.FechaFin
            };
        }
    }
}
