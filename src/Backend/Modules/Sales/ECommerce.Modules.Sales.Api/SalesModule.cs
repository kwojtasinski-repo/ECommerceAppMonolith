using ECommerce.Modules.Sales.Application;
using ECommerce.Modules.Sales.Domain;
using ECommerce.Modules.Sales.Infrastructure;
using ECommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Sales.Api
{
    internal class SalesModule : IModule
    {
        public const string BasePath = "sales-module";

        public string Name { get; } = "Sales";
        public string Path => BasePath;
        public IEnumerable<string> Policies { get; } = [];

        public void Register(IServiceCollection services)
        {
            services.AddDomain();
            services.AddApplication();
            services.AddInfrastructure();
        }

        public void Use(IApplicationBuilder app)
        {
        }
    }
}