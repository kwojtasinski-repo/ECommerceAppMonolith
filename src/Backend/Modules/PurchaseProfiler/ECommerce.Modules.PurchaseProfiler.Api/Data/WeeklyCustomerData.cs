namespace ECommerce.Modules.PurchaseProfiler.Api.Data
{
    public class WeeklyCustomerData
    {
        public Guid CustomerId { get; set; }
        public int WeekNumber { get; set; }
        public int PurchaseFrequency { get; set; }
        public float TotalSpent { get; set; }
        public int DaysSinceLastPurchase { get; set; }
    }
}
