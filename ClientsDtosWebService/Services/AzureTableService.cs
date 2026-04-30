using Azure.Data.Tables;
using Azure;
using System.Threading.Tasks;

namespace ClientsDtosWebService.Services
{
    public class AzureTableService
    {
        private readonly TableClient _tableClient;

        public AzureTableService(IConfiguration configuration)
        {
            var conn = configuration["AzureStorage:ConnectionString"];
            var tableName = configuration["AzureStorage:TableName"] ?? "Clients";

            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("AzureStorage:ConnectionString is not configured.");

            _tableClient = new TableClient(conn, tableName);
            _tableClient.CreateIfNotExists();
        }

        // Додати сутність
        public async Task AddAsync<T>(T entity) where T : class, ITableEntity, new()
        {
            await _tableClient.AddEntityAsync(entity);
        }

        // Отримати сутність
        public async Task<T?> GetAsync<T>(string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        // Оновити сутність
        public async Task UpdateAsync<T>(T entity) where T : class, ITableEntity, new()
        {
            await _tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);
        }

        // Видалити сутність
        public async Task DeleteAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        // Отримати всі сутності
        public async Task<List<T>> GetAllAsync<T>() where T : class, ITableEntity, new()
        {
            var results = new List<T>();
            await foreach (var entity in _tableClient.QueryAsync<T>())
            {
                results.Add(entity);
            }
            return results;
        }
    }
}

