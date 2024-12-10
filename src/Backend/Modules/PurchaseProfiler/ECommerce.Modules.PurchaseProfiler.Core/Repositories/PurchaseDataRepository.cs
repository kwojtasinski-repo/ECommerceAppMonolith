using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class PurchaseDataRepository(IGenericRepository<PurchaseData, long> genericRepository)
        : IPurchaseDataRepository
    {
        public async Task<PurchaseData> AddAsync(PurchaseData purchaseData)
        {
            return await genericRepository.AddAsync(purchaseData);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await genericRepository.DeleteAsync(key);
        }

        public async Task<PurchaseData?> GetByKeyAsync(string key)
        {
            return await genericRepository.GetByKeyAsync(key);
        }

        public async Task<PurchaseData?> UpdateAsync(PurchaseData purchaseData)
        {
            return await genericRepository.UpdateAsync(purchaseData);
        }
    }
}
