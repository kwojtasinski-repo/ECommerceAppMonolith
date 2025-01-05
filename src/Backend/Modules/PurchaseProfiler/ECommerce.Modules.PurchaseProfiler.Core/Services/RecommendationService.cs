using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal sealed class RecommendationService
        (
            IWeekPredictionRepository weekPredictionRepository,
            IProductRepository productRepository,
            IConfiguration configuration
        )
        : IRecommendationService
    {
        private readonly decimal _minProbabilityForRecommandationProducts = configuration.GetValue<decimal>("recommendations:minProbabilityForProducts");

        public async Task<List<Guid>> GetRecommendationOnCurrentWeek(Guid userId)
        {
            var currentDate = DateTime.UtcNow;
            var weekPredictions = await weekPredictionRepository.GetByYearWeekNumberAndUserIdAsync(currentDate.Year, ISOWeek.GetWeekOfYear(currentDate), userId);
            var predictedProducts = weekPredictions?.PredictedPurchases
                .Where(p => p.Probability * 100 > _minProbabilityForRecommandationProducts)?
                .Select(p => p.ProductId)?.ToList() ?? [];
            if (predictedProducts.Count == 0)
            {
                return [];
            }

            return await productRepository.GetProductsIdsByKeysAsync(predictedProducts);
        }
    }
}
