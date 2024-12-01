using Microsoft.ML.Data;

namespace ECommerce.Modules.PurchaseProfiler.Api.Data
{
    public class ProductRating
    {
        [LoadColumn(0)]
        public int CustomerId { get; set; }

        [LoadColumn(1)]
        public int ItemId { get; set; }

        [LoadColumn(2)]
        public float Rating { get; set; }
    }
}
