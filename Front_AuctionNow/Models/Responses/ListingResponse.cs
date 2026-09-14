namespace Front_AuctionNow.Models.Responses
{
    public class ListingResponse
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = "";
        public string Categoria { get; set; } = "";
        public string Estado { get; set; } = "";

        public int CantidadPujas { get; set; }

        public decimal PrecioActual { get; set; }
        public decimal Recaudacion { get; set; }

        public string EstadoAdjudicacion { get; set; } = "";

        public DateTime FechaFin { get; set; }
    }
}
