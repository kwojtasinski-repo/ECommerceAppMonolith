using ECommerce.Modules.PurchaseProfiler.Core.Database;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.PurchaseProfiler.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddArrangoDb();
            return services;
        }
    }
}
