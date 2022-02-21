using ECommerce.Modules.Items.Application.Policies.Image;
using ECommerce.Modules.Items.Application.Policies.Items;
using ECommerce.Modules.Items.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Items.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyAssemblyGen2")] //dodane dla generowania mockow do internali
namespace ECommerce.Modules.Items.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<ISaveFilePolicy, DefaultSaveFilePolicy>();
            services.AddTransient<IItemUpdatePolicy, ItemUpdatePolicy>();
            services.AddTransient<IItemDeletionPolicy, ItemDeletionPolicy>(); 
            services.AddSingleton<IEventMapper, EventMapper>();
            return services;
        }
    }
}