using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Subasta.Command;
using Dominio.Enums;

namespace Aplicacion.UseCases.Subasta.Handler
{
    public class CrearSubastaHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public CrearSubastaHandler(
            ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<CrearSubastaResponse> Handle(
            CrearSubastaCommand command)
        {
            if (command.PrecioBase <= 0)
            {
                throw new ArgumentException(
                    "El precio base debe ser mayor a cero.");
            }

            if (command.IncrementoMinimo <= 0)
            {
                throw new ArgumentException(
                    "El incremento mínimo debe ser mayor a cero.");
            }

            if (command.FechaFin <= command.FechaInicio)
            {
                throw new ArgumentException(
                    "La fecha de finalización debe ser posterior a la fecha de inicio.");
            }

            var estado = command.FechaInicio > DateTime.UtcNow
                ? EstadoSubasta.Programada
                : EstadoSubasta.Activa;

            var subasta = new Dominio.Entities.Subasta
            {
                VendedorId = command.VendedorId,
                CategoriaId = command.CategoriaId,
                Titulo = command.Titulo,
                Descripcion = command.Descripcion,
                UrlImagen = command.UrlImagen,
                PrecioBase = command.PrecioBase,
                IncrementoMinimo = command.IncrementoMinimo,
                FechaInicio = command.FechaInicio,
                FechaFin = command.FechaFin,
                Estado = estado,
                Version = 0
            };

            await _subastaRepository.AgregarAsync(subasta);

            return new CrearSubastaResponse
            {
                Id = subasta.Id,
                Titulo = subasta.Titulo,
                Estado = subasta.Estado.ToString()
            };
        }
    }
}