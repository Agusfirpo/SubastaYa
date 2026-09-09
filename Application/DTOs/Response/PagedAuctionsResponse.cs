using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class PagedAuctionsResponse
    {
        public IList<AuctionResponse> Items { get; set; } = new List<AuctionResponse>();

        public int Pagina { get; set; }

        public int TamanioPagina { get; set; }

        public int TotalItems { get; set; }

        public int TotalPaginas { get; set; }
    }
}
