using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using ECommerce.Modules.PurchaseProfiler.Core.External;
using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal class ComputePredictionCheckerService
        (
            ILogger<ComputePredictionCheckerService> logger,
            IServiceProvider serviceProvider
        )
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("ComputePredictionCheckerService is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("Starting database check...");
                    await ProcessComputePredictions();
                    logger.LogInformation("Database check completed.");

                    // Wait for 5 minutes before checking again.
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    logger.LogInformation("ComputePredictionCheckerService task was canceled.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while checking the database.");
                }
            }

            logger.LogInformation("TaskCheckerService is stopping.");
        }

        private async Task ProcessComputePredictions()
        {
            using var scope = serviceProvider.CreateScope();
            var computePredictionRepository = scope.ServiceProvider.GetRequiredService<IComputePredictionRepository>();
            var weekPredictionRepository = scope.ServiceProvider.GetRequiredService<IWeekPredictionRepository>();
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            var profilerClient = scope.ServiceProvider.GetRequiredService<IProfilerClient>();

            var paginationCollection = computePredictionRepository.GetPaginatedComputePredictions(50);
            List<ComputePrediction> computePredictions;

            while ((computePredictions = await paginationCollection.GetNextAsync()).Count != 0)
            {
                foreach (var computePrediction in computePredictions)
                {
                    // 24 weeks back
                    logger.LogInformation("ComputePredictionCheckerService, {methodName} generating dates for 24 weeks back for year '{year}' and week '{week}'", nameof(ProcessComputePredictions), computePrediction.Year, computePrediction.WeekNumber);
                    var weeks = GenerateDatesFor24Weeks(computePrediction.Year, computePrediction.WeekNumber);
                    var startDate = weeks.LastOrDefault().StartOfWeek;
                    var endDate = weeks.FirstOrDefault().EndOfWeek;

                    logger.LogInformation("ComputePredictionCheckerService, {methodName} getting orders for user with id '{userId}' and from date range '{startDate}' to '{endDate}'", nameof(ProcessComputePredictions), computePrediction.UserId, startDate, endDate);
                    var orders = await orderRepository.GetOrdersByUserIdAndOrderDateRangeAsync(computePrediction.UserId, startDate, endDate);
                    logger.LogInformation("ComputePredictionCheckerService, {methodName} extracting purchase products", nameof(ProcessComputePredictions));
                    var predictionData = ExtractPurchaseProducts(orders, weeks);

                    if (!predictionData.Any())
                    {
                        logger.LogInformation("ComputePredictionCheckerService, {methodName} prediction data is empty skipping prediciton request for user '{userId}', year '{year}' and week '{week}'", nameof(ProcessComputePredictions), computePrediction.UserId, computePrediction.Year, computePrediction.WeekNumber);
                        await computePredictionRepository.DeleteAsync(computePrediction.Key!);
                        continue;
                    }

                    logger.LogInformation("ComputePredictionCheckerService, {methodName} sending prediction request using {profilerClient}", nameof(ProcessComputePredictions), nameof(IProfilerClient));
                    var response = await profilerClient.PredictPurchases(new PredictionRequest
                    {
                        PurchaseHistory = predictionData.Select(p => p.Keys.ToList()).ToList(),
                        ProductFrequencies = predictionData.Select(p =>
                        {
                            var list = new List<ProductFrequency>();
                            foreach (var pd in p)
                            {
                                list.Add(new ProductFrequency { ProductId = pd.Key.ToString(), Quantity = pd.Value });
                            }
                            return list;
                        }).ToList(),
                        Labels = predictionData.Select(p => p.MaxBy(pd => pd.Value).Key).ToList()
                    });

                    if (response is not null)
                    {
                        logger.LogInformation("ComputePredictionCheckerService, {methodName} received prediction results and saving into database for user '{userId}', year '{year}' and week '{week}'", nameof(ProcessComputePredictions), computePrediction.UserId, computePrediction.Year, computePrediction.WeekNumber);
                        await weekPredictionRepository.AddAsync(new WeekPrediction
                        {
                            PredictedPurchases = response.Predictions?.FirstOrDefault()?
                                .Predictions?.Select(p => new PurchasePrediction
                                {
                                    Probability = p.Probability,
                                    ProductId = p.ProductId
                                })?.ToList() ?? [],
                            Year = computePrediction.Year,
                            WeekNumber = computePrediction.WeekNumber
                        });
                    }

                    await computePredictionRepository.DeleteAsync(computePrediction.Key!);
                }
            }
        }

        private IEnumerable<Dictionary<long, int>> ExtractPurchaseProducts(IEnumerable<Order> orders, IEnumerable<(DateTime StartOfWeek, DateTime EndOfWeek)> weeks)
        {
            if (orders is null || !orders.Any() || !weeks.Any())
            {
                return [];
            }

            var result = new List<Dictionary<long, int>>();

            foreach (var (startOfWeek, endOfWeek) in weeks)
            {
                var ordersInWeek = orders.Where(o => o.OrderDate >= startOfWeek && o.OrderDate <= endOfWeek);
                if (!ordersInWeek.Any())
                {
                    continue;
                }

                var productsInWeek = ordersInWeek
                    .SelectMany(o => o.Items)
                    .GroupBy(item => item.ItemKey)
                    .Select(group => new
                    {
                        ItemKey = group.Key,
                        TotalQuantity = group.Count()
                    })
                    .ToDictionary(k => k.ItemKey, v => v.TotalQuantity);

                result.Add(productsInWeek);
            }

            return result;
        }

        private IEnumerable<(DateTime StartOfWeek, DateTime EndOfWeek)> GenerateDatesFor24Weeks(int year, int week)
        {
            var result = new List<(DateTime, DateTime)>();

            for (int i = 0; i < 24; i++)
            {
                DateTime startOfWeek = FirstDateOfWeekISO8601(year, week);
                DateTime endOfWeek = startOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                result.Add((startOfWeek, endOfWeek));

                week--;
                if (week <= 0)
                {
                    year--;
                    week = ISOWeek.GetWeeksInYear(year);
                }
            }

            return result;
        }

        private DateTime FirstDateOfWeekISO8601(int year, int week)
        {
            // ISO 8601 weeks start on Monday, and the first week of the year contains January 4th.
            DateTime jan4 = new DateTime(year, 1, 4);
            int daysOffset = (int)jan4.DayOfWeek - (int)DayOfWeek.Monday;

            // Find the first Monday of the year
            DateTime firstMonday = jan4.AddDays(-daysOffset);

            // Calculate the start date of the target week
            return firstMonday.AddDays((week - 1) * 7);
        }
    }
}
