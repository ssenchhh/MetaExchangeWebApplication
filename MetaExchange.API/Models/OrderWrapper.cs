using System.Text.Json.Serialization;

namespace MetaExchange.API.Models
{
    public class OrderWrapper
    {
        [JsonPropertyName("Order")]
        public Order Order { get; set; }
    }
}
