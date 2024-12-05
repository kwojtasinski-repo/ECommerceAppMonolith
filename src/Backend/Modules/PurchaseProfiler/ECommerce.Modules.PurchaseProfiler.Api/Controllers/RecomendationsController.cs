using ECommerce.Modules.PurchaseProfiler.Api.Profiler;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.PurchaseProfiler.Api.Controllers
{
    internal class RecomendationsController(RecommendationService recommendationService)
        : BaseController
    {
        [HttpGet("{customerId}")]
        public IActionResult GetRecommendations(Guid customerId)
        {
            var recommendations = recommendationService.GetRecommendations(customerId);
            if (recommendations == null || recommendations.Count == 0)
            {
                return NotFound("No recommendations found.");
            }

            return Ok(recommendations);
        }
    }
}
