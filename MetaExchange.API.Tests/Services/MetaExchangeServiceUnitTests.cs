using FluentAssertions;
using Moq;
using MetaExchange.API.Data.Repositories;
using MetaExchange.API.Enums;
using MetaExchange.API.Models;
using MetaExchange.API.Services;

namespace MetaExchange.API.Tests.Services
{
    public class MetaExchangeServiceUnitTests
    {
        [Fact]
        public void MetaExchangeService_ValidInput_ReturnsCreatedOrders()
        {
            var service = CreateService();

            var result = service.GetRecommendedOrders(RequestType.Buy, 1);

            result.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [Fact]
        public void MetaExchangeService_UnsupportedRequestType_ThrowsInvalidOperationException()
        {
            var service = CreateService();

            Assert.Throws<InvalidOperationException>(() => service.GetRecommendedOrders((RequestType)9, 1));
        }

        [Fact]
        public void MetaExchangeService_AmountIsZeroAndLower_ThrowsArgumentException()
        {
            var service = CreateService();

            Assert.Throws<ArgumentException>(() => service.GetRecommendedOrders(default, 0));
            Assert.Throws<ArgumentException>(() => service.GetRecommendedOrders(default, -1));
        }

        private MetaExchangeService CreateService(IRepository<OrderBook> repository = null)
        {
            var mockRepository = new Mock<IRepository<OrderBook>>();
            mockRepository.Setup(r => r.GetAll())
                .Returns(() => new List<OrderBook>() 
                { 
                    CreateOrderBook()
                });

            return new MetaExchangeService(repository ?? mockRepository.Object);
        }

        private OrderBook CreateOrderBook()
        {
            return new OrderBook()
            {
                Id = 1,
                AcqTime = DateTime.Now,
                Asks = new List<OrderWrapper>()
                {
                    new OrderWrapper()
                    {
                        Order = new Order()
                        {
                            ExchangerId = 1,
                            Amount = 1,
                            Price = 100,
                        }
                    },
                    new OrderWrapper()
                    {
                        Order = new Order()
                        {
                            ExchangerId = 1,
                            Amount = 2,
                            Price = 100,
                        }
                    }
                },
                Bids = new List<OrderWrapper>()
                {
                    new OrderWrapper()
                    {
                        Order = new Order()
                        {
                            ExchangerId = 1,
                            Amount = 1,
                            Price = 100,
                        }
                    },
                    new OrderWrapper()
                    {
                        Order = new Order()
                        {
                            ExchangerId = 1,
                            Amount = 2,
                            Price = 100,
                        }
                    }
                }
            };
            
        }
    }
}
