using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            var paginationCollection = computePredictionRepository.GetPaginatedComputePredictions(50);
            List<ComputePrediction> computePredictions;

            while ((computePredictions = await paginationCollection.GetNextAsync()).Count != 0)
            {
                foreach (var computePrediction in computePredictions)
                {
                    // call other service

                    await weekPredictionRepository.AddAsync(new WeekPrediction
                    {
                        PredictedPurchases = [],
                        Year = computePrediction.Year,
                        WeekNumber = computePrediction.WeekNumber
                    });
                }
            }
        }
    }
}
