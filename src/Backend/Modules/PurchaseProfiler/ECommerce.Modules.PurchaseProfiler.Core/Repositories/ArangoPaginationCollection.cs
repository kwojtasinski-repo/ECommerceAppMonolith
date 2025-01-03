namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    public class ArangoPaginationCollection<T>
        (
            Func<int, int, Task<List<T>>> fetchFunction,
            int pageSize
        )
    {
        private int _currentOffset = 0;

        public async Task<List<T>> GetNextAsync()
        {
            var results = await fetchFunction(_currentOffset, pageSize);
            _currentOffset += pageSize;
            return results;
        }

    }
}
