using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class ComputePredictionRepository
        (
            IGenericRepository<ComputePrediction, long> genericRepository
        )
        : IComputePredictionRepository
    {
        public async Task<ComputePrediction> AddAsync(ComputePrediction entity)
        {
            return await genericRepository.AddAsync(entity);
        }

        public async Task<IEnumerable<ComputePrediction>> AddRangeAsync(IEnumerable<ComputePrediction> entities)
        {
            return await genericRepository.AddRangeAsync(entities);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await genericRepository.DeleteAsync(key);
        }

        public ArangoPaginationCollection<ComputePrediction> GetPaginatedComputePredictions(int pageSize)
        {
            return genericRepository.GetPaginatedResults(pageSize);
        }
    }
}
