using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Subasta
    {
        public int Id { get; set; } 

        public int VendedorId { get; set; } 

        public int CategoriaId {  get; set; }

        public string Titulo { get; set; }

        public string Descripcion {  get; set; }

        public string UrlImagen { get; set; }   

        public decimal PrecioBase { get; set; }

        public decimal IncrementoMinimo { get; set; }

        public DateTime FechaInicio { get; set; }    

        public DateTime FechaFin {  get; set; }

        public EstadoSubasta Estado {  get; set; }

        public int Version { get; set; }

        public Usuario Vendedor {  get; set; }

        public Categoria Categoria { get; set; }    

        public  IList<Puja>  Pujas { get; set; }= new List<Puja>();

        public IList<TransaccionLedger> Transacciones { get; set; } = new List<TransaccionLedger>();

    }
}
