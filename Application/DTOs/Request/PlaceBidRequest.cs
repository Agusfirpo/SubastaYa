using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.DTOs.Request
{
    public class PlaceBidRequest
    {
        public int CompradorId { get; set; }

        public decimal Monto { get; set; }
    }
}