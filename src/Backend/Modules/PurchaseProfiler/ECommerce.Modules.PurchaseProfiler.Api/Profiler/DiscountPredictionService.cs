using Microsoft.ML;

namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    internal class DiscountPredictionService
    {
        public static List<CustomerDiscountData> HistoricalDiscountData =
        [
            new () { PurchaseFrequency = 10, TotalPurchaseValue = 1200, DaysSinceLastPurchase = 5, Discount = 0.40f },
            new () { PurchaseFrequency = 5, TotalPurchaseValue = 600, DaysSinceLastPurchase = 15, Discount = 0.30f },
            new () { PurchaseFrequency = 3, TotalPurchaseValue = 300, DaysSinceLastPurchase = 30, Discount = 0.20f },
            new () { PurchaseFrequency = 1, TotalPurchaseValue = 100, DaysSinceLastPurchase = 60, Discount = 0.10f },
            new () { PurchaseFrequency = 0, TotalPurchaseValue = 50, DaysSinceLastPurchase = 90, Discount = 0.05f }
        ];

        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public DiscountPredictionService()
        {
            _mlContext = new MLContext();

            // Training data
            var data = _mlContext.Data.LoadFromEnumerable(HistoricalDiscountData);

            // ML.NET pipeline
            var pipeline = _mlContext.Transforms.Concatenate("Features", "PurchaseFrequency", "TotalPurchaseValue", "DaysSinceLastPurchase")
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Discount", maximumNumberOfIterations: 100));

            // Train the model
            _model = pipeline.Fit(data);
        }

        public decimal PredictDiscount(int purchaseFrequency, decimal totalPurchaseValue, int daysSinceLastPurchase)
        {
            // Create prediction engine
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<CustomerDiscountData, DiscountPrediction>(_model);

            // Predict
            var prediction = predictionEngine.Predict(new CustomerDiscountData
            {
                PurchaseFrequency = purchaseFrequency,
                TotalPurchaseValue = (float)totalPurchaseValue,
                DaysSinceLastPurchase = daysSinceLastPurchase
            });

            return (decimal)Math.Round(prediction.Discount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
