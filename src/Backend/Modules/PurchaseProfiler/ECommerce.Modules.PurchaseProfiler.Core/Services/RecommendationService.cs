using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using Microsoft.ML;
using System.Globalization;

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

            var orders = await orderRepository.GetOrdersByUserIdAsync(userId);
            if (orders is null || orders.Count == 0)
            {
                return CreateEmptyRecommendationResult();
            }

            var groupedOrders = GroupOrdersByWeek(orders);
            var trainingData = GetCustomerData(groupedOrders);
            var model = await fastTreePurchaseProfilerModel.GetModel(userId);
            model ??= await fastTreePurchaseProfilerModel.GenerateModel(trainingData, userId);
            var itemsMap = CreateItemsMap(orders);
            var inputData = GetCustomerData([groupedOrders[^1]]);
            var predictions = model.Transform(_mlContext.Data.LoadFromEnumerable(inputData));
            var predictedResults = _mlContext.Data.CreateEnumerable<CustomerPrediction>(predictions, reuseRowObject: false).ToList();

            return
            [
                new ()
                {
                    { "predictions", ExtractPredictedResults(predictedResults, inputData, itemsMap) }
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

        private List<CustomerData> GetCustomerData(IEnumerable<IGrouping<WeeklyOrderSummary, Order>> groupedOrders)
        {
            var result = new Dictionary<WeeklyOrderSummary, List<CustomerData>>();

            foreach (var groupOrder in groupedOrders)
            {
                if (!result.TryGetValue(groupOrder.Key, out var customerList))
                {
                    customerList = [];
                    result[groupOrder.Key] = customerList;
                }

                foreach (var order in groupOrder)
                {
                    if (order.CustomerKey <= 0)
                    {
                        continue;
                    }

                    foreach (var item in order.Items)
                    {
                        if (item.ItemKey <= 0)
                        {
                            continue;
                        }

                        var product = customerList.FirstOrDefault(i => i.ProductId == item.ItemKey);
                        if (product is null)
                        {
                            product = CreateCustomerData(order, item);
                            customerList.Add(product);
                        }
                        else
                        {
                            product.PurchaseFrequency++;
                            product.Price += product.Price;
                        }
                    }
                }
            }

            return result.Values.SelectMany(customerList => customerList).ToList();
        }

        private CustomerData CreateCustomerData(Order order, OrderItem item)
        {
            return new CustomerData
            {
                CustomerId = order.CustomerKey,
                Price = (float)item.Cost,
                ProductId = item.ItemKey,
                PurchaseFrequency = 1
            };
        }

        private List<IGrouping<WeeklyOrderSummary,Order>> GroupOrdersByWeek(List<Order> orders)
        {
            return orders.GroupBy(order => {
                            var calendar = CultureInfo.CurrentCulture.Calendar;
                            var dateTime = order.OrderDate;
                            var weekNumber = calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            return new WeeklyOrderSummary { Year = dateTime.Year, WeekNumber = weekNumber };
                        })
                        .OrderBy(summary => summary.Key.Year)
                        .ThenBy(summary => summary.Key.WeekNumber)
                        .ToList();
        }

        private Dictionary<long, Guid> CreateItemsMap(List<Order> orders)
        {
            return orders.SelectMany(order => order.Items)
                .Select(item => new { ProductKey = item.ItemKey, ProductId = item.ItemId })
                .DistinctBy(item => item.ProductKey)
                .ToDictionary(key => key.ProductKey, value => value.ProductId);
        }

        private Dictionary<Guid, float> ExtractPredictedResults(List<CustomerPrediction> customerPredictions, List<CustomerData> inputData, Dictionary<long, Guid> itemMap)
        {
            var results = new Dictionary<Guid, float>();
            var index = -1;
            foreach (var customerPrediction in customerPredictions)
            {
                index++;
                if (customerPrediction.Probability <= 0)
                {
                    continue;
                }

                if (index >= inputData.Count)
                {
                    continue;
                }

                if (!itemMap.TryGetValue((long)inputData[index].ProductId, out var productId))
                {
                    continue;
                }

                results.Add(productId, customerPrediction.Probability);
            }

            return results;
        }

        private class WeeklyOrderSummary
        {
            public int Year { get; set; }
            public int WeekNumber { get; set; }
        }
    }
}
