using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? UrlIcono { get; set; }
        public IList<Subasta> Subastas { get; set; } = new List<Subasta>(); 
    }
}
