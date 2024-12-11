using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public interface IUserModelRepository
    {
        Task<UserModel?> GetByKeyAsync(string key);
        Task<UserModel?> GetByUserIdAsync(Guid userId);
        Task<UserModel> AddAsync(UserModel userModel);
        Task<UserModel?> UpdateAsync(UserModel userModel);
        Task<bool> DeleteAsync(string key);
    }
}
