using Microsoft.AspNetCore.Mvc;
using MetaExchange.API.Controllers.Requests;
using MetaExchange.API.Controllers.Responses;
using MetaExchange.API.Services.Interfaces;

namespace MetaExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetaExchangeController : ControllerBase
    {
        private readonly IMetaExchangeService _metaExchangeService;

        public MetaExchangeController(IMetaExchangeService metaExchangeService)
        {
            _metaExchangeService = metaExchangeService;
        }

        /// <summary>
        /// Get recommended orders to buy or sell BTC with best price.
        /// </summary>
        /// <param name="requestType">Type of operation (Buy or Sell)</param>
        /// <param name="amount">Amount of BTC to buy or Sell</param>
        /// <returns></returns>
        [HttpPost("recomendations")]
        [ProducesResponseType(typeof(ExchangeResponse), StatusCodes.Status200OK)]
        public IActionResult GetRecommendedOrders(ExchangeRequest request)
        {
            try
            {
                var recommendedOrders = _metaExchangeService.GetRecommendedOrders(request.RequestType, request.Amount);

                var exchangeResponse = new ExchangeResponse
                {
                    RecommendedOrders = recommendedOrders,
                    TotalAmount = recommendedOrders.Sum(o => o.Amount),
                    TotalPrice = recommendedOrders.Sum(o => o.Price * o.Amount)
                };

                return Ok(exchangeResponse);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong");
            }
        }
    }
}