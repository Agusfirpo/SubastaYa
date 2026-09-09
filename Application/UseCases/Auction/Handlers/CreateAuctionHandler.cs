using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Command;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Subasta.Handler
{
    public class CreateAuctionHandler
    {
        private readonly IAuctionRepository _subastaRepository;
        private readonly IUnitOfWork _unidadTrabajo;
        public CreateAuctionHandler(
            IAuctionRepository subastaRepository , IUnitOfWork unidadTrabajo)
        {
            _subastaRepository = subastaRepository;
            _unidadTrabajo = unidadTrabajo;
        }
        public async Task<CreateAuctionResponse> Handle(CrearSubastaCommand command)
        {
            if (command.PrecioBase <= 0)
                throw new ValidationException("El precio base debe ser mayor a cero.");

            if (command.IncrementoMinimo <= 0)
                throw new ValidationException("El incremento mínimo debe ser mayor a cero.");

            if (command.FechaFin <= command.FechaInicio)
                throw new ValidationException("La fecha de finalización debe ser posterior a la fecha de inicio.");


            var estado = command.FechaInicio > DateTime.UtcNow
                ? AuctionStatus.Programada
                : AuctionStatus.Activa;

            var subasta = new Domain.Entities.Auction
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

            await _unidadTrabajo.EjecutarEnTransaccionAsync(async () =>
            {
                await _subastaRepository.AgregarAsync(subasta);
            });


            return new CreateAuctionResponse
            {
                Id = subasta.Id,
                Titulo = subasta.Titulo,
                Estado = subasta.Estado.ToString()
            };
        }


        


    }
}