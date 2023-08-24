using MetaExchange.API.Models;

namespace MetaExchange.API.Controllers.Responses
{
    public class ExchangeResponse
    {
        public IEnumerable<Order> RecommendedOrders { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
