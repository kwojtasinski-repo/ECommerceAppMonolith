using ECommerce.Modules.Currencies.Core;
using ECommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Currencies.Api
{
    public class CurrenciesModule : IModule
    {
        public const string BasePath = "currencies-module";
        public string Name => "Currencies";
        public string Path => BasePath;
        public IEnumerable<string> Policies { get; } = new[]
        {
            "currencies"
        };

        public void Register(IServiceCollection services)
        {
            services.AddCore();
        }

        public void Use(IApplicationBuilder app)
        {
        }
    }
}