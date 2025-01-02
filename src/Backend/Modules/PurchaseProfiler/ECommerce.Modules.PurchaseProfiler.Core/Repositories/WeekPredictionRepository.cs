using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class WeekPredictionRepository
        (
            IGenericRepository<WeekPrediction, long> genericRepository
        )
        : IWeekPredictionRepository
    {
        private readonly string _collectionName = genericRepository.CollectionName;

        public async Task<WeekPrediction> AddAsync(WeekPrediction weekPrediction)
        {
            return await genericRepository.AddAsync(weekPrediction);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await genericRepository.DeleteAsync(key);
        }

        public async Task<WeekPrediction?> GetByUserIdAsync(Guid userId)
        {
            var query = string.Format("FOR um IN {0} FILTER um.UserId == @userId RETURN um", _collectionName);
            var bindVars = new Dictionary<string, object> { { "userId", userId } };
            var result = await genericRepository.DbClient.Cursor.PostCursorAsync<WeekPrediction>(query, bindVars);
            return result.Result.FirstOrDefault();
        }

        public async Task<WeekPrediction?> UpdateAsync(WeekPrediction weekPrediction)
        {
            return await genericRepository.UpdateAsync(weekPrediction);
        }
    }
}
