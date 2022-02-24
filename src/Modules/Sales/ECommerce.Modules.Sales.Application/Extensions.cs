using ECommerce.Modules.Sales.Application.Orders.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Sales.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IOrderDeletionPolicy, OrderDeletionPolicy>();
            return services;
        }
    }
}