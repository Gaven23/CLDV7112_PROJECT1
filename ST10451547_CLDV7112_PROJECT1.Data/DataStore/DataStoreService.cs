using Azure;
using Azure.Data.Tables;

using Microsoft.Extensions.Logging;
using ST10451547_CLDV7112_PROJECT1.Data;
using ST10451547_CLDV7112_PROJECT1.Data.Entities;

namespace ST10451547_CLDV7112_PROJECT1
{
    public class DataStoreService : IDataStore
    {
        private readonly TableClient _tableClient;
        private const string TableName = "CustomerProfile";
        private const string eName = "Product";
        public DataStoreService(TableServiceClient client)
        {
            _tableClient = client.GetTableClient(TableName);
            _tableClient = client.GetTableClient(eName);
            CreateTableIfNotExists().GetAwaiter().GetResult();
        }

        private async Task CreateTableIfNotExists()
        {
            try
            {
                await _tableClient.CreateIfNotExistsAsync();
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error creating or accessing the table '{TableName}': {ex.Message}");
                throw; 
            }
        }

        public async Task<CustomerProfile?> GetCustomerProfileAsync(Guid profileId, CancellationToken cancellationToken = default)
        {
            try
            {
                string partitionKey = "YourPartitionKey"; 
                var response = await _tableClient.GetEntityAsync<CustomerProfile>(partitionKey, profileId.ToString(), cancellationToken: cancellationToken);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                Console.WriteLine($"Customer profile with ID '{profileId}' not found: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving customer profile: {ex.Message}");
                throw;
            }
        }

        public async Task SaveCustomerProfileAsync(CustomerProfile customerProfile, CancellationToken cancellationToken = default)
        {
            try
            {
                var partitionKey = "CustomerPartition";
                var rowKey = Guid.NewGuid().ToString();

                var entity = new CustomerProfile
                {
                    PartitionKey = partitionKey,
                    RowKey = rowKey,
                    CustomerName = customerProfile.CustomerName,
                    CustomerAddress = customerProfile.CustomerAddress,
                    CustomerCity = customerProfile.CustomerCity,
                    Timestamp = DateTime.UtcNow,
                };

                await _tableClient.AddEntityAsync(entity);

            }
            catch (RequestFailedException ex)
            {

                Console.WriteLine($"Error saving customer profile: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Unexpected error saving customer profile: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CustomerProfile>> GetCustomerProfilesAsync(CancellationToken cancellationToken = default)
        {
            var profiles = new List<CustomerProfile>();
            try
            {
                await foreach (var entity in _tableClient.QueryAsync<TableEntity>())
                {
                    profiles.Add(new CustomerProfile
                    {
                        PartitionKey = entity.PartitionKey,
                        RowKey = entity.RowKey,
                        CustomerAddress = entity.GetString("CustomerAddress"),
                        CustomerCity = entity.GetString("CustomerCity"),
                        CustomerName = entity.GetString("CustomerName"),
                    });

                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error querying customer profiles: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Unexpected error querying customer profiles: {ex.Message}");
                throw;
            }

            return profiles;
        }

        public async Task<Product?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            try
            {
                string partitionKey = "YourPartitionKey";
                var response = await _tableClient.GetEntityAsync<Product>(partitionKey, productId.ToString(), cancellationToken: cancellationToken);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                Console.WriteLine($"Product '{productId}' not found: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Product: {ex.Message}");
                throw;
            }
        }

        public async Task SaveProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            try
            {
                var partitionKey = "ProductPartition";
                var rowKey = Guid.NewGuid().ToString();

                var entity = new Product
                {
                    PartitionKey = partitionKey,
                    RowKey = rowKey,
                    Value = product.Price,
                    ProductName = product.ProductName,
                    Timestamp = DateTime.UtcNow,
                  
                };

                await _tableClient.AddEntityAsync(entity);
            }
            catch (RequestFailedException ex)
            {

                Console.WriteLine($"Error saving customer profile: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Unexpected error saving customer profile: {ex.Message}");
                throw;
            }
        }

    }
}
