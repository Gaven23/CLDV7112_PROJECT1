using Azure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10451547_CLDV7112_PROJECT1.Data.Entities
{
    public class Document
    {
        public string PartitionKey { get; set; }
        public string CustomerName { get; set; }
        public int PhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string RowKey { get; set; }
        public IFormFile Upload { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
