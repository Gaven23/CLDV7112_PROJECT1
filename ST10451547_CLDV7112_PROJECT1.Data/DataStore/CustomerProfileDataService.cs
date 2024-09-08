using Azure;
using Azure.Data.Tables;
using ST10451547_CLDV7112_PROJECT1.Data;
using ST10451547_CLDV7112_PROJECT1.Data.Entities;

namespace ST10451547_CLDV7112_PROJECT1
{
    public class CustomerProfileDataService : IDataStore
    {
        private readonly TableClient _tableClient;
        private const string TableName = "CustomerProfile";

        public CustomerProfileDataService(TableServiceClient client)
        {
            _tableClient = client.GetTableClient(TableName);
            // Ensure the table is created
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
                // Log exception details here
                Console.WriteLine($"Error creating or accessing the table '{TableName}': {ex.Message}");
                throw; // Re-throw exception to ensure the issue is surfaced
            }
        }

        public async Task<CustomerProfile?> GetCustomerProfileAsync(Guid profileId, CancellationToken cancellationToken = default)
        {
            try
            {
                string partitionKey = "YourPartitionKey"; // Set appropriate partition key if needed
                var response = await _tableClient.GetEntityAsync<CustomerProfile>(partitionKey, profileId.ToString(), cancellationToken: cancellationToken);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Handle not found error
                Console.WriteLine($"Customer profile with ID '{profileId}' not found: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Log and handle other exceptions
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
    }
}
