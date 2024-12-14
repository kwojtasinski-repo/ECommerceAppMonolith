using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.PurchaseProfiler.Core.Clients
{
    internal static class Extensions
    {
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services.AddScoped<IUserApiClient, UserApiClient>();
            return services;
        }
    }
}
