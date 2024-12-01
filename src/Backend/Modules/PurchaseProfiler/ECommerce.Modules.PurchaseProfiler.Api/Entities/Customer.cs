namespace ECommerce.Modules.PurchaseProfiler.Api.Entities
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public List<Product> PurchasedProducts { get; set; }

        public Customer(string customerId)
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
