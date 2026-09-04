using Aplicacion.DTOs.Response;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Mappers
{
    public static class TransaccionMapper
    {
        public static TransaccionResponse ToResponse(TransaccionLedger transaccion) =>
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