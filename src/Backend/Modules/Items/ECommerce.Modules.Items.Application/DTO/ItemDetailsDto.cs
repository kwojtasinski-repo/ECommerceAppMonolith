namespace ECommerce.Modules.Items.Application.DTO
{
    public class ItemDetailsDto : ItemDto
    {
        public string? Description { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public new IEnumerable<ImageUrl>? ImagesUrl { get; set; }
    }
}
