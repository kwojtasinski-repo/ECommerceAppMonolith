using ECommerce.Modules.PurchaseProfiler.Api.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Api.Profiler
{
    public class RecommendationEngine(List<Product> allProducts)
    {
        private readonly List<Product> _allProducts = allProducts;

        public List<Product> RecommendProducts(CustomerPurchaser customer)
        {
            var recommendedProducts = new List<Product>();

            foreach (var purchasedProduct in customer.PurchasedProducts)
            {
                var similarProducts = _allProducts.Where(p =>
                    p.Tags.Intersect(purchasedProduct.Tags).Any() && p.Id != purchasedProduct.Id).ToList();

                recommendedProducts.AddRange(similarProducts);
            }

            return recommendedProducts.Distinct().ToList();
        }
    }
}
