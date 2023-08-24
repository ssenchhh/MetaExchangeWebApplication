using MetaExchange.API.Enums;
using MetaExchange.API.Models;

namespace MetaExchange.API.Services.Interfaces
{
    public interface IMetaExchangeService
    {
        IEnumerable<Order> GetRecommendedOrders(RequestType requestType, decimal amount);
    }
}
