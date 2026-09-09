using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class WalletResponse
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public decimal SaldoTotal { get; set; }

        public decimal SaldoRetenido { get; set; }

        public decimal SaldoDisponible { get; set; }


    }
}
