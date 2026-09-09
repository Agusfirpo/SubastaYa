using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Auction
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
        public AuctionStatus Estado {  get; set; }
        public int Version { get; set; }
        public User Vendedor {  get; set; }
        public Category Categoria { get; set; }    
        public  IList<Bid>  Pujas { get; set; }= new List<Bid>();
        public IList<LedgerTransaction> Transacciones { get; set; } = new List<LedgerTransaction>();

    }
}
