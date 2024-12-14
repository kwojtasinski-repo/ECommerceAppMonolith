using ECommerce.Shared.Abstractions.Modules;

namespace ECommerce.Modules.PurchaseProfiler.Core.Clients
{
    internal sealed class ProductApiClient
        (
            IModuleClient moduleClient
        )
        : IProductApiClient
    {
        public async Task<GetProductResponse?> GetProduct(Guid productId)
        {
            return await moduleClient.SendAsync<GetProductResponse>("/products/get", new GetProduct(productId));
        }
    }
}
