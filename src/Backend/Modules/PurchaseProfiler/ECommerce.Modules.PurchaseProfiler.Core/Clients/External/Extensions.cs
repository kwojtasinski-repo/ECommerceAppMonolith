using ECommerce.Shared.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECommerce.Modules.PurchaseProfiler.Core.Clients.External
{
    internal static class Extensions
    {
        public static IServiceCollection AddExternalClients(this IServiceCollection services)
        {
            services.AddProfilerClient();
            return services;
        }

        private static IServiceCollection AddProfilerClient(this IServiceCollection services)
        {
            var options = services.GetOptions<ProfilerClientOptions>("profilerClient");
            services.Configure<ProfilerClientOptions>(config =>
            {
                config.ApiUrl = options.ApiUrl;
                config.Timeout = options.Timeout;
            });
            services.AddHttpClient("ProfilerClient", (sp, options) =>
            {
                var profilerOptions = sp.GetRequiredService<IOptions<ProfilerClientOptions>>();
                options.Timeout = TimeSpan.FromSeconds(profilerOptions.Value.Timeout);
                options.BaseAddress = new Uri(profilerOptions.Value.ApiUrl);
            });
            services.AddScoped<IProfilerClient, ProfilerClient>();
            return services;
        }
    }
}
