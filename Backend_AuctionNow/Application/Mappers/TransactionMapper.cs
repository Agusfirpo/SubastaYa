using Application.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionResponse ToResponse(LedgerTransaction transaccion) =>
        new()
        {
            Id = transaccion.Id,
            Tipo = transaccion.Tipo.ToString(),
            Monto = transaccion.Monto,
            Fecha = transaccion.Fecha,
            SubastaId = transaccion.SubastaId
        };
    }
}