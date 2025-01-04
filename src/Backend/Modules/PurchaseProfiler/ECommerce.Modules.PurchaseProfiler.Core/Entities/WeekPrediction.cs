namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class WeekPrediction : DocumentEntity, IDocumentEntity<long>
    {
        public override string CollectionName => "weekPredictions";

        public int Year { get; set; }
        public int WeekNumber { get; set; }
        public List<PurchasePrediction> PredictedPurchases { get; set; } = [];
        public Guid UserId { get; set; }
    }

    public class PurchasePrediction
    {
        public long ProductId { get; set; }
        public decimal Probability { get; set; }
    }
}
