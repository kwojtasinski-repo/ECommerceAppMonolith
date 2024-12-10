using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class UserModelRepository(IGenericRepository<UserModel, long> genericRepository)
        : IUserModelRepository
    {
        public async Task<UserModel> AddAsync(UserModel userModel)
        {
            return await genericRepository.AddAsync(userModel);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await genericRepository.DeleteAsync(key);
        }

        public async Task<UserModel?> GetByKeyAsync(string key)
        {
            return await genericRepository.GetByKeyAsync(key);
        }

        public async Task<UserModel?> UpdateAsync(UserModel userModel)
        {
            return await genericRepository.UpdateAsync(userModel);
        }
    }
}
