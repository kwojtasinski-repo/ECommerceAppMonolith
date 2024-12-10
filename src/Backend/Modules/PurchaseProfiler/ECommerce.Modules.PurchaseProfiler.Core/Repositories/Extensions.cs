using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal static class Extensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
