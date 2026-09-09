using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.UseCases.Puja.Command
{
    public class PlaceBidCommand
    {
        public int SubastaId { get; set; }

        public int CompradorId { get; set; }

        public decimal Monto { get; set; }
    }
}