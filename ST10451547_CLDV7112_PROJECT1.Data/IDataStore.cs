using ST10451547_CLDV7112_PROJECT1.Data.Entities;

namespace ST10451547_CLDV7112_PROJECT1.Data
{
    public interface IDataStore
    {
        #region Customer Profiles
        Task<IEnumerable<CustomerProfile>> GetCustomerProfilesAsync(CancellationToken cancellationToken = default);
        Task<CustomerProfile?> GetCustomerProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
        Task SaveCustomerProfileAsync(CustomerProfile customerProfile, CancellationToken cancellationToken = default);
        Task<Product?> GetProductAsync(Guid profileId, CancellationToken cancellationToken = default);
        Task SaveProductAsync(Product product, CancellationToken cancellationToken = default);
        #endregion
    }
}
