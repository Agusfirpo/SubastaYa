using Application.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class BidMapper
    {
        public static BidResponse ToResponse(Bid puja) =>
        new()
        {
            Id = puja.Id,
            SubastaId = puja.SubastaId,
            Monto = puja.Monto,
            FechaPuja = puja.FechaPuja,
            Usuario = $"Usuario***{puja.CompradorId}"
        };

        public static ParticipationResponse ToParticipacionResponse(
            Bid ultimaPujaUsuario)
        {
            var subasta = ultimaPujaUsuario.Subasta;

            var pujaMayor = subasta.Pujas
                .OrderByDescending(p => p.Monto)
                .FirstOrDefault();

            var esLider = pujaMayor?.CompradorId ==
                          ultimaPujaUsuario.CompradorId;

            var resultado = subasta.Estado.ToString() switch
            {
                "Finalizada" when esLider => "Ganada",
                "Finalizada" => "No ganada",
                "Desierta" => "Finalizada",
                _ when esLider => "Liderando",
                _ => "Superado"
            };

            return new ParticipationResponse
            {
                SubastaId = subasta.Id,
                Titulo = subasta.Titulo,
                EstadoSubasta = subasta.Estado.ToString(),
                MiUltimaPuja = ultimaPujaUsuario.Monto,
                PujaActual = pujaMayor?.Monto ?? subasta.PrecioBase,
                EsLider = esLider,
                Resultado = resultado,
                FechaFin = subasta.FechaFin
            };
        }
    }
}