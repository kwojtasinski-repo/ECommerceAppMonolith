using ECommerce.Modules.PurchaseProfiler.Core.Services;
using ECommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.PurchaseProfiler.Api.Controllers
{
    [Authorize]
    internal class RecomendationsController
        (
            IRecommendationService recommendationService,
            IContext context
        )
        : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetRecommendations()
        {
            var recommendations = await recommendationService.GetRecommendationOnCurrentWeek(context.Identity.Id);
            return Ok(recommendations);
        }
    }
}
