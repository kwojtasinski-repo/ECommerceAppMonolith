namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{        
    public class CustomerPrediction
    {
        // The predicted label (ProductId)
        public float PredictedLabel { get; set; }

        // Confidence scores for each ProductId
        public float[] Score { get; set; } = [];

        public float Probability => Sigmoid(Score);

        private static float Sigmoid(float[] scores)
        {
            if (scores is null || scores.Length == 0)
            {
                return 0;
            }

            return scores.Where(score => score > 0)
                .Select(score => 1 / (1 + (float)Math.Exp(-score)))
                .Average();
        }
    }
}
