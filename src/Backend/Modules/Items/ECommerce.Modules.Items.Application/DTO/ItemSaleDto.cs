namespace ECommerce.Modules.Items.Application.DTO
{
    public class ItemSaleDto
    {
        public Guid Id { get; set; }
        public ItemToSaleDto Item { get; set; } = null!;
        public decimal Cost { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    public record ItemToSaleDto(Guid Id, string ItemName, ImageUrl ImageUrl);
}
