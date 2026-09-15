namespace Front_AuctionNow.Models.Responses
{
    public class AuctionResponse
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = "";
        public string Categoria { get; set; } = "";
        public string? UrlImagen { get; set; }

        public decimal PrecioBase { get; set; }
        public decimal PujaActual { get; set; }

        public int CantidadPujas { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public string Estado { get; set; } = "";
    }

}
