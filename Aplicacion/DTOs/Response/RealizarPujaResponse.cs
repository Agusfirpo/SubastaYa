using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aplicacion.DTOs.Response
{
    public class RealizarPujaResponse
    {
        public int SubastaId { get; set; }

        public decimal Monto { get; set; }

        public decimal SaldoDisponible { get; set; }

        public DateTime FechaFin { get; set; }

        public bool TiempoExtendido { get; set; }
    }
}