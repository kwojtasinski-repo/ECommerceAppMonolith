using ECommerce.Modules.Contacts.Core;
using ECommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Contacts.Api
{
    public class ContactsModule : IModule
    {
        public const string BasePath = "contacts-module";
        public string Name => "Currencies";
        public string Path => BasePath;

        public void Register(IServiceCollection services)
        {
            services.AddCore();
        }

        public void Use(IApplicationBuilder app)
        {
        }
    }
}