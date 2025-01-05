namespace ECommerce.Modules.PurchaseProfiler.Core.Clients.External
{
    internal interface IProfilerClient
    {
        Task<PredictionResponse?> PredictPurchases(PredictionRequest request);
    }
}
