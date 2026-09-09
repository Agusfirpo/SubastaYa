using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.Response
{
    public class PujaResponse
    {
        public int Id {  get; set; }

        public int SubastaId { get; set; }

        public decimal Monto { get; set; }

        public DateTime FechaPuja { get; set; }

        public string Usuario { get; set; }


    }
}
