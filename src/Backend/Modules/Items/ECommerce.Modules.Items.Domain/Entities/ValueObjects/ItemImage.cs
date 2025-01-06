namespace ECommerce.Modules.Items.Domain.Entities.ValueObjects
{
    public class ItemImage
    {
        public string Url { get; set; } = string.Empty;
        public bool MainImage { get; set; } = false;
    }
}
