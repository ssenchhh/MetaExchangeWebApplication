using MetaExchange.API.Data.Providers.Interfaces;
using System.Text.Json;

namespace MetaExchange.API.Data.FileProviders
{
    public class JsonDataProvider<T> : IDataProvider<T>
    {
        public IEnumerable<T> GetDataFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string jsonContent = reader.ReadToEnd();
                return JsonSerializer.Deserialize<IEnumerable<T>>(jsonContent);
            }
        }
    }
}
