namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class ComputePrediction : DocumentEntity, IDocumentEntity<long>
    {
        public override string CollectionName => "computePredictions";

        public int Year { get; set; }
        public int WeekNumber { get; set; }
        public Guid UserId { get; set; }
    }
}
