using ECommerce.Shared.Abstractions.SchedulerJobs;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.SchedulerJobs
{
    public static class Extensions
    {
        public static IServiceCollection AddCronJob<TService, TImplementation>(this IServiceCollection services, Action<IScheduleConfig<TService>> options) 
            where TService : class, ISchedulerTask<TImplementation>
            where TImplementation : class, TService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }

            var config = new ScheduleConfig<TService>();
            options.Invoke(config);

            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<TService>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddScoped<TService, TImplementation>();
            services.AddSingleton<IScheduleConfig<TService>>(config);
            services.AddHostedService<CronJob<TService, TImplementation>>();

            return services;
        }
    }
}
