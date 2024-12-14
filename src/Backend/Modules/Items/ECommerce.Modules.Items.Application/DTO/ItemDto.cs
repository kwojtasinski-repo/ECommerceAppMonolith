namespace ECommerce.Modules.Items.Application.DTO
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public BrandDto Brand { get; set; } = null!;
        public TypeDto Type { get; set; } = null!;
        public ImageUrl? ImagesUrl { get; set; } = null!;
    }
}
