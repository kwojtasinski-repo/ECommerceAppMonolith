using ECommerce.Shared.Abstractions.SchedulerJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.SchedulerJobs
{
    internal sealed class CronJob<T, U> : CronJobService<T, U>, ICronJob<T, U>
        where T : class, ISchedulerTask<U>
        where U : class, T
    {
        private readonly ILogger<CronJob<T, U>> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CronJob(IScheduleConfig<T> config, ILogger<CronJob<T, U>> logger, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override async Task RunJob(CancellationToken cancellationToken)
        {
            var type = typeof(U);
            _logger.LogInformation("Cron '{cronJobTypeName}' is working.", type.Name);
            using var scope = _serviceProvider.CreateScope();
            var cronService = scope.ServiceProvider.GetRequiredService<T>();
            await cronService.DoWork(cancellationToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var type = typeof(U);
            _logger.LogInformation("Cron '{cronJobTypeName}' starts.", type.Name);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            var type = typeof(U);
            _logger.LogInformation("Cron '{cronJobTypeName}' starts.", type.Name);
            return base.StopAsync(cancellationToken);
        }
    }
}
