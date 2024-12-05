namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    internal partial class RecommendationService
    {
        public class CustomerPrediction
        {  
            // The raw score (log-odds)
            public float Score { get; set; }

            // Additional field for the probability if needed
            public float Probability => Sigmoid(Score);  // Calculate probability from score

            // Sigmoid function to convert the score to a probability
            private static float Sigmoid(float x)
            {
                if (x < 0) // Adjust threshold based on your data
                {
                    return 0; // Default value for edge cases
                }
                return 1 / (1 + (float)Math.Exp(-x));  // Sigmoid function
            }
        }
    }
}
