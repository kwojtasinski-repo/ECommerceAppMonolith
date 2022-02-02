using ECommerce.Modules.Currencies.Core.Clients;
using ECommerce.Modules.Currencies.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Currencies.Api")]
namespace ECommerce.Modules.Currencies.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddExternalClient();
            services.AddScoped<INbpClient, NbpClient>();
            services.AddSingleton<ICurrencyRepository, InMemoryCurrencyRepository>();
            services.AddSingleton<ICurrencyRateRepository, InMemoryCurrencyRateRepository>();

            return services;
        }
    }
}