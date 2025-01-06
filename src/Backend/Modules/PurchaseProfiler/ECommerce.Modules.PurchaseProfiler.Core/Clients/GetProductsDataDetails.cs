using ECommerce.Modules.PurchaseProfiler.Core.DTO;

namespace ECommerce.Modules.PurchaseProfiler.Core.Clients
{
    public record GetProductsDataDetails(IEnumerable<Guid> ProductIds);
    public record GetProductsDataDetailsResponse(List<ProductsDetailsDTO> Products);
}
