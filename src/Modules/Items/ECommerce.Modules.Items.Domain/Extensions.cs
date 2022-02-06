using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Items.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services;
        }
    }
}