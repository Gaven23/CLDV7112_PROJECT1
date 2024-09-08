using ST10451547_CLDV7112_PROJECT1.Data;
using ST10451547_CLDV7112_PROJECT1.Data.Entities;

namespace ST10451547_CLDV7112_PROJECT1.BusinessLogic
{
    public class CustomerProfileService
    {
        private readonly IDataStore _dataStore;
        public CustomerProfileService(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<IEnumerable<CustomerProfile>> GetCustomerProfilesAsync(CancellationToken cancellationToken = default)
        {
            return await _dataStore.GetCustomerProfilesAsync(cancellationToken);
        }

        public async Task AddCustomerProfileAsync(CustomerProfile customerProfile,CancellationToken cancellationToken = default)
        {
             await _dataStore.SaveCustomerProfileAsync(customerProfile, cancellationToken);
        }
    }
}
