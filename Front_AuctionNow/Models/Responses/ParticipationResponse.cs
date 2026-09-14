namespace Front_AuctionNow.Models.Responses
{
    public class ParticipationResponse
    {
        public int SubastaId { get; set; }

        public string Titulo { get; set; } = "";
        public string EstadoSubasta { get; set; } = "";

        public decimal MiUltimaPuja { get; set; }
        public decimal PujaActual { get; set; }

        public string Resultado { get; set; } = "";

        public DateTime FechaFin { get; set; }
    }

}
