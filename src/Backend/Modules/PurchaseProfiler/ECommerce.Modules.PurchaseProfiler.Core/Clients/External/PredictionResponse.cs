namespace ECommerce.Modules.PurchaseProfiler.Core.Clients.External
{
    internal class PredictionResponse
    {
        public List<PredictionResult> Predictions { get; set; } = [];
    }

    internal class PredictionResult
    {
        public int PurchaseGroup { get; set; }
        public List<PredictionValue> Predictions { get; set; } = [];
    }

    internal class PredictionValue
    {
        public long ProductId { get; set; }
        public decimal Probability { get; set; }
    }
}
