using Azure;
using Azure.Data.Tables;

namespace ClientsDtosWebService.Models
{
    public class Client: ITableEntity
    {
        // PartitionKey and RowKey are required properties for Azure Table Storage entities.
        public string PartitionKey { get; set; } = "Clients";

        // RowKey is typically a unique identifier for the entity. Here we use a GUID to ensure uniqueness.
        public string RowKey { get; set; } = Guid.NewGuid().ToString();

        // Additional properties for the user entry.
        public DateTimeOffset? Timestamp { get; set; }

        // ETag is used for concurrency control in Azure Table Storage. It allows you to handle updates and deletes safely.
        public ETag ETag { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
