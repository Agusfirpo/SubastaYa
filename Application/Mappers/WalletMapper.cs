using Application.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class WalletMapper
    {
        public static WalletResponse ToResponse(Wallet billetera) =>
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
