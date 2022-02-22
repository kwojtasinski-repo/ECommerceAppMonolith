using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Sales.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}