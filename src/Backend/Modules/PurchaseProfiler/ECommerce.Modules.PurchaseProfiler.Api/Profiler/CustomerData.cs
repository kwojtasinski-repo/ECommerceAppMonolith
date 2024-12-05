namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    internal partial class RecommendationService
    {
        public class CustomerData
        {
            public float CustomerId { get; set; }  // Customer ID
            public float ProductId { get; set; }   // Product ID
            public float Price { get; set; }       // Product price
            public float PurchaseFrequency { get; set; }  // Purchase frequency 
            public bool PurchasedProduct { get; set; } // New label (binary outcome) (target variable)
        }
    }
}
