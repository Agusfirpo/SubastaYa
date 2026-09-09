using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class LedgerTransaction
    {
        public int Id { get; set; }
        public int BilleteraId {  get; set; }
        public int? SubastaId {  get; set; }
        public TransactionType Tipo {  get; set; }
        public decimal Monto { get; set; }  
        public DateTime Fecha { get; set; } 
        public Wallet Billetera { get; set; } 
        public Auction? Subasta { get; set; }
    }
}
