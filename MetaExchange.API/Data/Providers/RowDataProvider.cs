using MetaExchange.API.Data.Providers.Interfaces;
using System.Text.Json;
using MetaExchange.API.Utility;

namespace MetaExchange.API.Data.FileProviders
{
    public class RowDataProvider<T> : IDataProvider<T>
    {
        public IEnumerable<T> GetDataFromFile(string fileName)
        {
            var filePath = PathUtility.FindFilePath(fileName);
            var jsonData = ExtractJsonRows(filePath);

            var entities = new List<T>();
            foreach (var row in jsonData)
            {
                var entity = JsonSerializer.Deserialize<T>(row);
                entities.Add(entity);
            }

            return entities;
        }

        private List<string> ExtractJsonRows(string pathToFile)
        {
            var jsonRows = new List<string>();
            using (StreamReader reader = new StreamReader(pathToFile))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        int tabIndex = line.IndexOf('\t');
                        if (tabIndex != -1)
                        {
                            string jsonPart = line.Substring(tabIndex + 1);
                            jsonRows.Add(jsonPart);
                        }
                    }
                }
            }

            return jsonRows;
        }
    }
}
