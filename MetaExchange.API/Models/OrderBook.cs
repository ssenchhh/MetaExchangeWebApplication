namespace MetaExchange.API.Models
{
    public class OrderBook
    {
        public int Id { get; set; }
        public DateTime AcqTime { get; set; }
        public IEnumerable<OrderWrapper> Bids { get; set; }
        public IEnumerable<OrderWrapper> Asks { get; set; }
    }
}
