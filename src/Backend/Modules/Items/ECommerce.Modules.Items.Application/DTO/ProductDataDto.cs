namespace ECommerce.Modules.Items.Application.DTO
{
    public record ProductDataDto(Guid ProductId, Guid ProductSaleId, decimal Cost, bool IsActivated);
}
