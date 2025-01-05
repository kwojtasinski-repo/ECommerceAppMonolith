namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    public interface IRecommendationService
    {
        Task<List<Guid>> GetRecommendationOnCurrentWeek(Guid userId);
    }
}
