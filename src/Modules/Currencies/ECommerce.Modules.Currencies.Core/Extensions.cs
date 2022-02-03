using ECommerce.Modules.Currencies.Core.Clients;
using ECommerce.Modules.Currencies.Core.Repositories;
using ECommerce.Modules.Currencies.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Currencies.Api")]
namespace ECommerce.Modules.Currencies.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddNbpClient();
            services.AddSingleton<ICurrencyRepository, InMemoryCurrencyRepository>();
            services.AddSingleton<ICurrencyRateRepository, InMemoryCurrencyRateRepository>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyRateService, CurrencyRateService>();

            return services;
        }
    }
}