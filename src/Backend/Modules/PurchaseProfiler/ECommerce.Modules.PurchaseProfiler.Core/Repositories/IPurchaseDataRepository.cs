using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public interface IPurchaseDataRepository
    {
        Task<PurchaseData?> GetByKeyAsync(string key);
        Task<PurchaseData> AddAsync(PurchaseData purchaseData);
        Task<PurchaseData?> UpdateAsync(PurchaseData purchaseData);
        Task<bool> DeleteAsync(string key);
    }
}
