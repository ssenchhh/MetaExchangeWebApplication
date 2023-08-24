using MetaExchange.API.Data.Providers.Interfaces;
using MetaExchange.API.Models;

namespace MetaExchange.API.Data.Repositories
{
    public class OrderBookRepository : IRepository<OrderBook>
    {
        private const string FileName = "order_books_data";
        private readonly IDataProvider<OrderBook> _fileProvider;

        public OrderBookRepository(IDataProvider<OrderBook> fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public IEnumerable<OrderBook> GetAll()
        {
            return _fileProvider.GetDataFromFile(FileName);
        }
    }
}
