using Aplicacion.DTOs.Response;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Helpers
{
    public static class BilleteraMapper
    {
        public static BilleteraResponse ToResponse(Billetera billetera) =>
        new()
        {
            Id = billetera.Id,
            UsuarioId = billetera.UsuarioId,
            SaldoTotal = billetera.SaldoTotal,
            SaldoRetenido = billetera.SaldoRetenido,
            SaldoDisponible = billetera.SaldoDisponible
        };
    }
}
