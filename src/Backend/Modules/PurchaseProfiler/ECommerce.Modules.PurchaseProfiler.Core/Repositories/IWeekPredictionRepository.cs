using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public interface IWeekPredictionRepository
    {
        Task<List<WeekPrediction>> GetByUserIdAsync(Guid userId);
        Task<WeekPrediction?> GetByYearWeekNumberAndUserIdAsync(int year, int week, Guid userId);
        Task<WeekPrediction> AddAsync(WeekPrediction weekPrediction);
        Task<WeekPrediction?> UpdateAsync(WeekPrediction weekPrediction);
        Task<bool> DeleteAsync(string key);
    }
}
