using ECommerce.Shared.Abstractions.SchedulerJobs;
using ECommerce.Shared.Infrastructure.SchedulerJobs;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFastTreePurchaseProfilerModel, FastTreePurchaseProfilerModel>();
            services.AddScoped<IRecommendationService, RecommendationService>();
            services.AddCronJob<ISchedulerTask<PredictionScheduler>, PredictionScheduler>(options =>
            {
                options.TimeZoneInfo = TimeZoneInfo.Local;
                // At minute 0 past every 2nd hour 
                options.CronExpression = @"0 */2 * * *";// https://crontab.guru/ info
            });
            services.AddHostedService<ComputePredictionCheckerService>();
            return services;
        }
    }
}
