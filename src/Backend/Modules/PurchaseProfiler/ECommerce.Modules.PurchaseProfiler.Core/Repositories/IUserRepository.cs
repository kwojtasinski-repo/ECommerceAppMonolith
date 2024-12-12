using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByKeyAsync(string key);
        Task<User?> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(Guid userId);
    }
}
