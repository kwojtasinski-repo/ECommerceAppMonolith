using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using Microsoft.ML;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal sealed class RecommendationService
        (
            IFastTreePurchaseProfilerModel fastTreePurchaseProfilerModel,
            IUserRepository userRepository,
            IOrderRepository orderRepository
        )
        : IRecommendationService
    {
        private readonly MLContext _mlContext = new();

        public async Task<List<Dictionary<string, object>>> GetRecommendations(Guid userId)
        {
            if (!await userRepository.ExistsAsync(userId))
            {
                return CreateEmptyRecommendationResult();
            }

            var orders = await orderRepository.GetOrdersByUserId(userId);
            if (orders is null || orders.Count == 0)
            {
                return CreateEmptyRecommendationResult();
            }

            var predictionData = new List<CustomerData>
            {
                new () { CustomerId = 12, ProductId = 10, Price = 100, PurchaseFrequency = 200 },
                new () { CustomerId = 2, ProductId = 3, Price = 150, PurchaseFrequency = 1000 },
                new () { CustomerId = 100, ProductId = 300, Price = 150, PurchaseFrequency = 1 },
            };

            var model = await fastTreePurchaseProfilerModel.GetModel(userId);
            if (model is null)
            {
                return CreateEmptyRecommendationResult();
            }

            var predictions = model.Transform(_mlContext.Data.LoadFromEnumerable(predictionData));
            var predictedResults = _mlContext.Data.CreateEnumerable<CustomerPrediction>(predictions, reuseRowObject: false).ToList();
            return
            [
                new ()
                {
                    { "predictions", predictedResults }
                }
            ];
        }

        private List<Dictionary<string, object>> CreateEmptyRecommendationResult()
        {
            return
            [
                new ()
                {
                    { "predictions", Enumerable.Empty<CustomerPrediction>() }
                }
            ];
        }
    }
}
