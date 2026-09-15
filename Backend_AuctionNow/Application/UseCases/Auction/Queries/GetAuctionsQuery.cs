using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace Application.UseCases.Subasta.Queries
    {
        public class GetAuctionsQuery
        {
            public string? Estado { get; set; }
            public int? CategoriaId { get; set; }
            public decimal? PrecioMinimo { get; set; }
            public decimal? PrecioMaximo { get; set; }
            public string? Orden { get; set; }
            public int Pagina { get; set; } = 1;
            public int TamanioPagina { get; set; } = 10;
            public string? Busqueda { get; set; }

        }
    }

