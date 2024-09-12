using Azure;
using Azure.Data.Tables;

namespace ST10451547_CLDV7112_PROJECT1.Data.Entities
{
    public class Product : ITableEntity
    {
        public int Price { get; set; }
        public int Value { get; set; }
        public string ProductName { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
