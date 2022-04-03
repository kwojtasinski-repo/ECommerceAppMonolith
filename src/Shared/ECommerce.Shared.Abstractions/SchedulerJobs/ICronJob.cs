namespace ECommerce.Shared.Abstractions.SchedulerJobs
{
    public interface ICronJob<T, U>
    {
        Task RunJob(CancellationToken cancellationToken);
    }
}
