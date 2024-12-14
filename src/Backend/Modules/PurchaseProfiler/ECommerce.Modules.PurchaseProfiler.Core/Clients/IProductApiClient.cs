namespace ECommerce.Modules.PurchaseProfiler.Core.Clients
{
    internal interface IProductApiClient
    {
        Task<GetProductResponse?> GetProduct(Guid productId);
    }
}
