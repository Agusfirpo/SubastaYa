using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace Aplicacion.UseCases.Subasta.Queries
    {
        public class ListarSubastasQuery
        {
            public string? Estado { get; set; }

            public int? CategoriaId { get; set; }

            public decimal? PrecioMinimo { get; set; }

            public decimal? PrecioMaximo { get; set; }

            public string? Orden { get; set; }
        }
    }

