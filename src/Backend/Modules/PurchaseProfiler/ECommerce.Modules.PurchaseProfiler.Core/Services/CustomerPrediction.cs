namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{        
    public class CustomerPrediction
    {  
        public float Score { get; set; }

        public float Probability => Sigmoid(Score);

        private static float Sigmoid(float x)
        {
            if (x < 0) 
            {
                return 0;
            }
            return 1 / (1 + (float)Math.Exp(-x));
        }
    }
}
