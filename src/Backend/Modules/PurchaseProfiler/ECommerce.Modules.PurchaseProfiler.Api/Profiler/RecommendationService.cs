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
        private readonly List<CustomerData> _customerData;

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
            var testDataSplit = dataSplit.TestSet;

            // Train the model
            _fastTreeModel = pipeline.Fit(trainDataSplit);
        }

        public List<Dictionary<string, object>> GetRecommendations(Guid customerId)
        {
            var customer = StaticData.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null) return new List<Dictionary<string, object>>();
            
            var predictionData = new List<CustomerData>
            {
                new CustomerData { CustomerId = 12, ProductId = 10, Price = 100, PurchaseFrequency = 200 },
                new CustomerData { CustomerId = 2, ProductId = 3, Price = 150, PurchaseFrequency = 1000 },
                new CustomerData { CustomerId = 100, ProductId = 300, Price = 150, PurchaseFrequency = 1 },
            };

            var predictions = _fastTreeModel.Transform(_mlContext.Data.LoadFromEnumerable(predictionData));

            // Display the results
            var predictedResults = _mlContext.Data.CreateEnumerable<CustomerPrediction>(predictions, reuseRowObject: false).ToList();
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "predictions", predictedResults }
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

        private static Random random = new Random();

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


        public class CustomerData
        {
            public float CustomerId { get; set; }  // Customer ID
            public float ProductId { get; set; }   // Product ID
            public float Price { get; set; }       // Product price
            public float PurchaseFrequency { get; set; }  // Purchase frequency 
            public bool PurchasedProduct { get; set; } // New label (binary outcome) (target variable)
        }

        public class CustomerPrediction
        {  
            // The raw score (log-odds)
            public float Score { get; set; }

            // Additional field for the probability if needed
            public float Probability => Sigmoid(Score);  // Calculate probability from score

            // Sigmoid function to convert the score to a probability
            private static float Sigmoid(float x)
            {
                if (x < 0) // Adjust threshold based on your data
                {
                    return 0; // Default value for edge cases
                }
                return 1 / (1 + (float)Math.Exp(-x));  // Sigmoid function
            }
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

    public class CustomerWeeklyData
    {
        public float WeekNumber { get; set; }
        public float PurchaseFrequency { get; set; }
        public float TotalSpent { get; set; }
        public float DaysSinceLastPurchase { get; set; }
        public float ProductTypeCount { get; set; }
        public float TotalProductCount { get; set; }
        [LoadColumn(0), ColumnName("Label")]
        public float NumberOfPurchases { get; set; }
        public float ProductId { get; set; } // Zamiana ProductId na float
    }

    public class CustomerPrediction
    {
        // Predykcja liczby zakupów
        public float NumberOfPurchases { get; set; }
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
