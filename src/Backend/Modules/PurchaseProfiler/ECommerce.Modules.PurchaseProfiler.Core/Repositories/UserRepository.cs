using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal sealed class UserRepository(
        IGenericRepository<User, long> genericRepository)
        : IUserRepository
    {
        private readonly string _collectionName = genericRepository.CollectionName;

        public async Task<User> AddAsync(User user)
        {
            return await genericRepository.AddAsync(user);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await genericRepository.DeleteAsync(key);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var query = string.Format("FOR u IN {0} FILTER u.email == @email RETURN u", _collectionName);
            var bindVars = new Dictionary<string, object> { { "email", email } };
            var result = await genericRepository.DbClient.Cursor.PostCursorAsync<User>(query, bindVars);
            return result.Result.FirstOrDefault();
        }

        public async Task<User?> GetByKeyAsync(string key)
        {
            return await genericRepository.GetByKeyAsync(key);
        }

        public async Task<User?> UpdateAsync(User user)
        {
            return await genericRepository.UpdateAsync(user);
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            var query = string.Format("FOR u IN {0} FILTER u.UserId == @userId RETURN 1", _collectionName);
            var bindVars = new Dictionary<string, object> { { "userId", userId } };
            var cursorResult = await genericRepository.DbClient.Cursor.PostCursorAsync<int>(query, bindVars, batchSize: 1);
            return cursorResult.Result.Any();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            var query = string.Format("FOR u IN {0} FILTER u.UserId == @userId RETURN u", _collectionName);
            var bindVars = new Dictionary<string, object> { { "userId", userId } };
            var cursorResult = await genericRepository.DbClient.Cursor.PostCursorAsync<User>(query, bindVars, batchSize: 1);
            return cursorResult.Result.FirstOrDefault();
        }

        public ArangoPaginationCollection<User> GetPaginatedUsers(int pageSize)
        {
            return genericRepository.GetPaginatedResults(pageSize);
        }
    }
}
