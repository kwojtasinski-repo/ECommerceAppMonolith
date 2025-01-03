using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public interface IComputePredictionRepository
    {
        Task<ComputePrediction> AddAsync(ComputePrediction entity);
        Task<bool> DeleteAsync(string key);
        Task<IEnumerable<ComputePrediction>> AddRangeAsync(IEnumerable<ComputePrediction> entities);
        ArangoPaginationCollection<ComputePrediction> GetPaginatedComputePredictions(int pageSize);
    }
}
