namespace ECommerce.Modules.PurchaseProfiler.Core.External
{
    internal interface IProfilerClient
    {
        Task<PredictionResponse?> PredictPurchases(PredictionRequest request);
    }
}
