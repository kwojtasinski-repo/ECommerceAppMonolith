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

        public async Task<List<WeekPrediction>> GetByUserIdAsync(Guid userId)
        {
            var query = string.Format("FOR wp IN {0} FILTER wp.UserId == @userId RETURN wp", _collectionName);
            var bindVars = new Dictionary<string, object> { { "userId", userId } };
            var result = await genericRepository.DbClient.Cursor.PostCursorAsync<WeekPrediction>(query, bindVars);
            return result.Result.ToList();
        }

        public async Task<WeekPrediction?> GetByYearWeekNumberAndUserIdAsync(int year, int week, Guid userId)
        {
            var query = string.Format("FOR wp IN {0} FILTER wp.Year == @year AND wp.WeekNumber == @week AND wp.UserId == @userId RETURN wp", _collectionName);
            var bindVars = new Dictionary<string, object> { { "year", year }, { "week", week }, { "userId", userId } };
            var result = await genericRepository.DbClient.Cursor.PostCursorAsync<WeekPrediction>(query, bindVars);
            return result.Result.FirstOrDefault();
        }

        public async Task<WeekPrediction?> UpdateAsync(WeekPrediction weekPrediction)
        {
            return await genericRepository.UpdateAsync(weekPrediction);
        }
    }
}
