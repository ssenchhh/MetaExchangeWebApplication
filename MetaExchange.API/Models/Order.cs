namespace MetaExchange.API.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public int ExchangerId { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Kind { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}
