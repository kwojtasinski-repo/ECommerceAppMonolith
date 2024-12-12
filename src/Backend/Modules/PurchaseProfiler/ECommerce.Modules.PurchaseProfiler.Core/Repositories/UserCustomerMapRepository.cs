using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class UserCustomerMapRepository
        (
            IGenericRepository<UserCustomersMap, long> genericRepository,
            ILogger<UserCustomerMapRepository> logger
        )
        : IUserCustomerMapRepository
    {
        private readonly string _collectionName = genericRepository.CollectionName;

        public async Task<UserCustomersMap> AddAsync(UserCustomersMap userCustomersMap)
        {
            return await genericRepository.AddAsync(userCustomersMap);
        }

        public async Task<List<UserCustomersMap>> GetAllByUserIdAsync(Guid userId)
        {
            var query = string.Format("FOR userCustomerMap IN {0} FILTER userCustomerMap.UserId == @userId RETURN userCustomerMap", _collectionName);
            var bindVars = new Dictionary<string, object> { { "userId", userId } };
            var response = await genericRepository.DbClient.Cursor.PostCursorAsync<UserCustomersMap>(query, bindVars);
            if (response is null || response.Error)
            {
                logger.LogError("There was an error while getting collections '{collection}' with userId '{userId}', status code: '{statusCode}'", _collectionName, userId, (int)(response?.Code ?? 0));
                return [];
            }
            return response.Result.ToList();
        }
    }
}
