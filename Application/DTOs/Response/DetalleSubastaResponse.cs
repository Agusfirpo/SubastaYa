using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aplicacion.DTOs.Response
{
    public class DetalleSubastaResponse
    {
        public int Id { get; set; }

        public string Titulo { get; set; } 

        public string Descripcion { get; set; } 

        public string UrlImagen { get; set; } 

        public string Categoria { get; set; } 

        public string Vendedor { get; set; } 

        public decimal PrecioBase { get; set; }

        public decimal IncrementoMinimo { get; set; }

        public decimal PujaActual { get; set; }

        public int CantidadPujas { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Estado { get; set; }
    }
}