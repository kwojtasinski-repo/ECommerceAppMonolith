using ECommerce.Modules.Items.Application;
using ECommerce.Modules.Items.Application.Commands.Items;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Domain;
using ECommerce.Modules.Items.Infrastructure;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Modules;
using ECommerce.Shared.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Items.Api
{
    public class ItemsModule : IModule
    {
        public const string BasePath = "items-module";
        public string Name => "Items";

        public string Path => BasePath;

        public IEnumerable<string> Policies { get; } = new[]
        {
            "items", "item-sale"
        };

        public void Register(IServiceCollection services)
        {
            services.AddDomain();
            services.AddInfrastructure();
            services.AddApplication();
        }

        public void Use(IApplicationBuilder app)
        {
            app.UseModuleRequests()
                .Subscribe<GetProductData, ProductDataDto>("/products/get", (command, sp) =>
                    sp.GetRequiredService<ICommandDispatcher>().SendAsync<ProductDataDto?>(command)
                )
                .Subscribe<GetProductsDataDetails, ProductsDataDetailsDto>("/products/get/details", (command, sp) =>
                    sp.GetRequiredService<ICommandDispatcher>().SendAsync<ProductsDataDetailsDto?>(command)
                );
        }
    }
}