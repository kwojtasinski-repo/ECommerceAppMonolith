namespace ECommerce.Shared.Abstractions.SchedulerJobs
{
    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
        string Name { get; set; }
    }
}
