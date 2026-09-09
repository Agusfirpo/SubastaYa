using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Wallet? Billetera { get; set; }
        public IList<Auction> Subastas { get; set; }= new List<Auction>();
        public IList<Bid> Pujas { get; set; }= new List<Bid>();
        public IList <AuditLog> AuditoriaLogs { get; set; }= new List<AuditLog>();


    }
}
