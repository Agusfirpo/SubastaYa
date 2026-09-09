using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Bid
    { 
        public int Id { get; set; }
        public int SubastaId { get; set; }
        public int CompradorId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPuja { get; set; }
        public Auction Subasta { get; set; } = null!;
        public User Comprador { get; set; } = null!;
    }
}
