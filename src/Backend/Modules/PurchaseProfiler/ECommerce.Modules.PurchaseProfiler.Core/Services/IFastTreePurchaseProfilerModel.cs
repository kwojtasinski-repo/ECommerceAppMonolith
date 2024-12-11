using Microsoft.ML;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal interface IFastTreePurchaseProfilerModel
    {
        Task<ITransformer?> GetModel(Guid userId);
        Task<ITransformer> GenerateModel(IEnumerable<CustomerData> trainData, Guid userId);
    }
}
