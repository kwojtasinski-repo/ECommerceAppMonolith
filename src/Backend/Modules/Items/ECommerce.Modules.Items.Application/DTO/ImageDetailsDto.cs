namespace ECommerce.Modules.Items.Application.DTO
{
    public class ImageDetailsDto : ImageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
