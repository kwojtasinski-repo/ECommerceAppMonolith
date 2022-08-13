using ECommerce.Shared.Abstractions.SchedulerJobs;

namespace ECommerce.Shared.Infrastructure.SchedulerJobs
{
    internal class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
        public string Name { get; set; }
    }
}
