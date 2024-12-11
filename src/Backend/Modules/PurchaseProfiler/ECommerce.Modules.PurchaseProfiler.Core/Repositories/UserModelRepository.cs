using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class UserModelRepository(IGenericRepository<UserModel, long> genericRepository)
        : IUserModelRepository
    {
        private readonly string _collectionName = genericRepository.CollectionName;

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

        public async Task<UserModel?> GetByUserIdAsync(Guid userId)
        {
            var query = string.Format("FOR um IN {0} FILTER um.UserId == @userId RETURN um", _collectionName);
            var bindVars = new Dictionary<string, object> { { "userId", userId } };
            var result = await genericRepository.DbClient.Cursor.PostCursorAsync<UserModel>(query, bindVars);
            return result.Result.FirstOrDefault();
        }

        public async Task<UserModel?> UpdateAsync(UserModel userModel)
        {
            return await genericRepository.UpdateAsync(userModel);
        }
    }
}
