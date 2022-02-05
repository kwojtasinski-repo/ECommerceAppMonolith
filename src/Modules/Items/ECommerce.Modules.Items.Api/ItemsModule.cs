using ECommerce.Modules.Items.Infrastructure;
using ECommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Items.Api
{
    public class ItemsModule : IModule
    {
        public const string BasePath = "items-module";
        public string Name => "Items";

        public string Path => BasePath;

        public void Register(IServiceCollection services)
        {
            services.AddInfrastructure();
        }

        public void Use(IApplicationBuilder app)
        {
        }
    }
}