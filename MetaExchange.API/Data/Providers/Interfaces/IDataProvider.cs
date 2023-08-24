namespace MetaExchange.API.Data.Providers.Interfaces
{
    public interface IDataProvider<T>
    {
        IEnumerable<T> GetDataFromFile(string fileName);
    }
}
