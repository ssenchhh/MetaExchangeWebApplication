using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MetaExchange.API.Controllers;
using MetaExchange.API.Controllers.Requests;
using MetaExchange.API.Enums;
using MetaExchange.API.Models;
using MetaExchange.API.Services.Interfaces;

namespace MetaExchange.API.Tests.Controllers
{
    public class MetaExchangeControllerUnitTests
    {
        [Fact]
        public void Controller_ValidRequest_Returns200Ok()
        {
            var controller = CreateController();
            var request = new ExchangeRequest();

            IActionResult result = controller.GetRecommendedOrders(request);
            var response = (OkObjectResult)result;

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public void Controller_InvalidRequest_Returns400BadRequest()
        {
            var mockMetaExchangeService = new Mock<IMetaExchangeService>();
            mockMetaExchangeService.Setup(serv => serv.GetRecommendedOrders(It.IsAny<RequestType>(), It.IsAny<decimal>()))
                .Throws(new InvalidOperationException());

            var controller = CreateController(mockMetaExchangeService.Object);
            var request = new ExchangeRequest();

            IActionResult result = controller.GetRecommendedOrders(request);
            var response = (BadRequestObjectResult)result;

            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        private MetaExchangeController CreateController(IMetaExchangeService metaExchangeService = null)
        {
            var mockMetaExchangeService = new Mock<IMetaExchangeService>();
            mockMetaExchangeService.Setup(serv => serv.GetRecommendedOrders(It.IsAny<RequestType>(), It.IsAny<decimal>()))
                .Returns(() => Enumerable.Empty<Order>());

            return new MetaExchangeController(metaExchangeService ?? mockMetaExchangeService.Object);
        }
    }
}
