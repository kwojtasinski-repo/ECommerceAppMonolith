namespace ECommerce.Modules.Sales.Domain.ItemSales.Entities
{
    public class ItemSale
    {
        public Guid Id { get; private set; }
        public string ItemName { get; private set; }
        public string BrandName { get; private set; }
        public string TypeName { get; private set; }
        public string? Description { get; private set; }
        public decimal Cost { get; private set; }
        public IEnumerable<string>? Tags { get; private set; }
        public IEnumerable<ImageUrl> ImagesUrl { get; private set; }
        public bool Active { get; private set; }

        public ItemSale(Guid id, string itemName, string brandName, string typeName, string? description, decimal cost, IEnumerable<string>? tags, IEnumerable<ImageUrl> imageUrls, bool active)
        {
            Id = id;
            ItemName = itemName;
            BrandName = brandName;
            TypeName = typeName;
            Description = description;
            Cost = cost;
            Tags = tags;
            ImagesUrl = imageUrls;
            Active = active;
        }
    }

    public class ImageUrl
    {
        public string Url { get; private set; }
        public bool MainImage { get; private set; }

        public ImageUrl(string url, bool mainImage)
        {
            Url = url;
            MainImage = mainImage;
        }
    }
}