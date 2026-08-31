using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.Response
{
    public class SubastasPaginadasResponse
    {
        public IList<SubastaResponse> Items { get; set; }
            = new List<SubastaResponse>();

        public int Pagina { get; set; }

        public int TamanioPagina { get; set; }

        public int TotalItems { get; set; }

        public int TotalPaginas { get; set; }
    }
}
