namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal interface IRecommendationService
    {
        Task<List<Dictionary<string, object>>> GetRecommendations(Guid userId);
    }
}
