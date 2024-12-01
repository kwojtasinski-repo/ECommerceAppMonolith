namespace ECommerce.Modules.PurchaseProfiler.Api.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public List<string> Tags { get; set; }

        public Product(string id, string name, string brand, string category, decimal price, List<string> tags)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Category = category;
            Price = price;
            Tags = tags;
        }
    }
}
