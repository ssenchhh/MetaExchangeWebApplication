namespace MetaExchange.API.Models
{
    public class ExchangerBalance
    {
        public int ExchangerId { get; set; }
        public decimal EURBalance { get; set; }
        public decimal BTCBalance { get; set; }
    }
}
