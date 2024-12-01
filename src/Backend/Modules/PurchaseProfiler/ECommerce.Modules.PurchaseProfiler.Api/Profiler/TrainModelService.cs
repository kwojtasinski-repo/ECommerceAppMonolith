using ECommerce.Modules.PurchaseProfiler.Api.Data;
using Microsoft.ML;

namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    public class TrainModelService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public TrainModelService()
        {
            _mlContext = new MLContext();

            // 1. Wczytaj dane
            var data = _mlContext.Data.LoadFromTextFile<ProductRating>(
                path: System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "data.csv",
                hasHeader: true,
                separatorChar: ',');

            // 2. Przygotowanie danych
            var dataPrepPipeline = _mlContext.Transforms.Conversion.MapValueToKey(
                                        outputColumnName: "CustomerIdEncoded",
                                        inputColumnName: "CustomerId")
                                    .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                                        outputColumnName: "ItemIdEncoded",
                                        inputColumnName: "ItemId"));

            // 3. Model rekomendacji
            var pipeline = dataPrepPipeline.Append(
                _mlContext.Recommendation().Trainers.MatrixFactorization(
                    labelColumnName: "Rating",
                    matrixColumnIndexColumnName: "CustomerIdEncoded",
                    matrixRowIndexColumnName: "ItemIdEncoded"));

            // 4. Trenuj model
            _model = pipeline.Fit(data);
        }

        public float Predict(int customerId, int itemId)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductRating, ProductRecommendation>(_model);

            var prediction = predictionEngine.Predict(new ProductRating
            {
                CustomerId = customerId,
                ItemId = itemId
            });

            return prediction.Score;
        }
    }
}
