namespace ECommerce.Modules.Items.Application.DTO
{
    public record ProductsDataDetailsDto(List<ProductsDetailsDto> Products);
    public record ProductsDetailsDto(Guid ProductId, Guid ProductSaleId, decimal Cost, bool Active, string Name, string Description, string BrandName, string TypeName, string ImageUrl);
}
