namespace MetaExchange.API.Data.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
    }
}
