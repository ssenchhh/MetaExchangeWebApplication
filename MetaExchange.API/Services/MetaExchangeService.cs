using MetaExchange.API.Data.Repositories;
using MetaExchange.API.Enums;
using MetaExchange.API.Models;
using MetaExchange.API.Services.Interfaces;

namespace MetaExchange.API.Services
{
    public class MetaExchangeService : IMetaExchangeService
    {
        private readonly IRepository<OrderBook> _orderBookRepository;

        public MetaExchangeService(IRepository<OrderBook> orderBookRepository)
        {
            _orderBookRepository = orderBookRepository;
        }

        public IEnumerable<Order> GetRecommendedOrders(RequestType requestType, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount should be greater than zero");

            var exchangers = _orderBookRepository.GetAll();
            exchangers = PrepareOrderBooks(exchangers);
            var balances = PrepareBalances(exchangers);
            bool isBuy;

            List<Order> ordersToProcess = new List<Order>();

            if (requestType == RequestType.Buy)
            {
                isBuy = true;
                ordersToProcess = exchangers.SelectMany(ob => ob.Asks).Select(o => o.Order).ToList();
                ordersToProcess = ordersToProcess.OrderBy(o => o.Price).ToList();
            }
            else if (requestType == RequestType.Sell)
            {
                isBuy = false;
                ordersToProcess = exchangers.SelectMany(ob => ob.Bids).Select(o => o.Order).ToList();
                ordersToProcess = ordersToProcess.OrderByDescending(o => o.Price).ToList();
            }
            else
            {
                throw new InvalidOperationException("Unsupported request type");
            }

            var ordersToReturn = CalculateRecommendedOrders(ordersToProcess, balances, isBuy, amount);

            return ordersToReturn;
        }

        private IEnumerable<Order> CalculateRecommendedOrders(
            IEnumerable<Order> ordersToProcess,
            IEnumerable<ExchangerBalance> balances,
            bool isBuy,
            decimal amount)
        {
            if(amount <= 0) return new List<Order>();

            List<Order> ordersToReturn = new List<Order>();
            foreach (var order in ordersToProcess)
            {
                var exchangerBalance = balances.FirstOrDefault(b => b.ExchangerId == order.ExchangerId);
                var balanceToConsider = isBuy ? exchangerBalance.EURBalance : exchangerBalance.BTCBalance;
                if (balanceToConsider <= 0) { continue; }

                if (isBuy)
                {
                    if (amount - order.Amount >= 0 && order.Amount * order.Price <= exchangerBalance.EURBalance)
                    {
                        ordersToReturn.Add(order);
                        exchangerBalance.EURBalance -= order.Amount * order.Price;
                        amount -= order.Amount;

                        if (amount == 0)
                            break;
                    }
                    else if (order.Amount * order.Price > exchangerBalance.EURBalance)
                    {
                        var availableAmount = exchangerBalance.EURBalance / order.Price;
                        var newAmount = amount <= availableAmount ? amount : availableAmount;
                        var newOrder = new Order()
                        {
                            Id = null,
                            ExchangerId = order.ExchangerId,
                            Price = order.Price,
                            Amount = newAmount,
                            Kind = order.Kind,
                            Type = order.Type,
                            Time = order.Time
                        };
                        ordersToReturn.Add(newOrder);

                        amount -= newAmount;
                        exchangerBalance.EURBalance -= newOrder.Amount * order.Price;

                        if (amount == 0)
                            break;
                    }
                    else
                    {
                        var newOrder = new Order()
                        {
                            Id = null,
                            ExchangerId = order.ExchangerId,
                            Price = order.Price,
                            Amount = amount,
                            Kind = order.Kind,
                            Type = order.Type,
                            Time = order.Time
                        };
                        ordersToReturn.Add(newOrder);

                        break;
                    }
                }
                else
                {
                    if (amount - order.Amount >= 0 && order.Amount <= exchangerBalance.BTCBalance)
                    {
                        ordersToReturn.Add(order);
                        exchangerBalance.BTCBalance -= order.Amount;
                        amount -= order.Amount;

                        if (amount == 0)
                            break;
                    }
                    else if (order.Amount > exchangerBalance.BTCBalance)
                    {
                        var newAmount = amount <= exchangerBalance.BTCBalance ? amount : exchangerBalance.BTCBalance;
                        var newOrder = new Order()
                        {
                            Id = null,
                            ExchangerId = order.ExchangerId,
                            Price = order.Price,
                            Amount = newAmount,
                            Kind = order.Kind,
                            Type = order.Type,
                            Time = order.Time
                        };
                        ordersToReturn.Add(newOrder);

                        amount -= newAmount;
                        exchangerBalance.BTCBalance -= newOrder.Amount;

                        if (amount == 0)
                            break;
                    }
                    else
                    {
                        var newOrder = new Order()
                        {
                            Id = null,
                            ExchangerId = order.ExchangerId,
                            Price = order.Price,
                            Amount = amount,
                            Kind = order.Kind,
                            Type = order.Type,
                            Time = order.Time
                        };
                        ordersToReturn.Add(newOrder);

                        break;
                    }
                }
            }

            return ordersToReturn;
        }

        private List<OrderBook> PrepareOrderBooks(IEnumerable<OrderBook> orderBooks)
        {
            var currentExchangerId = 1;

            foreach (var orderBook in orderBooks)
            {
                orderBook.Id = currentExchangerId;

                foreach (var ask in orderBook.Asks)
                {
                    ask.Order.ExchangerId = currentExchangerId;
                }
                foreach (var bid in orderBook.Bids)
                {
                    bid.Order.ExchangerId = currentExchangerId;
                }

                currentExchangerId++;
            }

            return orderBooks.ToList();
        }

        private List<ExchangerBalance> PrepareBalances(IEnumerable<OrderBook> orderBooks)
        {
            var balances = new List<ExchangerBalance>();
            foreach (var orderBook in orderBooks)
            {
                var balance = new ExchangerBalance()
                {
                    ExchangerId = orderBook.Id,
                    EURBalance = 1000,
                    BTCBalance = 0.1m
                };
                balances.Add(balance);
            }

            return balances;
        }
    }
}
