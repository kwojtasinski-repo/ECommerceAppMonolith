namespace ECommerce.Modules.PurchaseProfiler.Api.Entities
{
    public class CustomerPurchaser
    {
        public string CustomerId { get; set; }
        public List<Product> PurchasedProducts { get; set; }

        public CustomerPurchaser(string customerId)
        {
            CustomerId = customerId;
            PurchasedProducts = new List<Product>();
        }

        public void AddPurchasedProduct(Product product)
        {
            PurchasedProducts.Add(product);
        }
    }

}
