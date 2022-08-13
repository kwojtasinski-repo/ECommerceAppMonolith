namespace ECommerce.Shared.Infrastructure.SchedulerJobs
{
    internal interface ICronJob<T, U>
    {
        Task RunJob(CancellationToken cancellationToken);
    }
}
