using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public interface IUserCustomerMapRepository
    {
        Task<UserCustomersMap> AddAsync(UserCustomersMap userCustomersMap);
        Task<List<UserCustomersMap>> GetAllByUserIdAsync(Guid userId);
    }
}
