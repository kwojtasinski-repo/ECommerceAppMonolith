using Microsoft.ML;

namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    internal partial class RecommendationService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _fastTreeModel;
        private readonly List<CustomerData> _customerData;
        private static readonly Random random = new();

        public RecommendationService()
        {
            _mlContext = new MLContext();
            _customerData = GenerateSamples(1000);
            var trainData = _mlContext.Data.LoadFromEnumerable(_customerData);

            // Przekształcenie zmiennych wejściowych (Features) i etykiety (Label)
            var pipeline = _mlContext.Transforms.Concatenate("Features", "CustomerId", "ProductId", "Price", "PurchaseFrequency")
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(_mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "PurchasedProduct", featureColumnName: "Features",
                                                                                    numberOfLeaves: 25,
                                                                                    numberOfTrees: 150,
                                                                                    minimumExampleCountPerLeaf: 10,
                                                                                    learningRate: 0.05));
            var dataSplit = _mlContext.Data.TrainTestSplit(trainData, testFraction: 0.2);
            // Split the data into training and testing sets (80% train, 20% test)
            var trainDataSplit = dataSplit.TrainSet;

            // Train the model
            _fastTreeModel = pipeline.Fit(trainDataSplit);
        }

        public List<Dictionary<string, object>> GetRecommendations(Guid customerId)
        {
            var predictionData = new List<CustomerData>
            {
                new () { CustomerId = 12, ProductId = 10, Price = 100, PurchaseFrequency = 200 },
                new () { CustomerId = 2, ProductId = 3, Price = 150, PurchaseFrequency = 1000 },
                new () { CustomerId = 100, ProductId = 300, Price = 150, PurchaseFrequency = 1 },
            };

            var predictions = _fastTreeModel.Transform(_mlContext.Data.LoadFromEnumerable(predictionData));

            // Display the results
            var predictedResults = _mlContext.Data.CreateEnumerable<CustomerPrediction>(predictions, reuseRowObject: false).ToList();
            return
            [
                new ()
                {
                    { "predictions", predictedResults }
                }
            ];
        }

        // Generate random customer purchase data
        public static List<CustomerData> GenerateSamples(int sampleSize, float priceMin = 50, float priceMax = 500, int customerCount = 10, int productCount = 5)
        {
            var data = new List<CustomerData>();

            for (int i = 0; i < sampleSize; i++)
            {
                // Randomly choose a customer and product
                float customerId = random.Next(1, customerCount + 1);
                float productId = random.Next(1, productCount + 1);

                // Randomly generate a price within the specified range
                float price = (float)(random.NextDouble() * (priceMax - priceMin) + priceMin);

                // Randomly generate purchase frequency (e.g., how often they purchase this product)
                float purchaseFrequency = (float)(int)(random.NextDouble() * 5); // Example: between 0 and 10

                // Add the generated data point
                data.Add(new CustomerData
                {
                    CustomerId = customerId,
                    ProductId = productId,
                    Price = price,
                    PurchaseFrequency = purchaseFrequency,
                    PurchasedProduct = random.NextDouble() > 0.5
                });
            }

            return data;
        }
    }
}
