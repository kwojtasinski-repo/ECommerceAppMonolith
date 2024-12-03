using ECommerce.Modules.PurchaseProfiler.Api.Data;
using ECommerce.Modules.PurchaseProfiler.Api.Entities;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers.LightGbm;
using static ECommerce.Modules.PurchaseProfiler.Api.Data.StaticData;

namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    internal class RecommendationService
    {
        private readonly MLContext _mlContext;
        private ITransformer _fastTreeModel;

        public RecommendationService()
        {
            _mlContext = new MLContext();
            var expandedData = StaticData.WeeklyCustomerDataList.SelectMany(customer =>
                customer.PurchasedItemIds.Select(itemId => new WeeklyCustomerDataInput2
                {
                    CustomerId = 1,
                    WeekNumber = customer.WeekNumber,
                    PurchaseFrequency = customer.PurchaseFrequency,
                    TotalSpent = customer.TotalSpent,
                    DaysSinceLastPurchase = customer.DaysSinceLastPurchase,
                    ItemId = itemId.GetHashCode(),
                    Label = customer.PurchaseFrequency
                }));
            var data = _mlContext.Data.LoadFromEnumerable(expandedData);
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("CustomerId")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("ItemId"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(
                    "Label", "CustomerId", "ItemId", numberOfIterations: 20, learningRate: 0.1));

            // Trenowanie modelu
            _fastTreeModel = pipeline.Fit(data);
        }

        public List<Dictionary<string, object>> GetRecommendations(Guid customerId)
        {
            var customer = StaticData.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null) return new List<Dictionary<string, object>>();

            var predictionData = _mlContext.Data.LoadFromEnumerable(new List<object>
            {
                new WeeklyCustomerDataInput2
                {
                    CustomerId = 1,  // Alice
                    WeekNumber = 49, // Przewidywania na kolejny tydzień
                    PurchaseFrequency = 1, // Zakładana częstotliwość (może pochodzić np. z trendu w danych historycznych)
                    TotalSpent = 800, // Przewidywana wartość (oparta na trendach zakupowych)
                    DaysSinceLastPurchase = 7, // Typowe opóźnienie między zakupami
                    ItemId = Items[2].Id.GetHashCode()
                },
                new WeeklyCustomerDataInput2
                {
                    CustomerId = 1,  // Alice
                    WeekNumber = 49, // Przewidywania na kolejny tydzień
                    PurchaseFrequency = 1, // Zakładana częstotliwość (może pochodzić np. z trendu w danych historycznych)
                    TotalSpent = 800, // Przewidywana wartość (oparta na trendach zakupowych)
                    DaysSinceLastPurchase = 7, // Typowe opóźnienie między zakupami
                    ItemId = Items[0].Id.GetHashCode() // Historia zakupów może się powtarzać
                },
            });
            var predictions = _fastTreeModel.Transform(predictionData);
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "predictions", predictions }
                }
            };
            /*
    
            // Create the customer profile
            var customerProfile = CreateCustomerProfile(customer);

            // Prepare the recommendation list
            var recommendations = new List<Dictionary<string, object>>();

            foreach (var item in StaticData.Items)
            {
                // Skip inactive items
                if (!item.IsActive) continue;

                // Create the item profile
                var itemProfile = CreateItemProfile(item);

                // Combine customer profile and item profile into a feature vector
                var features = new ModelInput
                {
                    PurchaseFrequency = customerProfile.PurchaseFrequency,
                    TotalSpent = customerProfile.TotalSpent,
                    DaysSinceLastPurchase = customerProfile.DaysSinceLastPurchase,
                    Price = itemProfile.Price
                };

                // Predict the score using the ML model
                var predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_fastTreeModel);
                var prediction = predictionEngine.Predict(features);

                // Add the recommendation to the list
                recommendations.Add(new Dictionary<string, object>
                {
                    { "ItemId", item.Id },
                    { "ItemName", item.ItemName },
                    { "Score", prediction.Score }
                });
            }

            // Sort recommendations by prediction score in descending order
            return recommendations.OrderByDescending(r => (float)r["Score"]).ToList();*/
        }

        private void BuildModel(Guid customerId)
        {
            // Preparing the training data
            var trainingData = _mlContext.Data.LoadFromEnumerable(StaticData.CustomerWeeklyData.Where(c => c.CustomerId == customerId)
                                .SelectMany(customerData =>
                                    StaticData.Items.Select(item => new CustomerProfile
                                    {
                                        PurchaseFrequency = customerData.PurchaseFrequency,
                                        TotalSpent = customerData.TotalSpent,
                                        DaysSinceLastPurchase = customerData.DaysSinceLastPurchase,
                                        Price = (float)item.Price, // Include Price as part of the features
                                        Label = StaticData.Sales.Any(s => s.CustomerId == customerData.CustomerId && s.ItemId == item.Id) ? 1.0f : 0.0f
                                    })));

            // FastTree Pipeline
            var fastTreePipeline = _mlContext.Transforms.Concatenate("Features", "PurchaseFrequency", "TotalSpent", "DaysSinceLastPurchase", "Price")
                                        .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                                        .Append(_mlContext.Regression.Trainers.FastTree(labelColumnName: "Label", featureColumnName: "Features"));
            _fastTreeModel = fastTreePipeline.Fit(trainingData);
        }

        public List<RecommendedItem> GetRecommendationsWithDiscounts(Guid customerId)
        {
            var customerSales = StaticData.Sales
                .Where(s => s.CustomerId == customerId)
                .ToList();

            var purchaseFrequency = customerSales.Count;
            var totalPurchaseValue = customerSales.Sum(s => StaticData.Items.First(i => i.Id == s.ItemId).Price * s.PurchaseCount);
            var daysSinceLastPurchase = (DateTime.Now - customerSales.Max(s => s.LastPurchaseDate)).Days;

            // Predict discount
            var discount = new DiscountPredictionService().PredictDiscount(purchaseFrequency, totalPurchaseValue, daysSinceLastPurchase);

            // Generate recommendations
            var recommendedItems = StaticData.Items
                .Where(i => !customerSales.Any(s => s.ItemId == i.Id))
                .Select(item => new RecommendedItem
                {
                    Item = item,
                    Discount = discount
                })
                .Take(3)
                .ToList();

            return recommendedItems;
        }

        private CustomerProfile CreateCustomerProfile(Customer customer)
        {
            // Create profile based on purchases
            var profile = new CustomerProfile
            {
                // 1. Calculate PurchaseFrequency (number of transactions)
                PurchaseFrequency = StaticData.Sales.Count(s => s.CustomerId == customer.Id),

                // 2. Calculate TotalSpent (sum of all purchases)
                TotalSpent = (float)StaticData.Sales.Where(s => s.CustomerId == customer.Id)
                                             .Sum(s => StaticData.Items.First(i => i.Id == s.ItemId).Price * s.PurchaseCount),

                // 3. Calculate LastPurchaseRecency (days since last purchase)
                DaysSinceLastPurchase = (DateTime.Now - StaticData.Sales.Where(s => s.CustomerId == customer.Id)
                                                              .Max(s => s.LastPurchaseDate)).Days
            };

            return profile;
        }

        private ItemProfile CreateItemProfile(Item item)
        {
            // Create item profile
            return new ItemProfile
            {
                Price = (float) item.Price,
                Type = item.Type,
                Brand = item.Brand
            };
        }
    }

    public class ItemProfile
    {
        public string Brand { get; set; }
        public string Type { get; set; }
        public float  Price { get; set; }
    }

    public class CustomerProfile
    {
        public float PurchaseFrequency { get; set; }
        public float TotalSpent { get; set; }
        public float DaysSinceLastPurchase { get; set; }
        public float Price { get; set; }
        public float Label { get; set; }
    }

    internal class LightGbmPrediction
    {
        public float Score { get; set; }
    }

    internal class FastTreePrediction
    {
        public float Score { get; set; }
    }

    public class ModelInput
    {
        public float PurchaseFrequency { get; set; }
        public float TotalSpent { get; set; }
        public float DaysSinceLastPurchase { get; set; }
        public float Price { get; set; }
    }

    public class ModelOutput
    {
        public float Score { get; set; }
    }
}
