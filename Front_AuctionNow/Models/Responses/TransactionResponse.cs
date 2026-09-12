namespace Front_AuctionNow.Models.Responses
{
    public class TransactionResponse
    {
        public int? SubastaId { get; set; }
        public string Tipo { get; set; } = "";
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
    }

}
