namespace Front_AuctionNow.Models.Responses
{
    public class BidResponse
    {
        public string Usuario { get; set; } = "";
        public decimal Monto { get; set; }
        public DateTime FechaPuja { get; set; }
    }

}
