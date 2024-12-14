namespace ECommerce.Modules.Items.Application.DTO
{
    public class ItemSaleDetailsDto
    {
        public Guid Id { get; set; }
        public ItemDetailsDto Item { get; set; } = null!;
        public decimal Cost { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
