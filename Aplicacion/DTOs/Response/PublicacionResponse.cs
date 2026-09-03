using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aplicacion.DTOs.Response
{
    public class PublicacionResponse
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Categoria { get; set; } 

        public string Estado { get; set; }

        public int CantidadPujas { get; set; }

        public decimal PrecioActual { get; set; }

        public DateTime FechaFin { get; set; }
        public decimal Recaudacion { get; set; }
        public string EstadoAdjudicacion { get; set; }

    }
}