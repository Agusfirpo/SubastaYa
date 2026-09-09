using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }      
        public int UsuarioId { get; set; }
        public decimal SaldoTotal { get; set; }
        public decimal SaldoRetenido { get; set; }
        public decimal SaldoDisponible => SaldoTotal - SaldoRetenido; 
        public int Version { get; set; }
        public User Usuario { get; set; } = null;
        public IList<LedgerTransaction> Transacciones { get; set; } = new List<LedgerTransaction>();
    }
}
