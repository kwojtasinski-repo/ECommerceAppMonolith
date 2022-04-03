namespace ECommerce.Shared.Abstractions.SchedulerJobs
{
    public interface ISchedulerTask<T> where T : class
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
