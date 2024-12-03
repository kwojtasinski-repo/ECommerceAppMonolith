using ECommerce.Modules.PurchaseProfiler.Api.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    public class RecommendedItem
    {
        public Item Item { get; set; }
        public decimal Discount { get; set; }
    }
}
