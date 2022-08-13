namespace ECommerce.Shared.Abstractions.SchedulerJobs
{
    public interface ISchedulerTask<T> where T : class, ISchedulerTask<T>
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
