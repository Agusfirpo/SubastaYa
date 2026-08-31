using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aplicacion.DTOs.Request
{
    public class RealizarPujaRequest
    {
        public int CompradorId { get; set; }

        public decimal Monto { get; set; }
    }
}