namespace ECommerce.Modules.PurchaseProfiler.Core.DTO
{
    public record ProductsDetailsDTO(Guid ProductId, Guid ProductSaleId, decimal Cost, bool Active, string Name, string Description, string BrandName, string TypeName, string ImageUrl);
}
