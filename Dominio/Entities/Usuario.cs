using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Nombre { get; set; }

        public string PasswordHash { get; set; }

        public DateTime FechaRegistro { get; set; }

        public Billetera? Billetera { get; set; }

        public IList<Subasta> Subastas { get; set; }= new List<Subasta>();
        public IList<Puja> Pujas { get; set; }= new List<Puja>();

        public IList <AuditoriaLog> AuditoriaLogs { get; set; }= new List<AuditoriaLog>();


    }
}
