using ECommerce.Modules.PurchaseProfiler.Api.Entities;
using ECommerce.Modules.PurchaseProfiler.Api.Profiler;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.PurchaseProfiler.Api.Controllers
{
    internal class RecomendationsController : BaseController
    {
        [HttpGet]
        public ActionResult GetProfiler()
        {
            Product product = new ("1", "La", "a", "a", 999.99m, ["aaa"]);
            var products = new List<Product>
            { 
                new ("1", "Laptop", "BrandA", "Electronics", 999.99m, new List<string> { "technology", "computing", "laptop" }),
                new ("2", "Smartphone", "BrandB", "Electronics", 499.99m, new List<string> { "technology", "smartphone", "mobile" }),
                new ("3", "Tablet", "BrandC", "Electronics", 299.99m, new List<string> { "technology", "tablet", "computing" }),
                new ("4", "Headphones", "BrandD", "Accessories", 99.99m, new List<string> { "music", "audio", "headphones" }),
                new ("5", "Smartwatch", "BrandE", "Accessories", 199.99m, new List<string> { "technology", "smartwatch", "wearable" })
            };
            var customer1 = new Customer("1");
            customer1.AddPurchasedProduct(products[0]);

            var customer2 = new Customer("2");
            customer2.AddPurchasedProduct(products[3]);

            var recommendationEngine = new RecommendationEngine(products);

            var recommendationsForCustomer1 = recommendationEngine.RecommendProducts(customer1);
            var recommendationsForCustomer2 = recommendationEngine.RecommendProducts(customer2);
            return Ok(new Dictionary<string, IEnumerable<string>>
            {
                { customer1.CustomerId, recommendationsForCustomer1.Select(r => r.Name) },
                { customer2.CustomerId, recommendationsForCustomer2.Select(r => r.Name) },
            });
        }
    }
}
