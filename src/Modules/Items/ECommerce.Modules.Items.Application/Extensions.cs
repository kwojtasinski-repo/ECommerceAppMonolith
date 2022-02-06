using ECommerce.Modules.Items.Application.Policies.Image;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Items.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<ISaveFilePolicy, DefaultSaveFilePolicy>();
            return services;
        }
    }
}