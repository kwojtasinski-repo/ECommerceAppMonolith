namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    internal class SaleRecord
    {
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
        public int PurchaseCount { get; set; }
    }
}