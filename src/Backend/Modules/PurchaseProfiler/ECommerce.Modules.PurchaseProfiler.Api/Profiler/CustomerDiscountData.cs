namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    public class CustomerDiscountData
    {
        public float PurchaseFrequency { get; set; }
        public float TotalPurchaseValue { get; set; }
        public float DaysSinceLastPurchase { get; set; }
        public float Discount { get; set; } // Changed from decimal to float
    }
}
