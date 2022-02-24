namespace ECommerce.Modules.Sales.Application.Orders.DTO
{
    public class ItemCartDto
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public IEnumerable<string> ImagesUrl { get; set; }
        public decimal Cost { get; set; }
    }
}
