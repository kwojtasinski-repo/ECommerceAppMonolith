namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    public interface IRecommendationService
    {
        Task<List<Dictionary<string, object>>> GetRecommendations(Guid userId);
        Task<List<Guid>> GetRecommendationOnCurrentWeek(Guid userId);
    }
}
