using Azure.Data.Tables;
using Azure;

namespace ST10451547_CLDV7112_PROJECT1.Data.Entities
{
    public class CustomerProfile : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string CustomerName { get; set; }
        public int PhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
