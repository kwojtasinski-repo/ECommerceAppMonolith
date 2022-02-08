using ECommerce.Modules.Items.Application.Policies.Image;
using ECommerce.Modules.Items.Application.Policies.Items;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Items.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<ISaveFilePolicy, DefaultSaveFilePolicy>();
            services.AddTransient<IItemUpdatePolicy, ItemUpdatePolicy>();
            services.AddTransient<IItemDeletionPolicy, ItemDeletionPolicy>();
            return services;
        }
    }
}