namespace Front_AuctionNow.Models.Responses
{
    public class PagedAuctionsResponse
    {
        public IList<AuctionResponse> Items { get; set; } = [];

        public int TotalItems { get; set; }
        public int TotalPaginas { get; set; }

        public int Pagina { get; set; }
        public int TamanioPagina { get; set; }
    }

}
