using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace ECommerce.Modules.Sales.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services;
        }
    }
}
