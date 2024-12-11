using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.PurchaseProfiler.Core.Services
{
    internal static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFastTreePurchaseProfilerModel, FastTreePurchaseProfilerModel>();
            services.AddScoped<IRecommendationService, RecommendationService>();
            return services;
        }
    }
}
