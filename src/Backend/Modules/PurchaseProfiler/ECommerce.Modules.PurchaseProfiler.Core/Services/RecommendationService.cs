using Microsoft.ML;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal sealed class RecommendationService
        (
            IFastTreePurchaseProfilerModel fastTreePurchaseProfilerModel
        )
        : IRecommendationService
    {
        private readonly MLContext _mlContext = new();

        public async Task<List<Dictionary<string, object>>> GetRecommendations(Guid userId)
        {
            var predictionData = new List<CustomerData>
            {
                new () { CustomerId = 12, ProductId = 10, Price = 100, PurchaseFrequency = 200 },
                new () { CustomerId = 2, ProductId = 3, Price = 150, PurchaseFrequency = 1000 },
                new () { CustomerId = 100, ProductId = 300, Price = 150, PurchaseFrequency = 1 },
            };

            var model = await fastTreePurchaseProfilerModel.GetModel(userId);
            if (model is null)
            {
                return
               [
                   new ()
                    {
                        { "predictions", Enumerable.Empty<CustomerPrediction>() }
                    }
               ];
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
    }
}
