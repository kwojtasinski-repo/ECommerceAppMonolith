using ECommerce.Shared.Abstractions.SchedulerJobs;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.SchedulerJobs
{
    public static class Extensions
    {
        public static IServiceCollection AddCronJob<T, U>(this IServiceCollection services, Action<IScheduleConfig<T>> options) 
            where T : class, ISchedulerTask<U>
            where U : class
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }

            var config = new ScheduleConfig<T>();
            options.Invoke(config);

            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<CrobJob<T, U>>();

            return services;
        }
    }
}
