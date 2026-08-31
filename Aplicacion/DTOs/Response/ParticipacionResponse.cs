using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.Response
{
    public class ParticipacionResponse
    {
        public int SubastaId { get; set; }

        public string Titulo { get; set; } 

        public string EstadoSubasta { get; set; } 

        public decimal MiUltimaPuja { get; set; }

        public decimal PujaActual { get; set; }

        public bool EsLider { get; set; }

        public string Resultado { get; set; } 
        public DateTime FechaFin { get; set; }
    }
}