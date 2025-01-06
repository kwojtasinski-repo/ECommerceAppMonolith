using ECommerce.Modules.PurchaseProfiler.Core.Clients;
using ECommerce.Modules.PurchaseProfiler.Core.DTO;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    public interface IRecommendationService
    {
        Task<List<ProductsDetailsDTO>> GetRecommendationOnCurrentWeek(Guid userId);
    }
}
