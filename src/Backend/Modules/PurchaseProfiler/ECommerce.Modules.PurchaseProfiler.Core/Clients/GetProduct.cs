namespace ECommerce.Modules.PurchaseProfiler.Core.Clients
{
    public record GetProduct(Guid ProductId);
    public record GetProductResponse(Guid ProductId, Guid ProductSaleId, decimal Cost, bool IsActivated);
}
