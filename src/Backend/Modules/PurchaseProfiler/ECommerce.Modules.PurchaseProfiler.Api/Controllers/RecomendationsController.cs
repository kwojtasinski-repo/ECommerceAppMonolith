using ECommerce.Modules.PurchaseProfiler.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.PurchaseProfiler.Api.Controllers
{
    internal class RecomendationsController(IRecommendationService recommendationService)
        : BaseController
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRecommendations(Guid userId)
        {
            var recommendations = await recommendationService.GetRecommendationOnCurrentWeek(userId);
            return Ok(recommendations);
        }
    }
}
