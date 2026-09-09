using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class BidResponse
    {
        public int Id {  get; set; }

        public int SubastaId { get; set; }

        public decimal Monto { get; set; }

        public DateTime FechaPuja { get; set; }

        public string Usuario { get; set; }


    }
}
